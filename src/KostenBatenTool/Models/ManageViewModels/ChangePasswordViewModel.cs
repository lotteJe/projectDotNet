using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Huidig wachtwoord is verplicht.")]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nieuw wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Wachtwoord moet minstens 6 tekens bevatten waaronder één cijfer.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw Wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "Nieuwe en bevestig wachtwoord komen niet overeen")]
        public string ConfirmPassword { get; set; }
    }
}
