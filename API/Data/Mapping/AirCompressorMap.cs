using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class AirCompressorMap : IEntityTypeConfiguration<AirCompressor>
    {
        public void Configure(EntityTypeBuilder<AirCompressor> builder)
        {
            builder.ToTable("AirCompressor");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_airCompressor");

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

            builder.Property(prop => prop.AirCompressorValue)
                .HasColumnName("AirCompressorValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Thermodynamics)
                .HasColumnName("Thermodynamics")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Use)
                .HasColumnName("Use")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Local)
                .HasColumnName("Local")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Moisture)
                .HasColumnName("Moisture")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Maintenance)
                .HasColumnName("Maintenance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Cleaning)
                .HasColumnName("Cleaning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Temperature)
                .HasColumnName("Temperature")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.InletPressureControl)
                .HasColumnName("InletPressureControl")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}