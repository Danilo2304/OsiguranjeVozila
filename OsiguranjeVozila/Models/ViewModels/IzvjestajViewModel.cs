using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class IzvjestajViewModel
    {
        public IEnumerable<PolisaIzvjestajViewModel> Polise {  get; set; }

        public IEnumerable<ProdajaPolise> Prodaje { get; set; }

        public float? Prihod {  get; set; }
    }
}
