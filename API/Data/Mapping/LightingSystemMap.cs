using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class LightingSystemMap : IEntityTypeConfiguration<LightingSystem>
    {
        public void Configure(EntityTypeBuilder<LightingSystem> builder)
        {
            builder.ToTable("LightingSystem");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_lightingSystem");

            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int8");
            
            builder.Property(prop => prop.IndustrialSector)
                .IsRequired()
                .HasColumnName("IndustrialSector")
                .HasColumnType("varchar");

            builder.Property(prop => prop.IndustryName)
                .IsRequired()
                .HasColumnName("IndustryName")
                .HasColumnType("varchar");

            builder.Property(prop => prop.LightingSystemValue)
                .HasColumnName("LightingSystemValue")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Cleaning)
                .HasColumnName("Cleaning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ConstructionStructure)
                .HasColumnName("ConstructionStructure")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Operation)
                .HasColumnName("Operation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Reactor)
                .HasColumnName("Reactor")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}