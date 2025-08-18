using HR_Dash.Data;
using HR_Dash.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HR_Dash.Models.ViewModels;

namespace HR_Dash.Controllers
{
    //Controller som hanterar administration av anställda
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Visar alla anställda och hanterar sökning
        [HttpGet]
        public IActionResult Index(string? sokAnstalld)
        {
            var anstallda = _context.Anstallda.AsQueryable();

            if (sokAnstalld != null) 
            {
                //Sök anställd
                anstallda = anstallda
                    .Where(a => a.AnstalldName.ToLower().Contains(sokAnstalld));
                return View(anstallda);
            }
            

            ViewData["Filter"] = sokAnstalld;

            return View(anstallda.ToList());
        }

        //formulär för att registrera ny anställd
        [HttpGet]
        public IActionResult RegistreraAnstalld()
        {
            return View();
        }

        //Registrera ny anställd
        [HttpPost]
        public async Task<IActionResult> RegistreraAnstalld(RegistreraViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Losenord);
                if (result.Succeeded)
                {
                    //Om användaren är en manager, tilldela rollen Manager
                    if (model.Manager)
                    {
                        await _userManager.AddToRoleAsync(user, "Manager");
                        await _userManager.AddToRoleAsync(user, "Anstalld");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Anstalld");
                    }

                    //Skapa anställd
                    var anstalld = new Anstalld
                    {
                        AnstalldName = model.Namn,
                        AnstallningDatum = model.AnstallningDatum,
                        Email = model.Email,
                        ApplicationUserId = user.Id
                    };
                    _context.Anstallda.Add(anstalld);

                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Användaren har skapats.";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        //Ta bort anställd och användarkonto
        [HttpGet]
        public async Task<IActionResult> TabortAnstalld(int id)
        {
            var anstalld = await _context.Anstallda.FindAsync(id);
            if (anstalld == null)
            {
                return NotFound();
                
            }
            _context.Anstallda.Remove(anstalld);

            //Ta bort användarkontot
            var user = await _userManager.FindByIdAsync(anstalld.ApplicationUserId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin");
        }

        //Formnulär för att redigera anställd
        [HttpGet]
        public async Task<IActionResult> RedigeraAnstalld(int id)
        {
            var anstalld = await _context.Anstallda.FindAsync(id);
            if (anstalld == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(anstalld.ApplicationUserId);
            if (user == null)
            {
                return NotFound();
            }

            var checkManager = await _userManager.IsInRoleAsync(user, "Manager");

            var model = new RedigeraAnstalldViewModel
            {
                AnstalldId = anstalld.AnstalldId,
                Namn = anstalld.AnstalldName,
                Email = anstalld.Email,
                AnstallningDatum = anstalld.AnstallningDatum,
                Manager = checkManager
            };
            return View(model);
        }

        //Uppdatera anställd information
        [HttpPost]
        public async Task<IActionResult> RedigeraAnstalld(RedigeraAnstalldViewModel model)
        {
            if (ModelState.IsValid)
            {
                var anstalld = await _context.Anstallda.FindAsync(model.AnstalldId);
                if (anstalld == null)
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(anstalld.ApplicationUserId);
                if (user == null)
                {
                    return NotFound();
                }

                //Uppdatera anställd
                anstalld.AnstalldName = model.Namn;
                anstalld.Email = model.Email;
                anstalld.AnstallningDatum = model.AnstallningDatum;
                _context.Anstallda.Update(anstalld);

                //uppdatera manager roll
                var checkManager = await _userManager.IsInRoleAsync(user, "Manager");
                if (model.Manager && !checkManager)
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }
                else if (!model.Manager && checkManager)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Manager");
                }


                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View(model);
            }

        }
    }
}
