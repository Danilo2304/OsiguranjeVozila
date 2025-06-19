using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public interface IVoziloRepository
    {
        Task<Vozilo?> GetAsync(Guid id);

        Task<Vozilo> AddAsync(Vozilo vozilo);

        Task<IEnumerable<Vozilo>> GetAllAsync(string? searchQuery=null, string? sortBy=null, 
            string? sortDirection = null, int pageNumber = 1, int pageSize = 100);

        Task<Vozilo?> UpdateAsync(Vozilo vozilo);

        Task<Vozilo?> DeleteAsync(Guid id);

        Task<int> CountAsync();

        Task<bool> FindByRegistarskaOznaka(string registracija);
    }
}




