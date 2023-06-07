using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class WasteEnergyUseMap : IEntityTypeConfiguration<WasteEnergyUse>
    {
        public void Configure(EntityTypeBuilder<WasteEnergyUse> builder)
        {
            builder.ToTable("WasteEnergyUse");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_wasteEnergyUse");

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

            builder.Property(prop => prop.WasteEnergyUseValue)
                .HasColumnName("WasteEnergyUseValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.ChemicalTreatment)
                .HasColumnName("ChemicalTreatment")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.MechanicalTreatment)
                .HasColumnName("MechanicalTreatment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}