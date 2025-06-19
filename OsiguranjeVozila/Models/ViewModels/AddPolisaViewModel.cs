using Microsoft.AspNetCore.Mvc.Rendering;
using OsiguranjeVozila.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class AddPolisaViewModel
    {
        [Required(ErrorMessage ="Polje obavezno")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public int Trajanje { get; set; }

        public string? UslovOsiguranja { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public decimal NominalniIznos { get; set; }

        public IEnumerable<SelectListItem>? UsloviOsiguranja { get; set; }

        public string[]? SelektovaniUslovi { get; set; } = Array.Empty<string>();



        
    }
}
