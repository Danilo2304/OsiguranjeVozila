using Microsoft.AspNetCore.Mvc;
using OsiguranjeVozila.Models.ViewModels;
using OsiguranjeVozila.Repositories;

namespace OsiguranjeVozila.Controllers
{
    public class IzvjestajController : Controller
    {
        private readonly IIzvjestajRepository izvjestajRepository;
        private readonly IPolisaRepository polisaRepository;
        private readonly IProdajaRepository prodajaRepository;

        public IzvjestajController(IIzvjestajRepository izvjestajRepository, 
            IPolisaRepository polisaRepository,
            IProdajaRepository prodajaRepository)
        {
            this.izvjestajRepository = izvjestajRepository;
            this.polisaRepository = polisaRepository;
            this.prodajaRepository = prodajaRepository;
        }



        [HttpGet]
        public async Task<IActionResult> List(string? datumOd, string? datumDo) //vraca 3 izvjestaja
        {
            var prodaje = await prodajaRepository.GetAllAsync();
            var polise = prodaje.GroupBy(p => p.Polisa.Naziv).Select   //racuna koliko svaka polisa ima prodaja i grupise ih po nazivu 
                (x => new PolisaIzvjestajViewModel
            {
                Naziv = x.Key,
                BrojProdaja = x.Count()
            }).ToList();

            var isticuceProdaje = await izvjestajRepository.GetPoliseByDate();   //pronalazi sve polise koje isticu u narednih 30 dana

            float prihod = await izvjestajRepository.VratiPrihod(datumOd, datumDo);   //racuna prihod za odredjen period



            var izvjestaj = new IzvjestajViewModel
            {
                Polise = polise,
                Prodaje = isticuceProdaje,
                Prihod = prihod
            };

            ViewBag.DatumOd = datumOd;
            ViewBag.DatumDo = datumDo;

            return View(izvjestaj);
        }


    }
}



