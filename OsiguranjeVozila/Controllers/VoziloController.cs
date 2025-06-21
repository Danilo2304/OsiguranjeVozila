using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator, Zaposleni")]
    public class VoziloController : Controller
    {
        private readonly IVoziloRepository voziloRepository;

        public VoziloController(IVoziloRepository voziloRepository)
        {
            this.voziloRepository = voziloRepository;
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
            if(pageNumber < 1)
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

            var voziloExists = await voziloRepository.
                FindByRegistarskaOznaka(addVoziloViewModel.RegistarskaOznaka);

            if (voziloExists == true)
            {
                ModelState.AddModelError("RegistarskaOznaka", "Vozilo već postoji");
                return View(addVoziloViewModel);
            }

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

            if(vozilo != null)
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

            if(izmijenjenoVozilo != null)
            {
                TempData["SuccessMessage"] = "Podaci o vozilu su ažurirani!";
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditVoziloViewModel editVoziloViewModel) //brise vozilo
        {
            var vozilo = await voziloRepository.DeleteAsync(editVoziloViewModel.Id);

            if(vozilo != null)
            {
                TempData["SuccessMessage"] = "Vozilo je obrisano!";
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








