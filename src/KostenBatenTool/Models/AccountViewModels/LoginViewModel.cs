using System.ComponentModel.DataAnnotations;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
