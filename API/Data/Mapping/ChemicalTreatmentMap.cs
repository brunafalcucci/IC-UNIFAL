using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class ChemicalTreatmentMap : IEntityTypeConfiguration<ChemicalTreatment>
    {
        public void Configure(EntityTypeBuilder<ChemicalTreatment> builder)
        {
            builder.ToTable("ChemicalTreatment");

            builder.HasKey(prop => prop.Id)
                .HasName("pk_chemicalTreatment");

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

            builder.Property(prop => prop.ChemicalTreatmentValue)
                .HasColumnName("ChemicalTreatmentValue")
                .HasColumnType("varchar");

           builder.Property(prop => prop.Recycling)
                .HasColumnName("Recycling")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.RawMaterials)
                .HasColumnName("RawMaterials")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.WasteDisposal)
                .HasColumnName("WasteDisposal")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.OtherMaterials)
                .HasColumnName("OtherMaterials")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Liquid)
                .HasColumnName("Liquid")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solid)
                .HasColumnName("Solid")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solvents)
                .HasColumnName("Solvents")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solids)
                .HasColumnName("Solids")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solutes)
                .HasColumnName("Solutes")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SW)
                .HasColumnName("SW")
                .HasColumnType("varchar");

            builder.Property(prop => prop.CWP)
                .HasColumnName("CWP")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Maintenance)
                .HasColumnName("Maintenance")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WhiteWater)
                .HasColumnName("WhiteWater")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Oil)
                .HasColumnName("Oil")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecyclingOfInkAndCleaningSolventResidues)
                .HasColumnName("RecyclingOfInkAndCleaningSolventResidues")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Miscellaneous)
                .HasColumnName("Miscellaneous")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Metals)
                .HasColumnName("Metals")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Sand)
                .HasColumnName("Sand")
                .HasColumnType("varchar");

            builder.Property(prop => prop.General)
                .HasColumnName("General")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Reduction)
                .HasColumnName("Reduction")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Restoration)
                .HasColumnName("Restoration")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfMaterialsWithLessEnergyUse)
                .HasColumnName("UseOfMaterialsWithLessEnergyUse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AlterRawMaterialForLessEmission)
                .HasColumnName("AlterRawMaterialForLessEmission")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfMaterialsInRenewableContainers)
                .HasColumnName("UseOfMaterialsInRenewableContainers")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterBasedSubstitutes)
                .HasColumnName("WaterBasedSubstitutes")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SolutesLevel2)
                .HasColumnName("SolutesLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Flocculation)
                .HasColumnName("Flocculation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SludgeRemoval)
                .HasColumnName("SludgeRemoval")
                .HasColumnType("varchar");

            builder.Property(prop => prop.HeatGeneration)
                .HasColumnName("HeatGeneration")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Operation)
                .HasColumnName("Operation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SaleOfCombustibleWaste)
                .HasColumnName("SaleOfCombustibleWaste")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ManufacturersWornSolutions)
                .HasColumnName("ManufacturersWornSolutions")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WasteDisposalLevel2)
                .HasColumnName("WasteDisposalLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfHydraulicOilInTheIndustrialProcess)
                .HasColumnName("UseOfHydraulicOilInTheIndustrialProcess")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DistilledWaterRecycling)
                .HasColumnName("DistilledWaterRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfDistilledWaterInOtherApplications)
                .HasColumnName("ReuseOfDistilledWaterInOtherApplications")
                .HasColumnType("varchar");

            builder.Property(prop => prop.HydraulicOilReuse)
                .HasColumnName("HydraulicOilReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfUsedOil)
                .HasColumnName("ReuseOfUsedOil")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SaleOfRecyclerOil)
                .HasColumnName("SaleOfRecyclerOil")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solutions)
                .HasColumnName("Solutions")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Reuse)
                .HasColumnName("Reuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Sale)
                .HasColumnName("Sale")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Recovery)
                .HasColumnName("Recovery")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FoundrySandRecycling)
                .HasColumnName("FoundrySandRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfSandForOtherPurposes)
                .HasColumnName("UseOfSandForOtherPurposes")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Recycle)
                .HasColumnName("Recycle")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DecreaseContaminationOfEndPieces)
                .HasColumnName("DecreaseContaminationOfEndPieces")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Scraps)
                .HasColumnName("Scraps")
                .HasColumnType("varchar");
                
            builder.Property(prop => prop.Use)
                .HasColumnName("Use")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Emissions)
                .HasColumnName("Emissions")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Solvent)
                .HasColumnName("Solvent")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RestorationLevel2)
                .HasColumnName("RestorationLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfAqueousCleaningSystem)
                .HasColumnName("UseOfAqueousCleaningSystem")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FinishIndustrialProcessWithWaterBasedProduct)
                .HasColumnName("FinishIndustrialProcessWithWaterBasedProduct")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ProductsWithNeutralPH)
                .HasColumnName("ProductsWithNeutralPH")
                .HasColumnType("varchar");

            builder.Property(prop => prop.InorganicSolutions)
                .HasColumnName("InorganicSolutions")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ConvertHydrocarbonCleanersToLessToxicOnes)
                .HasColumnName("ConvertHydrocarbonCleanersToLessToxicOnes")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfFlocculantsToReduceSludge)
                .HasColumnName("UseOfFlocculantsToReduceSludge")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfPrecipitatingAgentsInWasteWaterTreatment)
                .HasColumnName("UseOfPrecipitatingAgentsInWasteWaterTreatment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfGreenhouseFilterToReduceSludge)
                .HasColumnName("UseOfGreenhouseFilterToReduceSludge")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RemovalOfSludgeFromEquipmentTanks)
                .HasColumnName("RemovalOfSludgeFromEquipmentTanks")
                .HasColumnType("varchar");

            builder.Property(prop => prop.InstallationOfIncineratorForSolidWaste)
                .HasColumnName("InstallationOfIncineratorForSolidWaste")
                .HasColumnType("varchar");

            builder.Property(prop => prop.BurningOfHeatWoodByProducts)
                .HasColumnName("BurningOfHeatWoodByProducts")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WasteOilBurning)
                .HasColumnName("WasteOilBurning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DirectWasteGasesToTheBoiler)
                .HasColumnName("DirectWasteGasesToTheBoiler")
                .HasColumnType("varchar");

            builder.Property(prop => prop.BurnWastePaperToGenerateHeat)
                .HasColumnName("BurnWastePaperToGenerateHeat")
                .HasColumnType("varchar");

            builder.Property(prop => prop.CheapWasteRemoval)
                .HasColumnName("CheapWasteRemoval")
                .HasColumnType("varchar");

            builder.Property(prop => prop.InstallationOfDisposalEquipment)
                .HasColumnName("InstallationOfDisposalEquipment")
                .HasColumnType("varchar");

            builder.Property(prop => prop.EquipmentCleaningTreatmentAndReuse)
                .HasColumnName("EquipmentCleaningTreatmentAndReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DevelopmentOfSpentSolutionsToTheManufacturer)
                .HasColumnName("DevelopmentOfSpentSolutionsToTheManufacturer")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecyclingSpentTanningSolution)
                .HasColumnName("RecyclingSpentTanningSolution")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfUsedAcidBaths)
                .HasColumnName("ReuseOfUsedAcidBaths")
                .HasColumnType("varchar");

            builder.Property(prop => prop.MetalWorkingFluidReuse)
                .HasColumnName("MetalWorkingFluidReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SaleOfUsedSheetsForRecycling)
                .HasColumnName("SaleOfUsedSheetsForRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SeparationOfMetalsForSaleForRecycling)
                .HasColumnName("SeparationOfMetalsForSaleForRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RecoveryOfMetalsForReuse)
                .HasColumnName("RecoveryOfMetalsForReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FilmRecyclingForSilverReuse)
                .HasColumnName("FilmRecyclingForSilverReuse")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FoundryAreaRecovery)
                .HasColumnName("FoundryAreaRecovery")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SeparationAndRecyclingOfScrapForFoundry)
                .HasColumnName("SeparationAndRecyclingOfScrapForFoundry")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.NonFerrousPowderRecycling)
                .HasColumnName("NonFerrousPowderRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.PaperProductRecycling)
                .HasColumnName("PaperProductRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RubberProductRecycling)
                .HasColumnName("RubberProductRecycling")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfScrapGlass)
                .HasColumnName("ReuseOfScrapGlass")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfScrapPlasticParts)
                .HasColumnName("ReuseOfScrapPlasticParts")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseOfPrintedPaperScrap)
                .HasColumnName("ReuseOfPrintedPaperScrap")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.Water)
                .HasColumnName("Water")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Employees)
                .HasColumnName("Employees")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Stopper)
                .HasColumnName("Stopper")
                .HasColumnType("varchar");
            
            builder.Property(prop => prop.SteamMinimization)
                .HasColumnName("SteamMinimization")
                .HasColumnType("varchar");

            builder.Property(prop => prop.RemovingRollersFromCleaningMachines)
                .HasColumnName("RemovingRollersFromCleaningMachines")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Cleaning)
                .HasColumnName("Cleaning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Distillation)
                .HasColumnName("Distillation")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReuseLevel2)
                .HasColumnName("ReuseLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.WaterLevel2)
                .HasColumnName("WaterLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SolventLevel2)
                .HasColumnName("SolventLevel2")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfSolventThatCanBeReused)
                .HasColumnName("UseOfSolventThatCanBeReused")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SubstituteHexavalentChromiumForTrivalent)
                .HasColumnName("SubstituteHexavalentChromiumForTrivalent")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ReplaceHeavyMetalReagentsWithNonHazardousOnes)
                .HasColumnName("ReplaceHeavyMetalReagentsWithNonHazardousOnes")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DryAllPartsOfTheWaterSeparator)
                .HasColumnName("DryAllPartsOfTheWaterSeparator")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfDeionizedWaterToRinseParts)
                .HasColumnName("UseOfDeionizedWaterToRinseParts")
                .HasColumnType("varchar");

            builder.Property(prop => prop.AvoidExcessSolventByOperators)
                .HasColumnName("AvoidExcessSolventByOperators")
                .HasColumnType("varchar");

            builder.Property(prop => prop.NumberOfEmployeesForPartsWashing)
                .HasColumnName("NumberOfEmployeesForPartsWashing")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfFittedLids)
                .HasColumnName("UseOfFittedLids")
                .HasColumnType("varchar");

            builder.Property(prop => prop.FloatingLidsOnMaterialTanks)
                .HasColumnName("FloatingLidsOnMaterialTanks")
                .HasColumnType("varchar");

            builder.Property(prop => prop.DecreaseSteamLosses)
                .HasColumnName("DecreaseSteamLosses")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfSteamGasRecovery)
                .HasColumnName("UseOfSteamGasRecovery")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfWaterBasedAdhesives)
                .HasColumnName("UseOfWaterBasedAdhesives")
                .HasColumnType("varchar");

            builder.Property(prop => prop.ConversionOfLiquidMaterialsForCleaning)
                .HasColumnName("ConversionOfLiquidMaterialsForCleaning")
                .HasColumnType("varchar");

            builder.Property(prop => prop.SolventReplacementForWaterBasedCuttingFluids)
                .HasColumnName("SolventReplacementForWaterBasedCuttingFluids")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfLessToxicAndVolatileSolvents)
                .HasColumnName("UseOfLessToxicAndVolatileSolvents")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfWaterBasedPaints)
                .HasColumnName("UseOfWaterBasedPaints")
                .HasColumnType("varchar");

            builder.Property(prop => prop.UseOfSoyBasedPaints)
                .HasColumnName("UseOfSoyBasedPaints")
                .HasColumnType("varchar");

            builder.Property(prop => prop.Ultima_Atualizacao)
                .HasColumnName("Ultima_Atualizacao")
                .HasColumnType("timestamp(0) without time zone")
                .HasDefaultValueSql("now()");
        }
    }
}