using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public interface IUslovOsiguranjaRepository
    {
        Task<UslovOsiguranja> AddAsync(UslovOsiguranja usv); //kreira novi uslov osiguranja

        Task<IEnumerable<UslovOsiguranja>> GetAllAsync(); //vraca listu svih uslova osiguranja

        Task<UslovOsiguranja?> GetAsync(Guid id); //vraca uslov osiguranja na osnovu id-ja

        Task<UslovOsiguranja?> UpdateAsync(UslovOsiguranja usv); // azurira uslov osiguranja

        Task<UslovOsiguranja?> DeleteAsync(Guid id); // brise uslov osiguranja
    }
}
