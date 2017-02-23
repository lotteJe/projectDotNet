using System.ComponentModel.DataAnnotations;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
       public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Wachtwoord onthouden")]
        public bool RememberMe { get; set; }
    }
}
