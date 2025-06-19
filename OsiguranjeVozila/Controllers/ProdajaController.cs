 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;


namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator, Zaposleni")]
    public class ProdajaController : Controller
    {
        private readonly IProdajaRepository prodajaRepository;
        private readonly IKlijentRepository klijentRepository;
        private readonly IVoziloRepository voziloRepository;
        private readonly IPolisaRepository polisaRepository;

        public ProdajaController(IProdajaRepository prodajaRepository, 
            IKlijentRepository klijentRepository,
            IVoziloRepository voziloRepository, IPolisaRepository polisaRepository)
        {
            this.prodajaRepository = prodajaRepository;
            this.klijentRepository = klijentRepository;
            this.voziloRepository = voziloRepository;
            this.polisaRepository = polisaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()  //puni dropdown-ove klijenti, polise i vozila na stranici za dodavanje nove prodaje
        {
            if (Request.Query["reset"].ToString() == "true")
            {
                HttpContext.Session.Remove("KlijentId");
                HttpContext.Session.Remove("VoziloId");

            }
            var klijentId = HttpContext.Session.GetString("KlijentId");
            var voziloId = HttpContext.Session.GetString("VoziloId");


            if (string.IsNullOrEmpty(klijentId))
            {
                return RedirectToAction("List", "Klijent", new { next = "Prodaja" });
            }

            var klijentResult = await klijentRepository.GetAllASync();

            if (string.IsNullOrEmpty(voziloId))
            {
                return RedirectToAction("List", "Vozilo", new { next = "Prodaja" });
            }

            var voziloResult = await voziloRepository.GetAllAsync();
            var polisaResult = await polisaRepository.GetAllAsync();


            ViewBag.KlijentList = klijentResult.OrderBy(x => x.Ime).Select(x =>
            {
                return new SelectListItem($"{x.Ime} {x.Prezime} {x.Email}", x.Id.ToString(),
                    x.Id.ToString() == klijentId);
            });


            ViewBag.VoziloList = voziloResult.OrderBy(x => x.Marka).Select(x =>
            {
                return new SelectListItem($"{x.Marka} {x.Model} {x.RegistarskaOznaka}", x.Id.ToString(),
                    x.Id.ToString() == voziloId);
            });

            ViewBag.PolisaList = polisaResult.OrderBy(x => x.Naziv).Select(x =>
            {
                return new SelectListItem($"{x.Naziv}", x.Id.ToString());
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProdajaViewModel addProdajaViewModel) //kreira novu prodaju
        {
            var polisa = await polisaRepository.GetAsync(addProdajaViewModel.PolisaId);
            var vozilo = await voziloRepository.GetAsync(addProdajaViewModel.VoziloId);
            var prodaje = await prodajaRepository.GetAllAsync();

            var existingProdaja = (await prodajaRepository.GetAllAsync()).FirstOrDefault(
                p=>p.KlijentId == addProdajaViewModel.KlijentId&&
                p.VoziloId == addProdajaViewModel.VoziloId&&
                p.PolisaId== addProdajaViewModel.PolisaId);

            if(existingProdaja != null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, 
                    "Prodaja sa istim klijentom, vozilom i polisom već postoji.");

                await PopulateDropdowns(
                    addProdajaViewModel.KlijentId.ToString(),
                    addProdajaViewModel.VoziloId.ToString(),
                    addProdajaViewModel.PolisaId.ToString());

                return View(addProdajaViewModel);
            }

            var existingKlijentVozilo = prodaje.FirstOrDefault
                (x => x.VoziloId == addProdajaViewModel.VoziloId &&
                x.KlijentId != addProdajaViewModel.KlijentId);

            if (existingKlijentVozilo != null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                    "Vozilo je već osigurano na drugo lice");

                await PopulateDropdowns(
                    addProdajaViewModel.KlijentId.ToString(),
                    addProdajaViewModel.VoziloId.ToString(),
                    addProdajaViewModel.PolisaId.ToString());

                return View(addProdajaViewModel);
            }


            ProdajaPolise novaProdaja = new ProdajaPolise
            {
                DatumKupovine = addProdajaViewModel.DatumKupovine,
                DatumIsteka = addProdajaViewModel.DatumKupovine.AddMonths(polisa.Trajanje),
                Cijena = polisa.NominalniIznos - addProdajaViewModel.UcesceUSteti,
                UcesceUSteti = addProdajaViewModel.UcesceUSteti,
                VoziloId = addProdajaViewModel.VoziloId,
                KlijentId = addProdajaViewModel.KlijentId,
                PolisaId = addProdajaViewModel.PolisaId,
            };

            await prodajaRepository.AddAsync(novaProdaja);

            return RedirectToAction("List", "Prodaja");
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection,
            string? datumOd, string? datumDo, int pageSize = 10, int pageNumber = 1) //vraca listu svih prodaja
        {
            var totalRecords = await prodajaRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            var prodaje = await prodajaRepository.GetAllAsync(searchQuery, sortBy, sortDirection, datumOd,
                datumDo, pageNumber, pageSize);


            ViewBag.DatumOd = datumOd;
            ViewBag.DatumDo = datumDo;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;

            return View(prodaje);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) //pronalazi prodaju po id-ju i salje na stranicu edit
        {
            var prodaja = await prodajaRepository.GetAsync(id);


            if (prodaja == null)
            {
                return NotFound();
            }
            var editProdajaViewModel = new EditProdajaViewModel
            {
                Id = prodaja.Id,
                DatumIsteka = prodaja.DatumIsteka,
                DatumKupovine = prodaja.DatumKupovine,
                Cijena = prodaja.Cijena,
                UcesceUSteti = prodaja.UcesceUSteti,
                KlijentId = prodaja.KlijentId,
                VoziloId = prodaja.VoziloId,
                PolisaId = prodaja.PolisaId
            };

            var voziloResult = await voziloRepository.GetAllAsync();
            var polisaResult = await polisaRepository.GetAllAsync();

            var klijentResult = await klijentRepository.GetAllASync();


            ViewBag.KlijentList = klijentResult.OrderBy(x => x.Ime).Select(x =>
            {
                return new SelectListItem($"{x.Ime} {x.Prezime} {x.Email}", x.Id.ToString(),
                    x.Id.ToString() == editProdajaViewModel.KlijentId.ToString());
            });


            ViewBag.VoziloList = voziloResult.OrderBy(x => x.Marka).Select(x =>
            {
                return new SelectListItem($"{x.Marka} {x.Model} {x.RegistarskaOznaka}", x.Id.ToString(),
                    x.Id.ToString() == editProdajaViewModel.VoziloId.ToString());
            });

            ViewBag.PolisaList = polisaResult.OrderBy(x => x.Naziv).Select(x =>
            {
                return new SelectListItem($"{x.Naziv}", x.Id.ToString(),
                    x.Id.ToString() == editProdajaViewModel.PolisaId.ToString());
            });

            return View(editProdajaViewModel);
        } 

        [HttpPost]
        public async Task<IActionResult> Edit(EditProdajaViewModel editProdajaViewModel) //azurira prodaju
        {
            
            var vozilo = await voziloRepository.GetAsync(editProdajaViewModel.VoziloId);
            var prodaje = await prodajaRepository.GetAllAsync();

            

            var existingKlijentVozilo = prodaje.FirstOrDefault
                (x => x.VoziloId == editProdajaViewModel.VoziloId &&
                x.KlijentId != editProdajaViewModel.KlijentId);

            if (existingKlijentVozilo != null)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty,
                    "Vozilo je već osigurano na drugo lice");

                await PopulateDropdowns(
                    editProdajaViewModel.KlijentId.ToString(),
                    editProdajaViewModel.VoziloId.ToString(),
                    editProdajaViewModel.PolisaId.ToString());

                return View(editProdajaViewModel);
            }

            var prodaja = new ProdajaPolise
            {
                Id = editProdajaViewModel.Id,
                DatumIsteka = editProdajaViewModel.DatumIsteka,
                DatumKupovine = editProdajaViewModel.DatumKupovine,
                UcesceUSteti = editProdajaViewModel.UcesceUSteti,
                Cijena = editProdajaViewModel.Cijena,
                KlijentId = editProdajaViewModel.KlijentId,
                VoziloId = editProdajaViewModel.VoziloId,
                PolisaId = editProdajaViewModel.PolisaId
            };



            var izmijenjenaProdaja = await prodajaRepository.UpdateAsync(prodaja);

            if (izmijenjenaProdaja != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditProdajaViewModel editProdajaViewModel) //brise prodaju
        {
            var prodaja = await prodajaRepository.DeleteAsync(editProdajaViewModel.Id);

            if (prodaja != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editProdajaViewModel.Id });

        }

        private async Task PopulateDropdowns(string? klijentId = null, string? voziloId = null,
            string? polisaId = null) //popunjava dropdown-ove za klijente,polise i vozila - linija 241
        {
            var klijenti = await klijentRepository.GetAllASync();
            var vozila = await voziloRepository.GetAllAsync();
            var polise = await polisaRepository.GetAllAsync();

            ViewBag.KlijentList = klijenti.OrderBy(x => x.Ime).Select(x =>
                new SelectListItem($"{x.Ime} {x.Prezime} {x.Email}", x.Id.ToString(), x.Id.ToString() == klijentId));

            ViewBag.VoziloList = vozila.OrderBy(x => x.Marka).Select(x =>
                new SelectListItem($"{x.Marka} {x.Model} {x.RegistarskaOznaka}", x.Id.ToString(), x.Id.ToString() == voziloId));

            ViewBag.PolisaList = polise.OrderBy(x => x.Naziv).Select(x =>
                new SelectListItem(x.Naziv, x.Id.ToString(), x.Id.ToString() == polisaId));
        }
    }
}







