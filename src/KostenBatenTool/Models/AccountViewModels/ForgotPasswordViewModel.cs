using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required (ErrorMessage="E-mailadres is verplicht.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
