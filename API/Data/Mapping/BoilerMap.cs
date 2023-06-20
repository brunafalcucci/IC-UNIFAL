using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class BoilerMap : IEntityTypeConfiguration<Boiler>
    {
        public void Configure(EntityTypeBuilder<Boiler> builder)
        {
            builder.ToTable("Boiler");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_boiler");

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

            builder.Property(prop => prop.BoilerValue)
                .HasColumnName("BoilerValue")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Thermodynamics)
                .HasColumnName("Thermodynamics")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Pressure)
                .HasColumnName("Pressure")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Condensed)
                .HasColumnName("Condensed")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Heat)
                .HasColumnName("Heat")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Place)
                .HasColumnName("Place")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

            builder.Property(prop => prop.StudiesAndMeasures)
                .HasColumnName("StudiesAndMeasures")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Inspection)
                .HasColumnName("Inspection")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Maintenance)
                .HasColumnName("Maintenance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}