namespace OsiguranjeVozila.Models.Domain
{
    public class ProdajaPolise
    {
        public Guid Id { get; set; }

        public DateTime DatumKupovine { get; set; }

        public DateTime DatumIsteka { get; set; }

        public decimal Cijena { get; set; }

        public decimal UcesceUSteti { get; set; }

        public Guid KlijentId { get; set; }
        public Klijent Klijent { get; set; }

        
        public Guid VoziloId { get; set; }
        public Vozilo Vozilo { get; set; }

        
        public Guid? PolisaId { get; set; }  
        public Polise? Polisa { get; set; }
    }
}


