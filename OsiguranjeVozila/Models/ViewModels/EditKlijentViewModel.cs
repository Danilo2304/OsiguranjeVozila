using OsiguranjeVozila.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class EditKlijentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        [EmailAddress(ErrorMessage = "Unesi validnu email adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string BrojTelefona { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public DateTime DatumRodjenja { get; set; }

        public List<Klijent> Klijenti { get; set; }
    }
}
