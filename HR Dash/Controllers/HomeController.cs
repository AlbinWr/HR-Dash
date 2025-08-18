using HR_Dash.Data;
using HR_Dash.Models;
using HR_Dash.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HR_Dash.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // Startsidan som visar nästa skift och kommande möten för anställda
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("LoggaIn", "Anvandare");
            }

            var anstalld = await _context.Anstallda.FirstOrDefaultAsync(a => a.ApplicationUserId == user.Id);
            if (anstalld == null)
            {
                return RedirectToAction("LoggaIn", "Anvandare");
            }

            //Hämtar nästa skift för den anställde
            var nastaSkift = await _context.Skift
               .Where(skift => skift.AnstalldId == anstalld.AnstalldId && skift.StartTid >= DateTime.Now)
               .OrderBy(skift => skift.StartTid)
               .FirstOrDefaultAsync();

            //Hämtar kommande möten för den anställde (5 stycken max)
            var kommandeMoten = await _context.MoteAnstallda
                .Where(mote => mote.Mote.StartTid >= DateTime.Now && mote.AnstalldId == anstalld.AnstalldId)
                .Select(mote => new StartsidaViewModel.MoteInfo
                {
                    Titel = mote.Mote.Titel,
                    StartTid = mote.Mote.StartTid,
                    Plats = mote.Mote.Plats
                })
                .OrderBy(mote => mote.StartTid)
                .Take(5)
                .ToListAsync();

            var viewModel = new StartsidaViewModel
            {
                NastaSkift = nastaSkift,
                KommandeMoten = kommandeMoten
            };

            return View(viewModel);
        }

        //standard vyer.
        [Authorize(Roles = "Admin, Anstalld")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
