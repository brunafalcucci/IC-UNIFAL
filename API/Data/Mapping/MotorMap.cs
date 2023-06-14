using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class MotorMap : IEntityTypeConfiguration<Motor>
    {
        public void Configure(EntityTypeBuilder<Motor> builder)
        {
            builder.ToTable("Motor");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_motor");

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

            builder.Property(prop => prop.MotorValue)
                .HasColumnName("MotorValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Temperature)
                .HasColumnName("Temperature")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Management)
                .HasColumnName("Management")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Performance)
                .HasColumnName("Performance")
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

            builder.Property(prop => prop.Noise)
                .HasColumnName("Noise")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Operation)
                .HasColumnName("Operation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ventilation)
                .HasColumnName("Ventilation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}