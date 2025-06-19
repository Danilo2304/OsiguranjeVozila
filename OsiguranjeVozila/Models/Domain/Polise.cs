namespace OsiguranjeVozila.Models.Domain
{
    public class Polise
    {
        public Guid Id { get; set; }

        public string Naziv { get; set; }

        public int Trajanje { get; set; }

        public decimal NominalniIznos { get; set; }

        public ICollection<UslovOsiguranja>? UsloviOsiguranja { get; set; }

        public ICollection<ProdajaPolise>? Prodaje { get; set; }


    }
}
