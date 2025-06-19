namespace OsiguranjeVozila.Models.Domain
{
    public class UslovOsiguranja
    {
        public Guid Id { get; set; }

        public string Naziv { get; set; }

        public string Opis { get; set; }

        public ICollection<Polise>? Polise { get; set; }
    }
}
