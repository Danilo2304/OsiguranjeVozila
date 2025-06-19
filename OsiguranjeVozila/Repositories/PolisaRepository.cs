using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public class PolisaRepository:IPolisaRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public PolisaRepository(OsiguranjeDbContext osiguranjeDbContext)
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
        }

        public async Task<Polise> AddAsync(Polise polisa) //kreira novu polisu
        {
            await osiguranjeDbContext.AddAsync(polisa);
            await osiguranjeDbContext.SaveChangesAsync();
            return polisa;
        }

        public async Task<Polise?> DeleteAsync(Guid id) // brise polisu
        {
            var polisa = await osiguranjeDbContext.Polise.FirstOrDefaultAsync(x => x.Id == id);
            if(polisa != null)
            {
                osiguranjeDbContext.Polise.Remove(polisa);
                await osiguranjeDbContext.SaveChangesAsync();
                return polisa;
            }

            return null;

        }

        public async Task<bool> FindPolisaByNaziv(string naziv) //provjerava da li polisa vec postoji na osnovu naziva
        {
            var polisa = await osiguranjeDbContext.Polise.FirstOrDefaultAsync(x => x.Naziv == naziv);

            if(polisa != null)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Polise>> GetAllAsync() //vraca listu svih polisa
        {
            return await osiguranjeDbContext.Polise.Include(x=>x.UsloviOsiguranja).ToListAsync();
        }

        public async Task<Polise?> GetAsync(Guid id) //pronalazi polisu na osnovu id-ja
        {
            return await osiguranjeDbContext.Polise.Include(x=>x.UsloviOsiguranja).FirstOrDefaultAsync(x=>x.Id == id);
        } 

        

        public async Task<Polise?> UpdateAsync(Polise polisa) //azurira polisu
        {
            var postojecaPolisa = await osiguranjeDbContext.Polise.Include
                (x => x.UsloviOsiguranja).FirstOrDefaultAsync(x => x.Id == polisa.Id);

            if(postojecaPolisa != null)
            {
                postojecaPolisa.Id = polisa.Id;
                postojecaPolisa.Naziv = polisa.Naziv;
                postojecaPolisa.Trajanje = polisa.Trajanje;
                postojecaPolisa.NominalniIznos = polisa.NominalniIznos;
                postojecaPolisa.UsloviOsiguranja = polisa.UsloviOsiguranja;

                await osiguranjeDbContext.SaveChangesAsync();
                return postojecaPolisa;
            }

            return null;
        }

        
    }
}


