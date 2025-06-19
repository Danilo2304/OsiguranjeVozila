using Microsoft.EntityFrameworkCore;
using OsiguranjeVozila.Models.Domain;

namespace OsiguranjeVozila.Data
{
    public class OsiguranjeDbContext : DbContext
    {
        public OsiguranjeDbContext(DbContextOptions<OsiguranjeDbContext> options) : base(options)
        {
        }

        public DbSet<Klijent> Klijenti { get; set; }

        public DbSet<Polise> Polise { get; set; }

        public DbSet<Vozilo> Vozila { get; set; }

        public DbSet<ProdajaPolise> Prodaje { get; set; }

        public DbSet<UslovOsiguranja> UslovOsiguranja { get; set; }

        
    }
}
