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

        public DbSet<Dados> Dados => Set<Dados>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dados>(new DadosMap().Configure);
        }
    }
}
