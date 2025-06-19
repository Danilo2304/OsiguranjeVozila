namespace OsiguranjeVozila.Models.Domain
{
    public class Vozilo
    {
        public Guid Id { get; set; }

        public string Tip { get; set; }

        public string Marka { get; set; }

        public string Model { get; set; }

        public string RegistarskaOznaka { get; set; }

        public int GodinaProizvodnje { get; set; }

        public float Kubikaza { get; set; }

        public int SnagaMotora { get; set; }

        public string BrojSasije { get; set; }

        public DateTime DatumRegistracije { get; set; }

        public DateTime DatumPrveRegistracije { get; set; }

        public ICollection<ProdajaPolise> Prodaje { get; set; }

        
    }
}
