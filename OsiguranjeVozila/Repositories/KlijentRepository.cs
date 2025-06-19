using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Data;
using OsiguranjeVozila.Models.Domain;
using System.Globalization;

namespace OsiguranjeVozila.Repositories
{
    public class KlijentRepository : IKlijentRepository
    {
        private readonly OsiguranjeDbContext osiguranjeDbContext;

        public KlijentRepository(OsiguranjeDbContext osiguranjeDbContext)
        {
            this.osiguranjeDbContext = osiguranjeDbContext;
        }



        public async Task<Klijent> AddAsync(Klijent klijent) //kreira novog klijenta
        {
            if(klijent != null)
            {
                await osiguranjeDbContext.Klijenti.AddAsync(klijent);
                await osiguranjeDbContext.SaveChangesAsync();
                return klijent;
                
            }

            return null;

        }



        public async Task<int> CountAsync() //vraca ukupan broj klijenata
        {
            return await osiguranjeDbContext.Klijenti.CountAsync();
        }



        public async Task<Klijent> DeleteAsync(Guid id) //brise klijenta
        {
            var klijent = await osiguranjeDbContext.Klijenti.FirstOrDefaultAsync(x => x.Id == id);

            if (klijent != null)
            {
                osiguranjeDbContext.Klijenti.Remove(klijent);
                await osiguranjeDbContext.SaveChangesAsync();
                return klijent;
            }
            return null;
        }

        public async Task<bool> FindByEmail(string email) //provjerava da li klijent sa proslijedjenim mailom postoji
        {
            var klijent = await osiguranjeDbContext.Klijenti.
                FirstOrDefaultAsync(x => x.Email == email);

            if (klijent != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Klijent>> GetAllASync(string? searchQuery,string? sortBy, string?
            sortDirection, int pageNumber = 1, int pageSize = 100) //vraca listu svih klijenata
        {
            var query = osiguranjeDbContext.Klijenti.AsQueryable();

            if(string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Ime.Contains(searchQuery) || x.Prezime.Contains(searchQuery));
                
            }

            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if(string.Equals(sortBy, "Ime", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Ime):query.OrderBy(x => x.Ime);
                }

                if (string.Equals(sortBy, "Prezime", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Prezime) : query.OrderBy(x => x.Prezime);
                }
            }

            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }



        public async Task<Klijent> GetAsync(Guid id) //vraca klijenta preko id-ja
        {
            return await osiguranjeDbContext.Klijenti.FirstOrDefaultAsync(x => x.Id == id);
            
        }




        public async Task<Klijent> UpdateAsync(Klijent klijent) //azurira klijenta
        {
            var postojeciKlijent = await osiguranjeDbContext.Klijenti.FirstOrDefaultAsync(x => x.Id ==  klijent.Id);

            if (postojeciKlijent != null)
            {
                postojeciKlijent.Id = klijent.Id;
                postojeciKlijent.Ime = klijent.Ime;
                postojeciKlijent.Prezime = klijent.Prezime;
                postojeciKlijent.Adresa = klijent.Adresa;
                postojeciKlijent.Email = klijent.Email;
                postojeciKlijent.BrojTelefona = klijent.BrojTelefona;
                postojeciKlijent.DatumRodjenja = klijent.DatumRodjenja;

                await osiguranjeDbContext.SaveChangesAsync();
                return postojeciKlijent;
            }

            return null;
        }

        

    }
}

