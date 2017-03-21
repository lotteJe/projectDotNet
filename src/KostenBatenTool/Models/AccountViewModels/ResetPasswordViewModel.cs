using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Het wachtwoord moet minstens 6 tekens bevatten.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Het wachtwoord en bevestig wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
