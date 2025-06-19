using OsiguranjeVozila.Models.Domain;
using OsiguranjeVozila.Models.ViewModels;

namespace OsiguranjeVozila.Repositories
{
    public interface IIzvjestajRepository
    {
        

        Task<List<ProdajaPolise>> GetPoliseByDate(); //vraca prodaje polisa koje isticu u narednih 30 dana

        Task<float> VratiPrihod(string? datumOd = null, string? datumDo = null); // vraca prihod za odredjeni vremenski period
    }
}
