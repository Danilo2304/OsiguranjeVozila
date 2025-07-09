using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator, Zaposleni")]
    public class VoziloController : Controller
    {
        private readonly IVoziloRepository voziloRepository;
        private readonly IProdajaRepository prodajaRepository;
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public VoziloController(IVoziloRepository voziloRepository, IProdajaRepository prodajaRepository,
            OsiguranjeDbContext osiguranjeDbContext)
        {
            this.voziloRepository = voziloRepository;
            this.prodajaRepository = prodajaRepository;
            this.osiguranjeDbContext = osiguranjeDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string? sortBy,
            string? sortDirection,
            int pageSize = 3, int pageNumber = 1) // vraca listu svih vozila
        {
            var totalRecords = await voziloRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
            {
                pageNumber--;
            }
            if (pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;



            var vozila = await voziloRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return View(vozila);


        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string next,
            AddVoziloViewModel addVoziloViewModel) //dodaje novo vozilo
        {
            if (await voziloRepository.FindByRegistarskaOznaka(addVoziloViewModel.RegistarskaOznaka))
            {
                ModelState.AddModelError("RegistarskaOznaka", "Registarska oznaka već postoji");
                return View(addVoziloViewModel);
            }

            if (await voziloRepository.FindByBrojSasije(addVoziloViewModel.BrojSasije))
            {
                ModelState.AddModelError("BrojSasije", "Broj šasije već postoji");
                return View(addVoziloViewModel);
            }

            if (addVoziloViewModel.GodinaProizvodnje < 1900 || addVoziloViewModel.GodinaProizvodnje > DateTime.Now.Year)
            {
                ModelState.AddModelError("GodinaProizvodnje", "Neispravna godina proizvodnje");
                return View(addVoziloViewModel);
            }

            if (addVoziloViewModel.GodinaProizvodnje > addVoziloViewModel.DatumPrveRegistracije.Year)
            {
                ModelState.AddModelError("DatumPrveRegistracije",
                    "Datum prve registracije ne može biti prije godine kad je vozilo proizvedeno");
                return View(addVoziloViewModel);
            }

            if (addVoziloViewModel.DatumRegistracije < (DateTime.Now.AddYears(-1)))
            {
                ModelState.AddModelError("DatumRegistracije", "Registracija vozila je istekla");
                return View(addVoziloViewModel);
            }

            var vozilo = new Vozilo
            {
                Tip = addVoziloViewModel.Tip,
                Marka = addVoziloViewModel.Marka,
                Model = addVoziloViewModel.Model,
                RegistarskaOznaka = addVoziloViewModel.RegistarskaOznaka,
                GodinaProizvodnje = addVoziloViewModel.GodinaProizvodnje,
                Kubikaza = addVoziloViewModel.Kubikaza,
                SnagaMotora = addVoziloViewModel.SnagaMotora,
                BrojSasije = addVoziloViewModel.BrojSasije,
                DatumRegistracije = addVoziloViewModel.DatumRegistracije,
                DatumPrveRegistracije = addVoziloViewModel.DatumPrveRegistracije
            };

           await voziloRepository.AddAsync(vozilo);

            TempData["SuccessMessage"] = "Novo vozilo je dodato!";

            if (next == "Prodaja")
            {
                HttpContext.Session.SetString("VoziloId", vozilo.Id.ToString());

                return RedirectToAction("Add", "Prodaja");
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) //pronalazi vozilo po id-ju i vraca ga na view
        {
            var vozilo = await voziloRepository.GetAsync(id);

            if (vozilo != null)
            {
                var editVoziloViewModel = new EditVoziloViewModel
                {
                    Id = vozilo.Id,
                    Tip = vozilo.Tip,
                    Marka = vozilo.Marka,
                    Model = vozilo.Model,
                    RegistarskaOznaka = vozilo.RegistarskaOznaka,
                    GodinaProizvodnje = vozilo.GodinaProizvodnje,
                    Kubikaza = vozilo.Kubikaza,
                    SnagaMotora = vozilo.SnagaMotora,
                    BrojSasije = vozilo.BrojSasije,
                    DatumRegistracije = vozilo.DatumRegistracije,
                    DatumPrveRegistracije = vozilo.DatumPrveRegistracije
                };
                return View(editVoziloViewModel);
            }


            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVoziloViewModel editVoziloViewModel) //azurira vozilo
        {
            if (await voziloRepository.FindByRegistarskaOznaka
                (editVoziloViewModel.RegistarskaOznaka, editVoziloViewModel.Id)) 
            {
                ModelState.AddModelError("RegistarskaOznaka", "Registarska oznaka već postoji");
                return View(editVoziloViewModel);
            }

            if (await voziloRepository.FindByBrojSasije(editVoziloViewModel.BrojSasije,editVoziloViewModel.Id))
            {
                ModelState.AddModelError("BrojSasije", "Broj šasije već postoji");
                return View(editVoziloViewModel);
            }

            if (editVoziloViewModel.GodinaProizvodnje < 1900 || editVoziloViewModel.GodinaProizvodnje > DateTime.Now.Year)
            {
                ModelState.AddModelError("GodinaProizvodnje", "Neispravna godina proizvodnje");
                return View(editVoziloViewModel);
            }

            if (editVoziloViewModel.GodinaProizvodnje > editVoziloViewModel.DatumPrveRegistracije.Year)
            {
                ModelState.AddModelError("DatumPrveRegistracije",
                    "Datum prve registracije ne može biti prije godine kad je vozilo proizvedeno");
                return View(editVoziloViewModel);
            }

            if (editVoziloViewModel.DatumRegistracije < (DateTime.Now.AddYears(-1)))
            {
                ModelState.AddModelError("DatumRegistracije", "Registracija vozila je istekla");
                return View(editVoziloViewModel);
            }

            var vozilo = new Vozilo
            {
                Id = editVoziloViewModel.Id,
                Tip = editVoziloViewModel.Tip,
                Marka = editVoziloViewModel.Marka,
                Model = editVoziloViewModel.Model,
                RegistarskaOznaka = editVoziloViewModel.RegistarskaOznaka,
                GodinaProizvodnje = editVoziloViewModel.GodinaProizvodnje,
                Kubikaza = editVoziloViewModel.Kubikaza,
                SnagaMotora = editVoziloViewModel.SnagaMotora,
                BrojSasije = editVoziloViewModel.BrojSasije,
                DatumRegistracije = editVoziloViewModel.DatumRegistracije,
                DatumPrveRegistracije = editVoziloViewModel.DatumPrveRegistracije
            };

            var izmijenjenoVozilo = await voziloRepository.UpdateAsync(vozilo);

            if (izmijenjenoVozilo != null)
            {
                TempData["SuccessMessage"] = "Podaci o vozilu su ažurirani!";
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditVoziloViewModel editVoziloViewModel) //brise vozilo
        {
            var prodaja = await prodajaRepository.GetByIdVozila(editVoziloViewModel.Id);
            

            if (prodaja == null)
            {
                var vozilo = await voziloRepository.DeleteAsync(editVoziloViewModel.Id);

                if (vozilo != null)
                {
                    TempData["SuccessMessage"] = "Vozilo je obrisano!";
                    return RedirectToAction("List");
                }
            }
            else
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editVoziloViewModel.Id });

        }



        [HttpGet]
        public async Task<IActionResult> Selected(EditVoziloViewModel editVoziloViewModel) // uzima id izabranog vozila i salje ga na add metodu prodaje
        {
            HttpContext.Session.SetString("VoziloId", editVoziloViewModel.Id.ToString());

            return RedirectToAction("Add", "Prodaja");
        }




    }
}








