using HR_Dash.Data;
using HR_Dash.Models;
using HR_Dash.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HR_Dash.Controllers
{
    //Controller som hanterar anställdas profil och redigering
    [Authorize]
    public class AnstalldController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AnstalldController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Visar anställdas profil
        [Authorize(Roles = "Admin, Anstalld")]
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            //Hämtar användarens ID och letar upp den anställda i databasen
            var userId = _userManager.GetUserId(User);
            var anstalld = await _context.Anstallda.FirstOrDefaultAsync(a => a.ApplicationUserId == userId);
            if (anstalld != null)
            {
                return View(anstalld);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RedigeraProfil()
        {
            var userId = _userManager.GetUserId(User);
            var anstalld = await _context.Anstallda.FirstOrDefaultAsync(a => a.ApplicationUserId == userId);
            if (anstalld != null)
            {
                var model = new RedigeraProfilViewModel
                {
                    AnstalldId = anstalld.AnstalldId,
                    AnstalldName = anstalld.AnstalldName,
                    Email = anstalld.Email,
                    Telefonnummer = anstalld.Telefonnummer
                };
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        //Hantera redigering av anställdas profil
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RedigeraProfil(RedigeraProfilViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Hämtar den anställda från databasen, och uppdaterar fälten
            var anstalld = await _context.Anstallda.FindAsync(model.AnstalldId);
            if (anstalld != null)
            {
                anstalld.AnstalldName = model.AnstalldName;
                anstalld.Email = model.Email;
                anstalld.Telefonnummer = model.Telefonnummer;

                await _context.SaveChangesAsync();

                return RedirectToAction("Profil");
            }
            return View(model);
        }
    }
}
