using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public class VoziloRepository : IVoziloRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public VoziloRepository(OsiguranjeDbContext osiguranjeDbContext )
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
        }

        public async Task<Vozilo> AddAsync(Vozilo vozilo)
        {
            
                await osiguranjeDbContext.Vozila.AddAsync(vozilo);
                await osiguranjeDbContext.SaveChangesAsync();
                return vozilo;
        }

        public Task<int> CountAsync()
        {
            return osiguranjeDbContext.Vozila.CountAsync();
        }

        public async Task<Vozilo?> DeleteAsync(Guid id)
        {
            Vozilo vozilo = await osiguranjeDbContext.Vozila.FirstOrDefaultAsync(x => x.Id == id);

            if (vozilo != null)
            {
                osiguranjeDbContext.Vozila.Remove(vozilo);
                await osiguranjeDbContext.SaveChangesAsync() ;
                return vozilo;
            }

            return null;
        }

        public async Task<bool> FindByBrojSasije(string brojSasije, Guid? id = null)
        {
           if(await osiguranjeDbContext.Vozila.AnyAsync(x=>x.BrojSasije == brojSasije && (id == null || x.Id != id)))

            {
                return true;
            }
           return false;
        }

        public async Task<bool> FindByRegistarskaOznaka(string registracija, Guid? id = null)
        {
            if(await osiguranjeDbContext.Vozila.AnyAsync(x=>x.RegistarskaOznaka == registracija && 
                    (id == null || x.Id != id)))
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Vozilo>> GetAllAsync(string? searchQuery, string? sortBy,
            string sortDirection, int pageNumber = 1, int pageSize = 100)
        {
            var query = osiguranjeDbContext.Vozila.AsQueryable();

            if(string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Marka.Contains(searchQuery) || x.Model.Contains(searchQuery)
                || x.RegistarskaOznaka.Contains(searchQuery));
            }

            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if(string.Equals(sortBy, "Marka", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Marka) : query.OrderBy(x => x.Marka);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Vozilo?> GetAsync(Guid id)
        {
            return await osiguranjeDbContext.Vozila.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Vozilo?> UpdateAsync(Vozilo vozilo)
        {
            var postojeceVozilo = await osiguranjeDbContext.Vozila.FirstOrDefaultAsync(x=>x.Id == vozilo.Id);

            if(postojeceVozilo != null)
            {
                postojeceVozilo.Id = vozilo.Id;
                postojeceVozilo.Tip = vozilo.Tip;
                postojeceVozilo.Model = vozilo.Model;
                postojeceVozilo.Marka = vozilo.Marka;
                postojeceVozilo.RegistarskaOznaka = vozilo.RegistarskaOznaka;
                postojeceVozilo.Kubikaza = vozilo.Kubikaza;
                postojeceVozilo.SnagaMotora = vozilo.SnagaMotora;
                postojeceVozilo.DatumPrveRegistracije = vozilo.DatumPrveRegistracije;
                postojeceVozilo.DatumRegistracije = vozilo.DatumRegistracije;
                postojeceVozilo.BrojSasije = vozilo.BrojSasije;

                await osiguranjeDbContext.SaveChangesAsync();
                return postojeceVozilo;
            }

            return null;
        }


    }
}


