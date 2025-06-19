using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Migrations;
using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminUsloviOsiguranjaController : Controller
    {
        private readonly IUslovOsiguranjaRepository uslovOsiguranjaRepository;

        public AdminUsloviOsiguranjaController(IUslovOsiguranjaRepository uslovOsiguranjaRepository)
        {
            this.uslovOsiguranjaRepository = uslovOsiguranjaRepository;
        }

        


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUslovOsiguranjaViewModel uslov) //dodaje novi uslov osiguranja
        {
            var uslovOsiguranja = new UslovOsiguranja
            {
                Naziv = uslov.Naziv,
                Opis = uslov.Opis,
            };

            await uslovOsiguranjaRepository.AddAsync(uslovOsiguranja);

            return RedirectToAction("List", "AdminUsloviOsiguranja");
        }

        [HttpGet]
        public async Task<IActionResult> List()  //vraca listu svih uslova osiguranja
        {
            var usloviOsiguranja = await uslovOsiguranjaRepository.GetAllAsync();

            return View(usloviOsiguranja);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)  //vraca konkretan uslov osiguranja nadjen po id-ju
        {
            var uslov = await uslovOsiguranjaRepository.GetAsync(id);

            if(uslov != null)
            {
                var editUslovOsiguranjaViewModel = new EditUslovOsiguranjaViewModel
                {
                    Id = uslov.Id,
                    Naziv = uslov.Naziv,
                    Opis = uslov.Opis
                };
                return View(editUslovOsiguranjaViewModel);
            }

            return View(null);
            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUslovOsiguranjaViewModel usloviOsiguranjaViewModel) //azurira prethodno nadjen uslov
        {
            var uslov = new UslovOsiguranja
            {
                Id=usloviOsiguranjaViewModel.Id,
                Naziv = usloviOsiguranjaViewModel.Naziv,
                Opis = usloviOsiguranjaViewModel.Opis
            };

            var izmijenjenUslov = await uslovOsiguranjaRepository.UpdateAsync(uslov);

            return RedirectToAction("List", "AdminUsloviOsiguranja");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditUslovOsiguranjaViewModel usloviOsiguranjaViewModel) //brise uslov osiguranja
        {
            var uslov = await uslovOsiguranjaRepository.DeleteAsync(usloviOsiguranjaViewModel.Id);

            if (uslov != null)
            {
                return View("List");
            }
            else
            {
                return RedirectToAction("Edit", new { id = usloviOsiguranjaViewModel.Id });
            }
            
        }
    }
}
