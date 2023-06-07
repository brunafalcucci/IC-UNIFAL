using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class criandotabelasnovas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChemicalTreatment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    ChemicalTreatmentValue = table.Column<string>(type: "varchar", nullable: true),
                    Recycling = table.Column<string>(type: "varchar", nullable: true),
                    RawMaterials = table.Column<string>(type: "varchar", nullable: true),
                    WasteDisposal = table.Column<string>(type: "varchar", nullable: true),
                    OtherMaterials = table.Column<string>(type: "varchar", nullable: true),
                    Liquid = table.Column<string>(type: "varchar", nullable: true),
                    Solid = table.Column<string>(type: "varchar", nullable: true),
                    Solvents = table.Column<string>(type: "varchar", nullable: true),
                    Solids = table.Column<string>(type: "varchar", nullable: true),
                    Solutes = table.Column<string>(type: "varchar", nullable: true),
                    SW = table.Column<string>(type: "varchar", nullable: true),
                    CWP = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    WhiteWater = table.Column<string>(type: "varchar", nullable: true),
                    Oil = table.Column<string>(type: "varchar", nullable: true),
                    RecyclingOfInkAndCleaningSolventResidues = table.Column<string>(type: "varchar", nullable: true),
                    Miscellaneous = table.Column<string>(type: "varchar", nullable: true),
                    Metals = table.Column<string>(type: "varchar", nullable: true),
                    Sand = table.Column<string>(type: "varchar", nullable: true),
                    General = table.Column<string>(type: "varchar", nullable: true),
                    Reduction = table.Column<string>(type: "varchar", nullable: true),
                    Restoration = table.Column<string>(type: "varchar", nullable: true),
                    UseOfMaterialsWithLessEnergyUse = table.Column<string>(type: "varchar", nullable: true),
                    AlterRawMaterialForLessEmission = table.Column<string>(type: "varchar", nullable: true),
                    UseOfMaterialsInRenewableContainers = table.Column<string>(type: "varchar", nullable: true),
                    WaterBasedSubstitutes = table.Column<string>(type: "varchar", nullable: true),
                    SolutesLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    Flocculation = table.Column<string>(type: "varchar", nullable: true),
                    SludgeRemoval = table.Column<string>(type: "varchar", nullable: true),
                    HeatGeneration = table.Column<string>(type: "varchar", nullable: true),
                    Operation = table.Column<string>(type: "varchar", nullable: true),
                    SaleOfCombustibleWaste = table.Column<string>(type: "varchar", nullable: true),
                    ManufacturersWornSolutions = table.Column<string>(type: "varchar", nullable: true),
                    WasteDisposalLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    UseOfHydraulicOilInTheIndustrialProcess = table.Column<string>(type: "varchar", nullable: true),
                    DistilledWaterRecycling = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfDistilledWaterInOtherApplications = table.Column<string>(type: "varchar", nullable: true),
                    HydraulicOilReuse = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfUsedOil = table.Column<string>(type: "varchar", nullable: true),
                    SaleOfRecyclerOil = table.Column<string>(type: "varchar", nullable: true),
                    Solutions = table.Column<string>(type: "varchar", nullable: true),
                    Reuse = table.Column<string>(type: "varchar", nullable: true),
                    Sale = table.Column<string>(type: "varchar", nullable: true),
                    Recovery = table.Column<string>(type: "varchar", nullable: true),
                    FoundrySandRecycling = table.Column<string>(type: "varchar", nullable: true),
                    UseOfSandForOtherPurposes = table.Column<string>(type: "varchar", nullable: true),
                    Recycle = table.Column<string>(type: "varchar", nullable: true),
                    DecreaseContaminationOfEndPieces = table.Column<string>(type: "varchar", nullable: true),
                    Scraps = table.Column<string>(type: "varchar", nullable: true),
                    Use = table.Column<string>(type: "varchar", nullable: true),
                    Emissions = table.Column<string>(type: "varchar", nullable: true),
                    Solvent = table.Column<string>(type: "varchar", nullable: true),
                    RestorationLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    UseOfAqueousCleaningSystem = table.Column<string>(type: "varchar", nullable: true),
                    FinishIndustrialProcessWithWaterBasedProduct = table.Column<string>(type: "varchar", nullable: true),
                    ProductsWithNeutralPH = table.Column<string>(type: "varchar", nullable: true),
                    InorganicSolutions = table.Column<string>(type: "varchar", nullable: true),
                    ConvertHydrocarbonCleanersToLessToxicOnes = table.Column<string>(type: "varchar", nullable: true),
                    UseOfFlocculantsToReduceSludge = table.Column<string>(type: "varchar", nullable: true),
                    UseOfPrecipitatingAgentsInWasteWaterTreatment = table.Column<string>(type: "varchar", nullable: true),
                    UseOfGreenhouseFilterToReduceSludge = table.Column<string>(type: "varchar", nullable: true),
                    RemovalOfSludgeFromEquipmentTanks = table.Column<string>(type: "varchar", nullable: true),
                    InstallationOfIncineratorForSolidWaste = table.Column<string>(type: "varchar", nullable: true),
                    BurningOfHeatWoodByProducts = table.Column<string>(type: "varchar", nullable: true),
                    WasteOilBurning = table.Column<string>(type: "varchar", nullable: true),
                    DirectWasteGasesToTheBoiler = table.Column<string>(type: "varchar", nullable: true),
                    BurnWastePaperToGenerateHeat = table.Column<string>(type: "varchar", nullable: true),
                    CheapWasteRemoval = table.Column<string>(type: "varchar", nullable: true),
                    InstallationOfDisposalEquipment = table.Column<string>(type: "varchar", nullable: true),
                    EquipmentCleaningTreatmentAndReuse = table.Column<string>(type: "varchar", nullable: true),
                    DevelopmentOfSpentSolutionsToTheManufacturer = table.Column<string>(type: "varchar", nullable: true),
                    RecyclingSpentTanningSolution = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfUsedAcidBaths = table.Column<string>(type: "varchar", nullable: true),
                    MetalWorkingFluidReuse = table.Column<string>(type: "varchar", nullable: true),
                    SaleOfUsedSheetsForRecycling = table.Column<string>(type: "varchar", nullable: true),
                    SeparationOfMetalsForSaleForRecycling = table.Column<string>(type: "varchar", nullable: true),
                    RecoveryOfMetalsForReuse = table.Column<string>(type: "varchar", nullable: true),
                    FilmRecyclingForSilverReuse = table.Column<string>(type: "varchar", nullable: true),
                    FoundryAreaRecovery = table.Column<string>(type: "varchar", nullable: true),
                    SeparationAndRecyclingOfScrapForFoundry = table.Column<string>(type: "varchar", nullable: true),
                    NonFerrousPowderRecycling = table.Column<string>(type: "varchar", nullable: true),
                    PaperProductRecycling = table.Column<string>(type: "varchar", nullable: true),
                    RubberProductRecycling = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfScrapGlass = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfScrapPlasticParts = table.Column<string>(type: "varchar", nullable: true),
                    ReuseOfPrintedPaperScrap = table.Column<string>(type: "varchar", nullable: true),
                    Water = table.Column<string>(type: "varchar", nullable: true),
                    Employees = table.Column<string>(type: "varchar", nullable: true),
                    Stopper = table.Column<string>(type: "varchar", nullable: true),
                    SteamMinimization = table.Column<string>(type: "varchar", nullable: true),
                    RemovingRollersFromCleaningMachines = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    Distillation = table.Column<string>(type: "varchar", nullable: true),
                    ReuseLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    WaterLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    SolventLevel2 = table.Column<string>(type: "varchar", nullable: true),
                    UseOfSolventThatCanBeReused = table.Column<string>(type: "varchar", nullable: true),
                    SubstituteHexavalentChromiumForTrivalent = table.Column<string>(type: "varchar", nullable: true),
                    ReplaceHeavyMetalReagentsWithNonHazardousOnes = table.Column<string>(type: "varchar", nullable: true),
                    DryAllPartsOfTheWaterSeparator = table.Column<string>(type: "varchar", nullable: true),
                    UseOfDeionizedWaterToRinseParts = table.Column<string>(type: "varchar", nullable: true),
                    AvoidExcessSolventByOperators = table.Column<string>(type: "varchar", nullable: true),
                    NumberOfEmployeesForPartsWashing = table.Column<string>(type: "varchar", nullable: true),
                    UseOfFittedLids = table.Column<string>(type: "varchar", nullable: true),
                    FloatingLidsOnMaterialTanks = table.Column<string>(type: "varchar", nullable: true),
                    DecreaseSteamLosses = table.Column<string>(type: "varchar", nullable: true),
                    UseOfSteamGasRecovery = table.Column<string>(type: "varchar", nullable: true),
                    UseOfWaterBasedAdhesives = table.Column<string>(type: "varchar", nullable: true),
                    ConversionOfLiquidMaterialsForCleaning = table.Column<string>(type: "varchar", nullable: true),
                    SolventReplacementForWaterBasedCuttingFluids = table.Column<string>(type: "varchar", nullable: true),
                    UseOfLessToxicAndVolatileSolvents = table.Column<string>(type: "varchar", nullable: true),
                    UseOfWaterBasedPaints = table.Column<string>(type: "varchar", nullable: true),
                    UseOfSoyBasedPaints = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chemicalTreatment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MechanicalTreatment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    MechanicalTreatmentValue = table.Column<string>(type: "varchar", nullable: true),
                    PostGenerationTreatment = table.Column<string>(type: "varchar", nullable: true),
                    WaterUse = table.Column<string>(type: "varchar", nullable: true),
                    Neutralization = table.Column<string>(type: "varchar", nullable: true),
                    MaterialConcentration = table.Column<string>(type: "varchar", nullable: true),
                    RemovalOfContaminants = table.Column<string>(type: "varchar", nullable: true),
                    Utility = table.Column<string>(type: "varchar", nullable: true),
                    Quality = table.Column<string>(type: "varchar", nullable: true),
                    Reduction = table.Column<string>(type: "varchar", nullable: true),
                    PH = table.Column<string>(type: "varchar", nullable: true),
                    Redox = table.Column<string>(type: "varchar", nullable: true),
                    OtherMethods = table.Column<string>(type: "varchar", nullable: true),
                    Evaporator = table.Column<string>(type: "varchar", nullable: true),
                    ReverseOsmosis = table.Column<string>(type: "varchar", nullable: true),
                    BiologicalTreatment = table.Column<string>(type: "varchar", nullable: true),
                    Chemistry = table.Column<string>(type: "varchar", nullable: true),
                    Physicist = table.Column<string>(type: "varchar", nullable: true),
                    CCWE = table.Column<string>(type: "varchar", nullable: true),
                    WT = table.Column<string>(type: "varchar", nullable: true),
                    DecreasedContaminationOfTreatmentWater = table.Column<string>(type: "varchar", nullable: true),
                    UseOfDeionizedWater = table.Column<string>(type: "varchar", nullable: true),
                    RegularCleaningOfDirtOnProductionLinesThatUseWater = table.Column<string>(type: "varchar", nullable: true),
                    Conversion = table.Column<string>(type: "varchar", nullable: true),
                    Equipment = table.Column<string>(type: "varchar", nullable: true),
                    Use = table.Column<string>(type: "varchar", nullable: true),
                    DistillerUse = table.Column<string>(type: "varchar", nullable: true),
                    UseOfAbsorption = table.Column<string>(type: "varchar", nullable: true),
                    UseOfAdsorption = table.Column<string>(type: "varchar", nullable: true),
                    UseOfMagneticSieve = table.Column<string>(type: "varchar", nullable: true),
                    UseOfFiltering = table.Column<string>(type: "varchar", nullable: true),
                    UseOfDecanter = table.Column<string>(type: "varchar", nullable: true),
                    CycloneSeparation = table.Column<string>(type: "varchar", nullable: true),
                    Recover = table.Column<string>(type: "varchar", nullable: true),
                    Equipments = table.Column<string>(type: "varchar", nullable: true),
                    ReplacementOfChlorineByO2 = table.Column<string>(type: "varchar", nullable: true),
                    WaterDevelopment = table.Column<string>(type: "varchar", nullable: true),
                    Chlorination = table.Column<string>(type: "varchar", nullable: true),
                    QuantificationOfWaterUse = table.Column<string>(type: "varchar", nullable: true),
                    UseOfValvesToControlEquipmentFlow = table.Column<string>(type: "varchar", nullable: true),
                    ReplacementOfTreatedWaterWithWellWater = table.Column<string>(type: "varchar", nullable: true),
                    WaterLevelControlInEquipment = table.Column<string>(type: "varchar", nullable: true),
                    EliminationOfLeaksInWaterLinesAndValves = table.Column<string>(type: "varchar", nullable: true),
                    ReplacementOfWaterRegretInProcesses = table.Column<string>(type: "varchar", nullable: true),
                    ReplacementOfWaterCoolingInProcesses = table.Column<string>(type: "varchar", nullable: true),
                    ReductionInWaterUse = table.Column<string>(type: "varchar", nullable: true),
                    UseOfCountercurrentRinsing = table.Column<string>(type: "varchar", nullable: true),
                    MinimalUseOfCoolingWater = table.Column<string>(type: "varchar", nullable: true),
                    RecoveryOfMetalsFromWater = table.Column<string>(type: "varchar", nullable: true),
                    WaterTreatmentAndReuse = table.Column<string>(type: "varchar", nullable: true),
                    RecoveryAndReuseOfCoolingWater = table.Column<string>(type: "varchar", nullable: true),
                    UseOfClosedProcessInTheProductionOfWasteWater = table.Column<string>(type: "varchar", nullable: true),
                    ReplacementOfCoolingWaterInIndustry = table.Column<string>(type: "varchar", nullable: true),
                    RecycledWaterMeasurementToReduceSewageFees = table.Column<string>(type: "varchar", nullable: true),
                    WaterTreatmentByMagneticTechnology = table.Column<string>(type: "varchar", nullable: true),
                    ImprovedProductionOfDeionizedWater = table.Column<string>(type: "varchar", nullable: true),
                    RecyclingOfChlorinatedWater = table.Column<string>(type: "varchar", nullable: true),
                    UseTheChlorinationWashWater = table.Column<string>(type: "varchar", nullable: true),
                    UsingChlorineInTheGasPhase = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mechanicalTreatment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WasteEnergyUse",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    WasteEnergyUseValue = table.Column<string>(type: "varchar", nullable: true),
                    ChemicalTreatment = table.Column<string>(type: "varchar", nullable: true),
                    MechanicalTreatment = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wasteEnergyUse", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChemicalTreatment");

            migrationBuilder.DropTable(
                name: "MechanicalTreatment");

            migrationBuilder.DropTable(
                name: "WasteEnergyUse");
        }
    }
}
