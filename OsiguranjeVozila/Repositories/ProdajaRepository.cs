using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public class ProdajaRepository : IProdajaRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;
        private readonly IPolisaRepository polisaRepository;

        public ProdajaRepository(OsiguranjeDbContext osiguranjeDbContext, IPolisaRepository polisaRepository)
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
            this.polisaRepository = polisaRepository;
        }

        public async Task<ProdajaPolise?> GetByIdPolise(Guid id) //vraca prodaju na osnovu id-ja polise
        {
            return await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.PolisaId == id); 
        }

        public async Task<ProdajaPolise?> GetByIdKlijenta(Guid id)
        {
            return await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.KlijentId == id);
        }

        public async Task<ProdajaPolise?> GetByIdVozila(Guid id)
        {
            return await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.VoziloId == id);
        }

        public async Task<ProdajaPolise> AddAsync(ProdajaPolise prodaja) //kreira novu prodaju
        {
            await osiguranjeDbContext.AddAsync(prodaja);
            await osiguranjeDbContext.SaveChangesAsync();
            return prodaja;
        }

        public async Task<ProdajaPolise?> DeleteAsync(Guid id) //brise prodaju
        {
            var postojecaProdaja = await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.Id == id);

            if (postojecaProdaja != null)
            {
                osiguranjeDbContext.Prodaje.Remove(postojecaProdaja);
                await osiguranjeDbContext.SaveChangesAsync();
                return postojecaProdaja;
            }

            return null;
        }

        public async Task<IEnumerable<ProdajaPolise>> GetAllAsync(string? searchQuery, string? sortBy,
            string? sortDirection, string? datumOd , string? datumDo, 
            int pageNumber = 1, int pageSize = 100) //vraca listu svih prodaja

        {
            var query = osiguranjeDbContext.Prodaje.Include(x => x.Klijent).Include(x => x.Polisa)
                .Include(x => x.Vozilo).AsQueryable();

            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Klijent.Ime.Contains(searchQuery) ||
                x.Klijent.Prezime.Contains(searchQuery) || x.Polisa.Naziv.Contains(searchQuery));

            }

            if ((string.IsNullOrEmpty(datumOd) && string.IsNullOrEmpty(datumDo)) == false)
            {
                var from = DateTime.Parse(datumOd);
                var to = DateTime.Parse(datumDo);

                query = query.Where(x => x.DatumKupovine <= from &&
                x.DatumIsteka >= to);
            }

            

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Ime", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Klijent.Ime) : 
                        query.OrderBy(x => x.Klijent.Ime);
                }
                if(string.Equals(sortBy, "DatumKupovine", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DatumKupovine) : 
                        query.OrderBy(x => x.DatumKupovine);
                }
                if (string.Equals(sortBy, "DatumIsteka", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DatumIsteka) :
                        query.OrderBy(x => x.DatumIsteka);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<ProdajaPolise?> GetAsync(Guid id) //vraca prodaju na osnovu id-ja
        {
            return await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<ProdajaPolise?> UpdateAsync(ProdajaPolise prodaja) //azurira prodaju
        {
            var postojecaProdaja = await osiguranjeDbContext.Prodaje.FirstOrDefaultAsync(x => x.Id == prodaja.Id);
            var polisa = await polisaRepository.GetAsync((Guid)prodaja.PolisaId);

            if (postojecaProdaja != null)
            {
                postojecaProdaja.PolisaId = prodaja.PolisaId;
                postojecaProdaja.KlijentId = prodaja.KlijentId;
                postojecaProdaja.VoziloId = prodaja.VoziloId;
                postojecaProdaja.Cijena = polisa.NominalniIznos - prodaja.UcesceUSteti; 
                postojecaProdaja.DatumKupovine = prodaja.DatumKupovine;
                postojecaProdaja.DatumIsteka = prodaja.DatumKupovine.AddMonths(polisa.Trajanje);
                postojecaProdaja.UcesceUSteti = prodaja.UcesceUSteti;

                await osiguranjeDbContext.SaveChangesAsync();
                return postojecaProdaja;
            }

            return null;
        }

        public async Task<IEnumerable<ProdajaPolise>> GetAllByPolisaIdAsync(Guid id) //vraca sve prodaje koje imaju istu polisu
        {
            var prodaja = await osiguranjeDbContext.Prodaje.Include(x => x.Klijent).Include(x => x.Polisa)
                .Include(x => x.Vozilo).Where(x => x.PolisaId == id).ToListAsync();

            return prodaja;
        }

        public async Task<IEnumerable<ProdajaPolise>> GetByKlijentIdAsync(Guid id) //vraca sve prodaje koje imaju istog klijenta
        {
            return await osiguranjeDbContext.Prodaje.Include(x => x.Klijent).Include(x => x.Vozilo)
                .Include(x => x.Polisa).Where(x => x.KlijentId == id).ToListAsync();
        }

        public async Task<int> CountAsync() //vraca broj prodaja
        {
            return await osiguranjeDbContext.Prodaje.CountAsync();
        }
    }
}
