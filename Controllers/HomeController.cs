using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nutava.Test.NumberToWord.Helpers;
using Nutava.Test.NumberToWord.Models;
using System;
using System.Diagnostics;
using System.Text;

namespace Nutava.Test.NumberToWord.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IOptions<AppSettings> _config;

        public HomeController(IOptions<AppSettings> config)
        {
            _config = config;
        }

        /// <summary>
        /// Gets and displays default index view.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            ViewBag.MaxInput = _config.Value.MaxInput;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Gets and displays input number in words.
        /// </summary>
        /// <param name="inputAmount"></param>
        /// <returns>string of words</returns>
        [HttpPost]
        public ActionResult GeInputNumberInWords(string inputAmount)
        {
            if (inputAmount == null)
            {
                throw new ArgumentNullException(nameof(inputAmount));
            }
            StringBuilder words = new StringBuilder();
            try
            {
                int decimalPlace = inputAmount.IndexOf(".");
                string numberInput = decimalPlace > 0 ? inputAmount.Substring(0, decimalPlace) : inputAmount;
                string decimalInput = decimalPlace > 0 ? inputAmount.Substring(decimalPlace + 1) : string.Empty;
                var result = NumberToWordConvertionHelper.ConvertNumberToWords(numberInput);
                result = string.IsNullOrEmpty(result) ? "Zero" : result;
                words.Append(string.Format("{0}{1}", result, " dollars "));
                if (!string.IsNullOrEmpty(decimalInput))
                {
                    var decimalResult = NumberToWordConvertionHelper.ConvertNumberToWords(decimalInput);
                    decimalResult = string.IsNullOrEmpty(decimalResult) ? "Zero" : decimalResult;
                    if(!decimalResult.Equals("Zero"))
                    words.Append(string.Format("{0}{1}{2}", "and ", decimalResult, " cents"));
                }
                words.Append(" only.");
            }
            catch { throw; }
            return Json(words.ToString());
        }
    }
}
