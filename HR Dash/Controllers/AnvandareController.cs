using HR_Dash.Models;
using HR_Dash.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HR_Dash.Controllers
{
    //Controller som hanterar inloggning, utloggning och lösenordsbyte
    public class AnvandareController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //Konstruktor som tar in SignInManager och UserManager för att hantera autentisering och användare
        public AnvandareController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //Visar inloggningssidan
        public IActionResult LoggaIn()
        {
            return View();
        }

        //Inloggning med användarnamn och lösenord
        [HttpPost]
        public async Task<IActionResult> LoggaIn(LoginViewModel model)
        {
            //Om modellen är giltig, försök logga in användaren
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Losenord, model.KomIhag, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ogiltig inloggning. Försök igen.");
                    return View(model);
                }
            }
            //Om modellen inte är giltig, visa inloggningssida igen
            return View(model);
        }

        //Visar vy för att ändra lösenord
        [HttpGet]
        [Authorize]
        public IActionResult AndraLosenord()
        {
            return View();
        }

        //Hantera ändring av lösenord
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AndraLosenord(AndraLosenordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Försök att ändra lösenordet
                    var result = await _userManager.ChangePasswordAsync(user, model.NuvarandeLosenord, model.NyttLosenord);
                    if (result.Succeeded)
                    {
                        // Om lösenordet ändrades, uppdatera användarens status
                        await _signInManager.RefreshSignInAsync(user);
                        ViewBag.Message = "Ditt lösenord har ändrats.";
                        return View(model);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            //Om modellen inte är giltig, visa ändra lösenord igen
            return View(model);
        }

        //Logga ut användaren
        public async Task<IActionResult> LoggaUt()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
