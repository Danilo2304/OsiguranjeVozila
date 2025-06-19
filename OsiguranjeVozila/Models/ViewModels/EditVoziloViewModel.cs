using OsiguranjeVozila.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace OsiguranjeVozila.Models.ViewModels
{
    public class EditVoziloViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Polje obavezno")]
        public string Tip { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string Marka { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string RegistarskaOznaka { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public int GodinaProizvodnje { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public float Kubikaza { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public int SnagaMotora { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public string BrojSasije { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public DateTime DatumRegistracije { get; set; }

        [Required(ErrorMessage = "Polje obavezno")]
        public DateTime DatumPrveRegistracije { get; set; }

        public ICollection<ProdajaPolise> Prodaje { get; set; }

        public Klijent? vlasnik { get; set; }
    }
}
