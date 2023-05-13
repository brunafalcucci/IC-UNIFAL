using Application.Models;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Application.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<CriticalityIndex> CriticalityIndex => Set<CriticalityIndex>();
        public DbSet<ChemicalTreatment> ChemicalTreatment => Set<ChemicalTreatment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CriticalityIndex>(new CriticalityIndexMap().Configure);
            modelBuilder.Entity<ChemicalTreatment>(new ChemicalTreatmentMap().Configure);
        }
    }
}
