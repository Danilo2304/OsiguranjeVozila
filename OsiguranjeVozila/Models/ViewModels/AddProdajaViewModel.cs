using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class AddProdajaViewModel
    {
        [Required(ErrorMessage ="Polje obavezno")]
        public DateTime DatumKupovine { get; set; }

        
        public DateTime DatumIsteka { get; set; }

        public decimal Cijena { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public decimal UcesceUSteti { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid KlijentId { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid VoziloId { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public Guid PolisaId { get; set; }

        public IEnumerable<SelectListItem> Klijenti { get; set; }
        public IEnumerable<SelectListItem> Vozila {  get; set; }
        public IEnumerable<SelectListItem> Polise {  get; set; }
    }
}
