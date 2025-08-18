using HR_Dash.Data;
using HR_Dash.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace HR_Dash.Controllers
{
    //Controller som hanterar rullande schema för anställda
    [Authorize(Roles = "Manager")]
    public class RullandeSchemaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RullandeSchemaController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Genererar skift baserat på anställds schema och startdatum
        private List<Skift> GenereraSkift(AnstalldSchema anstalldSchema, DateTime genereraTillDatum)
        {
            var skiftList = new List<Skift>();
            var schema = anstalldSchema.RullandeSchema;
            var monster = schema.MonsterString;
            var datum = anstalldSchema.Startdatum;

            var regex = new Regex(@"([JL])(\d+)");
            var matches = regex.Matches(monster);

            if (matches.Count == 0)
            {
                throw new InvalidOperationException("Mönster är ogiltigt.");
            }

            while (datum < genereraTillDatum)
            {
                foreach (Match jobbTyp in matches)
                {
                    var typ = jobbTyp.Groups[1].Value;
                    var antalDagar = int.Parse(jobbTyp.Groups[2].Value);

                    if (typ == "J")
                    {
                        for (int i = 0; i < antalDagar; i++)
                        {
                            if (datum >= genereraTillDatum)
                            {
                                break;
                            }

                            var startTid = datum.Add(schema.StartTid);
                            var slutTid = startTid.AddHours(schema.SkiftTimmar).AddMinutes(schema.SkiftMinuter);

                            skiftList.Add(new Skift
                            {
                                AnstalldId = anstalldSchema.AnstalldId,
                                StartTid = startTid,
                                SlutTid = slutTid,
                                AnstalldSchemaId = anstalldSchema.Id,
                                RullandeSchemaId = schema.IdSchema
                            });

                            datum = datum.AddDays(1);
                        }
                    }
                    else if (typ == "L")
                    {
                        datum = datum.AddDays(antalDagar);
                    }

                    if (datum >= genereraTillDatum)
                    {
                        break;
                    }
                }
            }
            return skiftList;
        }

        //Hämtar alla scheman och deras anställda
        public async Task<IActionResult> Index()
        {
            var scheman = await _context.RullandeScheman
                .Include(s => s.Anstallda)
                .ToListAsync();

            return View(scheman);
        }

        //Formulär för att skapa ett nytt schema
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Sparar det nya schemat
        [HttpPost]
        public async Task<IActionResult> Create(RullandeSchema schema)
        {
            // Validering: skiftet får inte vara noll minuter totalt
            var totalMinuter = schema.SkiftTimmar * 60 + schema.SkiftMinuter;
            if (totalMinuter <= 0)
            {
                ModelState.AddModelError("SkiftTimmar", "Skiftets längd måste vara minst 1 minut.");
                ModelState.AddModelError("SkiftMinuter", "Skiftets längd måste vara minst 1 minut.");
            }
            // Validering: mönster måste innehålla minst ett jobb
            if (string.IsNullOrWhiteSpace(schema.MonsterString) || !Regex.IsMatch(schema.MonsterString, @"[JL]\d+"))
            {
                ModelState.AddModelError("MonsterString", "Mönstret måste innehålla minst ett jobb (J eller L) med ett positivt heltal.");
            }

            if (ModelState.IsValid)
            {
                _context.RullandeScheman.Add(schema);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Schemat har sparats.";
                return RedirectToAction("Index");
            }
            return View(schema);
        }

        //Formulär för att redigera ett befintligt schema
        [HttpGet]
        public IActionResult TilldelaSchema(int id)
        {
            var schema = _context.RullandeScheman.FirstOrDefault(x => x.IdSchema == id);
            if (schema == null)
            {
                return NotFound();
            }

            var model = new TilldelaSchemaViewModel
            {
                IdRullandeSchema = schema.IdSchema,
                NamnSkift = schema.NamnSkift,
                AnstalldaLista = _context.Anstallda.Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList()
            };

            return View(model);
        }

        //Tilldelar anställda till schemat och genererar skift
        [HttpPost]
        public async Task<IActionResult> TilldelaSchema(TilldelaSchemaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AnstalldaLista = _context.Anstallda.Select(a => new SelectListItem
                {
                    Value = a.AnstalldId.ToString(),
                    Text = a.AnstalldName
                }).ToList();

                return View(model);
            }


            var schema = await _context.RullandeScheman.FindAsync(model.IdRullandeSchema);
            if (schema == null)
            {
                return NotFound();
            }

            var nyaTilldelningar = new List<AnstalldSchema>();
            foreach (var anstalldId in model.ValdaAnstallda)
            {
                var tilldelning = new AnstalldSchema
                {
                    AnstalldId = anstalldId,
                    RullandeSchemaId = schema.IdSchema,
                    Startdatum = model.Startdatum
                };
                nyaTilldelningar.Add(tilldelning);
            }

            _context.AnstalldSchema.AddRange(nyaTilldelningar);
            await _context.SaveChangesAsync();

            // Generera skift för varje ny tilldelning
            var tillDatum = DateTime.Today.AddMonths(3);
            foreach (var tilldelning in nyaTilldelningar)
            {
                var skift = GenereraSkift(tilldelning, tillDatum);
                _context.Skift.AddRange(skift);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Anställda tilldelades schemat.";
            return RedirectToAction("Index");
        }

        //Visa detaljer för ett schema och vilka anställda som är tilldelade
        [HttpGet]
        public async Task<IActionResult> Visa(int id)
        {
            var schema = await _context.RullandeScheman
                .Include(s => s.Anstallda)
                .ThenInclude(a => a.Anstalld)
                .FirstOrDefaultAsync(s => s.IdSchema == id);

            if (schema == null) return NotFound();

            return View(schema);
        }

        //Ta bort en anställd från schemat
        [HttpPost]
        public async Task<IActionResult> TaBortAnstalldSchema(int id)
        {
            var skift = await _context.AnstalldSchema
                .Include(s => s.RullandeSchema)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skift == null)
            {
                return NotFound();
            }

            // Hämta alla skift för den anställde i det aktuella schemat
            var skiftAttTaBort = await _context.Skift
                .Where(s => s.AnstalldId == skift.AnstalldId && s.StartTid >= skift.Startdatum)
                .ToListAsync();

            _context.Skift.RemoveRange(skiftAttTaBort);
            _context.AnstalldSchema.Remove(skift);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Anställd togs bort från schemat.";
            return RedirectToAction("Visa", new { id = skift.RullandeSchemaId });
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var schema = await _context.RullandeScheman
                .Include(r => r.Anstallda)
                .ThenInclude(a => a.Anstalld)
                .FirstOrDefaultAsync(r => r.IdSchema == id);

            if (schema == null)
            {
                return NotFound();
            }

            // Ta bort eventuella tilldelade skift kopplade till detta schema
            var skift = await _context.Skift
                .Where(skift => skift.RullandeSchemaId == id)
                .ToListAsync();
            _context.Skift.RemoveRange(skift);

            // Ta bort tilldelade anställda (schema-tilldelningar)
            _context.AnstalldSchema.RemoveRange(schema.Anstallda);

            // Ta bort själva schemat
            _context.RullandeScheman.Remove(schema);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Schemat har tagits bort.";
            return RedirectToAction("Index");
        }


    }
}
