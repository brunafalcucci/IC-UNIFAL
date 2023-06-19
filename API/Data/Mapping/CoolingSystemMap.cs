using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class CoolingSystemMap : IEntityTypeConfiguration<CoolingSystem>
    {
        public void Configure(EntityTypeBuilder<CoolingSystem> builder)
        {
            builder.ToTable("CoolingSystem");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_coolingSystem");

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

            builder.Property(prop => prop.CoolingSystemValue)
                .HasColumnName("CoolingSystemValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.SystemOperationCooling)
                .HasColumnName("SystemOperationCooling")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.HeatTransferCooling)
                .HasColumnName("HeatTransferCooling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Condenser)
                .HasColumnName("Condenser")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Thermodynamics)
                .HasColumnName("Thermodynamics")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Cleaning)
                .HasColumnName("Cleaning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ChillerWaste)
                .HasColumnName("ChillerWaste")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Air)
                .HasColumnName("Air")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Pressure)
                .HasColumnName("Pressure")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Refrigeration)
                .HasColumnName("Refrigeration")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Water)
                .HasColumnName("Water")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Temperature)
                .HasColumnName("Temperature")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Heat)
                .HasColumnName("Heat")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}