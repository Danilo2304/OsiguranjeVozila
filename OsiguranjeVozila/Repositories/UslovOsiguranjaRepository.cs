using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public class UslovOsiguranjaRepository : IUslovOsiguranjaRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public UslovOsiguranjaRepository(OsiguranjeDbContext osiguranjeDbContext)
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
        }

        public async Task<UslovOsiguranja?> GetAsync(Guid id) //vraca uslov osiguranja na osnovu id-ja
        {
            return await osiguranjeDbContext.UslovOsiguranja.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<IEnumerable<UslovOsiguranja>> GetAllAsync() //vraca listu svih uslova osiguranja
        {
            return await osiguranjeDbContext.UslovOsiguranja.ToListAsync();
        }
        
        async Task<UslovOsiguranja> IUslovOsiguranjaRepository.AddAsync(UslovOsiguranja usv) //kreira novi uslov osiguranja
        {
            await osiguranjeDbContext.UslovOsiguranja.AddAsync(usv);
            await osiguranjeDbContext.SaveChangesAsync();
            return usv;
        }
        
        async Task<UslovOsiguranja?> IUslovOsiguranjaRepository.UpdateAsync(UslovOsiguranja usv) //azurira uslov osiguranja
        {
            var uslov = await osiguranjeDbContext.UslovOsiguranja.FirstOrDefaultAsync(x => x.Id == usv.Id);

            if(uslov != null)
            {
                uslov.Id = usv.Id;
                uslov.Naziv = usv.Naziv;
                uslov.Opis = usv.Opis;

                await osiguranjeDbContext.SaveChangesAsync();
                return uslov;
            }

            return null;
        }

        async Task<UslovOsiguranja?> IUslovOsiguranjaRepository.DeleteAsync(Guid id) //brise uslov osiguranja
        {
            var uslov = await osiguranjeDbContext.UslovOsiguranja.FirstOrDefaultAsync(x => x.Id ==  id);

            if(uslov != null)
            {
                osiguranjeDbContext.UslovOsiguranja.Remove(uslov);

                await osiguranjeDbContext.SaveChangesAsync() ;

                return uslov;
            }

            return null;
        }
    }
}






