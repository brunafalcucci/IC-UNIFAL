using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class DadosMap : IEntityTypeConfiguration<Dados>
    {
        public void Configure(EntityTypeBuilder<Dados> builder)
        {
            builder.ToTable("Dados");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_dados");

            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int8");
            
            builder.Property(prop => prop.City)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("City")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Dado1)
                .HasColumnName("Dado1")
                .HasColumnType("numeric");

           builder.Property(prop => prop.Dado2)
                .HasColumnName("Dado2")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Dado3)
                .HasColumnName("Dado3")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Dado4)
                .HasColumnName("Dado4")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Dado5)
                .HasColumnName("Dado5")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Ultima_atualizacao)
                .HasColumnName("Ultima_atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}