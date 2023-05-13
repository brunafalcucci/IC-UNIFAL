using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class ChemicalTreatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column(TypeName = "int8")]
        public int? Id { get; set; }
        
        [Column(TypeName = "varchar")]
        [Required]
        public string? IndustrialSector { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        public string? IndustryName { get; set; }

        [Column(TypeName = "varchar")]
        public string? ChemicalTreatmentValue { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Recycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? RawMaterials { get; set; }

        [Column(TypeName = "varchar")]
        public string? WasteDisposal { get; set; }

        [Column(TypeName = "varchar")]
        public string? OtherMaterials { get; set; }

        [Column(TypeName = "varchar")]
        public string? Liquid { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solid { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solvents { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solids { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solutes { get; set; }

        [Column(TypeName = "varchar")]
        public string? SW { get; set; }

        [Column(TypeName = "varchar")]
        public string? CWP { get; set; }

        [Column(TypeName = "varchar")]
        public string? Maintenance { get; set; }

        [Column(TypeName = "varchar")]
        public string? WW { get; set; }

        [Column(TypeName = "varchar")]
        public string? Oil { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecyclingOfInkAndCleaningSolventResidues { get; set; }

        [Column(TypeName = "varchar")]
        public string? Miscellaneous { get; set; }

        [Column(TypeName = "varchar")]
        public string? Metals { get; set; }

        [Column(TypeName = "varchar")]
        public string? Sand { get; set; }

        [Column(TypeName = "varchar")]
        public string? General { get; set; }

        [Column(TypeName = "varchar")]
        public string? Reduction { get; set; }

        [Column(TypeName = "varchar")]
        public string? Restoration { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfMaterialsWithLessEnergyUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? AlterRawMaterialForLessEmission { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfMaterialsInRenewableContainers { get; set; }

        [Column(TypeName = "varchar")]
        public string? WBS { get; set; }

        [Column(TypeName = "varchar")]
        public string? SolutesLevel2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? Flocculation { get; set; }

        [Column(TypeName = "varchar")]
        public string? SludgeRemoval { get; set; }

        [Column(TypeName = "varchar")]
        public string? HeatGeneration { get; set; }

        [Column(TypeName = "varchar")]
        public string? Operation { get; set; }

        [Column(TypeName = "varchar")]
        public string? SaleOfCombustibleWaste { get; set; }

        [Column(TypeName = "varchar")]
        public string? ManufacturersWornSolutions { get; set; }

        [Column(TypeName = "varchar")]
        public string? WasteDisposalLevel2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfHydraulicOilInTheIndustrialProcess { get; set; }

        [Column(TypeName = "varchar")]
        public string? DistilledWaterRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfDistilledWaterInOtherApplications { get; set; }

        [Column(TypeName = "varchar")]
        public string? HydraulicOilReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfUsedOil { get; set; }

        [Column(TypeName = "varchar")]
        public string? SaleOfRecyclerOil { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solutions { get; set; }

        [Column(TypeName = "varchar")]
        public string? Reuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? Sale { get; set; }

        [Column(TypeName = "varchar")]
        public string? Recovery { get; set; }

        [Column(TypeName = "varchar")]
        public string? FoundrySandRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfSandForOtherPurposes { get; set; }

        [Column(TypeName = "varchar")]
        public string? Recycle { get; set; }

        [Column(TypeName = "varchar")]
        public string? DecreaseContaminationOfEndPieces { get; set; }

        [Column(TypeName = "varchar")]
        public string? Scraps { get; set; }

        [Column(TypeName = "varchar")]
        public string? Use { get; set; }

        [Column(TypeName = "varchar")]
        public string? Emissions { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solvent { get; set; }

        [Column(TypeName = "varchar")]
        public string? RestorationLevel2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfAqueousCleaningSystem { get; set; }

        [Column(TypeName = "varchar")]
        public string? FinishIndustrialProcessWithWaterBasedProduct { get; set; }

        [Column(TypeName = "varchar")]
        public string? ProductsWithNeutralPH { get; set; }

        [Column(TypeName = "varchar")]
        public string? InorganicSolutions { get; set; }

        [Column(TypeName = "varchar")]
        public string? ConvertHydrocarbonCleanersToLessToxicOnes { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfFlocculantsToReduceSludge { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfPrecipitatingAgentsInWasteWaterTreatment { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfGreenhouseFilterToReduceSludge { get; set; }

        [Column(TypeName = "varchar")]
        public string? RemovalOfSludgeFromEquipmentTanks { get; set; }

        [Column(TypeName = "varchar")]
        public string? InstallationOfIncineratorForSolidWaste { get; set; }

        [Column(TypeName = "varchar")]
        public string? BurningOfHeatWoodByProducts { get; set; }

        [Column(TypeName = "varchar")]
        public string? WasteOilBurning { get; set; }

        [Column(TypeName = "varchar")]
        public string? DirectWasteGasesToTheBoiler { get; set; }

        [Column(TypeName = "varchar")]
        public string? BurnWastePaperToGenerateHeat { get; set; }

        [Column(TypeName = "varchar")]
        public string? CheapWasteRemoval { get; set; }

        [Column(TypeName = "varchar")]
        public string? InstallationOfDisposalEquipment { get; set; }

        [Column(TypeName = "varchar")]
        public string? EquipmentCleaningTreatmentAndReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? DevelopmentOfSpentSolutionsToTheManufacturer { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecyclingSpentTanningSolution { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfUsedAcidBaths { get; set; }

        [Column(TypeName = "varchar")]
        public string? MetalWorkingFluidReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? SaleOfUsedSheetsForRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? SeparationOfMetalsForSaleForRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecoveryOfMetalsForReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? FilmRecyclingForSilverReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? FoundryAreaRecovery { get; set; }

        [Column(TypeName = "varchar")]
        public string? SeparationAndRecyclingOfScrapForFoundry { get; set; }

        [Column(TypeName = "varchar")]
        public string? NonFerrousPowderRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? PaperProductRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? RubberProductRecycling { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfScrapGlass { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfScrapPlasticParts { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReuseOfPrintedPaperScrap { get; set; }

        [Column(TypeName = "varchar")]
        public string? Water { get; set; }

        [Column(TypeName = "varchar")]
        public string? Employees { get; set; }

        [Column(TypeName = "varchar")]
        public string? Stopper { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? SteamMinimization { get; set; }

        [Column(TypeName = "varchar")]
        public string? RemovingRollersFromCleaningMachines { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solvent1 { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solvent2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solvent3 { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterLevel2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? SolventLevel2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfSolventThatCanBeReused { get; set; }

        [Column(TypeName = "varchar")]
        public string? SubstituteHexavalentChromiumForTrivalent { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReplaceHeavyMetalReagentsWithNonHazardousOnes { get; set; }

        [Column(TypeName = "varchar")]
        public string? DryAllPartsOfTheWaterSeparator { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfDeionizedWaterToRinseParts { get; set; }

        [Column(TypeName = "varchar")]
        public string? AvoidExcessSolventByOperators { get; set; }

        [Column(TypeName = "varchar")]
        public string? NumberOfEmployeesForPartsWashing { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfFittedLids { get; set; }

        [Column(TypeName = "varchar")]
        public string? FloatingLidsOnMaterialTanks { get; set; }

        [Column(TypeName = "varchar")]
        public string? DecreaseSteamLosses { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfSteamGasRecovery { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfWaterBasedAdhesives { get; set; }

        [Column(TypeName = "varchar")]
        public string? ConversionOfLiquidMaterialsForCleaning { get; set; }

        [Column(TypeName = "varchar")]
        public string? SolventReplacementForWaterBasedCuttingFluids { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfLessToxicAndVolatileSolvents { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfWaterBasedPaints { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfSoyBasedPaints { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}