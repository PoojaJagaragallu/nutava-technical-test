using System.ComponentModel.DataAnnotations;

namespace Nutava.Test.NumberToWord.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Gets/sets username
        /// </summary>
        [Required]
        
        public string UserName { get; set; }

        /// <summary>
        /// Gets/sets password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
