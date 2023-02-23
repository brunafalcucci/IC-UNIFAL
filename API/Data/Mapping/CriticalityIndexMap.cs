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
                .HasColumnType("numeric");

           builder.Property(prop => prop.CostsManagement)
                .HasColumnName("CostsManagement")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.IndustrialManagement)
                .HasColumnName("IndustrialManagement")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.EnvironmentalQuality)
                .HasColumnName("EnvironmentalQuality")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Investments)
                .HasColumnName("Investments")
                .HasColumnType("numeric");

            builder.Property(prop => prop.ElectricityExpensives)
                .HasColumnName("ElectricityExpensives")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Predictive)
                .HasColumnName("Predictive")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Maintenance)
                .HasColumnName("Maintenance")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Preventive)
                .HasColumnName("Preventive")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Corrective)
                .HasColumnName("Corrective")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Governance)
                .HasColumnName("Governance")
                .HasColumnType("numeric");

            builder.Property(prop => prop.EnvironmentalRisks)
                .HasColumnName("EnvironmentalRisks")
                .HasColumnType("numeric");

            builder.Property(prop => prop.EnergyUse)
                .HasColumnName("EnergyUse")
                .HasColumnType("numeric");

            builder.Property(prop => prop.EnergyGeneration)
                .HasColumnName("EnergyGeneration")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Technology)
                .HasColumnName("Technology")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Process)
                .HasColumnName("Process")
                .HasColumnType("numeric");

            builder.Property(prop => prop.TechnologyDataCollection)
                .HasColumnName("TechnologyDataCollection")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Instrumentation)
                .HasColumnName("Instrumentation")
                .HasColumnType("numeric");

            builder.Property(prop => prop.PredictiveOperation)
                .HasColumnName("PredictiveOperation")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Operation_Work)
                .HasColumnName("Operation_Work")
                .HasColumnType("numeric");

            builder.Property(prop => prop.AvailabilityOfRepairPersonnel)
                .HasColumnName("AvailabilityOfRepairPersonnel")
                .HasColumnType("numeric");

            builder.Property(prop => prop.OperationSensitivity)
                .HasColumnName("OperationSensitivity")
                .HasColumnType("numeric");

            builder.Property(prop => prop.MeanTimeBetweenFailures)
                .HasColumnName("MeanTimeBetweenFailures")
                .HasColumnType("numeric");

            builder.Property(prop => prop.WorkLoad)
                .HasColumnName("WorkLoad")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Activities)
                .HasColumnName("Activities")
                .HasColumnType("numeric");

            builder.Property(prop => prop.AverageRepairTime)
                .HasColumnName("AverageRepairTime")
                .HasColumnType("numeric");

            builder.Property(prop => prop.AvailabilityOfRequiredParts)
                .HasColumnName("AvailabilityOfRequiredParts")
                .HasColumnType("numeric");

            builder.Property(prop => prop.SkillLevels)
                .HasColumnName("SkillLevels")
                .HasColumnType("numeric");

            builder.Property(prop => prop.ManagementStrategy)
                .HasColumnName("ManagementStrategy")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Toxicity)
                .HasColumnName("Toxicity")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Solubility)
                .HasColumnName("Solubility")
                .HasColumnType("numeric");

            builder.Property(prop => prop.No_Renewable)
                .HasColumnName("No_Renewable")
                .HasColumnType("numeric");

            builder.Property(prop => prop.Renewable)
                .HasColumnName("Renewable")
                .HasColumnType("numeric");
            
            builder.Property(prop => prop.Ultima_atualizacao)
                .HasColumnName("Ultima_atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}