using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class CondenserMap : IEntityTypeConfiguration<Condenser>
    {
        public void Configure(EntityTypeBuilder<Condenser> builder)
        {
            builder.ToTable("Condenser");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_condenser");

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

            builder.Property(prop => prop.CondenserValue)
                .HasColumnName("CondenserValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Thermodynamics)
                .HasColumnName("Thermodynamics")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ventilation)
                .HasColumnName("Ventilation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Isolation)
                .HasColumnName("Isolation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Local)
                .HasColumnName("Local")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Heat)
                .HasColumnName("Heat")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Condensed)
                .HasColumnName("Condensed")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Pressure)
                .HasColumnName("Pressure")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}