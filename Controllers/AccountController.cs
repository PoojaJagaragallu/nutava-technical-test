using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nutava.Test.NumberToWord.Helpers;
using Nutava.Test.NumberToWord.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Nutava.Test.NumberToWord.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOptions<AppSettings> config;

        public AccountController(IOptions<AppSettings> config)
        {
            this.config = config;
        }

        /// <summary>
        /// Gets and displays login view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Registers new user.
        /// </summary>
        /// <param name="loginModel"></param>
        [HttpPost]
        public async Task<IActionResult> Register(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (RegisterUser(loginModel.UserName, loginModel.Password, out string error))
                {
                    await SetAuthenticatedPrincipal(loginModel.UserName);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = error;
            }
            return View("Login");
        }

        /// <summary>
        ///  Login existing user.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(loginModel.UserName, loginModel.Password, out string error))
                {
                    await SetAuthenticatedPrincipal(loginModel.UserName);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = error;
            }
            return View();
        }

        /// <summary>
        ///  Logs out current user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }

        /// <summary>
        /// Sets up authenticated principal 
        /// </summary>
        /// <param name="username"></param>
        private async Task SetAuthenticatedPrincipal(string username)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
        }

        /// <summary>
        ///  Checks for exixting user and registers if new.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="error"></param>
        /// <returns>retruns true if registered successfully.</returns>
        private bool RegisterUser(string username, string password, out string error)
        {
            try
            {
                error = "";
                var filePath = this.config.Value.UsersFilePath;
                var user = GetExistingUser(username);
                if (user == null)
                {
                    CreateIfMissing(filePath);
                    var fileMode = System.IO.File.Exists(filePath) ? FileMode.Append : FileMode.Create;
                    using FileStream fs = new FileStream(filePath, fileMode);
                    using StreamWriter sw = new StreamWriter(fs);
                    string fullText = (username + "," + EncryptionHelper.Encrypt(password));
                    sw.WriteLine(fullText);
                    return true;
                }
                else
                {
                    error = "User already exists.";
                    return false;
                }
            }
            catch
            {
                error = "Something went wrong! Please try again later.";
                return false;
            }
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="error"></param>
        /// <returns>retruns true if user credentials are valid.</returns>
        private bool ValidateUser(string username, string password, out string error)
        {
            try
            {
                error = "";
                var user = GetExistingUser(username);
                var encryptedPassword = EncryptionHelper.Encrypt(password);

                if (user == null)
                {
                    error = "No users found. Please register";
                    return false;
                }
                if (user != null && (user.Password != encryptedPassword))
                {
                    error = "Incorrect Password";
                    return false;
                }
                else if (user != null && (user.Password == encryptedPassword))
                {
                    return true;
                }
            }
            catch
            {
                error = "Something went wrong! Please try again later.";
                return false;
            }
            return false;
        }

        /// <summary>
        /// Checks and creates filedirectory if missing.
        /// </summary>
        /// <param name="filePath"></param>
        private void CreateIfMissing(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
        }
        /// <summary>
        /// Gets all the registered users.
        /// </summary>
        /// <returns>list of users</returns>
        private List<LoginModel> GetRegisteredUsers()
        {
            List<LoginModel> accounts = new List<LoginModel>();
            try
            {
                if (System.IO.File.Exists(this.config.Value.UsersFilePath))
                {
                    using FileStream fs = new FileStream(this.config.Value.UsersFilePath, FileMode.Open);
                    using StreamReader sr = new StreamReader(fs);
                    string content = sr.ReadToEnd();
                    string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        string[] column = line.Split(',');
                        LoginModel account = new LoginModel()
                        {
                            UserName = column[0],
                            Password = column[1]
                        };
                        accounts.Add(account);
                    }
                }
            }
            catch { }
            return accounts;
        }

        /// <summary>
        /// Get existing user information.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private LoginModel GetExistingUser(string username)
        {
            var registeredUsers = GetRegisteredUsers();
            return registeredUsers.Where(x => x.UserName == username).FirstOrDefault();
        }

    }
}
