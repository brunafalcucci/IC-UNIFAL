using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class MechanicalTreatmentMap : IEntityTypeConfiguration<MechanicalTreatment>
    {
        public void Configure(EntityTypeBuilder<MechanicalTreatment> builder)
        {
            builder.ToTable("MechanicalTreatment");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_mechanicalTreatment");

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

            builder.Property(prop => prop.MechanicalTreatmentValue)
                .HasColumnName("MechanicalTreatmentValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.PostGenerationTreatment)
                .HasColumnName("PostGenerationTreatment")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.WaterUse)
                .HasColumnName("WaterUse")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Neutralization)
                .HasColumnName("Neutralization")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.MaterialContamination)
                .HasColumnName("MaterialContamination")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RemovalOfContaminants)
                .HasColumnName("RemovalOfContaminants")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Utility)
                .HasColumnName("Utility")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Quality)
                .HasColumnName("Quality")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Reduction)
                .HasColumnName("Reduction")
                .HasColumnType("varchar");

            builder.Property(prop => prop.PH)
                .HasColumnName("PH")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Redox)
                .HasColumnName("Redox")
                .HasColumnType("varchar");

            builder.Property(prop => prop.OtherMethods)
                .HasColumnName("OtherMethods")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Evaporator)
                .HasColumnName("Evaporator")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReverseOsmosis)
                .HasColumnName("ReverseOsmosis")
                .HasColumnType("varchar");

            builder.Property(prop => prop.BiologicalTreatment)
                .HasColumnName("BiologicalTreatment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Chemistry)
                .HasColumnName("Chemistry")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Physicist)
                .HasColumnName("Physicist")
                .HasColumnType("varchar");

            builder.Property(prop => prop.CCWE)
                .HasColumnName("CCWE")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WT)
                .HasColumnName("WT")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DecreasedContaminationOfTreatmentWater)
                .HasColumnName("DecreasedContaminationOfTreatmentWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfDeionizedWater)
                .HasColumnName("UseOfDeionizedWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RegularCleaningOfDirtOnProductionLinesThatUseWater)
                .HasColumnName("RegularCleaningOfDirtOnProductionLinesThatUseWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Conversion)
                .HasColumnName("Conversion")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Equipment)
                .HasColumnName("Equipment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Use)
                .HasColumnName("Use")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DistillerUse)
                .HasColumnName("DistillerUse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfAbsorption)
                .HasColumnName("UseOfAbsorption")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfAdsorption)
                .HasColumnName("UseOfAdsorption")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfMagneticSieve)
                .HasColumnName("UseOfMagneticSieve")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfFiltering)
                .HasColumnName("UseOfFiltering")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfDecanter)
                .HasColumnName("UseOfDecanter")
                .HasColumnType("varchar");

            builder.Property(prop => prop.CycloneSeparation)
                .HasColumnName("CycloneSeparation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Recover)
                .HasColumnName("Recover")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Equipments)
                .HasColumnName("Equipments")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReplacementOfChlorineByO2)
                .HasColumnName("ReplacementOfChlorineByO2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterDevelopment)
                .HasColumnName("WaterDevelopment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Chlorination)
                .HasColumnName("Chlorination")
                .HasColumnType("varchar");

            builder.Property(prop => prop.QuantificationOfWaterUse)
                .HasColumnName("QuantificationOfWaterUse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfValvesToControlEquipmentFlow)
                .HasColumnName("UseOfValvesToControlEquipmentFlow")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReplacementOfTreatedWaterWithWellWater)
                .HasColumnName("ReplacementOfTreatedWaterWithWellWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterLevelControlInEquipment)
                .HasColumnName("WaterLevelControlInEquipment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.EliminationOfLeaksInWaterLinesAndValves)
                .HasColumnName("EliminationOfLeaksInWaterLinesAndValves")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.ReplacementOfWaterRegretInProcesses)
                .HasColumnName("ReplacementOfWaterRegretInProcesses")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReplacementOfWaterCoolingInProcesses)
                .HasColumnName("ReplacementOfWaterCoolingInProcesses")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReductionInWaterUse)
                .HasColumnName("ReductionInWaterUse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfCountercurrentRinsing)
                .HasColumnName("UseOfCountercurrentRinsing")
                .HasColumnType("varchar");

            builder.Property(prop => prop.MinimalUseOfCoolingWater)
                .HasColumnName("MinimalUseOfCoolingWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecoveryOfMetalsFromWater)
                .HasColumnName("RecoveryOfMetalsFromWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterTreatmentAndReuse)
                .HasColumnName("WaterTreatmentAndReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecoveryAndReuseOfCoolingWater)
                .HasColumnName("RecoveryAndReuseOfCoolingWater")
                .HasColumnType("varchar");
                
            builder.Property(prop => prop.UseOfClosedProcessInTheProductionOfWasteWater)
                .HasColumnName("UseOfClosedProcessInTheProductionOfWasteWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReplacementOfCoolingWaterInIndustry)
                .HasColumnName("ReplacementOfCoolingWaterInIndustry")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecycledWaterMeasurementToReduceSewageFees)
                .HasColumnName("RecycledWaterMeasurementToReduceSewageFees")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterTreatmentByMagneticTechnology)
                .HasColumnName("WaterTreatmentByMagneticTechnology")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ImprovedProductionOfDeionizedWater)
                .HasColumnName("ImprovedProductionOfDeionizedWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecyclingOfChlorinatedWater)
                .HasColumnName("RecyclingOfChlorinatedWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseTheChlorinationWashWater)
                .HasColumnName("UseTheChlorinationWashWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UsingChlorineInTheGasPhase)
                .HasColumnName("UsingChlorineInTheGasPhase")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}