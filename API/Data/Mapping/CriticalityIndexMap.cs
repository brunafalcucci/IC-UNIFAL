using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class CriticalityIndexMap : IEntityTypeConfiguration<CriticalityIndex>
    {
        public void Configure(EntityTypeBuilder<CriticalityIndex> builder)
        {
            builder.ToTable("CriticalityIndex");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_criticalityIndex");

            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int8");
            
            builder.Property(prop => prop.City)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("City")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.CriticalityIndexValue)
                .HasColumnName("CriticalityIndexValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.CostsManagement)
                .HasColumnName("CostsManagement")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.IndustrialManagement)
                .HasColumnName("IndustrialManagement")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.EnvironmentalQuality)
                .HasColumnName("EnvironmentalQuality")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Investments)
                .HasColumnName("Investments")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ElectricityExpensives)
                .HasColumnName("ElectricityExpensives")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Predictive)
                .HasColumnName("Predictive")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Maintenance)
                .HasColumnName("Maintenance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Preventive)
                .HasColumnName("Preventive")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Corrective)
                .HasColumnName("Corrective")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Governance)
                .HasColumnName("Governance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.EnvironmentalRisks)
                .HasColumnName("EnvironmentalRisks")
                .HasColumnType("varchar");

            builder.Property(prop => prop.EnergyUse)
                .HasColumnName("EnergyUse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.EnergyGeneration)
                .HasColumnName("EnergyGeneration")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Technology)
                .HasColumnName("Technology")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Process)
                .HasColumnName("Process")
                .HasColumnType("varchar");

            builder.Property(prop => prop.TechnologyDataCollection)
                .HasColumnName("TechnologyDataCollection")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Instrumentation)
                .HasColumnName("Instrumentation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.PredictiveOperation)
                .HasColumnName("PredictiveOperation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Operation_Work)
                .HasColumnName("Operation_Work")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AvailabilityOfRepairPersonnel)
                .HasColumnName("AvailabilityOfRepairPersonnel")
                .HasColumnType("varchar");

            builder.Property(prop => prop.OperationSensitivity)
                .HasColumnName("OperationSensitivity")
                .HasColumnType("varchar");

            builder.Property(prop => prop.MeanTimeBetweenFailures)
                .HasColumnName("MeanTimeBetweenFailures")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WorkLoad)
                .HasColumnName("WorkLoad")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Activities)
                .HasColumnName("Activities")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AverageRepairTime)
                .HasColumnName("AverageRepairTime")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AvailabilityOfRequiredParts)
                .HasColumnName("AvailabilityOfRequiredParts")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SkillLevels)
                .HasColumnName("SkillLevels")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ManagementStrategy)
                .HasColumnName("ManagementStrategy")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Toxicity)
                .HasColumnName("Toxicity")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solubility)
                .HasColumnName("Solubility")
                .HasColumnType("varchar");

            builder.Property(prop => prop.No_Renewable)
                .HasColumnName("No_Renewable")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Renewable)
                .HasColumnName("Renewable")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Ultima_atualizacao)
                .HasColumnName("Ultima_atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}