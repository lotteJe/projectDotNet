using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using KostenBatenTool.Models.AccountViewModels;
using KostenBatenTool.Services;
using KostenBatenTool.Models;
using KostenBatenTool.Models.Domain;
using KostenBatenTool.Models.ManageViewModels;
using MimeKit;
using MimeKit.Utils;

namespace KostenBatenTool.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IArbeidsBemiddelaarRepository _arbeidsBemiddelaarRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
           ILoggerFactory loggerFactory,
            IEmailService emailService, IArbeidsBemiddelaarRepository arbeidsBemiddelaarRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _emailService = emailService;
            _arbeidsBemiddelaarRepository = arbeidsBemiddelaarRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            ArbeidsBemiddelaar a = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(User.Identity.Name);
            return View(new EditViewModel(a));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ArbeidsBemiddelaar a = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(User.Identity.Name);
                a.Naam = model.Naam;
                a.Voornaam = model.Voornaam;
                a.Email = model.Email;
                a.EigenOrganisatie.Naam = model.NaamOrganisatie;
                a.EigenOrganisatie.Straat = model.Straat;
                a.EigenOrganisatie.Huisnummer = model.Huisnummer;
                a.EigenOrganisatie.Postcode = model.Postcode;
                a.EigenOrganisatie.Gemeente = model.Gemeente;
                _arbeidsBemiddelaarRepository.SaveChanges();
                ApplicationUser userApp = await _userManager.GetUserAsync(User);
                userApp.Naam = model.Naam;
                userApp.Voornaam = model.Voornaam;
                await _userManager.UpdateAsync(userApp);

                TempData["message"] = "Je gegevens werden succesvol gewijzigd.";
                return RedirectToAction(nameof(AnalyseController.Index), "Home");
            }
            return View(model);
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ArbeidsBemiddelaar test = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(model.Email);
            if (test == null)
            {
                ModelState.AddModelError("EmailReg", "Uw e-mailadres is onjuist, bent u al geregistreerd?");
            }
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    var user = await _userManager.FindByNameAsync(model.Email);
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError("EmailReg", "Uw wachtwoord is onjuist");
                    return View(model);
                }
            }

            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ArbeidsBemiddelaar test = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(model.Email);
            if (test != null)
            {
                ModelState.AddModelError("EmailGer", "U bent reeds geregistreerd.");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Naam = model.Naam, Gemeente = model.Gemeente, Voornaam = model.Voornaam, Postcode = model.Postcode, Huisnummer = model.Huisnummer, Straat = model.Straat, NaamOrganisatie = model.NaamOrganisatie };
                ArbeidsBemiddelaar arbeidsBemiddelaar = new ArbeidsBemiddelaar(model.Naam, model.Voornaam, model.Email, new Organisatie(model.NaamOrganisatie, model.Straat, model.Huisnummer, model.Postcode, model.Gemeente));
                _arbeidsBemiddelaarRepository.Add(arbeidsBemiddelaar);
                var result = await _userManager.CreateAsync(user, generateRandomPassword());
                if (result.Succeeded)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action(nameof(ResetPassword), "Account",
                        new { userId = user.Id, code = code, email = model.Email }, protocol: HttpContext.Request.Scheme);
                    MimeMessage emailMessage = new MimeMessage();
                    emailMessage.Subject = "Welkom bij KAIROS\' kosten-baten tool!";
                    BodyBuilder builder = new BodyBuilder();
                    builder.HtmlBody = @"<div style='background-color:#9CCD0B;color:white;padding-left:7%;padding-right:7%;padding-top:5%;padding-bottom:5%;font-family:Arial, Helvetica;color:#555555;font-size:14px;'>
                        <div style='background-color:rgba(255,255,255, 0.74);padding:7%; border-radius:5px;'> <img style='text-align:center' src='https://static.wixstatic.com/media/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png/v1/crop/x_2,y_0,w_1257,h_515/fill/w_500,h_205,al_c,usm_0.66_1.00_0.01/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png' alt='Logo Kairos' height='40px'/>
                        <br><br><b>Beste " + $"{model.Voornaam}" + @"</b>
                        <p>Leuk dat je gebruik wil maken van onze tool om werkgevers meer inzicht te geven in de kosten en baten bij het tewerkstellen van personen met een grote afstand tot de arbeidsmarkt.</p>
                        <p>Gelieve je registratie te voltooien via deze <a href='" + $"{callbackUrl}" + @"'>link</a>.</p>
                        <p>Veel succes met het gebruik van onze tool!</p>
                        <p>Wil je meer weten over wie we zijn en wat we doen, surf naar www.hetmomentvooriedereen.be.</p>
                        <p>Hartelijke groet</p>
                        <p>Het team van KAIROS</p>
</div>
                        </div>";
                    emailMessage.Body = builder.ToMessageBody();
                    await _emailService.SendEmailAsync("kairos.paperclip@gmail.com", model.Email, emailMessage);
                    // Comment out following line to prevent a new user automatically logged on.
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return View("RegisterConfirmed");
                }
                AddErrors(result);
            }
            return View(model);
        }
        private string generateRandomPassword()
        {
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[10];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < 10; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }
            return new string(chars);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            ArbeidsBemiddelaar test = _arbeidsBemiddelaarRepository.GetArbeidsBemiddelaar(model.Email);
            if (test == null)
            {
                ModelState.AddModelError("WWVergeten", "Uw e-mailadres werd niet gevonden.");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, email = model.Email }, protocol: HttpContext.Request.Scheme);
                MimeMessage emailMessage = new MimeMessage();
                emailMessage.Subject = "Paswoord KAIROS kosten-baten tool vergeten";
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = @"
                 <div style='background-color:#9CCD0B;color:white;padding-left:7%;padding-right:7%;padding-top:5%;padding-bottom:5%;font-family:Arial, Helvetica;color:#555555;font-size:14px;'>
                        <div style='background-color:rgba(255,255,255, 0.74);padding:7%; border-radius:5px;'> <img src='https://static.wixstatic.com/media/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png/v1/crop/x_2,y_0,w_1257,h_515/fill/w_500,h_205,al_c,usm_0.66_1.00_0.01/192f9b_a49f1a3533c149a2a803ee4ab519ef2e~mv2.png' alt='Logo Kairos' height='40px'/>
                 <br><br><b>Beste " + $"{user.Voornaam}" + @"</b>
                 <p>Je hebt aangegeven dat je jouw paswoord vergeten bent.</p>
                 <p>Via deze <a href='" + $"{callbackUrl}" + @"'>link</a> kan je een nieuw paswoord aanmaken.</p>
                 <p>Hartelijke groet</p>
                 <p>Het team van KAIROS</p>
</div>
</div>";
                emailMessage.Body = builder.ToMessageBody();
                await _emailService.SendEmailAsync("kairos.paperclip@gmail.com", model.Email, emailMessage);

                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string email = null)
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            MimeMessage message = new MimeMessage("Your security code is: " + code);
            if (model.SelectedProvider == "Email")
            {

                await _emailService.SendEmailAsync("kairos.paperclip@gmail.com", await _userManager.GetEmailAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        #endregion
    }
}
