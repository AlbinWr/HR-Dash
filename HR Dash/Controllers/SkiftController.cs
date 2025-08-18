using HR_Dash.Data;
using HR_Dash.Models;
using HR_Dash.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HR_Dash.Controllers
{
    //Controller som hanterar skift för anställda
    [Authorize]
    public class SkiftController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkiftController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Formulär för att skapa ett nytt skift
        [HttpGet]
        public IActionResult Create()
        {
            //Förfyller dagens datum och hämtar anställda för dropdown-lista
            var model = new SkapaSkiftViewModel
            {
                StartTid = DateTime.Today,
                SlutTid = DateTime.Today,
                AnstalldaList = _context.Anstallda
                    .Select(a => new SelectListItem
                    {
                        Value = a.AnstalldId.ToString(),
                        Text = a.AnstalldName
                    }).ToList()
            };
            return View(model);
        }

        //Skapar ett nytt skift
        [HttpPost]
        public async Task<IActionResult> Create(SkapaSkiftViewModel model)
        {
            // Validera att sluttid är efter starttid
            if (model.SlutTid <= model.StartTid)
            {
                ModelState.AddModelError("SlutTid", "Sluttid måste vara efter starttid.");
            }
            if (ModelState.IsValid)
            {
                // Skapa och spara nytt skift
                var skift = new Skift
                {
                    AnstalldId = model.AnstalldId,
                    StartTid = model.StartTid,
                    SlutTid = model.SlutTid
                };
                _context.Skift.Add(skift);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Skiftet har skapats!";
                return RedirectToAction("Index", "Home");
            }
            // Om model är ogiltig, återgå till vyn med felmeddelanden och fyll i dropdown-listan igen
            model.AnstalldaList = _context.Anstallda
                .Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList();
            return View(model);
        }

        //Visar anställdas skift
        public async Task<IActionResult> MinaSkift()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var anstalld = await _context.Anstallda
                .FirstOrDefaultAsync(anstalld => anstalld.ApplicationUserId == currentUser.Id);

            if (anstalld == null)
            {
                return NotFound("Du är inte kopplad till någon anställd.");
            }

            //Hämtar alla skift för den anställde, sorterade efter starttid
            var skift = await _context.Skift
                .Where(skift => skift.AnstalldId == anstalld.AnstalldId)
                .OrderBy(skift => skift.StartTid)
                .ToListAsync();

            return View(skift);
        }
    }
}
