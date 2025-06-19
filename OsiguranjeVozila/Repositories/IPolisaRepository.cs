using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Repositories
{
    public interface IPolisaRepository
    {
        Task<Polise> AddAsync(Polise polisa); //kreira novu polisu

        Task <IEnumerable<Polise>> GetAllAsync(); //vraca listu svih polisa

        Task <Polise?> GetAsync(Guid id); //pronalazi polisu na osnovu id-ja

        Task<Polise?> UpdateAsync(Polise polisa); //azurira polisu

        Task <Polise?> DeleteAsync(Guid id); //brise polisu

        Task<bool> FindPolisaByNaziv(string naziv); //provjerava da li polisa vec postoji na osnovu naziva
    }
}
