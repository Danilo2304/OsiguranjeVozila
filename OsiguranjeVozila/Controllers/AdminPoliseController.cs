using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPoliseController : Controller
    {
        private readonly IPolisaRepository polisaRepository;
        private readonly IUslovOsiguranjaRepository uslovOsiguranjaRepository;
        private readonly IProdajaRepository prodajaRepository;

        public AdminPoliseController(IPolisaRepository polisaRepository,
            IUslovOsiguranjaRepository uslovOsiguranjaRepository, 
            IProdajaRepository prodajaRepository)
        {
            this.polisaRepository = polisaRepository;
            this.uslovOsiguranjaRepository = uslovOsiguranjaRepository;
            this.prodajaRepository = prodajaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()     //get metoda puni dropdown za uslove osiguranja
        {
            var uslov = await uslovOsiguranjaRepository.GetAllAsync();

            var model = new AddPolisaViewModel()
            {
                UsloviOsiguranja = uslov.Select(x => new SelectListItem 
                { Text = x.Naziv, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPolisaViewModel dodajPolisuViewModel) //kreira novu polisu
        {
            var polisa = new Polise
            {
                Naziv = dodajPolisuViewModel.Naziv,
                Trajanje = dodajPolisuViewModel.Trajanje,
                NominalniIznos = dodajPolisuViewModel.NominalniIznos
            };

            var selektovaniUslovi = new List<UslovOsiguranja>();
            foreach (var selektovaniUslov in dodajPolisuViewModel.SelektovaniUslovi)
            {
                var selektovaniUslovId = Guid.Parse(selektovaniUslov);
                var uslov = await uslovOsiguranjaRepository.GetAsync(selektovaniUslovId);

                if (uslov != null)
                {
                    selektovaniUslovi.Add(uslov);
                }
            }

            polisa.UsloviOsiguranja = selektovaniUslovi;

            var existingPolisa = await polisaRepository.
                FindPolisaByNaziv(dodajPolisuViewModel.Naziv);

            if(existingPolisa == true)
            {
                ModelState.AddModelError("Naziv", "Polisa već postoji");
                return View(dodajPolisuViewModel);
            }

            await polisaRepository.AddAsync(polisa);



            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List() //vraca listu svih polisa
        {
            var polise = await polisaRepository.GetAllAsync();

            return View(polise);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)  //vraca selektovanu polisu nadjenu po id-ju
        {
            var polisa = await polisaRepository.GetAsync(id);
            var uslovi = await uslovOsiguranjaRepository.GetAllAsync();

            if (polisa != null)
            {
                var model = new EditPolisaViewModel
                {
                    Id = polisa.Id,
                    Naziv = polisa.Naziv,
                    NominalniIznos = polisa.NominalniIznos,
                    Trajanje = polisa.Trajanje,
                    UsloviOsiguranja = uslovi.Select(x => new SelectListItem
                    {
                        Text = x.Naziv,
                        Value = x.Id.ToString()
                    }),
                    SelektovaniUslovi = polisa.UsloviOsiguranja.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }


            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPolisaViewModel editPolisaViewModel) //azurira prethodno nadjenu polisu
        {
            var prodaja = await prodajaRepository.GetByIdPolise(editPolisaViewModel.Id);


            if (prodaja == null)
            {


                Polise polisa = new Polise
                {
                    Id = editPolisaViewModel.Id,
                    Naziv = editPolisaViewModel.Naziv,
                    NominalniIznos = editPolisaViewModel.NominalniIznos,
                    Trajanje = editPolisaViewModel.Trajanje
                };

                var selektovaniUslovi = new List<UslovOsiguranja>();
                foreach (var selektovaniUslov in editPolisaViewModel.SelektovaniUslovi)
                {
                    if (Guid.TryParse(selektovaniUslov, out var uslov))
                    {
                        var pronadjenUslov = await uslovOsiguranjaRepository.GetAsync(uslov);

                        if (pronadjenUslov != null)
                        {
                            selektovaniUslovi.Add(pronadjenUslov);
                        }
                    }
                }

                polisa.UsloviOsiguranja = selektovaniUslovi;

                var izmijenjenaPolisa = await polisaRepository.UpdateAsync(polisa);

                if (izmijenjenaPolisa != null)
                {
                    return RedirectToAction("List");
                }



                return RedirectToAction("Edit");
            }
            else
            {


                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditPolisaViewModel editPolisaViewModel) //brise polisu
        {
            var prodaja = await prodajaRepository.GetByIdPolise(editPolisaViewModel.Id);

            if (prodaja == null)
            {
                var polisa = await polisaRepository.DeleteAsync(editPolisaViewModel.Id);

                if (polisa != null)
                {
                    return RedirectToAction("List");
                }

                return RedirectToAction("Edit", new { id = editPolisaViewModel.Id });
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        
    }
}







