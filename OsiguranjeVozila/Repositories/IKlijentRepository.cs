using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public interface IKlijentRepository
    {
        Task<Klijent> GetAsync(Guid id); //vraca klijenta preko id-ja

        Task<IEnumerable<Klijent>> GetAllASync(string? searchQuery=null, string? sortBy=null,string? 
            sortDirection=null, int pageNumber=1,int pageSize = 100); //vraca listu svih klijenata

        Task<Klijent> AddAsync(Klijent klijent); //kreira novog klijenta

        Task<Klijent> UpdateAsync(Klijent klijent); //azurira klijenta

        Task<Klijent> DeleteAsync(Guid id); //brise klijenta

        Task<int> CountAsync(); //vraca ukupan broj klijenata

        Task<bool> FindByEmail(string email); //provjerava da li klijent sa proslijedjenim mailom postoji
    }
}
