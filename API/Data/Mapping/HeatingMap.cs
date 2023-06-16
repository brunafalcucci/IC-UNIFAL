using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class HeatingMap : IEntityTypeConfiguration<Heating>
    {
        public void Configure(EntityTypeBuilder<Heating> builder)
        {
            builder.ToTable("Heating");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_heating");

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

            builder.Property(prop => prop.HeatingValue)
                .HasColumnName("HeatingValue")
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

            builder.Property(prop => prop.Heat)
                .HasColumnName("Heat")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Temperature)
                .HasColumnName("Temperature")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Fluid)
                .HasColumnName("Fluid")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AirType)
                .HasColumnName("AirType")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Inspection)
                .HasColumnName("Inspection")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Isolation)
                .HasColumnName("Isolation")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Use)
                .HasColumnName("Use")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Place)
                .HasColumnName("Place")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}