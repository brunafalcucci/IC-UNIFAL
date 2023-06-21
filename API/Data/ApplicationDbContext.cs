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
        public DbSet<MechanicalTreatment> MechanicalTreatment => Set<MechanicalTreatment>();
        public DbSet<WasteEnergyUse> WasteEnergyUse => Set<WasteEnergyUse>();
        public DbSet<Motor> Motor => Set<Motor>();
        public DbSet<Heating> Heating => Set<Heating>();
        public DbSet<AirCompressor> AirCompressor => Set<AirCompressor>();
        public DbSet<CoolingSystem> CoolingSystem => Set<CoolingSystem>();
        public DbSet<LightingSystem> LightingSystem => Set<LightingSystem>();
        public DbSet<Condenser> Condenser => Set<Condenser>();
        public DbSet<Boiler> Boiler => Set<Boiler>();
        public DbSet<AirConditioning> AirConditioning => Set<AirConditioning>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CriticalityIndex>(new CriticalityIndexMap().Configure);
            modelBuilder.Entity<ChemicalTreatment>(new ChemicalTreatmentMap().Configure);
            modelBuilder.Entity<MechanicalTreatment>(new MechanicalTreatmentMap().Configure);
            modelBuilder.Entity<WasteEnergyUse>(new WasteEnergyUseMap().Configure);
            modelBuilder.Entity<Motor>(new MotorMap().Configure);
            modelBuilder.Entity<Heating>(new HeatingMap().Configure);
            modelBuilder.Entity<AirCompressor>(new AirCompressorMap().Configure);
            modelBuilder.Entity<CoolingSystem>(new CoolingSystemMap().Configure);
            modelBuilder.Entity<LightingSystem>(new LightingSystemMap().Configure);
            modelBuilder.Entity<Condenser>(new CondenserMap().Configure);
            modelBuilder.Entity<Boiler>(new BoilerMap().Configure);
            modelBuilder.Entity<AirConditioning>(new AirConditioningMap().Configure);
        }
    }
}
