using HR_Dash.Data;
using HR_Dash.Models;
using HR_Dash.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Dash.Controllers
{
    [Authorize]
    public class MoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Genererar en lista med starttider för möten
        private List<SelectListItem> GenereraStartTid(TimeSpan? valdTid = null)
        {
            var lista = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Välj starttid...", Disabled = true, Selected = !valdTid.HasValue }
            };

            // Skapar en lista med starttider från 08:00 till 16:00 med 15 minuters intervall
            for (var t = new TimeSpan(8, 0, 0); t <= new TimeSpan(16, 0, 0); t = t.Add(TimeSpan.FromMinutes(15)))
            {

                lista.Add(new SelectListItem
                {
                    Value = t.ToString(),
                    Text = t.ToString(@"hh\:mm"),
                    Selected = valdTid.HasValue && t == valdTid.Value
                });
            }

            return lista;
        }

        //Genererar en lista med längd på möten
        private List<SelectListItem> GenereraTidLangd()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "15", Text = "15 minuter" },
                new SelectListItem { Value = "30", Text = "30 minuter" },
                new SelectListItem { Value = "45", Text = "45 minuter" },
                new SelectListItem { Value = "60", Text = "60 minuter" }
            };
        }

        // Index-sida som visar alla möten för den inloggade användaren
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var anstalld = await _context.Anstallda
                .FirstOrDefaultAsync(a => a.ApplicationUserId == currentUser.Id);

            // Hämta alla möten där användaren är deltagare
            var moten = await _context.Moten
                .Where(mote =>
                    mote.ManagerId == currentUser.Id ||
                    (anstalld != null && mote.MoteAnstallda.Any(ma => ma.AnstalldId == anstalld.AnstalldId)))
                .Select(mote => new MotenViewModel
                {
                    MoteId = mote.MoteId,
                    Titel = mote.Titel,
                    Plats = mote.Plats,
                    StartTid = mote.StartTid,
                    SlutTid = mote.SlutTid,
                    Deltagare = mote.MoteAnstallda.Select(ma => ma.Anstalld.AnstalldName).ToList(),
                    IsManager = mote.ManagerId == currentUser.Id
                })
                .ToListAsync();

            return View(moten);
        }

        // Skapa nytt möte
        [HttpGet]
        public IActionResult Create()
        {
            var model = new SkapaMoteViewModel
            {
                //Hämtar alla anställda för att fylla i dropdown-listan
                AnstalldaList = _context.Anstallda.Select(anstalld => new SelectListItem
                {
                    Value = anstalld.AnstalldId.ToString(),
                    Text = anstalld.AnstalldName
                }).ToList(),
                LangdAlternativ = GenereraTidLangd(),
                StartTidAlternativ = GenereraStartTid(),
            };
            return View(model);
        }

        // Skapa möte
        [HttpPost]
        public async Task<IActionResult> Create(SkapaMoteViewModel model)
        {
            // Validera model
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return Unauthorized();
                }


                var start = model.Datum.Date.Add(model.Tid);
                var slut = start.AddMinutes(model.MoteLangd);
                var mote = new Mote
                {
                    Titel = model.Titel,
                    Plats = model.Plats,
                    Beskrivning = model.Beskrivning,
                    StartTid = start,
                    SlutTid = slut,
                    ManagerId = currentUser.Id,
                    MoteAnstallda = model.ValdaAnstallda
                        .Select(anstalldId => new MoteAnstalld
                        {
                            AnstalldId = anstalldId
                        }).ToList()
                };
                _context.Moten.Add(mote);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Möte har skapats.";
                return RedirectToAction("Create", "Mote");
            }
            else
            {
                model.AnstalldaList = _context.Anstallda.Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList();
                return View(model);
            }
        }

        //Hämtar möte att redigera
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var mote = await _context.Moten
                .Include(m => m.MoteAnstallda)
                .ThenInclude(ma => ma.Anstalld)
                .FirstOrDefaultAsync(m => m.MoteId == id);
            if (mote == null)
            {
                return NotFound();
            }

            var model = new SkapaMoteViewModel
            {
                MoteId = mote.MoteId,
                Titel = mote.Titel,
                Plats = mote.Plats,
                Beskrivning = mote.Beskrivning,
                Datum = mote.StartTid.Date,
                Tid = mote.StartTid.TimeOfDay,
                MoteLangd = (int)(mote.SlutTid - mote.StartTid).TotalMinutes,
                ValdaAnstallda = mote.MoteAnstallda.Select(ma => ma.AnstalldId).ToList(),

                AnstalldaList = _context.Anstallda.Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList(),
                StartTidAlternativ = new List<SelectListItem>()
            };
            model.StartTidAlternativ = GenereraStartTid(model.Tid);
            model.LangdAlternativ = GenereraTidLangd();
            return View("Create",model);
        }

        //Uppdaterar möte med de nya värdena
        [HttpPost]
        public async Task<IActionResult> Edit(SkapaMoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mote = await _context.Moten
                    .Include(m => m.MoteAnstallda)
                    .FirstOrDefaultAsync(m => m.MoteId == model.MoteId);
                if (mote == null)
                {
                    return NotFound();
                }
                mote.Titel = model.Titel;
                mote.Plats = model.Plats;
                mote.Beskrivning = model.Beskrivning;
                mote.StartTid = model.Datum.Date.Add(model.Tid);
                mote.SlutTid = mote.StartTid.AddMinutes(model.MoteLangd);
                // Uppdatera deltagare
                mote.MoteAnstallda.Clear();
                foreach (var anstalldId in model.ValdaAnstallda)
                {
                    mote.MoteAnstallda.Add(new MoteAnstalld { AnstalldId = anstalldId });
                }
                _context.Moten.Update(mote);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Mötet har uppdaterats.";

                return RedirectToAction("Index");
            }
            else
            {
                model.AnstalldaList = _context.Anstallda.Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList();

                model.StartTidAlternativ = GenereraStartTid();
                model.LangdAlternativ = GenereraTidLangd();

                return View("Create",model);
            }
        }

        // Tar bort ett möte
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var mote = await _context.Moten.FindAsync(id);
            if (mote == null)
            {
                return NotFound();
            }
            _context.Moten.Remove(mote);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mötet har tagits bort.";
            return RedirectToAction("Index");
        }

        // Visa en anställds möten
        public async Task<IActionResult> MinaMoten()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var anstalld = await _context.Anstallda
                .FirstOrDefaultAsync(anstalld => anstalld.ApplicationUserId == currentUser.Id);

            if (anstalld == null)
            {
                return NotFound("Du är inte kopplad till någon anställd.");
            }

            // Hämta möten där anställd är deltagare
            var minaMoten = await _context.Moten
                .Where(mote => mote.MoteAnstallda.Any(ma => ma.AnstalldId == anstalld.AnstalldId))
                .OrderBy(mote => mote.StartTid)
                .Select(mote => new MotenViewModel
                {
                    MoteId = mote.MoteId,
                    Titel = mote.Titel,
                    Plats = mote.Plats,
                    StartTid = mote.StartTid,
                    SlutTid = mote.SlutTid,
                    Deltagare = mote.MoteAnstallda.Select(ma => ma.Anstalld.AnstalldName).ToList(),
                    IsManager = mote.ManagerId == currentUser.Id
                })
                .ToListAsync();

            return View(minaMoten);
        }
    }
}
