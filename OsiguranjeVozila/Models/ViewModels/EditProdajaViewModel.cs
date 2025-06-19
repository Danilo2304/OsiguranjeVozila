using OsiguranjeVozila.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class EditProdajaViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Polje obavezno")]
        public DateTime DatumKupovine { get; set; }

        public DateTime DatumIsteka { get; set; }

        public decimal Cijena { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public decimal UcesceUSteti { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid KlijentId { get; set; }
        public Klijent Klijent { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid VoziloId { get; set; }
        public Vozilo Vozilo { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid? PolisaId { get; set; }  
        public Polise? Polisa { get; set; }
    }
}
