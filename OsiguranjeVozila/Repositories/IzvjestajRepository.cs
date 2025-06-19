
using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;
using System.Globalization;

namespace OsiguranjeVozila.Repositories
{
    public class IzvjestajRepository : IIzvjestajRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public IzvjestajRepository(OsiguranjeDbContext osiguranjeDbContext)
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
        }

        

        public async Task<List<ProdajaPolise>> GetPoliseByDate() //vraca prodaje polisa koje isticu u narednih 30 dana
        {
            var now = DateTime.Now;
            var future = now.AddMonths(1);

            var isticuceProdaje = await osiguranjeDbContext.Prodaje.Where(x => x.DatumIsteka> now &&
            x.DatumIsteka<future).Include(x => x.Polisa).Include(x => x.Klijent).Include(x => x.Vozilo).
            ToListAsync();

            return isticuceProdaje;
        }

        public async Task<float> VratiPrihod(string? datumOd, string? datumDo) // vraca prihod za odredjeni vremenski period
        {
            var prodaje = await osiguranjeDbContext.Prodaje.ToListAsync();

            float ukupniPrihod = 0;

            if (!string.IsNullOrWhiteSpace(datumOd) && !string.IsNullOrWhiteSpace(datumDo)) {
                foreach (var prodaja in prodaje)
                {
                    var pocetak = prodaja.DatumKupovine;
                    var kraj = prodaja.DatumIsteka;
                    var mjesecnaCijena = prodaja.Cijena / prodaja.Polisa.Trajanje;

                    var efektivniOd = pocetak > DateTime.Parse(datumOd) ? pocetak :
                        DateTime.Parse(datumOd);
                    var efektivniDo = kraj < DateTime.Parse(datumDo) ? kraj :
                        DateTime.Parse(datumDo);

                    if (efektivniOd <= efektivniDo)
                    {
                        int brojMjeseci = ((efektivniDo.Year - efektivniDo.Year) *
                            12 + efektivniDo.Month - efektivniOd.Month) + 1;

                        ukupniPrihod += brojMjeseci * (float)mjesecnaCijena;
                    }
                }
            }
            return ukupniPrihod;


        }
    }


}







