using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class VentilationMap : IEntityTypeConfiguration<Ventilation>
    {
        public void Configure(EntityTypeBuilder<Ventilation> builder)
        {
            builder.ToTable("Ventilation");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_ventilation");

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

            builder.Property(prop => prop.VentilationValue)
                .HasColumnName("VentilationValue")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Use)
                .HasColumnName("Use")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FanControl)
                .HasColumnName("FanControl")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Functionality)
                .HasColumnName("Functionality")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AirReduction)
                .HasColumnName("AirReduction")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AirRecycling)
                .HasColumnName("AirRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}