using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public interface IProdajaRepository
    {
        Task<ProdajaPolise> AddAsync(ProdajaPolise prodaja); //kreira novu prodaju

        Task<ProdajaPolise?> UpdateAsync(ProdajaPolise prodaja); //azurira prodaju

        Task<ProdajaPolise?> DeleteAsync(Guid id); //brise prodaju

        Task<ProdajaPolise?> GetAsync(Guid id); //vraca prodaju na osnovu id-ja

        Task<IEnumerable<ProdajaPolise>> GetAllAsync(string? searchQuery=null, string? sortBy=null, string? sortDirection= null,
            string? datumOd = null, string? datumDo = null, int pageNumber = 1, 
            int pageSize = 100); //vraca listu svih prodaja


        Task<ProdajaPolise> GetByIdPolise(Guid id); //vraca prodaju na osnovu id-ja polise

        Task<IEnumerable<ProdajaPolise>> GetAllByPolisaIdAsync(Guid id); //vraca sve prodaje koje imaju istu polisu

        Task<IEnumerable<ProdajaPolise>> GetByKlijentIdAsync(Guid id); //vraca sve prodaje koje imaju istog klijenta

        Task<int> CountAsync(); //vraca broj prodaja

        
    }
}
