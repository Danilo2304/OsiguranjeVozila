using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;
using System.Globalization;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator, Zaposleni")]
    public class KlijentController : Controller
    {
        private readonly IKlijentRepository klijentRepository;
        private readonly IProdajaRepository prodajaRepository;

        public KlijentController(IKlijentRepository klijentRepository, 
            IProdajaRepository prodajaRepository)
        {
            this.klijentRepository = klijentRepository;
            this.prodajaRepository = prodajaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, 
            string? sortBy, string? sortDirection,
            int pageSize = 10, int pageNumber = 1) //vraca listu svih klijenata
        {
            var totalRecords = await klijentRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if(pageNumber > totalPages)
            {
                pageNumber--;
            }
            if(pageNumber < 1)
            {
                pageNumber++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;


            var klijenti = await klijentRepository.GetAllASync(searchQuery,sortBy,sortDirection, 
                 pageNumber,pageSize);

            

            return View(klijenti);
         }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] string next, 
            AddKlijentViewModel addKlijentViewModel) //dodaj novog klijenta
        {


            var klijent = new Klijent
            {
                Ime = addKlijentViewModel.Ime,
                Prezime = addKlijentViewModel.Prezime,
                Email = addKlijentViewModel.Email,
                Adresa = addKlijentViewModel.Adresa,
                BrojTelefona = addKlijentViewModel.BrojTelefona,
                DatumRodjenja = addKlijentViewModel.DatumRodjenja

            };

            var klijentExists = await klijentRepository.FindByEmail(addKlijentViewModel.Email);

            if (klijentExists == true)
            {
                ModelState.AddModelError("Email", "Korisnik već postoji");
                return View(addKlijentViewModel);
            }



            await klijentRepository.AddAsync(klijent);

            TempData["SuccessMessage"] = "Novi klijent je dodat!";

            if (next == "Prodaja")
            {
                HttpContext.Session.SetString("KlijentId", klijent.Id.ToString());



                return RedirectToAction("Add", "Prodaja");
            }
                
            return RedirectToAction("List");
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)   //pronalazi klijenta po id-ju
        {
            var klijent = await klijentRepository.GetAsync(id);

            if (klijent != null)
            {
                EditKlijentViewModel editKlijentViewModel = new EditKlijentViewModel
                {
                    Ime = klijent.Ime,
                    Id = klijent.Id,
                    Prezime = klijent.Prezime,
                    Adresa = klijent.Adresa,
                    Email = klijent.Email,
                    BrojTelefona = klijent.BrojTelefona.ToString(),
                    DatumRodjenja = klijent.DatumRodjenja
                };
                return View(editKlijentViewModel);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditKlijentViewModel editKlijentViewModel) //azurira klijenta
        {
            Klijent klijent = new Klijent
            {
                Ime = editKlijentViewModel.Ime,
                Id = editKlijentViewModel.Id,
                Prezime = editKlijentViewModel.Prezime,
                Adresa = editKlijentViewModel.Adresa,
                Email = editKlijentViewModel.Email,
                BrojTelefona = editKlijentViewModel.BrojTelefona,
                DatumRodjenja = editKlijentViewModel.DatumRodjenja
            };

            var izmijenjenKlijent = await klijentRepository.UpdateAsync(klijent);

            if(izmijenjenKlijent != null)
            {
                TempData["SuccessMessage"] = "Podaci su uspješno ažurirani!";
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditKlijentViewModel editKlijentViewModel) //brise klijenta
        {
            var klijent = await klijentRepository.DeleteAsync(editKlijentViewModel.Id);

            if(klijent != null)
            {
                TempData["SuccessMessage"] = "Klijent je obrisan!";
                return RedirectToAction("List");
            }


            return RedirectToAction("Edit", new {id=editKlijentViewModel.Id});
        }

        

        [HttpGet]
        public async Task<IActionResult> Selected(EditKlijentViewModel editKlijentViewModel)// uzima id izabranog klijenta i salje ga na add metodu prodaje
        {
            HttpContext.Session.SetString("KlijentId", editKlijentViewModel.Id.ToString());

            return RedirectToAction("Add", "Prodaja");
        }

        
    }
}













