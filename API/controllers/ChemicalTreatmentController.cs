using System;
using Application.Models;
using Application.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AForge.Fuzzy;

namespace Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ChemicalTreatmentController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IChemicalTreatmentRepository _ChemicalTreatmentRepository;
        public ChemicalTreatmentController(IConfiguration configuration, IChemicalTreatmentRepository ChemicalTreatmentRepository)
        {
            Configuration = configuration;
            _ChemicalTreatmentRepository = ChemicalTreatmentRepository;
        }

        [HttpPost]
        public ActionResult<ChemicalTreatment> InsertChemicalTreatment([FromBody] ChemicalTreatment chemicalTreatment)
        {
            try
            {
                chemicalTreatment.WhiteWater = CalculateWhiteWater(Convert.ToDouble(chemicalTreatment.DistilledWaterRecycling), Convert.ToDouble(chemicalTreatment.ReuseOfDistilledWaterInOtherApplications));
                chemicalTreatment.Oil = CalculateOil(Convert.ToDouble(chemicalTreatment.HydraulicOilReuse), Convert.ToDouble(chemicalTreatment.ReuseOfUsedOil), Convert.ToDouble(chemicalTreatment.SaleOfRecyclerOil));
                chemicalTreatment.Solutions = CalculateSolutions(Convert.ToDouble(chemicalTreatment.EquipmentCleaningTreatmentAndReuse), Convert.ToDouble(chemicalTreatment.DevelopmentOfSpentSolutionsToTheManufacturer), Convert.ToDouble(chemicalTreatment.RecyclingSpentTanningSolution));
                chemicalTreatment.Reuse = CalculateReuse(Convert.ToDouble(chemicalTreatment.MetalWorkingFluidReuse), Convert.ToDouble(chemicalTreatment.ReuseOfUsedAcidBaths));
                chemicalTreatment.Sale = CalculateSale(Convert.ToDouble(chemicalTreatment.SaleOfUsedSheetsForRecycling), Convert.ToDouble(chemicalTreatment.SeparationOfMetalsForSaleForRecycling));
                chemicalTreatment.Recovery = CalculateRecovery(Convert.ToDouble(chemicalTreatment.RecoveryOfMetalsForReuse), Convert.ToDouble(chemicalTreatment.FilmRecyclingForSilverReuse), Convert.ToDouble(chemicalTreatment.FoundryAreaRecovery), Convert.ToDouble(chemicalTreatment.SeparationAndRecyclingOfScrapForFoundry));
                chemicalTreatment.Sand = CalculateSand(Convert.ToDouble(chemicalTreatment.FoundrySandRecycling), Convert.ToDouble(chemicalTreatment.UseOfSandForOtherPurposes));
                chemicalTreatment.Recycle = CalculateRecycle(Convert.ToDouble(chemicalTreatment.NonFerrousPowderRecycling), Convert.ToDouble(chemicalTreatment.RubberProductRecycling), Convert.ToDouble(chemicalTreatment.PaperProductRecycling));
                chemicalTreatment.Scraps = CalculateScraps(Convert.ToDouble(chemicalTreatment.ReuseOfScrapGlass), Convert.ToDouble(chemicalTreatment.ReuseOfScrapPlasticParts), Convert.ToDouble(chemicalTreatment.ReuseOfPrintedPaperScrap));
                chemicalTreatment.Water = CalculateWater(Convert.ToDouble(chemicalTreatment.DryAllPartsOfTheWaterSeparator), Convert.ToDouble(chemicalTreatment.UseOfDeionizedWaterToRinseParts));
                chemicalTreatment.Employees = CalculateEmployees(Convert.ToDouble(chemicalTreatment.AvoidExcessSolventByOperators), Convert.ToDouble(chemicalTreatment.NumberOfEmployeesForPartsWashing));
                chemicalTreatment.Stopper = CalculateStopper(Convert.ToDouble(chemicalTreatment.UseOfFittedLids), Convert.ToDouble(chemicalTreatment.FloatingLidsOnMaterialTanks));
                chemicalTreatment.SteamMinimization = CalculateSteamMinimization(Convert.ToDouble(chemicalTreatment.DecreaseSteamLosses), Convert.ToDouble(chemicalTreatment.UseOfSteamGasRecovery));
                chemicalTreatment.Solvent = CalculateSolvent(Convert.ToDouble(chemicalTreatment.Cleaning), Convert.ToDouble(chemicalTreatment.Distillation), Convert.ToDouble(chemicalTreatment.ReuseLevel2));
                chemicalTreatment.WaterLevel2 = CalculateWaterLevel2(Convert.ToDouble(chemicalTreatment.UseOfWaterBasedAdhesives), Convert.ToDouble(chemicalTreatment.ConversionOfLiquidMaterialsForCleaning), Convert.ToDouble(chemicalTreatment.SolventReplacementForWaterBasedCuttingFluids));
                chemicalTreatment.SolventLevel2 = CalculateSolventLevel2(Convert.ToDouble(chemicalTreatment.UseOfLessToxicAndVolatileSolvents), Convert.ToDouble(chemicalTreatment.UseOfWaterBasedPaints), Convert.ToDouble(chemicalTreatment.UseOfSoyBasedPaints));
                chemicalTreatment.Solids = CalculateSolids(Convert.ToDouble(chemicalTreatment.UseOfMaterialsWithLessEnergyUse), Convert.ToDouble(chemicalTreatment.AlterRawMaterialForLessEmission), Convert.ToDouble(chemicalTreatment.UseOfMaterialsInRenewableContainers));
                chemicalTreatment.WaterBasedSubstitutes = CalculateWaterBasedSubstitutes(Convert.ToDouble(chemicalTreatment.UseOfAqueousCleaningSystem), Convert.ToDouble(chemicalTreatment.FinishIndustrialProcessWithWaterBasedProduct));
                chemicalTreatment.InorganicSolutions = CalculateInorganicSolutions(Convert.ToDouble(chemicalTreatment.SubstituteHexavalentChromiumForTrivalent), Convert.ToDouble(chemicalTreatment.ReplaceHeavyMetalReagentsWithNonHazardousOnes));
                chemicalTreatment.Flocculation = CalculateFlocculation(Convert.ToDouble(chemicalTreatment.UseOfFlocculantsToReduceSludge), Convert.ToDouble(chemicalTreatment.UseOfPrecipitatingAgentsInWasteWaterTreatment));
                chemicalTreatment.SludgeRemoval = CalculateSludgeRemoval(Convert.ToDouble(chemicalTreatment.UseOfGreenhouseFilterToReduceSludge), Convert.ToDouble(chemicalTreatment.RemovalOfSludgeFromEquipmentTanks));
                chemicalTreatment.HeatGeneration = CalculateHeatGeneration(Convert.ToDouble(chemicalTreatment.InstallationOfIncineratorForSolidWaste), Convert.ToDouble(chemicalTreatment.BurningOfHeatWoodByProducts), Convert.ToDouble(chemicalTreatment.WasteOilBurning));
                chemicalTreatment.Operation = CalculateOperation(Convert.ToDouble(chemicalTreatment.DirectWasteGasesToTheBoiler), Convert.ToDouble(chemicalTreatment.BurnWastePaperToGenerateHeat));
                chemicalTreatment.WasteDisposalLevel2 = CalculateWasteDisposalLevel2(Convert.ToDouble(chemicalTreatment.CheapWasteRemoval), Convert.ToDouble(chemicalTreatment.InstallationOfDisposalEquipment));
                chemicalTreatment.Miscellaneous = CalculateMiscellaneous(Convert.ToDouble(chemicalTreatment.Reuse), Convert.ToDouble(chemicalTreatment.Solutions));
                chemicalTreatment.Metals = CalculateMetals(Convert.ToDouble(chemicalTreatment.Sale), Convert.ToDouble(chemicalTreatment.Recovery));
                chemicalTreatment.General = CalculateGeneral(Convert.ToDouble(chemicalTreatment.DecreaseContaminationOfEndPieces), Convert.ToDouble(chemicalTreatment.Recycle), Convert.ToDouble(chemicalTreatment.Scraps));
                chemicalTreatment.Use = CalculateUse(Convert.ToDouble(chemicalTreatment.Water), Convert.ToDouble(chemicalTreatment.Employees));
                chemicalTreatment.Emissions = CalculateEmissions(Convert.ToDouble(chemicalTreatment.Stopper), Convert.ToDouble(chemicalTreatment.SteamMinimization), Convert.ToDouble(chemicalTreatment.RemovingRollersFromCleaningMachines));
                chemicalTreatment.RestorationLevel2 = CalculateRestorationLevel2(Convert.ToDouble(chemicalTreatment.UseOfSolventThatCanBeReused), Convert.ToDouble(chemicalTreatment.WaterLevel2), Convert.ToDouble(chemicalTreatment.SolventLevel2));
                chemicalTreatment.SolutesLevel2 = CalculateSolutesLevel2(Convert.ToDouble(chemicalTreatment.ProductsWithNeutralPH), Convert.ToDouble(chemicalTreatment.ConvertHydrocarbonCleanersToLessToxicOnes), Convert.ToDouble(chemicalTreatment.InorganicSolutions));
                chemicalTreatment.SW = CalculateSW(Convert.ToDouble(chemicalTreatment.SludgeRemoval), Convert.ToDouble(chemicalTreatment.Flocculation));
                chemicalTreatment.CWP = CalculateCWP(Convert.ToDouble(chemicalTreatment.HeatGeneration), Convert.ToDouble(chemicalTreatment.Operation), Convert.ToDouble(chemicalTreatment.SaleOfCombustibleWaste));
                chemicalTreatment.Maintenance = CalculateMaintenance(Convert.ToDouble(chemicalTreatment.ManufacturersWornSolutions), Convert.ToDouble(chemicalTreatment.UseOfHydraulicOilInTheIndustrialProcess), Convert.ToDouble(chemicalTreatment.WasteDisposalLevel2));
                chemicalTreatment.Liquid = CalculateLiquid(Convert.ToDouble(chemicalTreatment.Miscellaneous), Convert.ToDouble(chemicalTreatment.Oil), Convert.ToDouble(chemicalTreatment.WhiteWater), Convert.ToDouble(chemicalTreatment.RecyclingOfInkAndCleaningSolventResidues));
                chemicalTreatment.Solid = CalculateSolid(Convert.ToDouble(chemicalTreatment.Metals), Convert.ToDouble(chemicalTreatment.Sand), Convert.ToDouble(chemicalTreatment.General));
                chemicalTreatment.Reduction = CalculateReduction(Convert.ToDouble(chemicalTreatment.Use), Convert.ToDouble(chemicalTreatment.Emissions));
                chemicalTreatment.Restoration = CalculateRestoration(Convert.ToDouble(chemicalTreatment.Solvent), Convert.ToDouble(chemicalTreatment.RestorationLevel2));
                chemicalTreatment.Solutes = CalculateSolutes(Convert.ToDouble(chemicalTreatment.WaterBasedSubstitutes), Convert.ToDouble(chemicalTreatment.SolutesLevel2));
                chemicalTreatment.WasteDisposal = CalculateWasteDisposal(Convert.ToDouble(chemicalTreatment.SW), Convert.ToDouble(chemicalTreatment.CWP), Convert.ToDouble(chemicalTreatment.Maintenance));
                chemicalTreatment.Recycling = CalculateRecycling(Convert.ToDouble(chemicalTreatment.OtherMaterials), Convert.ToDouble(chemicalTreatment.Solid), Convert.ToDouble(chemicalTreatment.Liquid));
                chemicalTreatment.Solvents = CalculateSolvents(Convert.ToDouble(chemicalTreatment.Reduction), Convert.ToDouble(chemicalTreatment.Restoration));
                chemicalTreatment.RawMaterials = CalculateRawMaterials(Convert.ToDouble(chemicalTreatment.Solutes), Convert.ToDouble(chemicalTreatment.Solids), Convert.ToDouble(chemicalTreatment.Solvents));
                chemicalTreatment.ChemicalTreatmentValue = CalculateChemicalTreatment(Convert.ToDouble(chemicalTreatment.Recycling), Convert.ToDouble(chemicalTreatment.RawMaterials), Convert.ToDouble(chemicalTreatment.WasteDisposal));

                return _ChemicalTreatmentRepository.InsertChemicalTreatment(chemicalTreatment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ChemicalTreatment> GetChemicalTreatmentById(int id)
        {
            try
            {
                return _ChemicalTreatmentRepository.GetChemicalTreatmentById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<ChemicalTreatment>> GetChemicalTreatmentByIndustry(string industryName)
        {
            try
            {
                return _ChemicalTreatmentRepository.GetChemicalTreatmentByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateWhiteWater(double distilledWaterRecyclingValue, double reuseOfDistilledWaterInOtherApplicationsValue)
        {
            LinguisticVariable distilledWaterRecycling = new( "DistilledWaterRecycling", 0, 10 );
            distilledWaterRecycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            distilledWaterRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            distilledWaterRecycling.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            distilledWaterRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            distilledWaterRecycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable reuseOfDistilledWaterInOtherApplications = new( "ReuseOfDistilledWaterInOtherApplications", 0, 10 );
            reuseOfDistilledWaterInOtherApplications.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reuseOfDistilledWaterInOtherApplications.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reuseOfDistilledWaterInOtherApplications.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            reuseOfDistilledWaterInOtherApplications.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reuseOfDistilledWaterInOtherApplications.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable whiteWater = new( "WhiteWater", 0, 10 );
            whiteWater.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            whiteWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            whiteWater.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            whiteWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            whiteWater.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( distilledWaterRecycling );
            fuzzyDB.AddVariable( reuseOfDistilledWaterInOtherApplications );
            fuzzyDB.AddVariable( whiteWater );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DistilledWaterRecycling IS VeryLow and ReuseOfDistilledWaterInOtherApplications IS VeryLow THEN WhiteWater IS VeryLow");
            IS.NewRule("Rule 2", "IF DistilledWaterRecycling IS VeryLow and ReuseOfDistilledWaterInOtherApplications IS Low THEN WhiteWater IS VeryLow");
            IS.NewRule("Rule 3", "IF DistilledWaterRecycling IS VeryLow and ReuseOfDistilledWaterInOtherApplications IS Medium THEN WhiteWater IS Low");
            IS.NewRule("Rule 4", "IF DistilledWaterRecycling IS VeryLow and ReuseOfDistilledWaterInOtherApplications IS High THEN WhiteWater IS Low");
            IS.NewRule("Rule 5", "IF DistilledWaterRecycling IS VeryLow and ReuseOfDistilledWaterInOtherApplications IS VeryHigh THEN WhiteWater IS Middle");
            IS.NewRule("Rule 6", "IF DistilledWaterRecycling IS Low and ReuseOfDistilledWaterInOtherApplications IS VeryLow THEN WhiteWater IS VeryLow");
            IS.NewRule("Rule 7", "IF DistilledWaterRecycling IS Low and ReuseOfDistilledWaterInOtherApplications IS Low THEN WhiteWater IS Low");
            IS.NewRule("Rule 8", "IF DistilledWaterRecycling IS Low and ReuseOfDistilledWaterInOtherApplications IS Medium THEN WhiteWater IS Low");
            IS.NewRule("Rule 9", "IF DistilledWaterRecycling IS Low and ReuseOfDistilledWaterInOtherApplications IS High THEN WhiteWater IS Middle");
            IS.NewRule("Rule 10", "IF DistilledWaterRecycling IS Low and ReuseOfDistilledWaterInOtherApplications IS VeryHigh THEN WhiteWater IS High");
            IS.NewRule("Rule 11", "IF DistilledWaterRecycling IS Middle and ReuseOfDistilledWaterInOtherApplications IS VeryLow THEN WhiteWater IS Low");
            IS.NewRule("Rule 12", "IF DistilledWaterRecycling IS Middle and ReuseOfDistilledWaterInOtherApplications IS Low THEN WhiteWater IS Low");
            IS.NewRule("Rule 13", "IF DistilledWaterRecycling IS Middle and ReuseOfDistilledWaterInOtherApplications IS Medium THEN WhiteWater IS Middle");
            IS.NewRule("Rule 14", "IF DistilledWaterRecycling IS Middle and ReuseOfDistilledWaterInOtherApplications IS High THEN WhiteWater IS High");
            IS.NewRule("Rule 15", "IF DistilledWaterRecycling IS Middle and ReuseOfDistilledWaterInOtherApplications IS VeryHigh THEN WhiteWater IS High");
            IS.NewRule("Rule 16", "IF DistilledWaterRecycling IS High and ReuseOfDistilledWaterInOtherApplications IS VeryLow THEN WhiteWater IS Low");
            IS.NewRule("Rule 17", "IF DistilledWaterRecycling IS High and ReuseOfDistilledWaterInOtherApplications IS Low THEN WhiteWater IS Middle");
            IS.NewRule("Rule 18", "IF DistilledWaterRecycling IS High and ReuseOfDistilledWaterInOtherApplications IS Medium THEN WhiteWater IS High");
            IS.NewRule("Rule 19", "IF DistilledWaterRecycling IS High and ReuseOfDistilledWaterInOtherApplications IS High THEN WhiteWater IS High");
            IS.NewRule("Rule 20", "IF DistilledWaterRecycling IS High and ReuseOfDistilledWaterInOtherApplications IS VeryHigh THEN WhiteWater IS VeryHigh");
            IS.NewRule("Rule 21", "IF DistilledWaterRecycling IS VeryHigh and ReuseOfDistilledWaterInOtherApplications IS VeryLow THEN WhiteWater IS Middle");
            IS.NewRule("Rule 22", "IF DistilledWaterRecycling IS VeryHigh and ReuseOfDistilledWaterInOtherApplications IS Low THEN WhiteWater IS High");
            IS.NewRule("Rule 23", "IF DistilledWaterRecycling IS VeryHigh and ReuseOfDistilledWaterInOtherApplications IS Medium THEN WhiteWater IS High");
            IS.NewRule("Rule 24", "IF DistilledWaterRecycling IS VeryHigh and ReuseOfDistilledWaterInOtherApplications IS High THEN WhiteWater IS VeryHigh");
            IS.NewRule("Rule 25", "IF DistilledWaterRecycling IS VeryHigh and ReuseOfDistilledWaterInOtherApplications IS VeryHigh THEN WhiteWater IS VeryHigh");

            IS.SetInput("DistilledWaterRecycling", (float)distilledWaterRecyclingValue);
            IS.SetInput("ReuseOfDistilledWaterInOtherApplications", (float)reuseOfDistilledWaterInOtherApplicationsValue);

            double resultado = IS.Evaluate("WhiteWater");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DistilledWaterRecycling", i == 0 ? (float)9.99 : 0);
                IS.SetInput("ReuseOfDistilledWaterInOtherApplications", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("WhiteWater");
            }
            double m = (IS.GetLinguisticVariable("WhiteWater").End - IS.GetLinguisticVariable("WhiteWater").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WhiteWater").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateOil(double hydraulicOilReuseValue, double reuseOfUsedOilValue, double saleOfRecyclerOilValue)
        {
            LinguisticVariable hydraulicOilReuse = new( "HydraulicOilReuse", 0, 10 );
            hydraulicOilReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            hydraulicOilReuse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            hydraulicOilReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reuseOfUsedOil = new( "ReuseOfUsedOil", 0, 10 );
            reuseOfUsedOil.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reuseOfUsedOil.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reuseOfUsedOil.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable saleOfRecyclerOil = new( "SaleOfRecyclerOil", 0, 10 );
            saleOfRecyclerOil.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            saleOfRecyclerOil.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            saleOfRecyclerOil.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable oil = new( "Oil", 0, 10 );
            oil.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            oil.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            oil.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            oil.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            oil.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( hydraulicOilReuse );
            fuzzyDB.AddVariable( reuseOfUsedOil );
            fuzzyDB.AddVariable( saleOfRecyclerOil );
            fuzzyDB.AddVariable( oil );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Low THEN Oil IS VeryLow");
            IS.NewRule("Rule 2", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Medium THEN Oil IS VeryLow");
            IS.NewRule("Rule 3", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS High THEN Oil IS Low");
            IS.NewRule("Rule 4", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Low THEN Oil IS VeryLow");
            IS.NewRule("Rule 5", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Medium THEN Oil IS Low");
            IS.NewRule("Rule 6", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS High THEN Oil IS Middle");
            IS.NewRule("Rule 7", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Low THEN Oil IS Low");
            IS.NewRule("Rule 8", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Medium THEN Oil IS Middle");
            IS.NewRule("Rule 9", "IF HydraulicOilReuse IS Low and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS High THEN Oil IS High");
            IS.NewRule("Rule 10", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Low THEN Oil IS VeryLow");
            IS.NewRule("Rule 11", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Medium THEN Oil IS Low");
            IS.NewRule("Rule 12", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS High THEN Oil IS Middle");
            IS.NewRule("Rule 13", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Low THEN Oil IS Low");
            IS.NewRule("Rule 14", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Medium THEN Oil IS Middle");
            IS.NewRule("Rule 15", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS High THEN Oil IS High");
            IS.NewRule("Rule 16", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Low THEN Oil IS Middle");
            IS.NewRule("Rule 17", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Medium THEN Oil IS High");
            IS.NewRule("Rule 18", "IF HydraulicOilReuse IS Medium and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS High THEN Oil IS VeryHigh");
            IS.NewRule("Rule 19", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Low THEN Oil IS Low");
            IS.NewRule("Rule 20", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS Medium THEN Oil IS Middle");
            IS.NewRule("Rule 21", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Low and SaleOfRecyclerOil IS High THEN Oil IS High");
            IS.NewRule("Rule 22", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Low THEN Oil IS Middle");
            IS.NewRule("Rule 23", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS Medium THEN Oil IS High");
            IS.NewRule("Rule 24", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS Medium and SaleOfRecyclerOil IS High THEN Oil IS VeryHigh");
            IS.NewRule("Rule 25", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Low THEN Oil IS High");
            IS.NewRule("Rule 26", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS Medium THEN Oil IS VeryHigh");
            IS.NewRule("Rule 27", "IF HydraulicOilReuse IS High and ReuseOfUsedOil IS High and SaleOfRecyclerOil IS High THEN Oil IS VeryHigh");

            IS.SetInput("HydraulicOilReuse", (float)hydraulicOilReuseValue);
            IS.SetInput("ReuseOfUsedOil", (float)reuseOfUsedOilValue);
            IS.SetInput("SaleOfRecyclerOil", (float)saleOfRecyclerOilValue);

            double resultado = IS.Evaluate("Oil");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("HydraulicOilReuse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReuseOfUsedOil", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SaleOfRecyclerOil", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Oil");
            }
            double m = (IS.GetLinguisticVariable("Oil").End - IS.GetLinguisticVariable("Oil").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Oil").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolutions(double equipmentCleaningTreatmentAndReuseValue, double developmentOfSpentSolutionsToTheManufacturerValue, double recyclingSpentTanningSolutionValue)
        {
            LinguisticVariable equipmentCleaningTreatmentAndReuse = new( "EquipmentCleaningTreatmentAndReuse", 0, 10 );
            equipmentCleaningTreatmentAndReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            equipmentCleaningTreatmentAndReuse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            equipmentCleaningTreatmentAndReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable developmentOfSpentSolutionsToTheManufacturer = new( "DevelopmentOfSpentSolutionsToTheManufacturer", 0, 10 );
            developmentOfSpentSolutionsToTheManufacturer.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            developmentOfSpentSolutionsToTheManufacturer.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            developmentOfSpentSolutionsToTheManufacturer.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recyclingSpentTanningSolution = new( "RecyclingSpentTanningSolution", 0, 10 );
            recyclingSpentTanningSolution.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recyclingSpentTanningSolution.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recyclingSpentTanningSolution.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solutions = new( "Solutions", 0, 10 );
            solutions.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solutions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solutions.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solutions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solutions.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( equipmentCleaningTreatmentAndReuse );
            fuzzyDB.AddVariable( developmentOfSpentSolutionsToTheManufacturer );
            fuzzyDB.AddVariable( recyclingSpentTanningSolution );
            fuzzyDB.AddVariable( solutions );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Low THEN Solutions IS VeryLow");
            IS.NewRule("Rule 2", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Medium THEN Solutions IS VeryLow");
            IS.NewRule("Rule 3", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS High THEN Solutions IS Low");
            IS.NewRule("Rule 4", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Low THEN Solutions IS VeryLow");
            IS.NewRule("Rule 5", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Medium THEN Solutions IS Low");
            IS.NewRule("Rule 6", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS High THEN Solutions IS Middle");
            IS.NewRule("Rule 7", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Low THEN Solutions IS Low");
            IS.NewRule("Rule 8", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Medium THEN Solutions IS Middle");
            IS.NewRule("Rule 9", "IF EquipmentCleaningTreatmentAndReuse IS Low and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS High THEN Solutions IS High");
            IS.NewRule("Rule 10", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Low THEN Solutions IS VeryLow");
            IS.NewRule("Rule 11", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Medium THEN Solutions IS Low");
            IS.NewRule("Rule 12", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS High THEN Solutions IS Middle");
            IS.NewRule("Rule 13", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Low THEN Solutions IS Low");
            IS.NewRule("Rule 14", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Medium THEN Solutions IS Middle");
            IS.NewRule("Rule 15", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS High THEN Solutions IS High");
            IS.NewRule("Rule 16", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Low THEN Solutions IS Middle");
            IS.NewRule("Rule 17", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Medium THEN Solutions IS High");
            IS.NewRule("Rule 18", "IF EquipmentCleaningTreatmentAndReuse IS Medium and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS High THEN Solutions IS VeryHigh");
            IS.NewRule("Rule 19", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Low THEN Solutions IS Low");
            IS.NewRule("Rule 20", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS Medium THEN Solutions IS Middle");
            IS.NewRule("Rule 21", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Low and RecyclingSpentTanningSolution IS High THEN Solutions IS High");
            IS.NewRule("Rule 22", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Low THEN Solutions IS Middle");
            IS.NewRule("Rule 23", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS Medium THEN Solutions IS High");
            IS.NewRule("Rule 24", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS Medium and RecyclingSpentTanningSolution IS High THEN Solutions IS VeryHigh");
            IS.NewRule("Rule 25", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Low THEN Solutions IS High");
            IS.NewRule("Rule 26", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS Medium THEN Solutions IS VeryHigh");
            IS.NewRule("Rule 27", "IF EquipmentCleaningTreatmentAndReuse IS High and DevelopmentOfSpentSolutionsToTheManufacturer IS High and RecyclingSpentTanningSolution IS High THEN Solutions IS VeryHigh");

            IS.SetInput("EquipmentCleaningTreatmentAndReuse", (float)equipmentCleaningTreatmentAndReuseValue);
            IS.SetInput("DevelopmentOfSpentSolutionsToTheManufacturer", (float)developmentOfSpentSolutionsToTheManufacturerValue);
            IS.SetInput("RecyclingSpentTanningSolution", (float)recyclingSpentTanningSolutionValue);

            double resultado = IS.Evaluate("Solutions");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("EquipmentCleaningTreatmentAndReuse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("DevelopmentOfSpentSolutionsToTheManufacturer", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RecyclingSpentTanningSolution", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Solutions");
            }
            double m = (IS.GetLinguisticVariable("Solutions").End - IS.GetLinguisticVariable("Solutions").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solutions").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateReuse(double metalWorkingFluidReuseValue, double reuseOfUsedAcidBathsValue)
        {
            LinguisticVariable metalWorkingFluidReuse = new( "MetalWorkingFluidReuse", 0, 10 );
            metalWorkingFluidReuse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            metalWorkingFluidReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            metalWorkingFluidReuse.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            metalWorkingFluidReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            metalWorkingFluidReuse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable reuseOfUsedAcidBaths = new( "ReuseOfUsedAcidBaths", 0, 10 );
            reuseOfUsedAcidBaths.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reuseOfUsedAcidBaths.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reuseOfUsedAcidBaths.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            reuseOfUsedAcidBaths.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reuseOfUsedAcidBaths.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable reuse = new( "Reuse", 0, 10 );
            reuse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reuse.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reuse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( metalWorkingFluidReuse );
            fuzzyDB.AddVariable( reuseOfUsedAcidBaths );
            fuzzyDB.AddVariable( reuse );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF MetalWorkingFluidReuse IS VeryLow and ReuseOfUsedAcidBaths IS VeryLow THEN Reuse IS VeryLow");
            IS.NewRule("Rule 2", "IF MetalWorkingFluidReuse IS VeryLow and ReuseOfUsedAcidBaths IS Low THEN Reuse IS VeryLow");
            IS.NewRule("Rule 3", "IF MetalWorkingFluidReuse IS VeryLow and ReuseOfUsedAcidBaths IS Medium THEN Reuse IS Low");
            IS.NewRule("Rule 4", "IF MetalWorkingFluidReuse IS VeryLow and ReuseOfUsedAcidBaths IS High THEN Reuse IS Low");
            IS.NewRule("Rule 5", "IF MetalWorkingFluidReuse IS VeryLow and ReuseOfUsedAcidBaths IS VeryHigh THEN Reuse IS Middle");
            IS.NewRule("Rule 6", "IF MetalWorkingFluidReuse IS Low and ReuseOfUsedAcidBaths IS VeryLow THEN Reuse IS VeryLow");
            IS.NewRule("Rule 7", "IF MetalWorkingFluidReuse IS Low and ReuseOfUsedAcidBaths IS Low THEN Reuse IS Low");
            IS.NewRule("Rule 8", "IF MetalWorkingFluidReuse IS Low and ReuseOfUsedAcidBaths IS Medium THEN Reuse IS Low");
            IS.NewRule("Rule 9", "IF MetalWorkingFluidReuse IS Low and ReuseOfUsedAcidBaths IS High THEN Reuse IS Middle");
            IS.NewRule("Rule 10", "IF MetalWorkingFluidReuse IS Low and ReuseOfUsedAcidBaths IS VeryHigh THEN Reuse IS High");
            IS.NewRule("Rule 11", "IF MetalWorkingFluidReuse IS Middle and ReuseOfUsedAcidBaths IS VeryLow THEN Reuse IS Low");
            IS.NewRule("Rule 12", "IF MetalWorkingFluidReuse IS Middle and ReuseOfUsedAcidBaths IS Low THEN Reuse IS Low");
            IS.NewRule("Rule 13", "IF MetalWorkingFluidReuse IS Middle and ReuseOfUsedAcidBaths IS Medium THEN Reuse IS Middle");
            IS.NewRule("Rule 14", "IF MetalWorkingFluidReuse IS Middle and ReuseOfUsedAcidBaths IS High THEN Reuse IS High");
            IS.NewRule("Rule 15", "IF MetalWorkingFluidReuse IS Middle and ReuseOfUsedAcidBaths IS VeryHigh THEN Reuse IS High");
            IS.NewRule("Rule 16", "IF MetalWorkingFluidReuse IS High and ReuseOfUsedAcidBaths IS VeryLow THEN Reuse IS Low");
            IS.NewRule("Rule 17", "IF MetalWorkingFluidReuse IS High and ReuseOfUsedAcidBaths IS Low THEN Reuse IS Middle");
            IS.NewRule("Rule 18", "IF MetalWorkingFluidReuse IS High and ReuseOfUsedAcidBaths IS Medium THEN Reuse IS High");
            IS.NewRule("Rule 19", "IF MetalWorkingFluidReuse IS High and ReuseOfUsedAcidBaths IS High THEN Reuse IS High");
            IS.NewRule("Rule 20", "IF MetalWorkingFluidReuse IS High and ReuseOfUsedAcidBaths IS VeryHigh THEN Reuse IS VeryHigh");
            IS.NewRule("Rule 21", "IF MetalWorkingFluidReuse IS VeryHigh and ReuseOfUsedAcidBaths IS VeryLow THEN Reuse IS Middle");
            IS.NewRule("Rule 22", "IF MetalWorkingFluidReuse IS VeryHigh and ReuseOfUsedAcidBaths IS Low THEN Reuse IS High");
            IS.NewRule("Rule 23", "IF MetalWorkingFluidReuse IS VeryHigh and ReuseOfUsedAcidBaths IS Medium THEN Reuse IS High");
            IS.NewRule("Rule 24", "IF MetalWorkingFluidReuse IS VeryHigh and ReuseOfUsedAcidBaths IS High THEN Reuse IS VeryHigh");
            IS.NewRule("Rule 25", "IF MetalWorkingFluidReuse IS VeryHigh and ReuseOfUsedAcidBaths IS VeryHigh THEN Reuse IS VeryHigh");

            IS.SetInput("MetalWorkingFluidReuse", (float)metalWorkingFluidReuseValue);
            IS.SetInput("ReuseOfUsedAcidBaths", (float)reuseOfUsedAcidBathsValue);

            double resultado = IS.Evaluate("Reuse");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("MetalWorkingFluidReuse", i == 0 ? (float)9.99 : 0);
                IS.SetInput("ReuseOfUsedAcidBaths", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Reuse");
            }
            double m = (IS.GetLinguisticVariable("Reuse").End - IS.GetLinguisticVariable("Reuse").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Reuse").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSale(double saleOfUsedSheetsForRecyclingValue, double separationOfMetalsForSaleForRecyclingValue)
        {
            LinguisticVariable saleOfUsedSheetsForRecycling = new( "SaleOfUsedSheetsForRecycling", 0, 10 );
            saleOfUsedSheetsForRecycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            saleOfUsedSheetsForRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            saleOfUsedSheetsForRecycling.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            saleOfUsedSheetsForRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            saleOfUsedSheetsForRecycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable separationOfMetalsForSaleForRecycling = new( "SeparationOfMetalsForSaleForRecycling", 0, 10 );
            separationOfMetalsForSaleForRecycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            separationOfMetalsForSaleForRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            separationOfMetalsForSaleForRecycling.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            separationOfMetalsForSaleForRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            separationOfMetalsForSaleForRecycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable sale = new( "Sale", 0, 10 );
            sale.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sale.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sale.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sale.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sale.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( saleOfUsedSheetsForRecycling );
            fuzzyDB.AddVariable( separationOfMetalsForSaleForRecycling );
            fuzzyDB.AddVariable( sale );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SaleOfUsedSheetsForRecycling IS VeryLow and SeparationOfMetalsForSaleForRecycling IS VeryLow THEN Sale IS VeryLow");
            IS.NewRule("Rule 2", "IF SaleOfUsedSheetsForRecycling IS VeryLow and SeparationOfMetalsForSaleForRecycling IS Low THEN Sale IS VeryLow");
            IS.NewRule("Rule 3", "IF SaleOfUsedSheetsForRecycling IS VeryLow and SeparationOfMetalsForSaleForRecycling IS Medium THEN Sale IS Low");
            IS.NewRule("Rule 4", "IF SaleOfUsedSheetsForRecycling IS VeryLow and SeparationOfMetalsForSaleForRecycling IS High THEN Sale IS Low");
            IS.NewRule("Rule 5", "IF SaleOfUsedSheetsForRecycling IS VeryLow and SeparationOfMetalsForSaleForRecycling IS VeryHigh THEN Sale IS Middle");
            IS.NewRule("Rule 6", "IF SaleOfUsedSheetsForRecycling IS Low and SeparationOfMetalsForSaleForRecycling IS VeryLow THEN Sale IS VeryLow");
            IS.NewRule("Rule 7", "IF SaleOfUsedSheetsForRecycling IS Low and SeparationOfMetalsForSaleForRecycling IS Low THEN Sale IS Low");
            IS.NewRule("Rule 8", "IF SaleOfUsedSheetsForRecycling IS Low and SeparationOfMetalsForSaleForRecycling IS Medium THEN Sale IS Low");
            IS.NewRule("Rule 9", "IF SaleOfUsedSheetsForRecycling IS Low and SeparationOfMetalsForSaleForRecycling IS High THEN Sale IS Middle");
            IS.NewRule("Rule 10", "IF SaleOfUsedSheetsForRecycling IS Low and SeparationOfMetalsForSaleForRecycling IS VeryHigh THEN Sale IS High");
            IS.NewRule("Rule 11", "IF SaleOfUsedSheetsForRecycling IS Middle and SeparationOfMetalsForSaleForRecycling IS VeryLow THEN Sale IS Low");
            IS.NewRule("Rule 12", "IF SaleOfUsedSheetsForRecycling IS Middle and SeparationOfMetalsForSaleForRecycling IS Low THEN Sale IS Low");
            IS.NewRule("Rule 13", "IF SaleOfUsedSheetsForRecycling IS Middle and SeparationOfMetalsForSaleForRecycling IS Medium THEN Sale IS Middle");
            IS.NewRule("Rule 14", "IF SaleOfUsedSheetsForRecycling IS Middle and SeparationOfMetalsForSaleForRecycling IS High THEN Sale IS High");
            IS.NewRule("Rule 15", "IF SaleOfUsedSheetsForRecycling IS Middle and SeparationOfMetalsForSaleForRecycling IS VeryHigh THEN Sale IS High");
            IS.NewRule("Rule 16", "IF SaleOfUsedSheetsForRecycling IS High and SeparationOfMetalsForSaleForRecycling IS VeryLow THEN Sale IS Low");
            IS.NewRule("Rule 17", "IF SaleOfUsedSheetsForRecycling IS High and SeparationOfMetalsForSaleForRecycling IS Low THEN Sale IS Middle");
            IS.NewRule("Rule 18", "IF SaleOfUsedSheetsForRecycling IS High and SeparationOfMetalsForSaleForRecycling IS Medium THEN Sale IS High");
            IS.NewRule("Rule 19", "IF SaleOfUsedSheetsForRecycling IS High and SeparationOfMetalsForSaleForRecycling IS High THEN Sale IS High");
            IS.NewRule("Rule 20", "IF SaleOfUsedSheetsForRecycling IS High and SeparationOfMetalsForSaleForRecycling IS VeryHigh THEN Sale IS VeryHigh");
            IS.NewRule("Rule 21", "IF SaleOfUsedSheetsForRecycling IS VeryHigh and SeparationOfMetalsForSaleForRecycling IS VeryLow THEN Sale IS Middle");
            IS.NewRule("Rule 22", "IF SaleOfUsedSheetsForRecycling IS VeryHigh and SeparationOfMetalsForSaleForRecycling IS Low THEN Sale IS High");
            IS.NewRule("Rule 23", "IF SaleOfUsedSheetsForRecycling IS VeryHigh and SeparationOfMetalsForSaleForRecycling IS Medium THEN Sale IS High");
            IS.NewRule("Rule 24", "IF SaleOfUsedSheetsForRecycling IS VeryHigh and SeparationOfMetalsForSaleForRecycling IS High THEN Sale IS VeryHigh");
            IS.NewRule("Rule 25", "IF SaleOfUsedSheetsForRecycling IS VeryHigh and SeparationOfMetalsForSaleForRecycling IS VeryHigh THEN Sale IS VeryHigh");

            IS.SetInput("SaleOfUsedSheetsForRecycling", (float)saleOfUsedSheetsForRecyclingValue);
            IS.SetInput("SeparationOfMetalsForSaleForRecycling", (float)separationOfMetalsForSaleForRecyclingValue);

            double resultado = IS.Evaluate("Sale");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SaleOfUsedSheetsForRecycling", i == 0 ? (float)9.99 : 0);
                IS.SetInput("SeparationOfMetalsForSaleForRecycling", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Sale");
            }
            double m = (IS.GetLinguisticVariable("Sale").End - IS.GetLinguisticVariable("Sale").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Sale").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRecovery(double recoveryOfMetalsForReuseValue, double filmRecyclingForSilverReuseValue, double foundryAreaRecoveryValue, double separationAndRecyclingOfScrapForFoundryValue)
        {
            LinguisticVariable recoveryOfMetalsForReuse = new( "RecoveryOfMetalsForReuse", 0, 10 );
            recoveryOfMetalsForReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recoveryOfMetalsForReuse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recoveryOfMetalsForReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable filmRecyclingForSilverReuse = new( "FilmRecyclingForSilverReuse", 0, 10 );
            filmRecyclingForSilverReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            filmRecyclingForSilverReuse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            filmRecyclingForSilverReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable foundryAreaRecovery = new( "FoundryAreaRecovery", 0, 10 );
            foundryAreaRecovery.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            foundryAreaRecovery.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            foundryAreaRecovery.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable separationAndRecyclingOfScrapForFoundry = new( "SeparationAndRecyclingOfScrapForFoundry", 0, 10 );
            separationAndRecyclingOfScrapForFoundry.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            separationAndRecyclingOfScrapForFoundry.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            separationAndRecyclingOfScrapForFoundry.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recovery = new( "Recovery", 0, 10 );
            recovery.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recovery.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recovery.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            recovery.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recovery.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( recoveryOfMetalsForReuse );
            fuzzyDB.AddVariable( filmRecyclingForSilverReuse );
            fuzzyDB.AddVariable( foundryAreaRecovery );
            fuzzyDB.AddVariable( separationAndRecyclingOfScrapForFoundry );
            fuzzyDB.AddVariable( recovery );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS VeryLow");
            IS.NewRule("Rule 2", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS VeryLow");
            IS.NewRule("Rule 3", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Low");
            IS.NewRule("Rule 4", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS VeryLow");
            IS.NewRule("Rule 5", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 6", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 7", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 8", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 9", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 10", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 11", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Low");
            IS.NewRule("Rule 12", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 13", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS High");
            IS.NewRule("Rule 14", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 15", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 16", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 17", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 18", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 19", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 20", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 21", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 22", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 23", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 24", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 25", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS High");
            IS.NewRule("Rule 26", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 27", "IF RecoveryOfMetalsForReuse IS Low and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 28", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS VeryLow");
            IS.NewRule("Rule 29", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Low");
            IS.NewRule("Rule 30", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 31", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 32", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 33", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 34", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 35", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 36", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 37", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 38", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 39", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 40", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 41", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 42", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 43", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 44", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 45", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 46", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 47", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 48", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 49", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 50", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 51", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 52", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS High");
            IS.NewRule("Rule 53", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS High");
            IS.NewRule("Rule 54", "IF RecoveryOfMetalsForReuse IS Medium and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 55", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Low");
            IS.NewRule("Rule 56", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 57", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS Middle");
            IS.NewRule("Rule 58", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 59", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 60", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 61", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS High");
            IS.NewRule("Rule 62", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS High");
            IS.NewRule("Rule 63", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Low and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 64", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 65", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 66", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 67", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 68", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 69", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS High");
            IS.NewRule("Rule 70", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS High");
            IS.NewRule("Rule 71", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS High");
            IS.NewRule("Rule 72", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS Medium and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 73", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 74", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS Middle");
            IS.NewRule("Rule 75", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Low and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 76", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS Middle");
            IS.NewRule("Rule 77", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS High");
            IS.NewRule("Rule 78", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS Medium and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 79", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Low THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 80", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS Medium THEN Recovery IS VeryHigh");
            IS.NewRule("Rule 81", "IF RecoveryOfMetalsForReuse IS High and FilmRecyclingForSilverReuse IS High and FoundryAreaRecovery IS High and SeparationAndRecyclingOfScrapForFoundry IS High THEN Recovery IS VeryHigh");

            IS.SetInput("RecoveryOfMetalsForReuse", (float)recoveryOfMetalsForReuseValue);
            IS.SetInput("FilmRecyclingForSilverReuse", (float)filmRecyclingForSilverReuseValue);
            IS.SetInput("FoundryAreaRecovery", (float)foundryAreaRecoveryValue);
            IS.SetInput("SeparationAndRecyclingOfScrapForFoundry", (float)separationAndRecyclingOfScrapForFoundryValue);

            double resultado = IS.Evaluate("Recovery");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("RecoveryOfMetalsForReuse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("FilmRecyclingForSilverReuse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("FoundryAreaRecovery", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SeparationAndRecyclingOfScrapForFoundry", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Recovery");
            }
            double m = (IS.GetLinguisticVariable("Recovery").End - IS.GetLinguisticVariable("Recovery").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Recovery").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSand(double foundrySandRecyclingValue, double useOfSandForOtherPurposesValue)
        {
            LinguisticVariable foundrySandRecycling = new( "FoundrySandRecycling", 0, 10 );
            foundrySandRecycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            foundrySandRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            foundrySandRecycling.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            foundrySandRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            foundrySandRecycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable useOfSandForOtherPurposes = new( "UseOfSandForOtherPurposes", 0, 10 );
            useOfSandForOtherPurposes.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfSandForOtherPurposes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfSandForOtherPurposes.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            useOfSandForOtherPurposes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfSandForOtherPurposes.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable sand = new( "Sand", 0, 10 );
            sand.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sand.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sand.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sand.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sand.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( foundrySandRecycling );
            fuzzyDB.AddVariable( useOfSandForOtherPurposes );
            fuzzyDB.AddVariable( sand );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF FoundrySandRecycling IS VeryLow and UseOfSandForOtherPurposes IS VeryLow THEN Sand IS VeryLow");
            IS.NewRule("Rule 2", "IF FoundrySandRecycling IS VeryLow and UseOfSandForOtherPurposes IS Low THEN Sand IS VeryLow");
            IS.NewRule("Rule 3", "IF FoundrySandRecycling IS VeryLow and UseOfSandForOtherPurposes IS Medium THEN Sand IS Low");
            IS.NewRule("Rule 4", "IF FoundrySandRecycling IS VeryLow and UseOfSandForOtherPurposes IS High THEN Sand IS Low");
            IS.NewRule("Rule 5", "IF FoundrySandRecycling IS VeryLow and UseOfSandForOtherPurposes IS VeryHigh THEN Sand IS Middle");
            IS.NewRule("Rule 6", "IF FoundrySandRecycling IS Low and UseOfSandForOtherPurposes IS VeryLow THEN Sand IS VeryLow");
            IS.NewRule("Rule 7", "IF FoundrySandRecycling IS Low and UseOfSandForOtherPurposes IS Low THEN Sand IS Low");
            IS.NewRule("Rule 8", "IF FoundrySandRecycling IS Low and UseOfSandForOtherPurposes IS Medium THEN Sand IS Low");
            IS.NewRule("Rule 9", "IF FoundrySandRecycling IS Low and UseOfSandForOtherPurposes IS High THEN Sand IS Middle");
            IS.NewRule("Rule 10", "IF FoundrySandRecycling IS Low and UseOfSandForOtherPurposes IS VeryHigh THEN Sand IS High");
            IS.NewRule("Rule 11", "IF FoundrySandRecycling IS Middle and UseOfSandForOtherPurposes IS VeryLow THEN Sand IS Low");
            IS.NewRule("Rule 12", "IF FoundrySandRecycling IS Middle and UseOfSandForOtherPurposes IS Low THEN Sand IS Low");
            IS.NewRule("Rule 13", "IF FoundrySandRecycling IS Middle and UseOfSandForOtherPurposes IS Medium THEN Sand IS Middle");
            IS.NewRule("Rule 14", "IF FoundrySandRecycling IS Middle and UseOfSandForOtherPurposes IS High THEN Sand IS High");
            IS.NewRule("Rule 15", "IF FoundrySandRecycling IS Middle and UseOfSandForOtherPurposes IS VeryHigh THEN Sand IS High");
            IS.NewRule("Rule 16", "IF FoundrySandRecycling IS High and UseOfSandForOtherPurposes IS VeryLow THEN Sand IS Low");
            IS.NewRule("Rule 17", "IF FoundrySandRecycling IS High and UseOfSandForOtherPurposes IS Low THEN Sand IS Middle");
            IS.NewRule("Rule 18", "IF FoundrySandRecycling IS High and UseOfSandForOtherPurposes IS Medium THEN Sand IS High");
            IS.NewRule("Rule 19", "IF FoundrySandRecycling IS High and UseOfSandForOtherPurposes IS High THEN Sand IS High");
            IS.NewRule("Rule 20", "IF FoundrySandRecycling IS High and UseOfSandForOtherPurposes IS VeryHigh THEN Sand IS VeryHigh");
            IS.NewRule("Rule 21", "IF FoundrySandRecycling IS VeryHigh and UseOfSandForOtherPurposes IS VeryLow THEN Sand IS Middle");
            IS.NewRule("Rule 22", "IF FoundrySandRecycling IS VeryHigh and UseOfSandForOtherPurposes IS Low THEN Sand IS High");
            IS.NewRule("Rule 23", "IF FoundrySandRecycling IS VeryHigh and UseOfSandForOtherPurposes IS Medium THEN Sand IS High");
            IS.NewRule("Rule 24", "IF FoundrySandRecycling IS VeryHigh and UseOfSandForOtherPurposes IS High THEN Sand IS VeryHigh");
            IS.NewRule("Rule 25", "IF FoundrySandRecycling IS VeryHigh and UseOfSandForOtherPurposes IS VeryHigh THEN Sand IS VeryHigh");

            IS.SetInput("FoundrySandRecycling", (float)foundrySandRecyclingValue);
            IS.SetInput("UseOfSandForOtherPurposes", (float)useOfSandForOtherPurposesValue);

            double resultado = IS.Evaluate("Sand");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("FoundrySandRecycling", i == 0 ? (float)9.99 : 0);
                IS.SetInput("UseOfSandForOtherPurposes", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Sand");
            }
            double m = (IS.GetLinguisticVariable("Sand").End - IS.GetLinguisticVariable("Sand").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Sand").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRecycle(double nonFerrousPowderRecyclingValue, double rubberProductRecyclingValue, double paperProductRecyclingValue)
        {
            LinguisticVariable nonFerrousPowderRecycling = new( "NonFerrousPowderRecycling", 0, 10 );
            nonFerrousPowderRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            nonFerrousPowderRecycling.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            nonFerrousPowderRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable rubberProductRecycling = new( "RubberProductRecycling", 0, 10 );
            rubberProductRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            rubberProductRecycling.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            rubberProductRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable paperProductRecycling = new( "PaperProductRecycling", 0, 10 );
            paperProductRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            paperProductRecycling.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            paperProductRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recycle = new( "Recycle", 0, 10 );
            recycle.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recycle.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recycle.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            recycle.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recycle.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( nonFerrousPowderRecycling );
            fuzzyDB.AddVariable( rubberProductRecycling );
            fuzzyDB.AddVariable( paperProductRecycling );
            fuzzyDB.AddVariable( recycle );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Low and PaperProductRecycling IS Low THEN Recycle IS VeryLow");
            IS.NewRule("Rule 2", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Low and PaperProductRecycling IS Medium THEN Recycle IS VeryLow");
            IS.NewRule("Rule 3", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Low and PaperProductRecycling IS High THEN Recycle IS Low");
            IS.NewRule("Rule 4", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Medium and PaperProductRecycling IS Low THEN Recycle IS VeryLow");
            IS.NewRule("Rule 5", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Medium and PaperProductRecycling IS Medium THEN Recycle IS Low");
            IS.NewRule("Rule 6", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS Medium and PaperProductRecycling IS High THEN Recycle IS Middle");
            IS.NewRule("Rule 7", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS High and PaperProductRecycling IS Low THEN Recycle IS Low");
            IS.NewRule("Rule 8", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS High and PaperProductRecycling IS Medium THEN Recycle IS Middle");
            IS.NewRule("Rule 9", "IF NonFerrousPowderRecycling IS Low and RubberProductRecycling IS High and PaperProductRecycling IS High THEN Recycle IS High");
            IS.NewRule("Rule 10", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Low and PaperProductRecycling IS Low THEN Recycle IS VeryLow");
            IS.NewRule("Rule 11", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Low and PaperProductRecycling IS Medium THEN Recycle IS Low");
            IS.NewRule("Rule 12", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Low and PaperProductRecycling IS High THEN Recycle IS Middle");
            IS.NewRule("Rule 13", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Medium and PaperProductRecycling IS Low THEN Recycle IS Low");
            IS.NewRule("Rule 14", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Medium and PaperProductRecycling IS Medium THEN Recycle IS Middle");
            IS.NewRule("Rule 15", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS Medium and PaperProductRecycling IS High THEN Recycle IS High");
            IS.NewRule("Rule 16", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS High and PaperProductRecycling IS Low THEN Recycle IS Middle");
            IS.NewRule("Rule 17", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS High and PaperProductRecycling IS Medium THEN Recycle IS High");
            IS.NewRule("Rule 18", "IF NonFerrousPowderRecycling IS Medium and RubberProductRecycling IS High and PaperProductRecycling IS High THEN Recycle IS VeryHigh");
            IS.NewRule("Rule 19", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Low and PaperProductRecycling IS Low THEN Recycle IS Low");
            IS.NewRule("Rule 20", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Low and PaperProductRecycling IS Medium THEN Recycle IS Middle");
            IS.NewRule("Rule 21", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Low and PaperProductRecycling IS High THEN Recycle IS High");
            IS.NewRule("Rule 22", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Medium and PaperProductRecycling IS Low THEN Recycle IS Middle");
            IS.NewRule("Rule 23", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Medium and PaperProductRecycling IS Medium THEN Recycle IS High");
            IS.NewRule("Rule 24", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS Medium and PaperProductRecycling IS High THEN Recycle IS VeryHigh");
            IS.NewRule("Rule 25", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS High and PaperProductRecycling IS Low THEN Recycle IS High");
            IS.NewRule("Rule 26", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS High and PaperProductRecycling IS Medium THEN Recycle IS VeryHigh");
            IS.NewRule("Rule 27", "IF NonFerrousPowderRecycling IS High and RubberProductRecycling IS High and PaperProductRecycling IS High THEN Recycle IS VeryHigh");

            IS.SetInput("NonFerrousPowderRecycling", (float)nonFerrousPowderRecyclingValue);
            IS.SetInput("RubberProductRecycling", (float)rubberProductRecyclingValue);
            IS.SetInput("PaperProductRecycling", (float)paperProductRecyclingValue);

            double resultado = IS.Evaluate("Recycle");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("NonFerrousPowderRecycling", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RubberProductRecycling", i == 0 ? 0 : (float)9.99);
                IS.SetInput("PaperProductRecycling", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Recycle");
            }
            double m = (IS.GetLinguisticVariable("Recycle").End - IS.GetLinguisticVariable("Recycle").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Recycle").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateScraps(double reuseOfScrapGlassValue, double reuseOfScrapPlasticPartsValue, double reuseOfPrintedPaperScrapValue)
        {
            LinguisticVariable reuseOfScrapGlass = new( "ReuseOfScrapGlass", 0, 10 );
            reuseOfScrapGlass.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reuseOfScrapGlass.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reuseOfScrapGlass.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reuseOfScrapPlasticParts = new( "ReuseOfScrapPlasticParts", 0, 10 );
            reuseOfScrapPlasticParts.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reuseOfScrapPlasticParts.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reuseOfScrapPlasticParts.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reuseOfPrintedPaperScrap = new( "ReuseOfPrintedPaperScrap", 0, 10 );
            reuseOfPrintedPaperScrap.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reuseOfPrintedPaperScrap.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reuseOfPrintedPaperScrap.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable scraps = new( "Scraps", 0, 10 );
            scraps.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            scraps.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            scraps.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            scraps.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            scraps.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reuseOfScrapGlass );
            fuzzyDB.AddVariable( reuseOfScrapPlasticParts );
            fuzzyDB.AddVariable( reuseOfPrintedPaperScrap );
            fuzzyDB.AddVariable( scraps );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS VeryLow");
            IS.NewRule("Rule 2", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS VeryLow");
            IS.NewRule("Rule 3", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS High THEN Scraps IS Low");
            IS.NewRule("Rule 4", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS VeryLow");
            IS.NewRule("Rule 5", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS Low");
            IS.NewRule("Rule 6", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS High THEN Scraps IS Middle");
            IS.NewRule("Rule 7", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS Low");
            IS.NewRule("Rule 8", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS Middle");
            IS.NewRule("Rule 9", "IF ReuseOfScrapGlass IS Low and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS High THEN Scraps IS High");
            IS.NewRule("Rule 10", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS VeryLow");
            IS.NewRule("Rule 11", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS Low");
            IS.NewRule("Rule 12", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS High THEN Scraps IS Middle");
            IS.NewRule("Rule 13", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS Low");
            IS.NewRule("Rule 14", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS Middle");
            IS.NewRule("Rule 15", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS High THEN Scraps IS High");
            IS.NewRule("Rule 16", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS Middle");
            IS.NewRule("Rule 17", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS High");
            IS.NewRule("Rule 18", "IF ReuseOfScrapGlass IS Medium and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS High THEN Scraps IS VeryHigh");
            IS.NewRule("Rule 19", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS Low");
            IS.NewRule("Rule 20", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS Middle");
            IS.NewRule("Rule 21", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Low and ReuseOfPrintedPaperScrap IS High THEN Scraps IS High");
            IS.NewRule("Rule 22", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS Middle");
            IS.NewRule("Rule 23", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS High");
            IS.NewRule("Rule 24", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS Medium and ReuseOfPrintedPaperScrap IS High THEN Scraps IS VeryHigh");
            IS.NewRule("Rule 25", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Low THEN Scraps IS High");
            IS.NewRule("Rule 26", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS Medium THEN Scraps IS VeryHigh");
            IS.NewRule("Rule 27", "IF ReuseOfScrapGlass IS High and ReuseOfScrapPlasticParts IS High and ReuseOfPrintedPaperScrap IS High THEN Scraps IS VeryHigh");

            IS.SetInput("ReuseOfScrapGlass", (float)reuseOfScrapGlassValue);
            IS.SetInput("ReuseOfScrapPlasticParts", (float)reuseOfScrapPlasticPartsValue);
            IS.SetInput("ReuseOfPrintedPaperScrap", (float)reuseOfPrintedPaperScrapValue);

            double resultado = IS.Evaluate("Scraps");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ReuseOfScrapGlass", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReuseOfScrapPlasticParts", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReuseOfPrintedPaperScrap", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Scraps");
            }
            double m = (IS.GetLinguisticVariable("Scraps").End - IS.GetLinguisticVariable("Scraps").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Scraps").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateWater(double dryAllPartsOfTheWaterSeparatorValue, double useOfDeionizedWaterToRinsePartsValue)
        {
            LinguisticVariable dryAllPartsOfTheWaterSeparator = new( "DryAllPartsOfTheWaterSeparator", 0, 10 );
            dryAllPartsOfTheWaterSeparator.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            dryAllPartsOfTheWaterSeparator.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            dryAllPartsOfTheWaterSeparator.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            dryAllPartsOfTheWaterSeparator.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            dryAllPartsOfTheWaterSeparator.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable useOfDeionizedWaterToRinseParts = new( "UseOfDeionizedWaterToRinseParts", 0, 10 );
            useOfDeionizedWaterToRinseParts.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfDeionizedWaterToRinseParts.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfDeionizedWaterToRinseParts.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            useOfDeionizedWaterToRinseParts.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfDeionizedWaterToRinseParts.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable water = new( "Water", 0, 10 );
            water.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            water.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            water.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            water.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            water.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( dryAllPartsOfTheWaterSeparator );
            fuzzyDB.AddVariable( useOfDeionizedWaterToRinseParts );
            fuzzyDB.AddVariable( water );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DryAllPartsOfTheWaterSeparator IS VeryLow and UseOfDeionizedWaterToRinseParts IS VeryLow THEN Water IS VeryLow");
            IS.NewRule("Rule 2", "IF DryAllPartsOfTheWaterSeparator IS VeryLow and UseOfDeionizedWaterToRinseParts IS Low THEN Water IS VeryLow");
            IS.NewRule("Rule 3", "IF DryAllPartsOfTheWaterSeparator IS VeryLow and UseOfDeionizedWaterToRinseParts IS Medium THEN Water IS Low");
            IS.NewRule("Rule 4", "IF DryAllPartsOfTheWaterSeparator IS VeryLow and UseOfDeionizedWaterToRinseParts IS High THEN Water IS Low");
            IS.NewRule("Rule 5", "IF DryAllPartsOfTheWaterSeparator IS VeryLow and UseOfDeionizedWaterToRinseParts IS VeryHigh THEN Water IS Middle");
            IS.NewRule("Rule 6", "IF DryAllPartsOfTheWaterSeparator IS Low and UseOfDeionizedWaterToRinseParts IS VeryLow THEN Water IS VeryLow");
            IS.NewRule("Rule 7", "IF DryAllPartsOfTheWaterSeparator IS Low and UseOfDeionizedWaterToRinseParts IS Low THEN Water IS Low");
            IS.NewRule("Rule 8", "IF DryAllPartsOfTheWaterSeparator IS Low and UseOfDeionizedWaterToRinseParts IS Medium THEN Water IS Low");
            IS.NewRule("Rule 9", "IF DryAllPartsOfTheWaterSeparator IS Low and UseOfDeionizedWaterToRinseParts IS High THEN Water IS Middle");
            IS.NewRule("Rule 10", "IF DryAllPartsOfTheWaterSeparator IS Low and UseOfDeionizedWaterToRinseParts IS VeryHigh THEN Water IS High");
            IS.NewRule("Rule 11", "IF DryAllPartsOfTheWaterSeparator IS Middle and UseOfDeionizedWaterToRinseParts IS VeryLow THEN Water IS Low");
            IS.NewRule("Rule 12", "IF DryAllPartsOfTheWaterSeparator IS Middle and UseOfDeionizedWaterToRinseParts IS Low THEN Water IS Low");
            IS.NewRule("Rule 13", "IF DryAllPartsOfTheWaterSeparator IS Middle and UseOfDeionizedWaterToRinseParts IS Medium THEN Water IS Middle");
            IS.NewRule("Rule 14", "IF DryAllPartsOfTheWaterSeparator IS Middle and UseOfDeionizedWaterToRinseParts IS High THEN Water IS High");
            IS.NewRule("Rule 15", "IF DryAllPartsOfTheWaterSeparator IS Middle and UseOfDeionizedWaterToRinseParts IS VeryHigh THEN Water IS High");
            IS.NewRule("Rule 16", "IF DryAllPartsOfTheWaterSeparator IS High and UseOfDeionizedWaterToRinseParts IS VeryLow THEN Water IS Low");
            IS.NewRule("Rule 17", "IF DryAllPartsOfTheWaterSeparator IS High and UseOfDeionizedWaterToRinseParts IS Low THEN Water IS Middle");
            IS.NewRule("Rule 18", "IF DryAllPartsOfTheWaterSeparator IS High and UseOfDeionizedWaterToRinseParts IS Medium THEN Water IS High");
            IS.NewRule("Rule 19", "IF DryAllPartsOfTheWaterSeparator IS High and UseOfDeionizedWaterToRinseParts IS High THEN Water IS High");
            IS.NewRule("Rule 20", "IF DryAllPartsOfTheWaterSeparator IS High and UseOfDeionizedWaterToRinseParts IS VeryHigh THEN Water IS VeryHigh");
            IS.NewRule("Rule 21", "IF DryAllPartsOfTheWaterSeparator IS VeryHigh and UseOfDeionizedWaterToRinseParts IS VeryLow THEN Water IS Middle");
            IS.NewRule("Rule 22", "IF DryAllPartsOfTheWaterSeparator IS VeryHigh and UseOfDeionizedWaterToRinseParts IS Low THEN Water IS High");
            IS.NewRule("Rule 23", "IF DryAllPartsOfTheWaterSeparator IS VeryHigh and UseOfDeionizedWaterToRinseParts IS Medium THEN Water IS High");
            IS.NewRule("Rule 24", "IF DryAllPartsOfTheWaterSeparator IS VeryHigh and UseOfDeionizedWaterToRinseParts IS High THEN Water IS VeryHigh");
            IS.NewRule("Rule 25", "IF DryAllPartsOfTheWaterSeparator IS VeryHigh and UseOfDeionizedWaterToRinseParts IS VeryHigh THEN Water IS VeryHigh");

            IS.SetInput("DryAllPartsOfTheWaterSeparator", (float)dryAllPartsOfTheWaterSeparatorValue);
            IS.SetInput("UseOfDeionizedWaterToRinseParts", (float)useOfDeionizedWaterToRinsePartsValue);

            double resultado = IS.Evaluate("Water");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DryAllPartsOfTheWaterSeparator", i == 0 ? (float)9.99 : 0);
                IS.SetInput("UseOfDeionizedWaterToRinseParts", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Water");
            }
            double m = (IS.GetLinguisticVariable("Water").End - IS.GetLinguisticVariable("Water").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Water").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateEmployees(double avoidExcessSolventByOperatorsValue, double numberOfEmployeesForPartsWashingValue)
        {
            LinguisticVariable avoidExcessSolventByOperators = new( "AvoidExcessSolventByOperators", 0, 10 );
            avoidExcessSolventByOperators.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            avoidExcessSolventByOperators.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            avoidExcessSolventByOperators.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            avoidExcessSolventByOperators.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            avoidExcessSolventByOperators.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable numberOfEmployeesForPartsWashing = new( "NumberOfEmployeesForPartsWashing", 0, 10 );
            numberOfEmployeesForPartsWashing.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            numberOfEmployeesForPartsWashing.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            numberOfEmployeesForPartsWashing.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            numberOfEmployeesForPartsWashing.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            numberOfEmployeesForPartsWashing.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable employees = new( "Employees", 0, 10 );
            employees.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            employees.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            employees.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            employees.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            employees.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( avoidExcessSolventByOperators );
            fuzzyDB.AddVariable( numberOfEmployeesForPartsWashing );
            fuzzyDB.AddVariable( employees );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF AvoidExcessSolventByOperators IS VeryLow and NumberOfEmployeesForPartsWashing IS VeryLow THEN Employees IS VeryLow");
            IS.NewRule("Rule 2", "IF AvoidExcessSolventByOperators IS VeryLow and NumberOfEmployeesForPartsWashing IS Low THEN Employees IS VeryLow");
            IS.NewRule("Rule 3", "IF AvoidExcessSolventByOperators IS VeryLow and NumberOfEmployeesForPartsWashing IS Medium THEN Employees IS Low");
            IS.NewRule("Rule 4", "IF AvoidExcessSolventByOperators IS VeryLow and NumberOfEmployeesForPartsWashing IS High THEN Employees IS Low");
            IS.NewRule("Rule 5", "IF AvoidExcessSolventByOperators IS VeryLow and NumberOfEmployeesForPartsWashing IS VeryHigh THEN Employees IS Middle");
            IS.NewRule("Rule 6", "IF AvoidExcessSolventByOperators IS Low and NumberOfEmployeesForPartsWashing IS VeryLow THEN Employees IS VeryLow");
            IS.NewRule("Rule 7", "IF AvoidExcessSolventByOperators IS Low and NumberOfEmployeesForPartsWashing IS Low THEN Employees IS Low");
            IS.NewRule("Rule 8", "IF AvoidExcessSolventByOperators IS Low and NumberOfEmployeesForPartsWashing IS Medium THEN Employees IS Low");
            IS.NewRule("Rule 9", "IF AvoidExcessSolventByOperators IS Low and NumberOfEmployeesForPartsWashing IS High THEN Employees IS Middle");
            IS.NewRule("Rule 10", "IF AvoidExcessSolventByOperators IS Low and NumberOfEmployeesForPartsWashing IS VeryHigh THEN Employees IS High");
            IS.NewRule("Rule 11", "IF AvoidExcessSolventByOperators IS Middle and NumberOfEmployeesForPartsWashing IS VeryLow THEN Employees IS Low");
            IS.NewRule("Rule 12", "IF AvoidExcessSolventByOperators IS Middle and NumberOfEmployeesForPartsWashing IS Low THEN Employees IS Low");
            IS.NewRule("Rule 13", "IF AvoidExcessSolventByOperators IS Middle and NumberOfEmployeesForPartsWashing IS Medium THEN Employees IS Middle");
            IS.NewRule("Rule 14", "IF AvoidExcessSolventByOperators IS Middle and NumberOfEmployeesForPartsWashing IS High THEN Employees IS High");
            IS.NewRule("Rule 15", "IF AvoidExcessSolventByOperators IS Middle and NumberOfEmployeesForPartsWashing IS VeryHigh THEN Employees IS High");
            IS.NewRule("Rule 16", "IF AvoidExcessSolventByOperators IS High and NumberOfEmployeesForPartsWashing IS VeryLow THEN Employees IS Low");
            IS.NewRule("Rule 17", "IF AvoidExcessSolventByOperators IS High and NumberOfEmployeesForPartsWashing IS Low THEN Employees IS Middle");
            IS.NewRule("Rule 18", "IF AvoidExcessSolventByOperators IS High and NumberOfEmployeesForPartsWashing IS Medium THEN Employees IS High");
            IS.NewRule("Rule 19", "IF AvoidExcessSolventByOperators IS High and NumberOfEmployeesForPartsWashing IS High THEN Employees IS High");
            IS.NewRule("Rule 20", "IF AvoidExcessSolventByOperators IS High and NumberOfEmployeesForPartsWashing IS VeryHigh THEN Employees IS VeryHigh");
            IS.NewRule("Rule 21", "IF AvoidExcessSolventByOperators IS VeryHigh and NumberOfEmployeesForPartsWashing IS VeryLow THEN Employees IS Middle");
            IS.NewRule("Rule 22", "IF AvoidExcessSolventByOperators IS VeryHigh and NumberOfEmployeesForPartsWashing IS Low THEN Employees IS High");
            IS.NewRule("Rule 23", "IF AvoidExcessSolventByOperators IS VeryHigh and NumberOfEmployeesForPartsWashing IS Medium THEN Employees IS High");
            IS.NewRule("Rule 24", "IF AvoidExcessSolventByOperators IS VeryHigh and NumberOfEmployeesForPartsWashing IS High THEN Employees IS VeryHigh");
            IS.NewRule("Rule 25", "IF AvoidExcessSolventByOperators IS VeryHigh and NumberOfEmployeesForPartsWashing IS VeryHigh THEN Employees IS VeryHigh");

            IS.SetInput("AvoidExcessSolventByOperators", (float)avoidExcessSolventByOperatorsValue);
            IS.SetInput("NumberOfEmployeesForPartsWashing", (float)numberOfEmployeesForPartsWashingValue);

            double resultado = IS.Evaluate("Employees");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("AvoidExcessSolventByOperators", i == 0 ? (float)9.99 : 0);
                IS.SetInput("NumberOfEmployeesForPartsWashing", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Employees");
            }
            double m = (IS.GetLinguisticVariable("Employees").End - IS.GetLinguisticVariable("Employees").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Employees").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateStopper(double useOfFittedLidsValue, double floatingLidsOnMaterialTanksValue)
        {
            LinguisticVariable useOfFittedLids = new( "UseOfFittedLids", 0, 10 );
            useOfFittedLids.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfFittedLids.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfFittedLids.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            useOfFittedLids.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfFittedLids.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable floatingLidsOnMaterialTanks = new( "FloatingLidsOnMaterialTanks", 0, 10 );
            floatingLidsOnMaterialTanks.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            floatingLidsOnMaterialTanks.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            floatingLidsOnMaterialTanks.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            floatingLidsOnMaterialTanks.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            floatingLidsOnMaterialTanks.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable stopper = new( "Stopper", 0, 10 );
            stopper.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            stopper.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            stopper.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            stopper.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            stopper.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfFittedLids );
            fuzzyDB.AddVariable( floatingLidsOnMaterialTanks );
            fuzzyDB.AddVariable( stopper );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfFittedLids IS VeryLow and FloatingLidsOnMaterialTanks IS VeryLow THEN Stopper IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfFittedLids IS VeryLow and FloatingLidsOnMaterialTanks IS Low THEN Stopper IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfFittedLids IS VeryLow and FloatingLidsOnMaterialTanks IS Medium THEN Stopper IS Low");
            IS.NewRule("Rule 4", "IF UseOfFittedLids IS VeryLow and FloatingLidsOnMaterialTanks IS High THEN Stopper IS Low");
            IS.NewRule("Rule 5", "IF UseOfFittedLids IS VeryLow and FloatingLidsOnMaterialTanks IS VeryHigh THEN Stopper IS Middle");
            IS.NewRule("Rule 6", "IF UseOfFittedLids IS Low and FloatingLidsOnMaterialTanks IS VeryLow THEN Stopper IS VeryLow");
            IS.NewRule("Rule 7", "IF UseOfFittedLids IS Low and FloatingLidsOnMaterialTanks IS Low THEN Stopper IS Low");
            IS.NewRule("Rule 8", "IF UseOfFittedLids IS Low and FloatingLidsOnMaterialTanks IS Medium THEN Stopper IS Low");
            IS.NewRule("Rule 9", "IF UseOfFittedLids IS Low and FloatingLidsOnMaterialTanks IS High THEN Stopper IS Middle");
            IS.NewRule("Rule 10", "IF UseOfFittedLids IS Low and FloatingLidsOnMaterialTanks IS VeryHigh THEN Stopper IS High");
            IS.NewRule("Rule 11", "IF UseOfFittedLids IS Middle and FloatingLidsOnMaterialTanks IS VeryLow THEN Stopper IS Low");
            IS.NewRule("Rule 12", "IF UseOfFittedLids IS Middle and FloatingLidsOnMaterialTanks IS Low THEN Stopper IS Low");
            IS.NewRule("Rule 13", "IF UseOfFittedLids IS Middle and FloatingLidsOnMaterialTanks IS Medium THEN Stopper IS Middle");
            IS.NewRule("Rule 14", "IF UseOfFittedLids IS Middle and FloatingLidsOnMaterialTanks IS High THEN Stopper IS High");
            IS.NewRule("Rule 15", "IF UseOfFittedLids IS Middle and FloatingLidsOnMaterialTanks IS VeryHigh THEN Stopper IS High");
            IS.NewRule("Rule 16", "IF UseOfFittedLids IS High and FloatingLidsOnMaterialTanks IS VeryLow THEN Stopper IS Low");
            IS.NewRule("Rule 17", "IF UseOfFittedLids IS High and FloatingLidsOnMaterialTanks IS Low THEN Stopper IS Middle");
            IS.NewRule("Rule 18", "IF UseOfFittedLids IS High and FloatingLidsOnMaterialTanks IS Medium THEN Stopper IS High");
            IS.NewRule("Rule 19", "IF UseOfFittedLids IS High and FloatingLidsOnMaterialTanks IS High THEN Stopper IS High");
            IS.NewRule("Rule 20", "IF UseOfFittedLids IS High and FloatingLidsOnMaterialTanks IS VeryHigh THEN Stopper IS VeryHigh");
            IS.NewRule("Rule 21", "IF UseOfFittedLids IS VeryHigh and FloatingLidsOnMaterialTanks IS VeryLow THEN Stopper IS Middle");
            IS.NewRule("Rule 22", "IF UseOfFittedLids IS VeryHigh and FloatingLidsOnMaterialTanks IS Low THEN Stopper IS High");
            IS.NewRule("Rule 23", "IF UseOfFittedLids IS VeryHigh and FloatingLidsOnMaterialTanks IS Medium THEN Stopper IS High");
            IS.NewRule("Rule 24", "IF UseOfFittedLids IS VeryHigh and FloatingLidsOnMaterialTanks IS High THEN Stopper IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfFittedLids IS VeryHigh and FloatingLidsOnMaterialTanks IS VeryHigh THEN Stopper IS VeryHigh");

            IS.SetInput("UseOfFittedLids", (float)useOfFittedLidsValue);
            IS.SetInput("FloatingLidsOnMaterialTanks", (float)floatingLidsOnMaterialTanksValue);

            double resultado = IS.Evaluate("Stopper");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfFittedLids", i == 0 ? (float)9.99 : 0);
                IS.SetInput("FloatingLidsOnMaterialTanks", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Stopper");
            }
            double m = (IS.GetLinguisticVariable("Stopper").End - IS.GetLinguisticVariable("Stopper").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Stopper").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSteamMinimization(double decreaseSteamLossesValue, double useOfSteamGasRecoveryValue)
        {
            LinguisticVariable decreaseSteamLosses = new( "DecreaseSteamLosses", 0, 10 );
            decreaseSteamLosses.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            decreaseSteamLosses.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            decreaseSteamLosses.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            decreaseSteamLosses.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            decreaseSteamLosses.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable useOfSteamGasRecovery = new( "UseOfSteamGasRecovery", 0, 10 );
            useOfSteamGasRecovery.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfSteamGasRecovery.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfSteamGasRecovery.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            useOfSteamGasRecovery.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfSteamGasRecovery.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable steamMinimization = new( "SteamMinimization", 0, 10 );
            steamMinimization.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            steamMinimization.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            steamMinimization.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            steamMinimization.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            steamMinimization.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( decreaseSteamLosses );
            fuzzyDB.AddVariable( useOfSteamGasRecovery );
            fuzzyDB.AddVariable( steamMinimization );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DecreaseSteamLosses IS VeryLow and UseOfSteamGasRecovery IS VeryLow THEN SteamMinimization IS VeryLow");
            IS.NewRule("Rule 2", "IF DecreaseSteamLosses IS VeryLow and UseOfSteamGasRecovery IS Low THEN SteamMinimization IS VeryLow");
            IS.NewRule("Rule 3", "IF DecreaseSteamLosses IS VeryLow and UseOfSteamGasRecovery IS Medium THEN SteamMinimization IS Low");
            IS.NewRule("Rule 4", "IF DecreaseSteamLosses IS VeryLow and UseOfSteamGasRecovery IS High THEN SteamMinimization IS Low");
            IS.NewRule("Rule 5", "IF DecreaseSteamLosses IS VeryLow and UseOfSteamGasRecovery IS VeryHigh THEN SteamMinimization IS Middle");
            IS.NewRule("Rule 6", "IF DecreaseSteamLosses IS Low and UseOfSteamGasRecovery IS VeryLow THEN SteamMinimization IS VeryLow");
            IS.NewRule("Rule 7", "IF DecreaseSteamLosses IS Low and UseOfSteamGasRecovery IS Low THEN SteamMinimization IS Low");
            IS.NewRule("Rule 8", "IF DecreaseSteamLosses IS Low and UseOfSteamGasRecovery IS Medium THEN SteamMinimization IS Low");
            IS.NewRule("Rule 9", "IF DecreaseSteamLosses IS Low and UseOfSteamGasRecovery IS High THEN SteamMinimization IS Middle");
            IS.NewRule("Rule 10", "IF DecreaseSteamLosses IS Low and UseOfSteamGasRecovery IS VeryHigh THEN SteamMinimization IS High");
            IS.NewRule("Rule 11", "IF DecreaseSteamLosses IS Middle and UseOfSteamGasRecovery IS VeryLow THEN SteamMinimization IS Low");
            IS.NewRule("Rule 12", "IF DecreaseSteamLosses IS Middle and UseOfSteamGasRecovery IS Low THEN SteamMinimization IS Low");
            IS.NewRule("Rule 13", "IF DecreaseSteamLosses IS Middle and UseOfSteamGasRecovery IS Medium THEN SteamMinimization IS Middle");
            IS.NewRule("Rule 14", "IF DecreaseSteamLosses IS Middle and UseOfSteamGasRecovery IS High THEN SteamMinimization IS High");
            IS.NewRule("Rule 15", "IF DecreaseSteamLosses IS Middle and UseOfSteamGasRecovery IS VeryHigh THEN SteamMinimization IS High");
            IS.NewRule("Rule 16", "IF DecreaseSteamLosses IS High and UseOfSteamGasRecovery IS VeryLow THEN SteamMinimization IS Low");
            IS.NewRule("Rule 17", "IF DecreaseSteamLosses IS High and UseOfSteamGasRecovery IS Low THEN SteamMinimization IS Middle");
            IS.NewRule("Rule 18", "IF DecreaseSteamLosses IS High and UseOfSteamGasRecovery IS Medium THEN SteamMinimization IS High");
            IS.NewRule("Rule 19", "IF DecreaseSteamLosses IS High and UseOfSteamGasRecovery IS High THEN SteamMinimization IS High");
            IS.NewRule("Rule 20", "IF DecreaseSteamLosses IS High and UseOfSteamGasRecovery IS VeryHigh THEN SteamMinimization IS VeryHigh");
            IS.NewRule("Rule 21", "IF DecreaseSteamLosses IS VeryHigh and UseOfSteamGasRecovery IS VeryLow THEN SteamMinimization IS Middle");
            IS.NewRule("Rule 22", "IF DecreaseSteamLosses IS VeryHigh and UseOfSteamGasRecovery IS Low THEN SteamMinimization IS High");
            IS.NewRule("Rule 23", "IF DecreaseSteamLosses IS VeryHigh and UseOfSteamGasRecovery IS Medium THEN SteamMinimization IS High");
            IS.NewRule("Rule 24", "IF DecreaseSteamLosses IS VeryHigh and UseOfSteamGasRecovery IS High THEN SteamMinimization IS VeryHigh");
            IS.NewRule("Rule 25", "IF DecreaseSteamLosses IS VeryHigh and UseOfSteamGasRecovery IS VeryHigh THEN SteamMinimization IS VeryHigh");

            IS.SetInput("DecreaseSteamLosses", (float)decreaseSteamLossesValue);
            IS.SetInput("UseOfSteamGasRecovery", (float)useOfSteamGasRecoveryValue);

            double resultado = IS.Evaluate("SteamMinimization");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DecreaseSteamLosses", i == 0 ? (float)9.99 : 0);
                IS.SetInput("UseOfSteamGasRecovery", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("SteamMinimization");
            }
            double m = (IS.GetLinguisticVariable("SteamMinimization").End - IS.GetLinguisticVariable("SteamMinimization").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SteamMinimization").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolvent(double cleaningValue, double distillationValue, double reuseLevel2Value)
        {
            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            cleaning.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            cleaning.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable distillation = new( "Distillation", 0, 10 );
            distillation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            distillation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            distillation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reuseLevel2 = new( "ReuseLevel2", 0, 10 );
            reuseLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reuseLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reuseLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solvent = new( "Solvent", 0, 10 );
            solvent.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solvent.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solvent.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solvent.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solvent.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( distillation );
            fuzzyDB.AddVariable( reuseLevel2 );
            fuzzyDB.AddVariable( solvent );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Cleaning IS Low and Distillation IS Low and ReuseLevel2 IS Low THEN Solvent IS VeryLow");
            IS.NewRule("Rule 2", "IF Cleaning IS Low and Distillation IS Low and ReuseLevel2 IS Medium THEN Solvent IS VeryLow");
            IS.NewRule("Rule 3", "IF Cleaning IS Low and Distillation IS Low and ReuseLevel2 IS High THEN Solvent IS Low");
            IS.NewRule("Rule 4", "IF Cleaning IS Low and Distillation IS Medium and ReuseLevel2 IS Low THEN Solvent IS VeryLow");
            IS.NewRule("Rule 5", "IF Cleaning IS Low and Distillation IS Medium and ReuseLevel2 IS Medium THEN Solvent IS Low");
            IS.NewRule("Rule 6", "IF Cleaning IS Low and Distillation IS Medium and ReuseLevel2 IS High THEN Solvent IS Middle");
            IS.NewRule("Rule 7", "IF Cleaning IS Low and Distillation IS High and ReuseLevel2 IS Low THEN Solvent IS Low");
            IS.NewRule("Rule 8", "IF Cleaning IS Low and Distillation IS High and ReuseLevel2 IS Medium THEN Solvent IS Middle");
            IS.NewRule("Rule 9", "IF Cleaning IS Low and Distillation IS High and ReuseLevel2 IS High THEN Solvent IS High");
            IS.NewRule("Rule 10", "IF Cleaning IS Medium and Distillation IS Low and ReuseLevel2 IS Low THEN Solvent IS VeryLow");
            IS.NewRule("Rule 11", "IF Cleaning IS Medium and Distillation IS Low and ReuseLevel2 IS Medium THEN Solvent IS Low");
            IS.NewRule("Rule 12", "IF Cleaning IS Medium and Distillation IS Low and ReuseLevel2 IS High THEN Solvent IS Middle");
            IS.NewRule("Rule 13", "IF Cleaning IS Medium and Distillation IS Medium and ReuseLevel2 IS Low THEN Solvent IS Low");
            IS.NewRule("Rule 14", "IF Cleaning IS Medium and Distillation IS Medium and ReuseLevel2 IS Medium THEN Solvent IS Middle");
            IS.NewRule("Rule 15", "IF Cleaning IS Medium and Distillation IS Medium and ReuseLevel2 IS High THEN Solvent IS High");
            IS.NewRule("Rule 16", "IF Cleaning IS Medium and Distillation IS High and ReuseLevel2 IS Low THEN Solvent IS Middle");
            IS.NewRule("Rule 17", "IF Cleaning IS Medium and Distillation IS High and ReuseLevel2 IS Medium THEN Solvent IS High");
            IS.NewRule("Rule 18", "IF Cleaning IS Medium and Distillation IS High and ReuseLevel2 IS High THEN Solvent IS VeryHigh");
            IS.NewRule("Rule 19", "IF Cleaning IS High and Distillation IS Low and ReuseLevel2 IS Low THEN Solvent IS Low");
            IS.NewRule("Rule 20", "IF Cleaning IS High and Distillation IS Low and ReuseLevel2 IS Medium THEN Solvent IS Middle");
            IS.NewRule("Rule 21", "IF Cleaning IS High and Distillation IS Low and ReuseLevel2 IS High THEN Solvent IS High");
            IS.NewRule("Rule 22", "IF Cleaning IS High and Distillation IS Medium and ReuseLevel2 IS Low THEN Solvent IS Middle");
            IS.NewRule("Rule 23", "IF Cleaning IS High and Distillation IS Medium and ReuseLevel2 IS Medium THEN Solvent IS High");
            IS.NewRule("Rule 24", "IF Cleaning IS High and Distillation IS Medium and ReuseLevel2 IS High THEN Solvent IS VeryHigh");
            IS.NewRule("Rule 25", "IF Cleaning IS High and Distillation IS High and ReuseLevel2 IS Low THEN Solvent IS High");
            IS.NewRule("Rule 26", "IF Cleaning IS High and Distillation IS High and ReuseLevel2 IS Medium THEN Solvent IS VeryHigh");
            IS.NewRule("Rule 27", "IF Cleaning IS High and Distillation IS High and ReuseLevel2 IS High THEN Solvent IS VeryHigh");

            IS.SetInput("Cleaning", (float)cleaningValue);
            IS.SetInput("Distillation", (float)distillationValue);
            IS.SetInput("ReuseLevel2", (float)reuseLevel2Value);

            double resultado = IS.Evaluate("Solvent");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Cleaning", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Distillation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReuseLevel2", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Solvent");
            }
            double m = (IS.GetLinguisticVariable("Solvent").End - IS.GetLinguisticVariable("Solvent").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solvent").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateWaterLevel2(double useOfWaterBasedAdhesivesValue, double conversionOfLiquidMaterialsForCleaningValue, double solventReplacementForWaterBasedCuttingFluidsValue)
        {
            LinguisticVariable useOfWaterBasedAdhesives = new( "UseOfWaterBasedAdhesives", 0, 10 );
            useOfWaterBasedAdhesives.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfWaterBasedAdhesives.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfWaterBasedAdhesives.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable conversionOfLiquidMaterialsForCleaning = new( "ConversionOfLiquidMaterialsForCleaning", 0, 10 );
            conversionOfLiquidMaterialsForCleaning.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            conversionOfLiquidMaterialsForCleaning.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            conversionOfLiquidMaterialsForCleaning.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solventReplacementForWaterBasedCuttingFluids = new( "SolventReplacementForWaterBasedCuttingFluids", 0, 10 );
            solventReplacementForWaterBasedCuttingFluids.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solventReplacementForWaterBasedCuttingFluids.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solventReplacementForWaterBasedCuttingFluids.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable waterLevel2 = new( "WaterLevel2", 0, 10 );
            waterLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterLevel2.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfWaterBasedAdhesives );
            fuzzyDB.AddVariable( conversionOfLiquidMaterialsForCleaning );
            fuzzyDB.AddVariable( solventReplacementForWaterBasedCuttingFluids );
            fuzzyDB.AddVariable( waterLevel2 );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 4", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 6", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 7", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 8", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 9", "IF UseOfWaterBasedAdhesives IS Low and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS High");
            IS.NewRule("Rule 10", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 12", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 13", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 14", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 15", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS High");
            IS.NewRule("Rule 16", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 17", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS High");
            IS.NewRule("Rule 18", "IF UseOfWaterBasedAdhesives IS Medium and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS VeryHigh");
            IS.NewRule("Rule 19", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS Low");
            IS.NewRule("Rule 20", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 21", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Low and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS High");
            IS.NewRule("Rule 22", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS Middle");
            IS.NewRule("Rule 23", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS High");
            IS.NewRule("Rule 24", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS Medium and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Low THEN WaterLevel2 IS High");
            IS.NewRule("Rule 26", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS Medium THEN WaterLevel2 IS VeryHigh");
            IS.NewRule("Rule 27", "IF UseOfWaterBasedAdhesives IS High and ConversionOfLiquidMaterialsForCleaning IS High and SolventReplacementForWaterBasedCuttingFluids IS High THEN WaterLevel2 IS VeryHigh");

            IS.SetInput("UseOfWaterBasedAdhesives", (float)useOfWaterBasedAdhesivesValue);
            IS.SetInput("ConversionOfLiquidMaterialsForCleaning", (float)conversionOfLiquidMaterialsForCleaningValue);
            IS.SetInput("SolventReplacementForWaterBasedCuttingFluids", (float)solventReplacementForWaterBasedCuttingFluidsValue);

            double resultado = IS.Evaluate("WaterLevel2");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfWaterBasedAdhesives", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ConversionOfLiquidMaterialsForCleaning", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SolventReplacementForWaterBasedCuttingFluids", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("WaterLevel2");
            }
            double m = (IS.GetLinguisticVariable("WaterLevel2").End - IS.GetLinguisticVariable("WaterLevel2").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WaterLevel2").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolventLevel2(double useOfLessToxicAndVolatileSolventsValue, double useOfWaterBasedPaintsValue, double useOfSoyBasedPaintsValue)
        {
            LinguisticVariable useOfLessToxicAndVolatileSolvents = new( "UseOfLessToxicAndVolatileSolvents", 0, 10 );
            useOfLessToxicAndVolatileSolvents.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfLessToxicAndVolatileSolvents.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfLessToxicAndVolatileSolvents.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfWaterBasedPaints = new( "UseOfWaterBasedPaints", 0, 10 );
            useOfWaterBasedPaints.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfWaterBasedPaints.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfWaterBasedPaints.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable UseOfSoyBasedPaints = new( "UseOfSoyBasedPaints", 0, 10 );
            UseOfSoyBasedPaints.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            UseOfSoyBasedPaints.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            UseOfSoyBasedPaints.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solventLevel2 = new( "SolventLevel2", 0, 10 );
            solventLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solventLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solventLevel2.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solventLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solventLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfLessToxicAndVolatileSolvents );
            fuzzyDB.AddVariable( useOfWaterBasedPaints );
            fuzzyDB.AddVariable( UseOfSoyBasedPaints );
            fuzzyDB.AddVariable( solventLevel2 );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 4", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 6", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 7", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 8", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 9", "IF UseOfLessToxicAndVolatileSolvents IS Low and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS High");
            IS.NewRule("Rule 10", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 12", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 13", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 14", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 15", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS High");
            IS.NewRule("Rule 16", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 17", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS High");
            IS.NewRule("Rule 18", "IF UseOfLessToxicAndVolatileSolvents IS Medium and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS VeryHigh");
            IS.NewRule("Rule 19", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS Low");
            IS.NewRule("Rule 20", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 21", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Low and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS High");
            IS.NewRule("Rule 22", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS Middle");
            IS.NewRule("Rule 23", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS High");
            IS.NewRule("Rule 24", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS Medium and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Low THEN SolventLevel2 IS High");
            IS.NewRule("Rule 26", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS Medium THEN SolventLevel2 IS VeryHigh");
            IS.NewRule("Rule 27", "IF UseOfLessToxicAndVolatileSolvents IS High and UseOfWaterBasedPaints IS High and UseOfSoyBasedPaints IS High THEN SolventLevel2 IS VeryHigh");

            IS.SetInput("UseOfLessToxicAndVolatileSolvents", (float)useOfLessToxicAndVolatileSolventsValue);
            IS.SetInput("UseOfWaterBasedPaints", (float)useOfWaterBasedPaintsValue);
            IS.SetInput("UseOfSoyBasedPaints", (float)useOfSoyBasedPaintsValue);

            double resultado = IS.Evaluate("SolventLevel2");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfLessToxicAndVolatileSolvents", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfWaterBasedPaints", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfSoyBasedPaints", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("SolventLevel2");
            }
            double m = (IS.GetLinguisticVariable("SolventLevel2").End - IS.GetLinguisticVariable("SolventLevel2").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SolventLevel2").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolids(double useOfMaterialsWithLessEnergyUseValue, double alterRawMaterialForLessEmissionValue, double useOfMaterialsInRenewableContainersValue)
        {
            LinguisticVariable useOfMaterialsWithLessEnergyUse = new( "UseOfMaterialsWithLessEnergyUse", 0, 10 );
            useOfMaterialsWithLessEnergyUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfMaterialsWithLessEnergyUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfMaterialsWithLessEnergyUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable alterRawMaterialForLessEmission = new( "AlterRawMaterialForLessEmission", 0, 10 );
            alterRawMaterialForLessEmission.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            alterRawMaterialForLessEmission.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            alterRawMaterialForLessEmission.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfMaterialsInRenewableContainers = new( "UseOfMaterialsInRenewableContainers", 0, 10 );
            useOfMaterialsInRenewableContainers.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfMaterialsInRenewableContainers.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfMaterialsInRenewableContainers.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solids = new( "Solids", 0, 10 );
            solids.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solids.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solids.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solids.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solids.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfMaterialsWithLessEnergyUse );
            fuzzyDB.AddVariable( alterRawMaterialForLessEmission );
            fuzzyDB.AddVariable( useOfMaterialsInRenewableContainers );
            fuzzyDB.AddVariable( solids );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS High THEN Solids IS Low");
            IS.NewRule("Rule 4", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS Low");
            IS.NewRule("Rule 6", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS High THEN Solids IS Middle");
            IS.NewRule("Rule 7", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS Low");
            IS.NewRule("Rule 8", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS Middle");
            IS.NewRule("Rule 9", "IF UseOfMaterialsWithLessEnergyUse IS Low and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS High THEN Solids IS High");
            IS.NewRule("Rule 10", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS Low");
            IS.NewRule("Rule 12", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS High THEN Solids IS Middle");
            IS.NewRule("Rule 13", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS Low");
            IS.NewRule("Rule 14", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS Middle");
            IS.NewRule("Rule 15", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS High THEN Solids IS High");
            IS.NewRule("Rule 16", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS Middle");
            IS.NewRule("Rule 17", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS High");
            IS.NewRule("Rule 18", "IF UseOfMaterialsWithLessEnergyUse IS Medium and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS High THEN Solids IS VeryHigh");
            IS.NewRule("Rule 19", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS Low");
            IS.NewRule("Rule 20", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS Middle");
            IS.NewRule("Rule 21", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Low and UseOfMaterialsInRenewableContainers IS High THEN Solids IS High");
            IS.NewRule("Rule 22", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS Middle");
            IS.NewRule("Rule 23", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS High");
            IS.NewRule("Rule 24", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS Medium and UseOfMaterialsInRenewableContainers IS High THEN Solids IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Low THEN Solids IS High");
            IS.NewRule("Rule 26", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS Medium THEN Solids IS VeryHigh");
            IS.NewRule("Rule 27", "IF UseOfMaterialsWithLessEnergyUse IS High and AlterRawMaterialForLessEmission IS High and UseOfMaterialsInRenewableContainers IS High THEN Solids IS VeryHigh");

            IS.SetInput("UseOfMaterialsWithLessEnergyUse", (float)useOfMaterialsWithLessEnergyUseValue);
            IS.SetInput("AlterRawMaterialForLessEmission", (float)alterRawMaterialForLessEmissionValue);
            IS.SetInput("UseOfMaterialsInRenewableContainers", (float)useOfMaterialsInRenewableContainersValue);

            double resultado = IS.Evaluate("Solids");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfMaterialsWithLessEnergyUse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("AlterRawMaterialForLessEmission", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfMaterialsInRenewableContainers", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Solids");
            }
            double m = (IS.GetLinguisticVariable("Solids").End - IS.GetLinguisticVariable("Solids").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solids").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateWaterBasedSubstitutes(double useOfAqueousCleaningSystemValue, double finishIndustrialProcessWithWaterBasedProductValue)
        {
            LinguisticVariable useOfAqueousCleaningSystem = new( "UseOfAqueousCleaningSystem", 0, 10 );
            useOfAqueousCleaningSystem.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfAqueousCleaningSystem.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfAqueousCleaningSystem.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            useOfAqueousCleaningSystem.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfAqueousCleaningSystem.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable finishIndustrialProcessWithWaterBasedProduct = new( "FinishIndustrialProcessWithWaterBasedProduct", 0, 10 );
            finishIndustrialProcessWithWaterBasedProduct.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            finishIndustrialProcessWithWaterBasedProduct.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            finishIndustrialProcessWithWaterBasedProduct.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            finishIndustrialProcessWithWaterBasedProduct.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            finishIndustrialProcessWithWaterBasedProduct.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable waterBasedSubstitutes = new( "WaterBasedSubstitutes", 0, 10 );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfAqueousCleaningSystem );
            fuzzyDB.AddVariable( finishIndustrialProcessWithWaterBasedProduct );
            fuzzyDB.AddVariable( waterBasedSubstitutes );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfAqueousCleaningSystem IS VeryLow and FinishIndustrialProcessWithWaterBasedProduct IS VeryLow THEN WaterBasedSubstitutes IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfAqueousCleaningSystem IS VeryLow and FinishIndustrialProcessWithWaterBasedProduct IS Low THEN WaterBasedSubstitutes IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfAqueousCleaningSystem IS VeryLow and FinishIndustrialProcessWithWaterBasedProduct IS Medium THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 4", "IF UseOfAqueousCleaningSystem IS VeryLow and FinishIndustrialProcessWithWaterBasedProduct IS High THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 5", "IF UseOfAqueousCleaningSystem IS VeryLow and FinishIndustrialProcessWithWaterBasedProduct IS VeryHigh THEN WaterBasedSubstitutes IS Middle");
            IS.NewRule("Rule 6", "IF UseOfAqueousCleaningSystem IS Low and FinishIndustrialProcessWithWaterBasedProduct IS VeryLow THEN WaterBasedSubstitutes IS VeryLow");
            IS.NewRule("Rule 7", "IF UseOfAqueousCleaningSystem IS Low and FinishIndustrialProcessWithWaterBasedProduct IS Low THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 8", "IF UseOfAqueousCleaningSystem IS Low and FinishIndustrialProcessWithWaterBasedProduct IS Medium THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 9", "IF UseOfAqueousCleaningSystem IS Low and FinishIndustrialProcessWithWaterBasedProduct IS High THEN WaterBasedSubstitutes IS Middle");
            IS.NewRule("Rule 10", "IF UseOfAqueousCleaningSystem IS Low and FinishIndustrialProcessWithWaterBasedProduct IS VeryHigh THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 11", "IF UseOfAqueousCleaningSystem IS Middle and FinishIndustrialProcessWithWaterBasedProduct IS VeryLow THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 12", "IF UseOfAqueousCleaningSystem IS Middle and FinishIndustrialProcessWithWaterBasedProduct IS Low THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 13", "IF UseOfAqueousCleaningSystem IS Middle and FinishIndustrialProcessWithWaterBasedProduct IS Medium THEN WaterBasedSubstitutes IS Middle");
            IS.NewRule("Rule 14", "IF UseOfAqueousCleaningSystem IS Middle and FinishIndustrialProcessWithWaterBasedProduct IS High THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 15", "IF UseOfAqueousCleaningSystem IS Middle and FinishIndustrialProcessWithWaterBasedProduct IS VeryHigh THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 16", "IF UseOfAqueousCleaningSystem IS High and FinishIndustrialProcessWithWaterBasedProduct IS VeryLow THEN WaterBasedSubstitutes IS Low");
            IS.NewRule("Rule 17", "IF UseOfAqueousCleaningSystem IS High and FinishIndustrialProcessWithWaterBasedProduct IS Low THEN WaterBasedSubstitutes IS Middle");
            IS.NewRule("Rule 18", "IF UseOfAqueousCleaningSystem IS High and FinishIndustrialProcessWithWaterBasedProduct IS Medium THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 19", "IF UseOfAqueousCleaningSystem IS High and FinishIndustrialProcessWithWaterBasedProduct IS High THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 20", "IF UseOfAqueousCleaningSystem IS High and FinishIndustrialProcessWithWaterBasedProduct IS VeryHigh THEN WaterBasedSubstitutes IS VeryHigh");
            IS.NewRule("Rule 21", "IF UseOfAqueousCleaningSystem IS VeryHigh and FinishIndustrialProcessWithWaterBasedProduct IS VeryLow THEN WaterBasedSubstitutes IS Middle");
            IS.NewRule("Rule 22", "IF UseOfAqueousCleaningSystem IS VeryHigh and FinishIndustrialProcessWithWaterBasedProduct IS Low THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 23", "IF UseOfAqueousCleaningSystem IS VeryHigh and FinishIndustrialProcessWithWaterBasedProduct IS Medium THEN WaterBasedSubstitutes IS High");
            IS.NewRule("Rule 24", "IF UseOfAqueousCleaningSystem IS VeryHigh and FinishIndustrialProcessWithWaterBasedProduct IS High THEN WaterBasedSubstitutes IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfAqueousCleaningSystem IS VeryHigh and FinishIndustrialProcessWithWaterBasedProduct IS VeryHigh THEN WaterBasedSubstitutes IS VeryHigh");

            IS.SetInput("UseOfAqueousCleaningSystem", (float)useOfAqueousCleaningSystemValue);
            IS.SetInput("FinishIndustrialProcessWithWaterBasedProduct", (float)finishIndustrialProcessWithWaterBasedProductValue);

            double resultado = IS.Evaluate("WaterBasedSubstitutes");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfAqueousCleaningSystem", i == 0 ? (float)9.99 : 0);
                IS.SetInput("FinishIndustrialProcessWithWaterBasedProduct", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("WaterBasedSubstitutes");
            }
            double m = (IS.GetLinguisticVariable("WaterBasedSubstitutes").End - IS.GetLinguisticVariable("WaterBasedSubstitutes").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WaterBasedSubstitutes").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateInorganicSolutions(double substituteHexavalentChromiumForTrivalentValue, double replaceHeavyMetalReagentsWithNonHazardousOnesValue)
        {
            LinguisticVariable substituteHexavalentChromiumForTrivalent = new( "SubstituteHexavalentChromiumForTrivalent", 0, 10 );
            substituteHexavalentChromiumForTrivalent.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            substituteHexavalentChromiumForTrivalent.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            substituteHexavalentChromiumForTrivalent.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            substituteHexavalentChromiumForTrivalent.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            substituteHexavalentChromiumForTrivalent.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable replaceHeavyMetalReagentsWithNonHazardousOnes = new( "ReplaceHeavyMetalReagentsWithNonHazardousOnes", 0, 10 );
            replaceHeavyMetalReagentsWithNonHazardousOnes.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            replaceHeavyMetalReagentsWithNonHazardousOnes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            replaceHeavyMetalReagentsWithNonHazardousOnes.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            replaceHeavyMetalReagentsWithNonHazardousOnes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            replaceHeavyMetalReagentsWithNonHazardousOnes.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable inorganicSolutions = new( "InorganicSolutions", 0, 10 );
            inorganicSolutions.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( substituteHexavalentChromiumForTrivalent );
            fuzzyDB.AddVariable( replaceHeavyMetalReagentsWithNonHazardousOnes );
            fuzzyDB.AddVariable( inorganicSolutions );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SubstituteHexavalentChromiumForTrivalent IS VeryLow and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryLow THEN InorganicSolutions IS VeryLow");
            IS.NewRule("Rule 2", "IF SubstituteHexavalentChromiumForTrivalent IS VeryLow and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Low THEN InorganicSolutions IS VeryLow");
            IS.NewRule("Rule 3", "IF SubstituteHexavalentChromiumForTrivalent IS VeryLow and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Medium THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 4", "IF SubstituteHexavalentChromiumForTrivalent IS VeryLow and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS High THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 5", "IF SubstituteHexavalentChromiumForTrivalent IS VeryLow and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryHigh THEN InorganicSolutions IS Middle");
            IS.NewRule("Rule 6", "IF SubstituteHexavalentChromiumForTrivalent IS Low and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryLow THEN InorganicSolutions IS VeryLow");
            IS.NewRule("Rule 7", "IF SubstituteHexavalentChromiumForTrivalent IS Low and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Low THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 8", "IF SubstituteHexavalentChromiumForTrivalent IS Low and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Medium THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 9", "IF SubstituteHexavalentChromiumForTrivalent IS Low and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS High THEN InorganicSolutions IS Middle");
            IS.NewRule("Rule 10", "IF SubstituteHexavalentChromiumForTrivalent IS Low and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryHigh THEN InorganicSolutions IS High");
            IS.NewRule("Rule 11", "IF SubstituteHexavalentChromiumForTrivalent IS Middle and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryLow THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 12", "IF SubstituteHexavalentChromiumForTrivalent IS Middle and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Low THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 13", "IF SubstituteHexavalentChromiumForTrivalent IS Middle and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Medium THEN InorganicSolutions IS Middle");
            IS.NewRule("Rule 14", "IF SubstituteHexavalentChromiumForTrivalent IS Middle and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS High THEN InorganicSolutions IS High");
            IS.NewRule("Rule 15", "IF SubstituteHexavalentChromiumForTrivalent IS Middle and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryHigh THEN InorganicSolutions IS High");
            IS.NewRule("Rule 16", "IF SubstituteHexavalentChromiumForTrivalent IS High and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryLow THEN InorganicSolutions IS Low");
            IS.NewRule("Rule 17", "IF SubstituteHexavalentChromiumForTrivalent IS High and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Low THEN InorganicSolutions IS Middle");
            IS.NewRule("Rule 18", "IF SubstituteHexavalentChromiumForTrivalent IS High and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Medium THEN InorganicSolutions IS High");
            IS.NewRule("Rule 19", "IF SubstituteHexavalentChromiumForTrivalent IS High and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS High THEN InorganicSolutions IS High");
            IS.NewRule("Rule 20", "IF SubstituteHexavalentChromiumForTrivalent IS High and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryHigh THEN InorganicSolutions IS VeryHigh");
            IS.NewRule("Rule 21", "IF SubstituteHexavalentChromiumForTrivalent IS VeryHigh and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryLow THEN InorganicSolutions IS Middle");
            IS.NewRule("Rule 22", "IF SubstituteHexavalentChromiumForTrivalent IS VeryHigh and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Low THEN InorganicSolutions IS High");
            IS.NewRule("Rule 23", "IF SubstituteHexavalentChromiumForTrivalent IS VeryHigh and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS Medium THEN InorganicSolutions IS High");
            IS.NewRule("Rule 24", "IF SubstituteHexavalentChromiumForTrivalent IS VeryHigh and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS High THEN InorganicSolutions IS VeryHigh");
            IS.NewRule("Rule 25", "IF SubstituteHexavalentChromiumForTrivalent IS VeryHigh and ReplaceHeavyMetalReagentsWithNonHazardousOnes IS VeryHigh THEN InorganicSolutions IS VeryHigh");

            IS.SetInput("SubstituteHexavalentChromiumForTrivalent", (float)substituteHexavalentChromiumForTrivalentValue);
            IS.SetInput("ReplaceHeavyMetalReagentsWithNonHazardousOnes", (float)replaceHeavyMetalReagentsWithNonHazardousOnesValue);

            double resultado = IS.Evaluate("InorganicSolutions");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SubstituteHexavalentChromiumForTrivalent", i == 0 ? (float)9.99 : 0);
                IS.SetInput("ReplaceHeavyMetalReagentsWithNonHazardousOnes", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("InorganicSolutions");
            }
            double m = (IS.GetLinguisticVariable("InorganicSolutions").End - IS.GetLinguisticVariable("InorganicSolutions").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("InorganicSolutions").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateFlocculation(double useOfFlocculantsToReduceSludgeValue, double useOfPrecipitatingAgentsInWasteWaterTreatmentValue)
        {
            LinguisticVariable useOfFlocculantsToReduceSludge = new( "UseOfFlocculantsToReduceSludge", 0, 10 );
            useOfFlocculantsToReduceSludge.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfFlocculantsToReduceSludge.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfFlocculantsToReduceSludge.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            useOfFlocculantsToReduceSludge.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfFlocculantsToReduceSludge.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable useOfPrecipitatingAgentsInWasteWaterTreatment = new( "UseOfPrecipitatingAgentsInWasteWaterTreatment", 0, 10 );
            useOfPrecipitatingAgentsInWasteWaterTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfPrecipitatingAgentsInWasteWaterTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfPrecipitatingAgentsInWasteWaterTreatment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            useOfPrecipitatingAgentsInWasteWaterTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfPrecipitatingAgentsInWasteWaterTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable flocculation = new( "Flocculation", 0, 10 );
            flocculation.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            flocculation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            flocculation.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            flocculation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            flocculation.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfFlocculantsToReduceSludge );
            fuzzyDB.AddVariable( useOfPrecipitatingAgentsInWasteWaterTreatment );
            fuzzyDB.AddVariable( flocculation );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfFlocculantsToReduceSludge IS VeryLow and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryLow THEN Flocculation IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfFlocculantsToReduceSludge IS VeryLow and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Low THEN Flocculation IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfFlocculantsToReduceSludge IS VeryLow and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Medium THEN Flocculation IS Low");
            IS.NewRule("Rule 4", "IF UseOfFlocculantsToReduceSludge IS VeryLow and UseOfPrecipitatingAgentsInWasteWaterTreatment IS High THEN Flocculation IS Low");
            IS.NewRule("Rule 5", "IF UseOfFlocculantsToReduceSludge IS VeryLow and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryHigh THEN Flocculation IS Middle");
            IS.NewRule("Rule 6", "IF UseOfFlocculantsToReduceSludge IS Low and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryLow THEN Flocculation IS VeryLow");
            IS.NewRule("Rule 7", "IF UseOfFlocculantsToReduceSludge IS Low and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Low THEN Flocculation IS Low");
            IS.NewRule("Rule 8", "IF UseOfFlocculantsToReduceSludge IS Low and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Medium THEN Flocculation IS Low");
            IS.NewRule("Rule 9", "IF UseOfFlocculantsToReduceSludge IS Low and UseOfPrecipitatingAgentsInWasteWaterTreatment IS High THEN Flocculation IS Middle");
            IS.NewRule("Rule 10", "IF UseOfFlocculantsToReduceSludge IS Low and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryHigh THEN Flocculation IS High");
            IS.NewRule("Rule 11", "IF UseOfFlocculantsToReduceSludge IS Middle and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryLow THEN Flocculation IS Low");
            IS.NewRule("Rule 12", "IF UseOfFlocculantsToReduceSludge IS Middle and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Low THEN Flocculation IS Low");
            IS.NewRule("Rule 13", "IF UseOfFlocculantsToReduceSludge IS Middle and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Medium THEN Flocculation IS Middle");
            IS.NewRule("Rule 14", "IF UseOfFlocculantsToReduceSludge IS Middle and UseOfPrecipitatingAgentsInWasteWaterTreatment IS High THEN Flocculation IS High");
            IS.NewRule("Rule 15", "IF UseOfFlocculantsToReduceSludge IS Middle and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryHigh THEN Flocculation IS High");
            IS.NewRule("Rule 16", "IF UseOfFlocculantsToReduceSludge IS High and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryLow THEN Flocculation IS Low");
            IS.NewRule("Rule 17", "IF UseOfFlocculantsToReduceSludge IS High and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Low THEN Flocculation IS Middle");
            IS.NewRule("Rule 18", "IF UseOfFlocculantsToReduceSludge IS High and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Medium THEN Flocculation IS High");
            IS.NewRule("Rule 19", "IF UseOfFlocculantsToReduceSludge IS High and UseOfPrecipitatingAgentsInWasteWaterTreatment IS High THEN Flocculation IS High");
            IS.NewRule("Rule 20", "IF UseOfFlocculantsToReduceSludge IS High and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryHigh THEN Flocculation IS VeryHigh");
            IS.NewRule("Rule 21", "IF UseOfFlocculantsToReduceSludge IS VeryHigh and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryLow THEN Flocculation IS Middle");
            IS.NewRule("Rule 22", "IF UseOfFlocculantsToReduceSludge IS VeryHigh and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Low THEN Flocculation IS High");
            IS.NewRule("Rule 23", "IF UseOfFlocculantsToReduceSludge IS VeryHigh and UseOfPrecipitatingAgentsInWasteWaterTreatment IS Medium THEN Flocculation IS High");
            IS.NewRule("Rule 24", "IF UseOfFlocculantsToReduceSludge IS VeryHigh and UseOfPrecipitatingAgentsInWasteWaterTreatment IS High THEN Flocculation IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfFlocculantsToReduceSludge IS VeryHigh and UseOfPrecipitatingAgentsInWasteWaterTreatment IS VeryHigh THEN Flocculation IS VeryHigh");

            IS.SetInput("UseOfFlocculantsToReduceSludge", (float)useOfFlocculantsToReduceSludgeValue);
            IS.SetInput("UseOfPrecipitatingAgentsInWasteWaterTreatment", (float)useOfPrecipitatingAgentsInWasteWaterTreatmentValue);

            double resultado = IS.Evaluate("Flocculation");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfFlocculantsToReduceSludge", i == 0 ? (float)9.99 : 0);
                IS.SetInput("UseOfPrecipitatingAgentsInWasteWaterTreatment", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Flocculation");
            }
            double m = (IS.GetLinguisticVariable("Flocculation").End - IS.GetLinguisticVariable("Flocculation").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Flocculation").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSludgeRemoval(double useOfGreenhouseFilterToReduceSludgeValue, double removalOfSludgeFromEquipmentTanksValue)
        {
            LinguisticVariable useOfGreenhouseFilterToReduceSludge = new( "UseOfGreenhouseFilterToReduceSludge", 0, 10 );
            useOfGreenhouseFilterToReduceSludge.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            useOfGreenhouseFilterToReduceSludge.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            useOfGreenhouseFilterToReduceSludge.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            useOfGreenhouseFilterToReduceSludge.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            useOfGreenhouseFilterToReduceSludge.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable removalOfSludgeFromEquipmentTanks = new( "RemovalOfSludgeFromEquipmentTanks", 0, 10 );
            removalOfSludgeFromEquipmentTanks.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            removalOfSludgeFromEquipmentTanks.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            removalOfSludgeFromEquipmentTanks.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            removalOfSludgeFromEquipmentTanks.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            removalOfSludgeFromEquipmentTanks.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable sludgeRemoval = new( "SludgeRemoval", 0, 10 );
            sludgeRemoval.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfGreenhouseFilterToReduceSludge );
            fuzzyDB.AddVariable( removalOfSludgeFromEquipmentTanks );
            fuzzyDB.AddVariable( sludgeRemoval );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfGreenhouseFilterToReduceSludge IS VeryLow and RemovalOfSludgeFromEquipmentTanks IS VeryLow THEN SludgeRemoval IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfGreenhouseFilterToReduceSludge IS VeryLow and RemovalOfSludgeFromEquipmentTanks IS Low THEN SludgeRemoval IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfGreenhouseFilterToReduceSludge IS VeryLow and RemovalOfSludgeFromEquipmentTanks IS Medium THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 4", "IF UseOfGreenhouseFilterToReduceSludge IS VeryLow and RemovalOfSludgeFromEquipmentTanks IS High THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 5", "IF UseOfGreenhouseFilterToReduceSludge IS VeryLow and RemovalOfSludgeFromEquipmentTanks IS VeryHigh THEN SludgeRemoval IS Middle");
            IS.NewRule("Rule 6", "IF UseOfGreenhouseFilterToReduceSludge IS Low and RemovalOfSludgeFromEquipmentTanks IS VeryLow THEN SludgeRemoval IS VeryLow");
            IS.NewRule("Rule 7", "IF UseOfGreenhouseFilterToReduceSludge IS Low and RemovalOfSludgeFromEquipmentTanks IS Low THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 8", "IF UseOfGreenhouseFilterToReduceSludge IS Low and RemovalOfSludgeFromEquipmentTanks IS Medium THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 9", "IF UseOfGreenhouseFilterToReduceSludge IS Low and RemovalOfSludgeFromEquipmentTanks IS High THEN SludgeRemoval IS Middle");
            IS.NewRule("Rule 10", "IF UseOfGreenhouseFilterToReduceSludge IS Low and RemovalOfSludgeFromEquipmentTanks IS VeryHigh THEN SludgeRemoval IS High");
            IS.NewRule("Rule 11", "IF UseOfGreenhouseFilterToReduceSludge IS Middle and RemovalOfSludgeFromEquipmentTanks IS VeryLow THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 12", "IF UseOfGreenhouseFilterToReduceSludge IS Middle and RemovalOfSludgeFromEquipmentTanks IS Low THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 13", "IF UseOfGreenhouseFilterToReduceSludge IS Middle and RemovalOfSludgeFromEquipmentTanks IS Medium THEN SludgeRemoval IS Middle");
            IS.NewRule("Rule 14", "IF UseOfGreenhouseFilterToReduceSludge IS Middle and RemovalOfSludgeFromEquipmentTanks IS High THEN SludgeRemoval IS High");
            IS.NewRule("Rule 15", "IF UseOfGreenhouseFilterToReduceSludge IS Middle and RemovalOfSludgeFromEquipmentTanks IS VeryHigh THEN SludgeRemoval IS High");
            IS.NewRule("Rule 16", "IF UseOfGreenhouseFilterToReduceSludge IS High and RemovalOfSludgeFromEquipmentTanks IS VeryLow THEN SludgeRemoval IS Low");
            IS.NewRule("Rule 17", "IF UseOfGreenhouseFilterToReduceSludge IS High and RemovalOfSludgeFromEquipmentTanks IS Low THEN SludgeRemoval IS Middle");
            IS.NewRule("Rule 18", "IF UseOfGreenhouseFilterToReduceSludge IS High and RemovalOfSludgeFromEquipmentTanks IS Medium THEN SludgeRemoval IS High");
            IS.NewRule("Rule 19", "IF UseOfGreenhouseFilterToReduceSludge IS High and RemovalOfSludgeFromEquipmentTanks IS High THEN SludgeRemoval IS High");
            IS.NewRule("Rule 20", "IF UseOfGreenhouseFilterToReduceSludge IS High and RemovalOfSludgeFromEquipmentTanks IS VeryHigh THEN SludgeRemoval IS VeryHigh");
            IS.NewRule("Rule 21", "IF UseOfGreenhouseFilterToReduceSludge IS VeryHigh and RemovalOfSludgeFromEquipmentTanks IS VeryLow THEN SludgeRemoval IS Middle");
            IS.NewRule("Rule 22", "IF UseOfGreenhouseFilterToReduceSludge IS VeryHigh and RemovalOfSludgeFromEquipmentTanks IS Low THEN SludgeRemoval IS High");
            IS.NewRule("Rule 23", "IF UseOfGreenhouseFilterToReduceSludge IS VeryHigh and RemovalOfSludgeFromEquipmentTanks IS Medium THEN SludgeRemoval IS High");
            IS.NewRule("Rule 24", "IF UseOfGreenhouseFilterToReduceSludge IS VeryHigh and RemovalOfSludgeFromEquipmentTanks IS High THEN SludgeRemoval IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfGreenhouseFilterToReduceSludge IS VeryHigh and RemovalOfSludgeFromEquipmentTanks IS VeryHigh THEN SludgeRemoval IS VeryHigh");

            IS.SetInput("UseOfGreenhouseFilterToReduceSludge", (float)useOfGreenhouseFilterToReduceSludgeValue);
            IS.SetInput("RemovalOfSludgeFromEquipmentTanks", (float)removalOfSludgeFromEquipmentTanksValue);

            double resultado = IS.Evaluate("SludgeRemoval");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfGreenhouseFilterToReduceSludge", i == 0 ? (float)9.99 : 0);
                IS.SetInput("RemovalOfSludgeFromEquipmentTanks", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("SludgeRemoval");
            }
            double m = (IS.GetLinguisticVariable("SludgeRemoval").End - IS.GetLinguisticVariable("SludgeRemoval").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SludgeRemoval").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateHeatGeneration(double installationOfIncineratorForSolidWasteValue, double burningOfHeatWoodByProductsValue, double wasteOilBurningValue)
        {
            LinguisticVariable installationOfIncineratorForSolidWaste = new( "InstallationOfIncineratorForSolidWaste", 0, 10 );
            installationOfIncineratorForSolidWaste.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            installationOfIncineratorForSolidWaste.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            installationOfIncineratorForSolidWaste.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable burningOfHeatWoodByProducts = new( "BurningOfHeatWoodByProducts", 0, 10 );
            burningOfHeatWoodByProducts.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            burningOfHeatWoodByProducts.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            burningOfHeatWoodByProducts.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable wasteOilBurning = new( "WasteOilBurning", 0, 10 );
            wasteOilBurning.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            wasteOilBurning.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            wasteOilBurning.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable heatGeneration = new( "HeatGeneration", 0, 10 );
            heatGeneration.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            heatGeneration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            heatGeneration.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            heatGeneration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            heatGeneration.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( installationOfIncineratorForSolidWaste );
            fuzzyDB.AddVariable( burningOfHeatWoodByProducts );
            fuzzyDB.AddVariable( wasteOilBurning );
            fuzzyDB.AddVariable( heatGeneration );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Low THEN HeatGeneration IS VeryLow");
            IS.NewRule("Rule 2", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Medium THEN HeatGeneration IS VeryLow");
            IS.NewRule("Rule 3", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS High THEN HeatGeneration IS Low");
            IS.NewRule("Rule 4", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Low THEN HeatGeneration IS VeryLow");
            IS.NewRule("Rule 5", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Medium THEN HeatGeneration IS Low");
            IS.NewRule("Rule 6", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS High THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 7", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Low THEN HeatGeneration IS Low");
            IS.NewRule("Rule 8", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Medium THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 9", "IF InstallationOfIncineratorForSolidWaste IS Low and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS High THEN HeatGeneration IS High");
            IS.NewRule("Rule 10", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Low THEN HeatGeneration IS VeryLow");
            IS.NewRule("Rule 11", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Medium THEN HeatGeneration IS Low");
            IS.NewRule("Rule 12", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS High THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 13", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Low THEN HeatGeneration IS Low");
            IS.NewRule("Rule 14", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Medium THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 15", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS High THEN HeatGeneration IS High");
            IS.NewRule("Rule 16", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Low THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 17", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Medium THEN HeatGeneration IS High");
            IS.NewRule("Rule 18", "IF InstallationOfIncineratorForSolidWaste IS Medium and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS High THEN HeatGeneration IS VeryHigh");
            IS.NewRule("Rule 19", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Low THEN HeatGeneration IS Low");
            IS.NewRule("Rule 20", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS Medium THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 21", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Low and WasteOilBurning IS High THEN HeatGeneration IS High");
            IS.NewRule("Rule 22", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Low THEN HeatGeneration IS Middle");
            IS.NewRule("Rule 23", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS Medium THEN HeatGeneration IS High");
            IS.NewRule("Rule 24", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS Medium and WasteOilBurning IS High THEN HeatGeneration IS VeryHigh");
            IS.NewRule("Rule 25", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Low THEN HeatGeneration IS High");
            IS.NewRule("Rule 26", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS Medium THEN HeatGeneration IS VeryHigh");
            IS.NewRule("Rule 27", "IF InstallationOfIncineratorForSolidWaste IS High and BurningOfHeatWoodByProducts IS High and WasteOilBurning IS High THEN HeatGeneration IS VeryHigh");

            IS.SetInput("InstallationOfIncineratorForSolidWaste", (float)installationOfIncineratorForSolidWasteValue);
            IS.SetInput("BurningOfHeatWoodByProducts", (float)burningOfHeatWoodByProductsValue);
            IS.SetInput("WasteOilBurning", (float)wasteOilBurningValue);

            double resultado = IS.Evaluate("HeatGeneration");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("InstallationOfIncineratorForSolidWaste", i == 0 ? 0 : (float)9.99);
                IS.SetInput("BurningOfHeatWoodByProducts", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WasteOilBurning", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("HeatGeneration");
            }
            double m = (IS.GetLinguisticVariable("HeatGeneration").End - IS.GetLinguisticVariable("HeatGeneration").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("HeatGeneration").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateOperation(double directWasteGasesToTheBoilerValue, double burnWastePaperToGenerateHeatValue)
        {
            LinguisticVariable directWasteGasesToTheBoiler = new( "DirectWasteGasesToTheBoiler", 0, 10 );
            directWasteGasesToTheBoiler.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            directWasteGasesToTheBoiler.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            directWasteGasesToTheBoiler.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            directWasteGasesToTheBoiler.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            directWasteGasesToTheBoiler.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable burnWastePaperToGenerateHeat = new( "BurnWastePaperToGenerateHeat", 0, 10 );
            burnWastePaperToGenerateHeat.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            burnWastePaperToGenerateHeat.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            burnWastePaperToGenerateHeat.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            burnWastePaperToGenerateHeat.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            burnWastePaperToGenerateHeat.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable operation = new( "Operation", 0, 10 );
            operation.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            operation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            operation.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            operation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            operation.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( directWasteGasesToTheBoiler );
            fuzzyDB.AddVariable( burnWastePaperToGenerateHeat );
            fuzzyDB.AddVariable( operation );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DirectWasteGasesToTheBoiler IS VeryLow and BurnWastePaperToGenerateHeat IS VeryLow THEN Operation IS VeryLow");
            IS.NewRule("Rule 2", "IF DirectWasteGasesToTheBoiler IS VeryLow and BurnWastePaperToGenerateHeat IS Low THEN Operation IS VeryLow");
            IS.NewRule("Rule 3", "IF DirectWasteGasesToTheBoiler IS VeryLow and BurnWastePaperToGenerateHeat IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 4", "IF DirectWasteGasesToTheBoiler IS VeryLow and BurnWastePaperToGenerateHeat IS High THEN Operation IS Low");
            IS.NewRule("Rule 5", "IF DirectWasteGasesToTheBoiler IS VeryLow and BurnWastePaperToGenerateHeat IS VeryHigh THEN Operation IS Middle");
            IS.NewRule("Rule 6", "IF DirectWasteGasesToTheBoiler IS Low and BurnWastePaperToGenerateHeat IS VeryLow THEN Operation IS VeryLow");
            IS.NewRule("Rule 7", "IF DirectWasteGasesToTheBoiler IS Low and BurnWastePaperToGenerateHeat IS Low THEN Operation IS Low");
            IS.NewRule("Rule 8", "IF DirectWasteGasesToTheBoiler IS Low and BurnWastePaperToGenerateHeat IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 9", "IF DirectWasteGasesToTheBoiler IS Low and BurnWastePaperToGenerateHeat IS High THEN Operation IS Middle");
            IS.NewRule("Rule 10", "IF DirectWasteGasesToTheBoiler IS Low and BurnWastePaperToGenerateHeat IS VeryHigh THEN Operation IS High");
            IS.NewRule("Rule 11", "IF DirectWasteGasesToTheBoiler IS Middle and BurnWastePaperToGenerateHeat IS VeryLow THEN Operation IS Low");
            IS.NewRule("Rule 12", "IF DirectWasteGasesToTheBoiler IS Middle and BurnWastePaperToGenerateHeat IS Low THEN Operation IS Low");
            IS.NewRule("Rule 13", "IF DirectWasteGasesToTheBoiler IS Middle and BurnWastePaperToGenerateHeat IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 14", "IF DirectWasteGasesToTheBoiler IS Middle and BurnWastePaperToGenerateHeat IS High THEN Operation IS High");
            IS.NewRule("Rule 15", "IF DirectWasteGasesToTheBoiler IS Middle and BurnWastePaperToGenerateHeat IS VeryHigh THEN Operation IS High");
            IS.NewRule("Rule 16", "IF DirectWasteGasesToTheBoiler IS High and BurnWastePaperToGenerateHeat IS VeryLow THEN Operation IS Low");
            IS.NewRule("Rule 17", "IF DirectWasteGasesToTheBoiler IS High and BurnWastePaperToGenerateHeat IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 18", "IF DirectWasteGasesToTheBoiler IS High and BurnWastePaperToGenerateHeat IS Medium THEN Operation IS High");
            IS.NewRule("Rule 19", "IF DirectWasteGasesToTheBoiler IS High and BurnWastePaperToGenerateHeat IS High THEN Operation IS High");
            IS.NewRule("Rule 20", "IF DirectWasteGasesToTheBoiler IS High and BurnWastePaperToGenerateHeat IS VeryHigh THEN Operation IS VeryHigh");
            IS.NewRule("Rule 21", "IF DirectWasteGasesToTheBoiler IS VeryHigh and BurnWastePaperToGenerateHeat IS VeryLow THEN Operation IS Middle");
            IS.NewRule("Rule 22", "IF DirectWasteGasesToTheBoiler IS VeryHigh and BurnWastePaperToGenerateHeat IS Low THEN Operation IS High");
            IS.NewRule("Rule 23", "IF DirectWasteGasesToTheBoiler IS VeryHigh and BurnWastePaperToGenerateHeat IS Medium THEN Operation IS High");
            IS.NewRule("Rule 24", "IF DirectWasteGasesToTheBoiler IS VeryHigh and BurnWastePaperToGenerateHeat IS High THEN Operation IS VeryHigh");
            IS.NewRule("Rule 25", "IF DirectWasteGasesToTheBoiler IS VeryHigh and BurnWastePaperToGenerateHeat IS VeryHigh THEN Operation IS VeryHigh");

            IS.SetInput("DirectWasteGasesToTheBoiler", (float)directWasteGasesToTheBoilerValue);
            IS.SetInput("BurnWastePaperToGenerateHeat", (float)burnWastePaperToGenerateHeatValue);

            double resultado = IS.Evaluate("Operation");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DirectWasteGasesToTheBoiler", i == 0 ? (float)9.99 : 0);
                IS.SetInput("BurnWastePaperToGenerateHeat", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Operation");
            }
            double m = (IS.GetLinguisticVariable("Operation").End - IS.GetLinguisticVariable("Operation").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Operation").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateWasteDisposalLevel2(double cheapWasteRemovalValue, double installationOfDisposalEquipmentValue)
        {
            LinguisticVariable cheapWasteRemoval = new( "CheapWasteRemoval", 0, 10 );
            cheapWasteRemoval.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            cheapWasteRemoval.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            cheapWasteRemoval.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            cheapWasteRemoval.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            cheapWasteRemoval.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable installationOfDisposalEquipment = new( "InstallationOfDisposalEquipment", 0, 10 );
            installationOfDisposalEquipment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            installationOfDisposalEquipment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            installationOfDisposalEquipment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            installationOfDisposalEquipment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            installationOfDisposalEquipment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable wasteDisposalLevel2 = new( "WasteDisposalLevel2", 0, 10 );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( cheapWasteRemoval );
            fuzzyDB.AddVariable( installationOfDisposalEquipment );
            fuzzyDB.AddVariable( wasteDisposalLevel2 );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF CheapWasteRemoval IS VeryLow and InstallationOfDisposalEquipment IS VeryLow THEN WasteDisposalLevel2 IS VeryLow");
            IS.NewRule("Rule 2", "IF CheapWasteRemoval IS VeryLow and InstallationOfDisposalEquipment IS Low THEN WasteDisposalLevel2 IS VeryLow");
            IS.NewRule("Rule 3", "IF CheapWasteRemoval IS VeryLow and InstallationOfDisposalEquipment IS Medium THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 4", "IF CheapWasteRemoval IS VeryLow and InstallationOfDisposalEquipment IS High THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 5", "IF CheapWasteRemoval IS VeryLow and InstallationOfDisposalEquipment IS VeryHigh THEN WasteDisposalLevel2 IS Middle");
            IS.NewRule("Rule 6", "IF CheapWasteRemoval IS Low and InstallationOfDisposalEquipment IS VeryLow THEN WasteDisposalLevel2 IS VeryLow");
            IS.NewRule("Rule 7", "IF CheapWasteRemoval IS Low and InstallationOfDisposalEquipment IS Low THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 8", "IF CheapWasteRemoval IS Low and InstallationOfDisposalEquipment IS Medium THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 9", "IF CheapWasteRemoval IS Low and InstallationOfDisposalEquipment IS High THEN WasteDisposalLevel2 IS Middle");
            IS.NewRule("Rule 10", "IF CheapWasteRemoval IS Low and InstallationOfDisposalEquipment IS VeryHigh THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 11", "IF CheapWasteRemoval IS Middle and InstallationOfDisposalEquipment IS VeryLow THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 12", "IF CheapWasteRemoval IS Middle and InstallationOfDisposalEquipment IS Low THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 13", "IF CheapWasteRemoval IS Middle and InstallationOfDisposalEquipment IS Medium THEN WasteDisposalLevel2 IS Middle");
            IS.NewRule("Rule 14", "IF CheapWasteRemoval IS Middle and InstallationOfDisposalEquipment IS High THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 15", "IF CheapWasteRemoval IS Middle and InstallationOfDisposalEquipment IS VeryHigh THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 16", "IF CheapWasteRemoval IS High and InstallationOfDisposalEquipment IS VeryLow THEN WasteDisposalLevel2 IS Low");
            IS.NewRule("Rule 17", "IF CheapWasteRemoval IS High and InstallationOfDisposalEquipment IS Low THEN WasteDisposalLevel2 IS Middle");
            IS.NewRule("Rule 18", "IF CheapWasteRemoval IS High and InstallationOfDisposalEquipment IS Medium THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 19", "IF CheapWasteRemoval IS High and InstallationOfDisposalEquipment IS High THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 20", "IF CheapWasteRemoval IS High and InstallationOfDisposalEquipment IS VeryHigh THEN WasteDisposalLevel2 IS VeryHigh");
            IS.NewRule("Rule 21", "IF CheapWasteRemoval IS VeryHigh and InstallationOfDisposalEquipment IS VeryLow THEN WasteDisposalLevel2 IS Middle");
            IS.NewRule("Rule 22", "IF CheapWasteRemoval IS VeryHigh and InstallationOfDisposalEquipment IS Low THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 23", "IF CheapWasteRemoval IS VeryHigh and InstallationOfDisposalEquipment IS Medium THEN WasteDisposalLevel2 IS High");
            IS.NewRule("Rule 24", "IF CheapWasteRemoval IS VeryHigh and InstallationOfDisposalEquipment IS High THEN WasteDisposalLevel2 IS VeryHigh");
            IS.NewRule("Rule 25", "IF CheapWasteRemoval IS VeryHigh and InstallationOfDisposalEquipment IS VeryHigh THEN WasteDisposalLevel2 IS VeryHigh");

            IS.SetInput("CheapWasteRemoval", (float)cheapWasteRemovalValue);
            IS.SetInput("InstallationOfDisposalEquipment", (float)installationOfDisposalEquipmentValue);

            double resultado = IS.Evaluate("WasteDisposalLevel2");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("CheapWasteRemoval", i == 0 ? (float)9.99 : 0);
                IS.SetInput("InstallationOfDisposalEquipment", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("WasteDisposalLevel2");
            }
            double m = (IS.GetLinguisticVariable("WasteDisposalLevel2").End - IS.GetLinguisticVariable("WasteDisposalLevel2").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WasteDisposalLevel2").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateMiscellaneous(double reuseValue, double solutionsValue)
        {
            LinguisticVariable reuse = new( "Reuse", 0, 10 );
            reuse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reuse.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reuse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable solutions = new( "Solutions", 0, 10 );
            solutions.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solutions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solutions.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            solutions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solutions.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable miscellaneous = new( "Miscellaneous", 0, 10 );
            miscellaneous.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            miscellaneous.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            miscellaneous.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            miscellaneous.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            miscellaneous.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reuse );
            fuzzyDB.AddVariable( solutions );
            fuzzyDB.AddVariable( miscellaneous );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Reuse IS VeryLow and Solutions IS VeryLow THEN Miscellaneous IS VeryLow");
            IS.NewRule("Rule 2", "IF Reuse IS VeryLow and Solutions IS Low THEN Miscellaneous IS VeryLow");
            IS.NewRule("Rule 3", "IF Reuse IS VeryLow and Solutions IS Medium THEN Miscellaneous IS Low");
            IS.NewRule("Rule 4", "IF Reuse IS VeryLow and Solutions IS High THEN Miscellaneous IS Low");
            IS.NewRule("Rule 5", "IF Reuse IS VeryLow and Solutions IS VeryHigh THEN Miscellaneous IS Middle");
            IS.NewRule("Rule 6", "IF Reuse IS Low and Solutions IS VeryLow THEN Miscellaneous IS VeryLow");
            IS.NewRule("Rule 7", "IF Reuse IS Low and Solutions IS Low THEN Miscellaneous IS Low");
            IS.NewRule("Rule 8", "IF Reuse IS Low and Solutions IS Medium THEN Miscellaneous IS Low");
            IS.NewRule("Rule 9", "IF Reuse IS Low and Solutions IS High THEN Miscellaneous IS Middle");
            IS.NewRule("Rule 10", "IF Reuse IS Low and Solutions IS VeryHigh THEN Miscellaneous IS High");
            IS.NewRule("Rule 11", "IF Reuse IS Middle and Solutions IS VeryLow THEN Miscellaneous IS Low");
            IS.NewRule("Rule 12", "IF Reuse IS Middle and Solutions IS Low THEN Miscellaneous IS Low");
            IS.NewRule("Rule 13", "IF Reuse IS Middle and Solutions IS Medium THEN Miscellaneous IS Middle");
            IS.NewRule("Rule 14", "IF Reuse IS Middle and Solutions IS High THEN Miscellaneous IS High");
            IS.NewRule("Rule 15", "IF Reuse IS Middle and Solutions IS VeryHigh THEN Miscellaneous IS High");
            IS.NewRule("Rule 16", "IF Reuse IS High and Solutions IS VeryLow THEN Miscellaneous IS Low");
            IS.NewRule("Rule 17", "IF Reuse IS High and Solutions IS Low THEN Miscellaneous IS Middle");
            IS.NewRule("Rule 18", "IF Reuse IS High and Solutions IS Medium THEN Miscellaneous IS High");
            IS.NewRule("Rule 19", "IF Reuse IS High and Solutions IS High THEN Miscellaneous IS High");
            IS.NewRule("Rule 20", "IF Reuse IS High and Solutions IS VeryHigh THEN Miscellaneous IS VeryHigh");
            IS.NewRule("Rule 21", "IF Reuse IS VeryHigh and Solutions IS VeryLow THEN Miscellaneous IS Middle");
            IS.NewRule("Rule 22", "IF Reuse IS VeryHigh and Solutions IS Low THEN Miscellaneous IS High");
            IS.NewRule("Rule 23", "IF Reuse IS VeryHigh and Solutions IS Medium THEN Miscellaneous IS High");
            IS.NewRule("Rule 24", "IF Reuse IS VeryHigh and Solutions IS High THEN Miscellaneous IS VeryHigh");
            IS.NewRule("Rule 25", "IF Reuse IS VeryHigh and Solutions IS VeryHigh THEN Miscellaneous IS VeryHigh");

            IS.SetInput("Reuse", (float)reuseValue);
            IS.SetInput("Solutions", (float)solutionsValue);

            double resultado = IS.Evaluate("Miscellaneous");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Reuse", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Solutions", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Miscellaneous");
            }
            double m = (IS.GetLinguisticVariable("Miscellaneous").End - IS.GetLinguisticVariable("Miscellaneous").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Miscellaneous").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateMetals(double saleValue, double recoveryValue)
        {
            LinguisticVariable sale = new( "Sale", 0, 10 );
            sale.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sale.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sale.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sale.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sale.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable recovery = new( "Recovery", 0, 10 );
            recovery.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recovery.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recovery.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            recovery.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recovery.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable metals = new( "Metals", 0, 10 );
            metals.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            metals.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            metals.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            metals.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            metals.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( sale );
            fuzzyDB.AddVariable( recovery );
            fuzzyDB.AddVariable( metals );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Sale IS VeryLow and Recovery IS VeryLow THEN Metals IS VeryLow");
            IS.NewRule("Rule 2", "IF Sale IS VeryLow and Recovery IS Low THEN Metals IS VeryLow");
            IS.NewRule("Rule 3", "IF Sale IS VeryLow and Recovery IS Medium THEN Metals IS Low");
            IS.NewRule("Rule 4", "IF Sale IS VeryLow and Recovery IS High THEN Metals IS Low");
            IS.NewRule("Rule 5", "IF Sale IS VeryLow and Recovery IS VeryHigh THEN Metals IS Middle");
            IS.NewRule("Rule 6", "IF Sale IS Low and Recovery IS VeryLow THEN Metals IS VeryLow");
            IS.NewRule("Rule 7", "IF Sale IS Low and Recovery IS Low THEN Metals IS Low");
            IS.NewRule("Rule 8", "IF Sale IS Low and Recovery IS Medium THEN Metals IS Low");
            IS.NewRule("Rule 9", "IF Sale IS Low and Recovery IS High THEN Metals IS Middle");
            IS.NewRule("Rule 10", "IF Sale IS Low and Recovery IS VeryHigh THEN Metals IS High");
            IS.NewRule("Rule 11", "IF Sale IS Middle and Recovery IS VeryLow THEN Metals IS Low");
            IS.NewRule("Rule 12", "IF Sale IS Middle and Recovery IS Low THEN Metals IS Low");
            IS.NewRule("Rule 13", "IF Sale IS Middle and Recovery IS Medium THEN Metals IS Middle");
            IS.NewRule("Rule 14", "IF Sale IS Middle and Recovery IS High THEN Metals IS High");
            IS.NewRule("Rule 15", "IF Sale IS Middle and Recovery IS VeryHigh THEN Metals IS High");
            IS.NewRule("Rule 16", "IF Sale IS High and Recovery IS VeryLow THEN Metals IS Low");
            IS.NewRule("Rule 17", "IF Sale IS High and Recovery IS Low THEN Metals IS Middle");
            IS.NewRule("Rule 18", "IF Sale IS High and Recovery IS Medium THEN Metals IS High");
            IS.NewRule("Rule 19", "IF Sale IS High and Recovery IS High THEN Metals IS High");
            IS.NewRule("Rule 20", "IF Sale IS High and Recovery IS VeryHigh THEN Metals IS VeryHigh");
            IS.NewRule("Rule 21", "IF Sale IS VeryHigh and Recovery IS VeryLow THEN Metals IS Middle");
            IS.NewRule("Rule 22", "IF Sale IS VeryHigh and Recovery IS Low THEN Metals IS High");
            IS.NewRule("Rule 23", "IF Sale IS VeryHigh and Recovery IS Medium THEN Metals IS High");
            IS.NewRule("Rule 24", "IF Sale IS VeryHigh and Recovery IS High THEN Metals IS VeryHigh");
            IS.NewRule("Rule 25", "IF Sale IS VeryHigh and Recovery IS VeryHigh THEN Metals IS VeryHigh");

            IS.SetInput("Sale", (float)saleValue);
            IS.SetInput("Recovery", (float)recoveryValue);

            double resultado = IS.Evaluate("Metals");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Sale", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Recovery", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Metals");
            }
            double m = (IS.GetLinguisticVariable("Metals").End - IS.GetLinguisticVariable("Metals").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Metals").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateGeneral(double decreaseContaminationOfEndPiecesValue, double recycleValue, double scrapsValue)
        {
            LinguisticVariable decreaseContaminationOfEndPieces = new( "DecreaseContaminationOfEndPieces", 0, 10 );
            decreaseContaminationOfEndPieces.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            decreaseContaminationOfEndPieces.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            decreaseContaminationOfEndPieces.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recycle = new( "Recycle", 0, 10 );
            recycle.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recycle.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recycle.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable scraps = new( "Scraps", 0, 10 );
            scraps.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            scraps.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            scraps.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable general = new( "General", 0, 10 );
            general.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            general.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            general.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            general.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            general.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( decreaseContaminationOfEndPieces );
            fuzzyDB.AddVariable( recycle );
            fuzzyDB.AddVariable( scraps );
            fuzzyDB.AddVariable( general );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Low and Scraps IS Low THEN General IS VeryLow");
            IS.NewRule("Rule 2", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Low and Scraps IS Medium THEN General IS VeryLow");
            IS.NewRule("Rule 3", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Low and Scraps IS High THEN General IS Low");
            IS.NewRule("Rule 4", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Medium and Scraps IS Low THEN General IS VeryLow");
            IS.NewRule("Rule 5", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Medium and Scraps IS Medium THEN General IS Low");
            IS.NewRule("Rule 6", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS Medium and Scraps IS High THEN General IS Middle");
            IS.NewRule("Rule 7", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS High and Scraps IS Low THEN General IS Low");
            IS.NewRule("Rule 8", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS High and Scraps IS Medium THEN General IS Middle");
            IS.NewRule("Rule 9", "IF DecreaseContaminationOfEndPieces IS Low and Recycle IS High and Scraps IS High THEN General IS High");
            IS.NewRule("Rule 10", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Low and Scraps IS Low THEN General IS VeryLow");
            IS.NewRule("Rule 11", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Low and Scraps IS Medium THEN General IS Low");
            IS.NewRule("Rule 12", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Low and Scraps IS High THEN General IS Middle");
            IS.NewRule("Rule 13", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Medium and Scraps IS Low THEN General IS Low");
            IS.NewRule("Rule 14", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Medium and Scraps IS Medium THEN General IS Middle");
            IS.NewRule("Rule 15", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS Medium and Scraps IS High THEN General IS High");
            IS.NewRule("Rule 16", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS High and Scraps IS Low THEN General IS Middle");
            IS.NewRule("Rule 17", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS High and Scraps IS Medium THEN General IS High");
            IS.NewRule("Rule 18", "IF DecreaseContaminationOfEndPieces IS Medium and Recycle IS High and Scraps IS High THEN General IS VeryHigh");
            IS.NewRule("Rule 19", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Low and Scraps IS Low THEN General IS Low");
            IS.NewRule("Rule 20", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Low and Scraps IS Medium THEN General IS Middle");
            IS.NewRule("Rule 21", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Low and Scraps IS High THEN General IS High");
            IS.NewRule("Rule 22", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Medium and Scraps IS Low THEN General IS Middle");
            IS.NewRule("Rule 23", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Medium and Scraps IS Medium THEN General IS High");
            IS.NewRule("Rule 24", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS Medium and Scraps IS High THEN General IS VeryHigh");
            IS.NewRule("Rule 25", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS High and Scraps IS Low THEN General IS High");
            IS.NewRule("Rule 26", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS High and Scraps IS Medium THEN General IS VeryHigh");
            IS.NewRule("Rule 27", "IF DecreaseContaminationOfEndPieces IS High and Recycle IS High and Scraps IS High THEN General IS VeryHigh");

            IS.SetInput("DecreaseContaminationOfEndPieces", (float)decreaseContaminationOfEndPiecesValue);
            IS.SetInput("Recycle", (float)recycleValue);
            IS.SetInput("Scraps", (float)scrapsValue);

            double resultado = IS.Evaluate("General");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DecreaseContaminationOfEndPieces", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Recycle", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Scraps", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("General");
            }
            double m = (IS.GetLinguisticVariable("General").End - IS.GetLinguisticVariable("General").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("General").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateUse(double waterValue, double employeesValue)
        {
            LinguisticVariable water = new( "Water", 0, 10 );
            water.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            water.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            water.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            water.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            water.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable employees = new( "Employees", 0, 10 );
            employees.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            employees.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            employees.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            employees.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            employees.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            use.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            use.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( water );
            fuzzyDB.AddVariable( employees );
            fuzzyDB.AddVariable( use );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Water IS VeryLow and Employees IS VeryLow THEN Use IS VeryLow");
            IS.NewRule("Rule 2", "IF Water IS VeryLow and Employees IS Low THEN Use IS VeryLow");
            IS.NewRule("Rule 3", "IF Water IS VeryLow and Employees IS Medium THEN Use IS Low");
            IS.NewRule("Rule 4", "IF Water IS VeryLow and Employees IS High THEN Use IS Low");
            IS.NewRule("Rule 5", "IF Water IS VeryLow and Employees IS VeryHigh THEN Use IS Middle");
            IS.NewRule("Rule 6", "IF Water IS Low and Employees IS VeryLow THEN Use IS VeryLow");
            IS.NewRule("Rule 7", "IF Water IS Low and Employees IS Low THEN Use IS Low");
            IS.NewRule("Rule 8", "IF Water IS Low and Employees IS Medium THEN Use IS Low");
            IS.NewRule("Rule 9", "IF Water IS Low and Employees IS High THEN Use IS Middle");
            IS.NewRule("Rule 10", "IF Water IS Low and Employees IS VeryHigh THEN Use IS High");
            IS.NewRule("Rule 11", "IF Water IS Middle and Employees IS VeryLow THEN Use IS Low");
            IS.NewRule("Rule 12", "IF Water IS Middle and Employees IS Low THEN Use IS Low");
            IS.NewRule("Rule 13", "IF Water IS Middle and Employees IS Medium THEN Use IS Middle");
            IS.NewRule("Rule 14", "IF Water IS Middle and Employees IS High THEN Use IS High");
            IS.NewRule("Rule 15", "IF Water IS Middle and Employees IS VeryHigh THEN Use IS High");
            IS.NewRule("Rule 16", "IF Water IS High and Employees IS VeryLow THEN Use IS Low");
            IS.NewRule("Rule 17", "IF Water IS High and Employees IS Low THEN Use IS Middle");
            IS.NewRule("Rule 18", "IF Water IS High and Employees IS Medium THEN Use IS High");
            IS.NewRule("Rule 19", "IF Water IS High and Employees IS High THEN Use IS High");
            IS.NewRule("Rule 20", "IF Water IS High and Employees IS VeryHigh THEN Use IS VeryHigh");
            IS.NewRule("Rule 21", "IF Water IS VeryHigh and Employees IS VeryLow THEN Use IS Middle");
            IS.NewRule("Rule 22", "IF Water IS VeryHigh and Employees IS Low THEN Use IS High");
            IS.NewRule("Rule 23", "IF Water IS VeryHigh and Employees IS Medium THEN Use IS High");
            IS.NewRule("Rule 24", "IF Water IS VeryHigh and Employees IS High THEN Use IS VeryHigh");
            IS.NewRule("Rule 25", "IF Water IS VeryHigh and Employees IS VeryHigh THEN Use IS VeryHigh");

            IS.SetInput("Water", (float)waterValue);
            IS.SetInput("Employees", (float)employeesValue);

            double resultado = IS.Evaluate("Use");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Water", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Employees", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Use");
            }
            double m = (IS.GetLinguisticVariable("Use").End - IS.GetLinguisticVariable("Use").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Use").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateEmissions(double stopperValue, double steamMinimizationValue, double removingRollersFromCleaningMachinesValue)
        {
            LinguisticVariable stopper = new( "Stopper", 0, 10 );
            stopper.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            stopper.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            stopper.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable steamMinimization = new( "SteamMinimization", 0, 10 );
            steamMinimization.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            steamMinimization.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            steamMinimization.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable removingRollersFromCleaningMachines = new( "RemovingRollersFromCleaningMachines", 0, 10 );
            removingRollersFromCleaningMachines.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            removingRollersFromCleaningMachines.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            removingRollersFromCleaningMachines.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable emissions = new( "Emissions", 0, 10 );
            emissions.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            emissions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            emissions.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            emissions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            emissions.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( stopper );
            fuzzyDB.AddVariable( steamMinimization );
            fuzzyDB.AddVariable( removingRollersFromCleaningMachines );
            fuzzyDB.AddVariable( emissions );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Stopper IS Low and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS VeryLow");
            IS.NewRule("Rule 2", "IF Stopper IS Low and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS VeryLow");
            IS.NewRule("Rule 3", "IF Stopper IS Low and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS High THEN Emissions IS Low");
            IS.NewRule("Rule 4", "IF Stopper IS Low and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS VeryLow");
            IS.NewRule("Rule 5", "IF Stopper IS Low and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS Low");
            IS.NewRule("Rule 6", "IF Stopper IS Low and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS High THEN Emissions IS Middle");
            IS.NewRule("Rule 7", "IF Stopper IS Low and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS Low");
            IS.NewRule("Rule 8", "IF Stopper IS Low and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS Middle");
            IS.NewRule("Rule 9", "IF Stopper IS Low and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS High THEN Emissions IS High");
            IS.NewRule("Rule 10", "IF Stopper IS Medium and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS VeryLow");
            IS.NewRule("Rule 11", "IF Stopper IS Medium and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS Low");
            IS.NewRule("Rule 12", "IF Stopper IS Medium and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS High THEN Emissions IS Middle");
            IS.NewRule("Rule 13", "IF Stopper IS Medium and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS Low");
            IS.NewRule("Rule 14", "IF Stopper IS Medium and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS Middle");
            IS.NewRule("Rule 15", "IF Stopper IS Medium and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS High THEN Emissions IS High");
            IS.NewRule("Rule 16", "IF Stopper IS Medium and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS Middle");
            IS.NewRule("Rule 17", "IF Stopper IS Medium and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS High");
            IS.NewRule("Rule 18", "IF Stopper IS Medium and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS High THEN Emissions IS VeryHigh");
            IS.NewRule("Rule 19", "IF Stopper IS High and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS Low");
            IS.NewRule("Rule 20", "IF Stopper IS High and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS Middle");
            IS.NewRule("Rule 21", "IF Stopper IS High and SteamMinimization IS Low and RemovingRollersFromCleaningMachines IS High THEN Emissions IS High");
            IS.NewRule("Rule 22", "IF Stopper IS High and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS Middle");
            IS.NewRule("Rule 23", "IF Stopper IS High and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS High");
            IS.NewRule("Rule 24", "IF Stopper IS High and SteamMinimization IS Medium and RemovingRollersFromCleaningMachines IS High THEN Emissions IS VeryHigh");
            IS.NewRule("Rule 25", "IF Stopper IS High and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Low THEN Emissions IS High");
            IS.NewRule("Rule 26", "IF Stopper IS High and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS Medium THEN Emissions IS VeryHigh");
            IS.NewRule("Rule 27", "IF Stopper IS High and SteamMinimization IS High and RemovingRollersFromCleaningMachines IS High THEN Emissions IS VeryHigh");

            IS.SetInput("Stopper", (float)stopperValue);
            IS.SetInput("SteamMinimization", (float)steamMinimizationValue);
            IS.SetInput("RemovingRollersFromCleaningMachines", (float)removingRollersFromCleaningMachinesValue);

            double resultado = IS.Evaluate("Emissions");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Stopper", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SteamMinimization", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RemovingRollersFromCleaningMachines", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Emissions");
            }
            double m = (IS.GetLinguisticVariable("Emissions").End - IS.GetLinguisticVariable("Emissions").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Emissions").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRestorationLevel2(double useOfSolventThatCanBeReusedValue, double waterLevel2Value, double solventLevel2Value)
        {
            LinguisticVariable useOfSolventThatCanBeReused = new( "UseOfSolventThatCanBeReused", 0, 10 );
            useOfSolventThatCanBeReused.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfSolventThatCanBeReused.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfSolventThatCanBeReused.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable waterLevel2 = new( "WaterLevel2", 0, 10 );
            waterLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            waterLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            waterLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solventLevel2 = new( "SolventLevel2", 0, 10 );
            solventLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solventLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solventLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable restorationLevel2 = new( "RestorationLevel2", 0, 10 );
            restorationLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            restorationLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            restorationLevel2.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            restorationLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            restorationLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfSolventThatCanBeReused );
            fuzzyDB.AddVariable( waterLevel2 );
            fuzzyDB.AddVariable( solventLevel2 );
            fuzzyDB.AddVariable( restorationLevel2 );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Low and SolventLevel2 IS Low THEN RestorationLevel2 IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Low and SolventLevel2 IS Medium THEN RestorationLevel2 IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Low and SolventLevel2 IS High THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 4", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Medium and SolventLevel2 IS Low THEN RestorationLevel2 IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Medium and SolventLevel2 IS Medium THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 6", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS Medium and SolventLevel2 IS High THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 7", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS High and SolventLevel2 IS Low THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 8", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS High and SolventLevel2 IS Medium THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 9", "IF UseOfSolventThatCanBeReused IS Low and WaterLevel2 IS High and SolventLevel2 IS High THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 10", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Low and SolventLevel2 IS Low THEN RestorationLevel2 IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Low and SolventLevel2 IS Medium THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 12", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Low and SolventLevel2 IS High THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 13", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Medium and SolventLevel2 IS Low THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 14", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Medium and SolventLevel2 IS Medium THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 15", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS Medium and SolventLevel2 IS High THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 16", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS High and SolventLevel2 IS Low THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 17", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS High and SolventLevel2 IS Medium THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 18", "IF UseOfSolventThatCanBeReused IS Medium and WaterLevel2 IS High and SolventLevel2 IS High THEN RestorationLevel2 IS VeryHigh");
            IS.NewRule("Rule 19", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Low and SolventLevel2 IS Low THEN RestorationLevel2 IS Low");
            IS.NewRule("Rule 20", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Low and SolventLevel2 IS Medium THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 21", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Low and SolventLevel2 IS High THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 22", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Medium and SolventLevel2 IS Low THEN RestorationLevel2 IS Middle");
            IS.NewRule("Rule 23", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Medium and SolventLevel2 IS Medium THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 24", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS Medium and SolventLevel2 IS High THEN RestorationLevel2 IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS High and SolventLevel2 IS Low THEN RestorationLevel2 IS High");
            IS.NewRule("Rule 26", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS High and SolventLevel2 IS Medium THEN RestorationLevel2 IS VeryHigh");
            IS.NewRule("Rule 27", "IF UseOfSolventThatCanBeReused IS High and WaterLevel2 IS High and SolventLevel2 IS High THEN RestorationLevel2 IS VeryHigh");

            IS.SetInput("UseOfSolventThatCanBeReused", (float)useOfSolventThatCanBeReusedValue);
            IS.SetInput("WaterLevel2", (float)waterLevel2Value);
            IS.SetInput("SolventLevel2", (float)solventLevel2Value);

            double resultado = IS.Evaluate("RestorationLevel2");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfSolventThatCanBeReused", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WaterLevel2", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SolventLevel2", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("RestorationLevel2");
            }
            double m = (IS.GetLinguisticVariable("RestorationLevel2").End - IS.GetLinguisticVariable("RestorationLevel2").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("RestorationLevel2").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolutesLevel2(double productsWithNeutralPHValue, double convertHydrocarbonCleanersToLessToxicOnesValue, double inorganicSolutionsValue)
        {
            LinguisticVariable productsWithNeutralPH = new( "ProductsWithNeutralPH", 0, 10 );
            productsWithNeutralPH.AddLabel( new FuzzySet( "Acid", new TrapezoidalFunction(0, 0, (float)5.6, 7) ) );
            productsWithNeutralPH.AddLabel( new FuzzySet( "Neutral", new TrapezoidalFunction((float)5.6, 7, (float)8.2) ) );
            productsWithNeutralPH.AddLabel( new FuzzySet( "Alkali", new TrapezoidalFunction(7, (float)8.2, 14, 14) ) );

            LinguisticVariable convertHydrocarbonCleanersToLessToxicOnes = new( "ConvertHydrocarbonCleanersToLessToxicOnes", 0, 10 );
            convertHydrocarbonCleanersToLessToxicOnes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            convertHydrocarbonCleanersToLessToxicOnes.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            convertHydrocarbonCleanersToLessToxicOnes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable inorganicSolutions = new( "InorganicSolutions", 0, 10 );
            inorganicSolutions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            inorganicSolutions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solutesLevel2 = new( "SolutesLevel2", 0, 10 );
            solutesLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solutesLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solutesLevel2.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solutesLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solutesLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( productsWithNeutralPH );
            fuzzyDB.AddVariable( convertHydrocarbonCleanersToLessToxicOnes );
            fuzzyDB.AddVariable( inorganicSolutions );
            fuzzyDB.AddVariable( solutesLevel2 );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Low THEN SolutesLevel2 IS VeryLow");
            IS.NewRule("Rule 2", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Medium THEN SolutesLevel2 IS VeryLow");
            IS.NewRule("Rule 3", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS High THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 4", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Low THEN SolutesLevel2 IS VeryLow");
            IS.NewRule("Rule 5", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Medium THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 6", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS High THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 7", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Low THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 8", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Medium THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 9", "IF ProductsWithNeutralPH IS Acid and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS High THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 10", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Low THEN SolutesLevel2 IS VeryLow");
            IS.NewRule("Rule 11", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Medium THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 12", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS High THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 13", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Low THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 14", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Medium THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 15", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS High THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 16", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Low THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 17", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Medium THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 18", "IF ProductsWithNeutralPH IS Neutral and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS High THEN SolutesLevel2 IS VeryHigh");
            IS.NewRule("Rule 19", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Low THEN SolutesLevel2 IS Low");
            IS.NewRule("Rule 20", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS Medium THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 21", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Low and InorganicSolutions IS High THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 22", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Low THEN SolutesLevel2 IS Middle");
            IS.NewRule("Rule 23", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS Medium THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 24", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS Medium and InorganicSolutions IS High THEN SolutesLevel2 IS VeryHigh");
            IS.NewRule("Rule 25", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Low THEN SolutesLevel2 IS High");
            IS.NewRule("Rule 26", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS Medium THEN SolutesLevel2 IS VeryHigh");
            IS.NewRule("Rule 27", "IF ProductsWithNeutralPH IS Alkali and ConvertHydrocarbonCleanersToLessToxicOnes IS High and InorganicSolutions IS High THEN SolutesLevel2 IS VeryHigh");

            IS.SetInput("ProductsWithNeutralPH", (float)productsWithNeutralPHValue);
            IS.SetInput("ConvertHydrocarbonCleanersToLessToxicOnes", (float)convertHydrocarbonCleanersToLessToxicOnesValue);
            IS.SetInput("InorganicSolutions", (float)inorganicSolutionsValue);

            double resultado = IS.Evaluate("SolutesLevel2");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ProductsWithNeutralPH", i == 0 ? 0 : (float)13.99);
                IS.SetInput("ConvertHydrocarbonCleanersToLessToxicOnes", i == 0 ? 0 : (float)9.99);
                IS.SetInput("InorganicSolutions", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("SolutesLevel2");
            }
            double m = (IS.GetLinguisticVariable("SolutesLevel2").End - IS.GetLinguisticVariable("SolutesLevel2").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SolutesLevel2").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSW(double sludgeRemovalValue, double flocculationValue)
        {
            LinguisticVariable sludgeRemoval = new( "SludgeRemoval", 0, 10 );
            sludgeRemoval.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sludgeRemoval.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable flocculation = new( "Flocculation", 0, 10 );
            flocculation.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            flocculation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            flocculation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            flocculation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            flocculation.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable sw = new( "SW", 0, 10 );
            sw.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            sw.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            sw.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            sw.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            sw.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( sludgeRemoval );
            fuzzyDB.AddVariable( flocculation );
            fuzzyDB.AddVariable( sw );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SludgeRemoval IS VeryLow and Flocculation IS VeryLow THEN SW IS VeryLow");
            IS.NewRule("Rule 2", "IF SludgeRemoval IS VeryLow and Flocculation IS Low THEN SW IS VeryLow");
            IS.NewRule("Rule 3", "IF SludgeRemoval IS VeryLow and Flocculation IS Medium THEN SW IS Low");
            IS.NewRule("Rule 4", "IF SludgeRemoval IS VeryLow and Flocculation IS High THEN SW IS Low");
            IS.NewRule("Rule 5", "IF SludgeRemoval IS VeryLow and Flocculation IS VeryHigh THEN SW IS Middle");
            IS.NewRule("Rule 6", "IF SludgeRemoval IS Low and Flocculation IS VeryLow THEN SW IS VeryLow");
            IS.NewRule("Rule 7", "IF SludgeRemoval IS Low and Flocculation IS Low THEN SW IS Low");
            IS.NewRule("Rule 8", "IF SludgeRemoval IS Low and Flocculation IS Medium THEN SW IS Low");
            IS.NewRule("Rule 9", "IF SludgeRemoval IS Low and Flocculation IS High THEN SW IS Middle");
            IS.NewRule("Rule 10", "IF SludgeRemoval IS Low and Flocculation IS VeryHigh THEN SW IS High");
            IS.NewRule("Rule 11", "IF SludgeRemoval IS Middle and Flocculation IS VeryLow THEN SW IS Low");
            IS.NewRule("Rule 12", "IF SludgeRemoval IS Middle and Flocculation IS Low THEN SW IS Low");
            IS.NewRule("Rule 13", "IF SludgeRemoval IS Middle and Flocculation IS Medium THEN SW IS Middle");
            IS.NewRule("Rule 14", "IF SludgeRemoval IS Middle and Flocculation IS High THEN SW IS High");
            IS.NewRule("Rule 15", "IF SludgeRemoval IS Middle and Flocculation IS VeryHigh THEN SW IS High");
            IS.NewRule("Rule 16", "IF SludgeRemoval IS High and Flocculation IS VeryLow THEN SW IS Low");
            IS.NewRule("Rule 17", "IF SludgeRemoval IS High and Flocculation IS Low THEN SW IS Middle");
            IS.NewRule("Rule 18", "IF SludgeRemoval IS High and Flocculation IS Medium THEN SW IS High");
            IS.NewRule("Rule 19", "IF SludgeRemoval IS High and Flocculation IS High THEN SW IS High");
            IS.NewRule("Rule 20", "IF SludgeRemoval IS High and Flocculation IS VeryHigh THEN SW IS VeryHigh");
            IS.NewRule("Rule 21", "IF SludgeRemoval IS VeryHigh and Flocculation IS VeryLow THEN SW IS Middle");
            IS.NewRule("Rule 22", "IF SludgeRemoval IS VeryHigh and Flocculation IS Low THEN SW IS High");
            IS.NewRule("Rule 23", "IF SludgeRemoval IS VeryHigh and Flocculation IS Medium THEN SW IS High");
            IS.NewRule("Rule 24", "IF SludgeRemoval IS VeryHigh and Flocculation IS High THEN SW IS VeryHigh");
            IS.NewRule("Rule 25", "IF SludgeRemoval IS VeryHigh and Flocculation IS VeryHigh THEN SW IS VeryHigh");

            IS.SetInput("SludgeRemoval", (float)sludgeRemovalValue);
            IS.SetInput("Flocculation", (float)flocculationValue);

            double resultado = IS.Evaluate("SW");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SludgeRemoval", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Flocculation", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("SW");
            }
            double m = (IS.GetLinguisticVariable("SW").End - IS.GetLinguisticVariable("SW").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SW").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateCWP(double heatGenerationValue, double operationValue, double saleOfCombustibleWasteValue)
        {
            LinguisticVariable heatGeneration = new( "HeatGeneration", 0, 10 );
            heatGeneration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            heatGeneration.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            heatGeneration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable operation = new( "Operation", 0, 10 );
            operation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            operation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            operation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable saleOfCombustibleWaste = new( "SaleOfCombustibleWaste", 0, 10 );
            saleOfCombustibleWaste.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            saleOfCombustibleWaste.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            saleOfCombustibleWaste.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable cwp = new( "CWP", 0, 10 );
            cwp.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            cwp.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            cwp.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            cwp.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            cwp.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( heatGeneration );
            fuzzyDB.AddVariable( operation );
            fuzzyDB.AddVariable( saleOfCombustibleWaste );
            fuzzyDB.AddVariable( cwp );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF HeatGeneration IS Low and Operation IS Low and SaleOfCombustibleWaste IS Low THEN CWP IS VeryLow");
            IS.NewRule("Rule 2", "IF HeatGeneration IS Low and Operation IS Low and SaleOfCombustibleWaste IS Medium THEN CWP IS VeryLow");
            IS.NewRule("Rule 3", "IF HeatGeneration IS Low and Operation IS Low and SaleOfCombustibleWaste IS High THEN CWP IS Low");
            IS.NewRule("Rule 4", "IF HeatGeneration IS Low and Operation IS Medium and SaleOfCombustibleWaste IS Low THEN CWP IS VeryLow");
            IS.NewRule("Rule 5", "IF HeatGeneration IS Low and Operation IS Medium and SaleOfCombustibleWaste IS Medium THEN CWP IS Low");
            IS.NewRule("Rule 6", "IF HeatGeneration IS Low and Operation IS Medium and SaleOfCombustibleWaste IS High THEN CWP IS Middle");
            IS.NewRule("Rule 7", "IF HeatGeneration IS Low and Operation IS High and SaleOfCombustibleWaste IS Low THEN CWP IS Low");
            IS.NewRule("Rule 8", "IF HeatGeneration IS Low and Operation IS High and SaleOfCombustibleWaste IS Medium THEN CWP IS Middle");
            IS.NewRule("Rule 9", "IF HeatGeneration IS Low and Operation IS High and SaleOfCombustibleWaste IS High THEN CWP IS High");
            IS.NewRule("Rule 10", "IF HeatGeneration IS Medium and Operation IS Low and SaleOfCombustibleWaste IS Low THEN CWP IS VeryLow");
            IS.NewRule("Rule 11", "IF HeatGeneration IS Medium and Operation IS Low and SaleOfCombustibleWaste IS Medium THEN CWP IS Low");
            IS.NewRule("Rule 12", "IF HeatGeneration IS Medium and Operation IS Low and SaleOfCombustibleWaste IS High THEN CWP IS Middle");
            IS.NewRule("Rule 13", "IF HeatGeneration IS Medium and Operation IS Medium and SaleOfCombustibleWaste IS Low THEN CWP IS Low");
            IS.NewRule("Rule 14", "IF HeatGeneration IS Medium and Operation IS Medium and SaleOfCombustibleWaste IS Medium THEN CWP IS Middle");
            IS.NewRule("Rule 15", "IF HeatGeneration IS Medium and Operation IS Medium and SaleOfCombustibleWaste IS High THEN CWP IS High");
            IS.NewRule("Rule 16", "IF HeatGeneration IS Medium and Operation IS High and SaleOfCombustibleWaste IS Low THEN CWP IS Middle");
            IS.NewRule("Rule 17", "IF HeatGeneration IS Medium and Operation IS High and SaleOfCombustibleWaste IS Medium THEN CWP IS High");
            IS.NewRule("Rule 18", "IF HeatGeneration IS Medium and Operation IS High and SaleOfCombustibleWaste IS High THEN CWP IS VeryHigh");
            IS.NewRule("Rule 19", "IF HeatGeneration IS High and Operation IS Low and SaleOfCombustibleWaste IS Low THEN CWP IS Low");
            IS.NewRule("Rule 20", "IF HeatGeneration IS High and Operation IS Low and SaleOfCombustibleWaste IS Medium THEN CWP IS Middle");
            IS.NewRule("Rule 21", "IF HeatGeneration IS High and Operation IS Low and SaleOfCombustibleWaste IS High THEN CWP IS High");
            IS.NewRule("Rule 22", "IF HeatGeneration IS High and Operation IS Medium and SaleOfCombustibleWaste IS Low THEN CWP IS Middle");
            IS.NewRule("Rule 23", "IF HeatGeneration IS High and Operation IS Medium and SaleOfCombustibleWaste IS Medium THEN CWP IS High");
            IS.NewRule("Rule 24", "IF HeatGeneration IS High and Operation IS Medium and SaleOfCombustibleWaste IS High THEN CWP IS VeryHigh");
            IS.NewRule("Rule 25", "IF HeatGeneration IS High and Operation IS High and SaleOfCombustibleWaste IS Low THEN CWP IS High");
            IS.NewRule("Rule 26", "IF HeatGeneration IS High and Operation IS High and SaleOfCombustibleWaste IS Medium THEN CWP IS VeryHigh");
            IS.NewRule("Rule 27", "IF HeatGeneration IS High and Operation IS High and SaleOfCombustibleWaste IS High THEN CWP IS VeryHigh");

            IS.SetInput("HeatGeneration", (float)heatGenerationValue);
            IS.SetInput("Operation", (float)operationValue);
            IS.SetInput("SaleOfCombustibleWaste", (float)saleOfCombustibleWasteValue);

            double resultado = IS.Evaluate("CWP");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("HeatGeneration", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Operation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("SaleOfCombustibleWaste", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("CWP");
            }
            double m = (IS.GetLinguisticVariable("CWP").End - IS.GetLinguisticVariable("CWP").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("CWP").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateMaintenance(double manufacturersWornSolutionsValue, double useOfHydraulicOilInTheIndustrialProcessValue, double wasteDisposalLevel2Value)
        {
            LinguisticVariable manufacturersWornSolutions = new( "ManufacturersWornSolutions", 0, 10 );
            manufacturersWornSolutions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            manufacturersWornSolutions.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            manufacturersWornSolutions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfHydraulicOilInTheIndustrialProcess = new( "UseOfHydraulicOilInTheIndustrialProcess", 0, 10 );
            useOfHydraulicOilInTheIndustrialProcess.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfHydraulicOilInTheIndustrialProcess.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfHydraulicOilInTheIndustrialProcess.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable wasteDisposalLevel2 = new( "WasteDisposalLevel2", 0, 10 );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            wasteDisposalLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            maintenance.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( manufacturersWornSolutions );
            fuzzyDB.AddVariable( useOfHydraulicOilInTheIndustrialProcess );
            fuzzyDB.AddVariable( wasteDisposalLevel2 );
            fuzzyDB.AddVariable( maintenance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Low THEN Maintenance IS VeryLow");
            IS.NewRule("Rule 2", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Medium THEN Maintenance IS VeryLow");
            IS.NewRule("Rule 3", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS High THEN Maintenance IS Low");
            IS.NewRule("Rule 4", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Low THEN Maintenance IS VeryLow");
            IS.NewRule("Rule 5", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Medium THEN Maintenance IS Low");
            IS.NewRule("Rule 6", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS High THEN Maintenance IS Middle");
            IS.NewRule("Rule 7", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Low THEN Maintenance IS Low");
            IS.NewRule("Rule 8", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Medium THEN Maintenance IS Middle");
            IS.NewRule("Rule 9", "IF ManufacturersWornSolutions IS Low and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS High THEN Maintenance IS High");
            IS.NewRule("Rule 10", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Low THEN Maintenance IS VeryLow");
            IS.NewRule("Rule 11", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Medium THEN Maintenance IS Low");
            IS.NewRule("Rule 12", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS High THEN Maintenance IS Middle");
            IS.NewRule("Rule 13", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Low THEN Maintenance IS Low");
            IS.NewRule("Rule 14", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Medium THEN Maintenance IS Middle");
            IS.NewRule("Rule 15", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS High THEN Maintenance IS High");
            IS.NewRule("Rule 16", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Low THEN Maintenance IS Middle");
            IS.NewRule("Rule 17", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Medium THEN Maintenance IS High");
            IS.NewRule("Rule 18", "IF ManufacturersWornSolutions IS Medium and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS High THEN Maintenance IS VeryHigh");
            IS.NewRule("Rule 19", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Low THEN Maintenance IS Low");
            IS.NewRule("Rule 20", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS Medium THEN Maintenance IS Middle");
            IS.NewRule("Rule 21", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Low and WasteDisposalLevel2 IS High THEN Maintenance IS High");
            IS.NewRule("Rule 22", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Low THEN Maintenance IS Middle");
            IS.NewRule("Rule 23", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS Medium THEN Maintenance IS High");
            IS.NewRule("Rule 24", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS Medium and WasteDisposalLevel2 IS High THEN Maintenance IS VeryHigh");
            IS.NewRule("Rule 25", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Low THEN Maintenance IS High");
            IS.NewRule("Rule 26", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS Medium THEN Maintenance IS VeryHigh");
            IS.NewRule("Rule 27", "IF ManufacturersWornSolutions IS High and UseOfHydraulicOilInTheIndustrialProcess IS High and WasteDisposalLevel2 IS High THEN Maintenance IS VeryHigh");

            IS.SetInput("ManufacturersWornSolutions", (float)manufacturersWornSolutionsValue);
            IS.SetInput("UseOfHydraulicOilInTheIndustrialProcess", (float)useOfHydraulicOilInTheIndustrialProcessValue);
            IS.SetInput("WasteDisposalLevel2", (float)wasteDisposalLevel2Value);

            double resultado = IS.Evaluate("Maintenance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ManufacturersWornSolutions", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfHydraulicOilInTheIndustrialProcess", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WasteDisposalLevel2", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Maintenance");
            }
            double m = (IS.GetLinguisticVariable("Maintenance").End - IS.GetLinguisticVariable("Maintenance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Maintenance").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateLiquid(double miscellaneousValue, double oilValue, double whiteWaterValue, double recyclingOfInkAndCleaningSolventResiduesValue)
        {
            LinguisticVariable miscellaneous = new( "Miscellaneous", 0, 10 );
            miscellaneous.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            miscellaneous.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            miscellaneous.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable oil = new( "Oil", 0, 10 );
            oil.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            oil.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            oil.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable whiteWater = new( "WhiteWater", 0, 10 );
            whiteWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            whiteWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            whiteWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recyclingOfInkAndCleaningSolventResidues = new( "RecyclingOfInkAndCleaningSolventResidues", 0, 10 );
            recyclingOfInkAndCleaningSolventResidues.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recyclingOfInkAndCleaningSolventResidues.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recyclingOfInkAndCleaningSolventResidues.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable liquid = new( "Liquid", 0, 10 );
            liquid.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            liquid.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            liquid.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            liquid.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            liquid.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( miscellaneous );
            fuzzyDB.AddVariable( oil );
            fuzzyDB.AddVariable( whiteWater );
            fuzzyDB.AddVariable( recyclingOfInkAndCleaningSolventResidues );
            fuzzyDB.AddVariable( liquid );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS VeryLow");
            IS.NewRule("Rule 2", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS VeryLow");
            IS.NewRule("Rule 3", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Low");
            IS.NewRule("Rule 4", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS VeryLow");
            IS.NewRule("Rule 5", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 6", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 7", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 8", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 9", "IF Miscellaneous IS Low and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 10", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 11", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Low");
            IS.NewRule("Rule 12", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 13", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS High");
            IS.NewRule("Rule 14", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 15", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 16", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 17", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 18", "IF Miscellaneous IS Low and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 19", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 20", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 21", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 22", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 23", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 24", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 25", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS High");
            IS.NewRule("Rule 26", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 27", "IF Miscellaneous IS Low and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 28", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS VeryLow");
            IS.NewRule("Rule 29", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Low");
            IS.NewRule("Rule 30", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 31", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 32", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 33", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 34", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 35", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 36", "IF Miscellaneous IS Medium and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 37", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 38", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 39", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 40", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 41", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 42", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 43", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 44", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 45", "IF Miscellaneous IS Medium and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 46", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 47", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 48", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 49", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 50", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 51", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 52", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS High");
            IS.NewRule("Rule 53", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS High");
            IS.NewRule("Rule 54", "IF Miscellaneous IS Medium and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 55", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Low");
            IS.NewRule("Rule 56", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 57", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS Middle");
            IS.NewRule("Rule 58", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 59", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 60", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 61", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS High");
            IS.NewRule("Rule 62", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS High");
            IS.NewRule("Rule 63", "IF Miscellaneous IS High and Oil IS Low and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 64", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 65", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 66", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 67", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 68", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 69", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS High");
            IS.NewRule("Rule 70", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS High");
            IS.NewRule("Rule 71", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS High");
            IS.NewRule("Rule 72", "IF Miscellaneous IS High and Oil IS Medium and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 73", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 74", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS Middle");
            IS.NewRule("Rule 75", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Low and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 76", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS Middle");
            IS.NewRule("Rule 77", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS High");
            IS.NewRule("Rule 78", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS Medium and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 79", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Low THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 80", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS Medium THEN Liquid IS VeryHigh");
            IS.NewRule("Rule 81", "IF Miscellaneous IS High and Oil IS High and WhiteWater IS High and RecyclingOfInkAndCleaningSolventResidues IS High THEN Liquid IS VeryHigh");

            IS.SetInput("Miscellaneous", (float)miscellaneousValue);
            IS.SetInput("Oil", (float)oilValue);
            IS.SetInput("WhiteWater", (float)whiteWaterValue);
            IS.SetInput("RecyclingOfInkAndCleaningSolventResidues", (float)recyclingOfInkAndCleaningSolventResiduesValue);

            double resultado = IS.Evaluate("Liquid");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Miscellaneous", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Oil", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WhiteWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RecyclingOfInkAndCleaningSolventResidues", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Liquid");
            }
            double m = (IS.GetLinguisticVariable("Liquid").End - IS.GetLinguisticVariable("Liquid").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Liquid").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolid(double metalsValue, double sandValue, double generalValue)
        {
            LinguisticVariable metals = new( "Metals", 0, 10 );
            metals.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            metals.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            metals.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable sand = new( "Sand", 0, 10 );
            sand.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            sand.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            sand.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable general = new( "General", 0, 10 );
            general.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            general.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            general.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solid = new( "Solid", 0, 10 );
            solid.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solid.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solid.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solid.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solid.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( metals );
            fuzzyDB.AddVariable( sand );
            fuzzyDB.AddVariable( general );
            fuzzyDB.AddVariable( solid );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Metals IS Low and Sand IS Low and General IS Low THEN Solid IS VeryLow");
            IS.NewRule("Rule 2", "IF Metals IS Low and Sand IS Low and General IS Medium THEN Solid IS VeryLow");
            IS.NewRule("Rule 3", "IF Metals IS Low and Sand IS Low and General IS High THEN Solid IS Low");
            IS.NewRule("Rule 4", "IF Metals IS Low and Sand IS Medium and General IS Low THEN Solid IS VeryLow");
            IS.NewRule("Rule 5", "IF Metals IS Low and Sand IS Medium and General IS Medium THEN Solid IS Low");
            IS.NewRule("Rule 6", "IF Metals IS Low and Sand IS Medium and General IS High THEN Solid IS Middle");
            IS.NewRule("Rule 7", "IF Metals IS Low and Sand IS High and General IS Low THEN Solid IS Low");
            IS.NewRule("Rule 8", "IF Metals IS Low and Sand IS High and General IS Medium THEN Solid IS Middle");
            IS.NewRule("Rule 9", "IF Metals IS Low and Sand IS High and General IS High THEN Solid IS High");
            IS.NewRule("Rule 10", "IF Metals IS Medium and Sand IS Low and General IS Low THEN Solid IS VeryLow");
            IS.NewRule("Rule 11", "IF Metals IS Medium and Sand IS Low and General IS Medium THEN Solid IS Low");
            IS.NewRule("Rule 12", "IF Metals IS Medium and Sand IS Low and General IS High THEN Solid IS Middle");
            IS.NewRule("Rule 13", "IF Metals IS Medium and Sand IS Medium and General IS Low THEN Solid IS Low");
            IS.NewRule("Rule 14", "IF Metals IS Medium and Sand IS Medium and General IS Medium THEN Solid IS Middle");
            IS.NewRule("Rule 15", "IF Metals IS Medium and Sand IS Medium and General IS High THEN Solid IS High");
            IS.NewRule("Rule 16", "IF Metals IS Medium and Sand IS High and General IS Low THEN Solid IS Middle");
            IS.NewRule("Rule 17", "IF Metals IS Medium and Sand IS High and General IS Medium THEN Solid IS High");
            IS.NewRule("Rule 18", "IF Metals IS Medium and Sand IS High and General IS High THEN Solid IS VeryHigh");
            IS.NewRule("Rule 19", "IF Metals IS High and Sand IS Low and General IS Low THEN Solid IS Low");
            IS.NewRule("Rule 20", "IF Metals IS High and Sand IS Low and General IS Medium THEN Solid IS Middle");
            IS.NewRule("Rule 21", "IF Metals IS High and Sand IS Low and General IS High THEN Solid IS High");
            IS.NewRule("Rule 22", "IF Metals IS High and Sand IS Medium and General IS Low THEN Solid IS Middle");
            IS.NewRule("Rule 23", "IF Metals IS High and Sand IS Medium and General IS Medium THEN Solid IS High");
            IS.NewRule("Rule 24", "IF Metals IS High and Sand IS Medium and General IS High THEN Solid IS VeryHigh");
            IS.NewRule("Rule 25", "IF Metals IS High and Sand IS High and General IS Low THEN Solid IS High");
            IS.NewRule("Rule 26", "IF Metals IS High and Sand IS High and General IS Medium THEN Solid IS VeryHigh");
            IS.NewRule("Rule 27", "IF Metals IS High and Sand IS High and General IS High THEN Solid IS VeryHigh");

            IS.SetInput("Metals", (float)metalsValue);
            IS.SetInput("Sand", (float)sandValue);
            IS.SetInput("General", (float)generalValue);

            double resultado = IS.Evaluate("Solid");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Metals", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Sand", i == 0 ? 0 : (float)9.99);
                IS.SetInput("General", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Solid");
            }
            double m = (IS.GetLinguisticVariable("Solid").End - IS.GetLinguisticVariable("Solid").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solid").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateReduction(double useValue, double emissionsValue)
        {
            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            use.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            use.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable emissions = new( "Emissions", 0, 10 );
            emissions.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            emissions.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            emissions.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            emissions.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            emissions.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable reduction = new( "Reduction", 0, 10 );
            reduction.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reduction.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reduction.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reduction.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reduction.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( use );
            fuzzyDB.AddVariable( emissions );
            fuzzyDB.AddVariable( reduction );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Use IS VeryLow and Emissions IS VeryLow THEN Reduction IS VeryLow");
            IS.NewRule("Rule 2", "IF Use IS VeryLow and Emissions IS Low THEN Reduction IS VeryLow");
            IS.NewRule("Rule 3", "IF Use IS VeryLow and Emissions IS Medium THEN Reduction IS Low");
            IS.NewRule("Rule 4", "IF Use IS VeryLow and Emissions IS High THEN Reduction IS Low");
            IS.NewRule("Rule 5", "IF Use IS VeryLow and Emissions IS VeryHigh THEN Reduction IS Middle");
            IS.NewRule("Rule 6", "IF Use IS Low and Emissions IS VeryLow THEN Reduction IS VeryLow");
            IS.NewRule("Rule 7", "IF Use IS Low and Emissions IS Low THEN Reduction IS Low");
            IS.NewRule("Rule 8", "IF Use IS Low and Emissions IS Medium THEN Reduction IS Low");
            IS.NewRule("Rule 9", "IF Use IS Low and Emissions IS High THEN Reduction IS Middle");
            IS.NewRule("Rule 10", "IF Use IS Low and Emissions IS VeryHigh THEN Reduction IS High");
            IS.NewRule("Rule 11", "IF Use IS Middle and Emissions IS VeryLow THEN Reduction IS Low");
            IS.NewRule("Rule 12", "IF Use IS Middle and Emissions IS Low THEN Reduction IS Low");
            IS.NewRule("Rule 13", "IF Use IS Middle and Emissions IS Medium THEN Reduction IS Middle");
            IS.NewRule("Rule 14", "IF Use IS Middle and Emissions IS High THEN Reduction IS High");
            IS.NewRule("Rule 15", "IF Use IS Middle and Emissions IS VeryHigh THEN Reduction IS High");
            IS.NewRule("Rule 16", "IF Use IS High and Emissions IS VeryLow THEN Reduction IS Low");
            IS.NewRule("Rule 17", "IF Use IS High and Emissions IS Low THEN Reduction IS Middle");
            IS.NewRule("Rule 18", "IF Use IS High and Emissions IS Medium THEN Reduction IS High");
            IS.NewRule("Rule 19", "IF Use IS High and Emissions IS High THEN Reduction IS High");
            IS.NewRule("Rule 20", "IF Use IS High and Emissions IS VeryHigh THEN Reduction IS VeryHigh");
            IS.NewRule("Rule 21", "IF Use IS VeryHigh and Emissions IS VeryLow THEN Reduction IS Middle");
            IS.NewRule("Rule 22", "IF Use IS VeryHigh and Emissions IS Low THEN Reduction IS High");
            IS.NewRule("Rule 23", "IF Use IS VeryHigh and Emissions IS Medium THEN Reduction IS High");
            IS.NewRule("Rule 24", "IF Use IS VeryHigh and Emissions IS High THEN Reduction IS VeryHigh");
            IS.NewRule("Rule 25", "IF Use IS VeryHigh and Emissions IS VeryHigh THEN Reduction IS VeryHigh");

            IS.SetInput("Use", (float)useValue);
            IS.SetInput("Emissions", (float)emissionsValue);

            double resultado = IS.Evaluate("Reduction");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Use", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Emissions", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Reduction");
            }
            double m = (IS.GetLinguisticVariable("Reduction").End - IS.GetLinguisticVariable("Reduction").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Reduction").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRestoration(double solventValue, double restorationLevel2Value)
        {
            LinguisticVariable solvent = new( "Solvent", 0, 10 );
            solvent.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solvent.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solvent.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solvent.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solvent.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable restorationLevel2 = new( "RestorationLevel2", 0, 10 );
            restorationLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            restorationLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            restorationLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            restorationLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            restorationLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable restoration = new( "Restoration", 0, 10 );
            restoration.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            restoration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            restoration.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            restoration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            restoration.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( solvent );
            fuzzyDB.AddVariable( restorationLevel2 );
            fuzzyDB.AddVariable( restoration );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Solvent IS VeryLow and RestorationLevel2 IS VeryLow THEN Restoration IS VeryLow");
            IS.NewRule("Rule 2", "IF Solvent IS VeryLow and RestorationLevel2 IS Low THEN Restoration IS VeryLow");
            IS.NewRule("Rule 3", "IF Solvent IS VeryLow and RestorationLevel2 IS Medium THEN Restoration IS Low");
            IS.NewRule("Rule 4", "IF Solvent IS VeryLow and RestorationLevel2 IS High THEN Restoration IS Low");
            IS.NewRule("Rule 5", "IF Solvent IS VeryLow and RestorationLevel2 IS VeryHigh THEN Restoration IS Middle");
            IS.NewRule("Rule 6", "IF Solvent IS Low and RestorationLevel2 IS VeryLow THEN Restoration IS VeryLow");
            IS.NewRule("Rule 7", "IF Solvent IS Low and RestorationLevel2 IS Low THEN Restoration IS Low");
            IS.NewRule("Rule 8", "IF Solvent IS Low and RestorationLevel2 IS Medium THEN Restoration IS Low");
            IS.NewRule("Rule 9", "IF Solvent IS Low and RestorationLevel2 IS High THEN Restoration IS Middle");
            IS.NewRule("Rule 10", "IF Solvent IS Low and RestorationLevel2 IS VeryHigh THEN Restoration IS High");
            IS.NewRule("Rule 11", "IF Solvent IS Middle and RestorationLevel2 IS VeryLow THEN Restoration IS Low");
            IS.NewRule("Rule 12", "IF Solvent IS Middle and RestorationLevel2 IS Low THEN Restoration IS Low");
            IS.NewRule("Rule 13", "IF Solvent IS Middle and RestorationLevel2 IS Medium THEN Restoration IS Middle");
            IS.NewRule("Rule 14", "IF Solvent IS Middle and RestorationLevel2 IS High THEN Restoration IS High");
            IS.NewRule("Rule 15", "IF Solvent IS Middle and RestorationLevel2 IS VeryHigh THEN Restoration IS High");
            IS.NewRule("Rule 16", "IF Solvent IS High and RestorationLevel2 IS VeryLow THEN Restoration IS Low");
            IS.NewRule("Rule 17", "IF Solvent IS High and RestorationLevel2 IS Low THEN Restoration IS Middle");
            IS.NewRule("Rule 18", "IF Solvent IS High and RestorationLevel2 IS Medium THEN Restoration IS High");
            IS.NewRule("Rule 19", "IF Solvent IS High and RestorationLevel2 IS High THEN Restoration IS High");
            IS.NewRule("Rule 20", "IF Solvent IS High and RestorationLevel2 IS VeryHigh THEN Restoration IS VeryHigh");
            IS.NewRule("Rule 21", "IF Solvent IS VeryHigh and RestorationLevel2 IS VeryLow THEN Restoration IS Middle");
            IS.NewRule("Rule 22", "IF Solvent IS VeryHigh and RestorationLevel2 IS Low THEN Restoration IS High");
            IS.NewRule("Rule 23", "IF Solvent IS VeryHigh and RestorationLevel2 IS Medium THEN Restoration IS High");
            IS.NewRule("Rule 24", "IF Solvent IS VeryHigh and RestorationLevel2 IS High THEN Restoration IS VeryHigh");
            IS.NewRule("Rule 25", "IF Solvent IS VeryHigh and RestorationLevel2 IS VeryHigh THEN Restoration IS VeryHigh");

            IS.SetInput("Solvent", (float)solventValue);
            IS.SetInput("RestorationLevel2", (float)restorationLevel2Value);

            double resultado = IS.Evaluate("Restoration");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Solvent", i == 0 ? (float)9.99 : 0);
                IS.SetInput("RestorationLevel2", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Restoration");
            }
            double m = (IS.GetLinguisticVariable("Restoration").End - IS.GetLinguisticVariable("Restoration").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Restoration").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolutes(double waterBasedSubstitutesValue, double solutesLevel2Value)
        {
            LinguisticVariable waterBasedSubstitutes = new( "WaterBasedSubstitutes", 0, 10 );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterBasedSubstitutes.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable solutesLevel2 = new( "SolutesLevel2", 0, 10 );
            solutesLevel2.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solutesLevel2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solutesLevel2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            solutesLevel2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solutesLevel2.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable solutes = new( "Solutes", 0, 10 );
            solutes.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solutes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solutes.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solutes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solutes.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( waterBasedSubstitutes );
            fuzzyDB.AddVariable( solutesLevel2 );
            fuzzyDB.AddVariable( solutes );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF WaterBasedSubstitutes IS VeryLow and SolutesLevel2 IS VeryLow THEN Solutes IS VeryLow");
            IS.NewRule("Rule 2", "IF WaterBasedSubstitutes IS VeryLow and SolutesLevel2 IS Low THEN Solutes IS VeryLow");
            IS.NewRule("Rule 3", "IF WaterBasedSubstitutes IS VeryLow and SolutesLevel2 IS Medium THEN Solutes IS Low");
            IS.NewRule("Rule 4", "IF WaterBasedSubstitutes IS VeryLow and SolutesLevel2 IS High THEN Solutes IS Low");
            IS.NewRule("Rule 5", "IF WaterBasedSubstitutes IS VeryLow and SolutesLevel2 IS VeryHigh THEN Solutes IS Middle");
            IS.NewRule("Rule 6", "IF WaterBasedSubstitutes IS Low and SolutesLevel2 IS VeryLow THEN Solutes IS VeryLow");
            IS.NewRule("Rule 7", "IF WaterBasedSubstitutes IS Low and SolutesLevel2 IS Low THEN Solutes IS Low");
            IS.NewRule("Rule 8", "IF WaterBasedSubstitutes IS Low and SolutesLevel2 IS Medium THEN Solutes IS Low");
            IS.NewRule("Rule 9", "IF WaterBasedSubstitutes IS Low and SolutesLevel2 IS High THEN Solutes IS Middle");
            IS.NewRule("Rule 10", "IF WaterBasedSubstitutes IS Low and SolutesLevel2 IS VeryHigh THEN Solutes IS High");
            IS.NewRule("Rule 11", "IF WaterBasedSubstitutes IS Middle and SolutesLevel2 IS VeryLow THEN Solutes IS Low");
            IS.NewRule("Rule 12", "IF WaterBasedSubstitutes IS Middle and SolutesLevel2 IS Low THEN Solutes IS Low");
            IS.NewRule("Rule 13", "IF WaterBasedSubstitutes IS Middle and SolutesLevel2 IS Medium THEN Solutes IS Middle");
            IS.NewRule("Rule 14", "IF WaterBasedSubstitutes IS Middle and SolutesLevel2 IS High THEN Solutes IS High");
            IS.NewRule("Rule 15", "IF WaterBasedSubstitutes IS Middle and SolutesLevel2 IS VeryHigh THEN Solutes IS High");
            IS.NewRule("Rule 16", "IF WaterBasedSubstitutes IS High and SolutesLevel2 IS VeryLow THEN Solutes IS Low");
            IS.NewRule("Rule 17", "IF WaterBasedSubstitutes IS High and SolutesLevel2 IS Low THEN Solutes IS Middle");
            IS.NewRule("Rule 18", "IF WaterBasedSubstitutes IS High and SolutesLevel2 IS Medium THEN Solutes IS High");
            IS.NewRule("Rule 19", "IF WaterBasedSubstitutes IS High and SolutesLevel2 IS High THEN Solutes IS High");
            IS.NewRule("Rule 20", "IF WaterBasedSubstitutes IS High and SolutesLevel2 IS VeryHigh THEN Solutes IS VeryHigh");
            IS.NewRule("Rule 21", "IF WaterBasedSubstitutes IS VeryHigh and SolutesLevel2 IS VeryLow THEN Solutes IS Middle");
            IS.NewRule("Rule 22", "IF WaterBasedSubstitutes IS VeryHigh and SolutesLevel2 IS Low THEN Solutes IS High");
            IS.NewRule("Rule 23", "IF WaterBasedSubstitutes IS VeryHigh and SolutesLevel2 IS Medium THEN Solutes IS High");
            IS.NewRule("Rule 24", "IF WaterBasedSubstitutes IS VeryHigh and SolutesLevel2 IS High THEN Solutes IS VeryHigh");
            IS.NewRule("Rule 25", "IF WaterBasedSubstitutes IS VeryHigh and SolutesLevel2 IS VeryHigh THEN Solutes IS VeryHigh");

            IS.SetInput("WaterBasedSubstitutes", (float)waterBasedSubstitutesValue);
            IS.SetInput("SolutesLevel2", (float)solutesLevel2Value);

            double resultado = IS.Evaluate("Solutes");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("WaterBasedSubstitutes", i == 0 ? (float)9.99 : 0);
                IS.SetInput("SolutesLevel2", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Solutes");
            }
            double m = (IS.GetLinguisticVariable("Solutes").End - IS.GetLinguisticVariable("Solutes").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solutes").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateWasteDisposal(double swValue, double cwpValue, double maintenanceValue)
        {
            LinguisticVariable sw = new( "SW", 0, 10 );
            sw.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            sw.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            sw.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable cwp = new( "CWP", 0, 10 );
            cwp.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            cwp.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            cwp.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable wasteDisposal = new( "WasteDisposal", 0, 10 );
            wasteDisposal.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            wasteDisposal.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            wasteDisposal.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            wasteDisposal.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            wasteDisposal.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( sw );
            fuzzyDB.AddVariable( cwp );
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( wasteDisposal );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SW IS Low and CWP IS Low and Maintenance IS Low THEN WasteDisposal IS VeryLow");
            IS.NewRule("Rule 2", "IF SW IS Low and CWP IS Low and Maintenance IS Medium THEN WasteDisposal IS VeryLow");
            IS.NewRule("Rule 3", "IF SW IS Low and CWP IS Low and Maintenance IS High THEN WasteDisposal IS Low");
            IS.NewRule("Rule 4", "IF SW IS Low and CWP IS Medium and Maintenance IS Low THEN WasteDisposal IS VeryLow");
            IS.NewRule("Rule 5", "IF SW IS Low and CWP IS Medium and Maintenance IS Medium THEN WasteDisposal IS Low");
            IS.NewRule("Rule 6", "IF SW IS Low and CWP IS Medium and Maintenance IS High THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 7", "IF SW IS Low and CWP IS High and Maintenance IS Low THEN WasteDisposal IS Low");
            IS.NewRule("Rule 8", "IF SW IS Low and CWP IS High and Maintenance IS Medium THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 9", "IF SW IS Low and CWP IS High and Maintenance IS High THEN WasteDisposal IS High");
            IS.NewRule("Rule 10", "IF SW IS Medium and CWP IS Low and Maintenance IS Low THEN WasteDisposal IS VeryLow");
            IS.NewRule("Rule 11", "IF SW IS Medium and CWP IS Low and Maintenance IS Medium THEN WasteDisposal IS Low");
            IS.NewRule("Rule 12", "IF SW IS Medium and CWP IS Low and Maintenance IS High THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 13", "IF SW IS Medium and CWP IS Medium and Maintenance IS Low THEN WasteDisposal IS Low");
            IS.NewRule("Rule 14", "IF SW IS Medium and CWP IS Medium and Maintenance IS Medium THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 15", "IF SW IS Medium and CWP IS Medium and Maintenance IS High THEN WasteDisposal IS High");
            IS.NewRule("Rule 16", "IF SW IS Medium and CWP IS High and Maintenance IS Low THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 17", "IF SW IS Medium and CWP IS High and Maintenance IS Medium THEN WasteDisposal IS High");
            IS.NewRule("Rule 18", "IF SW IS Medium and CWP IS High and Maintenance IS High THEN WasteDisposal IS VeryHigh");
            IS.NewRule("Rule 19", "IF SW IS High and CWP IS Low and Maintenance IS Low THEN WasteDisposal IS Low");
            IS.NewRule("Rule 20", "IF SW IS High and CWP IS Low and Maintenance IS Medium THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 21", "IF SW IS High and CWP IS Low and Maintenance IS High THEN WasteDisposal IS High");
            IS.NewRule("Rule 22", "IF SW IS High and CWP IS Medium and Maintenance IS Low THEN WasteDisposal IS Middle");
            IS.NewRule("Rule 23", "IF SW IS High and CWP IS Medium and Maintenance IS Medium THEN WasteDisposal IS High");
            IS.NewRule("Rule 24", "IF SW IS High and CWP IS Medium and Maintenance IS High THEN WasteDisposal IS VeryHigh");
            IS.NewRule("Rule 25", "IF SW IS High and CWP IS High and Maintenance IS Low THEN WasteDisposal IS High");
            IS.NewRule("Rule 26", "IF SW IS High and CWP IS High and Maintenance IS Medium THEN WasteDisposal IS VeryHigh");
            IS.NewRule("Rule 27", "IF SW IS High and CWP IS High and Maintenance IS High THEN WasteDisposal IS VeryHigh");

            IS.SetInput("SW", (float)swValue);
            IS.SetInput("CWP", (float)cwpValue);
            IS.SetInput("Maintenance", (float)maintenanceValue);

            double resultado = IS.Evaluate("WasteDisposal");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SW", i == 0 ? 0 : (float)9.99);
                IS.SetInput("CWP", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Maintenance", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("WasteDisposal");
            }
            double m = (IS.GetLinguisticVariable("WasteDisposal").End - IS.GetLinguisticVariable("WasteDisposal").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WasteDisposal").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRecycling(double otherMateriasValue, double solidValue, double liquidValue)
        {
            LinguisticVariable otherMaterials = new( "OtherMaterials", 0, 10 );
            otherMaterials.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            otherMaterials.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            otherMaterials.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solid = new( "Solid", 0, 10 );
            solid.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solid.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solid.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable liquid = new( "Liquid", 0, 10 );
            liquid.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            liquid.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            liquid.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recycling = new( "Recycling", 0, 10 );
            recycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recycling.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            recycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( otherMaterials );
            fuzzyDB.AddVariable( solid );
            fuzzyDB.AddVariable( liquid );
            fuzzyDB.AddVariable( recycling );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF OtherMaterials IS Low and Solid IS Low and Liquid IS Low THEN Recycling IS VeryLow");
            IS.NewRule("Rule 2", "IF OtherMaterials IS Low and Solid IS Low and Liquid IS Medium THEN Recycling IS VeryLow");
            IS.NewRule("Rule 3", "IF OtherMaterials IS Low and Solid IS Low and Liquid IS High THEN Recycling IS Low");
            IS.NewRule("Rule 4", "IF OtherMaterials IS Low and Solid IS Medium and Liquid IS Low THEN Recycling IS VeryLow");
            IS.NewRule("Rule 5", "IF OtherMaterials IS Low and Solid IS Medium and Liquid IS Medium THEN Recycling IS Low");
            IS.NewRule("Rule 6", "IF OtherMaterials IS Low and Solid IS Medium and Liquid IS High THEN Recycling IS Middle");
            IS.NewRule("Rule 7", "IF OtherMaterials IS Low and Solid IS High and Liquid IS Low THEN Recycling IS Low");
            IS.NewRule("Rule 8", "IF OtherMaterials IS Low and Solid IS High and Liquid IS Medium THEN Recycling IS Middle");
            IS.NewRule("Rule 9", "IF OtherMaterials IS Low and Solid IS High and Liquid IS High THEN Recycling IS High");
            IS.NewRule("Rule 10", "IF OtherMaterials IS Medium and Solid IS Low and Liquid IS Low THEN Recycling IS VeryLow");
            IS.NewRule("Rule 11", "IF OtherMaterials IS Medium and Solid IS Low and Liquid IS Medium THEN Recycling IS Low");
            IS.NewRule("Rule 12", "IF OtherMaterials IS Medium and Solid IS Low and Liquid IS High THEN Recycling IS Middle");
            IS.NewRule("Rule 13", "IF OtherMaterials IS Medium and Solid IS Medium and Liquid IS Low THEN Recycling IS Low");
            IS.NewRule("Rule 14", "IF OtherMaterials IS Medium and Solid IS Medium and Liquid IS Medium THEN Recycling IS Middle");
            IS.NewRule("Rule 15", "IF OtherMaterials IS Medium and Solid IS Medium and Liquid IS High THEN Recycling IS High");
            IS.NewRule("Rule 16", "IF OtherMaterials IS Medium and Solid IS High and Liquid IS Low THEN Recycling IS Middle");
            IS.NewRule("Rule 17", "IF OtherMaterials IS Medium and Solid IS High and Liquid IS Medium THEN Recycling IS High");
            IS.NewRule("Rule 18", "IF OtherMaterials IS Medium and Solid IS High and Liquid IS High THEN Recycling IS VeryHigh");
            IS.NewRule("Rule 19", "IF OtherMaterials IS High and Solid IS Low and Liquid IS Low THEN Recycling IS Low");
            IS.NewRule("Rule 20", "IF OtherMaterials IS High and Solid IS Low and Liquid IS Medium THEN Recycling IS Middle");
            IS.NewRule("Rule 21", "IF OtherMaterials IS High and Solid IS Low and Liquid IS High THEN Recycling IS High");
            IS.NewRule("Rule 22", "IF OtherMaterials IS High and Solid IS Medium and Liquid IS Low THEN Recycling IS Middle");
            IS.NewRule("Rule 23", "IF OtherMaterials IS High and Solid IS Medium and Liquid IS Medium THEN Recycling IS High");
            IS.NewRule("Rule 24", "IF OtherMaterials IS High and Solid IS Medium and Liquid IS High THEN Recycling IS VeryHigh");
            IS.NewRule("Rule 25", "IF OtherMaterials IS High and Solid IS High and Liquid IS Low THEN Recycling IS High");
            IS.NewRule("Rule 26", "IF OtherMaterials IS High and Solid IS High and Liquid IS Medium THEN Recycling IS VeryHigh");
            IS.NewRule("Rule 27", "IF OtherMaterials IS High and Solid IS High and Liquid IS High THEN Recycling IS VeryHigh");

            IS.SetInput("OtherMaterials", (float)otherMateriasValue);
            IS.SetInput("Solid", (float)solidValue);
            IS.SetInput("Liquid", (float)liquidValue);

            double resultado = IS.Evaluate("Recycling");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("OtherMaterials", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Solid", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Liquid", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Recycling");
            }
            double m = (IS.GetLinguisticVariable("Recycling").End - IS.GetLinguisticVariable("Recycling").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Recycling").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateSolvents(double reductionValue, double restorationValue)
        {
            LinguisticVariable reduction = new( "Reduction", 0, 10 );
            reduction.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reduction.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reduction.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reduction.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reduction.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable restoration = new( "Restoration", 0, 10 );
            restoration.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            restoration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            restoration.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            restoration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            restoration.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable solvents = new( "Solvents", 0, 10 );
            solvents.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            solvents.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            solvents.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            solvents.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            solvents.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reduction );
            fuzzyDB.AddVariable( restoration );
            fuzzyDB.AddVariable( solvents );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Reduction IS VeryLow and Restoration IS VeryLow THEN Solvents IS VeryLow");
            IS.NewRule("Rule 2", "IF Reduction IS VeryLow and Restoration IS Low THEN Solvents IS VeryLow");
            IS.NewRule("Rule 3", "IF Reduction IS VeryLow and Restoration IS Medium THEN Solvents IS Low");
            IS.NewRule("Rule 4", "IF Reduction IS VeryLow and Restoration IS High THEN Solvents IS Low");
            IS.NewRule("Rule 5", "IF Reduction IS VeryLow and Restoration IS VeryHigh THEN Solvents IS Middle");
            IS.NewRule("Rule 6", "IF Reduction IS Low and Restoration IS VeryLow THEN Solvents IS VeryLow");
            IS.NewRule("Rule 7", "IF Reduction IS Low and Restoration IS Low THEN Solvents IS Low");
            IS.NewRule("Rule 8", "IF Reduction IS Low and Restoration IS Medium THEN Solvents IS Low");
            IS.NewRule("Rule 9", "IF Reduction IS Low and Restoration IS High THEN Solvents IS Middle");
            IS.NewRule("Rule 10", "IF Reduction IS Low and Restoration IS VeryHigh THEN Solvents IS High");
            IS.NewRule("Rule 11", "IF Reduction IS Middle and Restoration IS VeryLow THEN Solvents IS Low");
            IS.NewRule("Rule 12", "IF Reduction IS Middle and Restoration IS Low THEN Solvents IS Low");
            IS.NewRule("Rule 13", "IF Reduction IS Middle and Restoration IS Medium THEN Solvents IS Middle");
            IS.NewRule("Rule 14", "IF Reduction IS Middle and Restoration IS High THEN Solvents IS High");
            IS.NewRule("Rule 15", "IF Reduction IS Middle and Restoration IS VeryHigh THEN Solvents IS High");
            IS.NewRule("Rule 16", "IF Reduction IS High and Restoration IS VeryLow THEN Solvents IS Low");
            IS.NewRule("Rule 17", "IF Reduction IS High and Restoration IS Low THEN Solvents IS Middle");
            IS.NewRule("Rule 18", "IF Reduction IS High and Restoration IS Medium THEN Solvents IS High");
            IS.NewRule("Rule 19", "IF Reduction IS High and Restoration IS High THEN Solvents IS High");
            IS.NewRule("Rule 20", "IF Reduction IS High and Restoration IS VeryHigh THEN Solvents IS VeryHigh");
            IS.NewRule("Rule 21", "IF Reduction IS VeryHigh and Restoration IS VeryLow THEN Solvents IS Middle");
            IS.NewRule("Rule 22", "IF Reduction IS VeryHigh and Restoration IS Low THEN Solvents IS High");
            IS.NewRule("Rule 23", "IF Reduction IS VeryHigh and Restoration IS Medium THEN Solvents IS High");
            IS.NewRule("Rule 24", "IF Reduction IS VeryHigh and Restoration IS High THEN Solvents IS VeryHigh");
            IS.NewRule("Rule 25", "IF Reduction IS VeryHigh and Restoration IS VeryHigh THEN Solvents IS VeryHigh");

            IS.SetInput("Reduction", (float)reductionValue);
            IS.SetInput("Restoration", (float)restorationValue);

            double resultado = IS.Evaluate("Solvents");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Reduction", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Restoration", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Solvents");
            }
            double m = (IS.GetLinguisticVariable("Solvents").End - IS.GetLinguisticVariable("Solvents").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Solvents").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateRawMaterials(double solutesValue, double solidsValue, double solventsValue)
        {
            LinguisticVariable solutes = new( "Solutes", 0, 10 );
            solutes.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solutes.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solutes.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solids = new( "Solids", 0, 10 );
            solids.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solids.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solids.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable solvents = new( "Solvents", 0, 10 );
            solvents.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            solvents.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            solvents.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable rawMaterials = new( "RawMaterials", 0, 10 );
            rawMaterials.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            rawMaterials.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            rawMaterials.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            rawMaterials.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            rawMaterials.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( solutes );
            fuzzyDB.AddVariable( solids );
            fuzzyDB.AddVariable( solvents );
            fuzzyDB.AddVariable( rawMaterials );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Solutes IS Low and Solids IS Low and Solvents IS Low THEN RawMaterials IS VeryLow");
            IS.NewRule("Rule 2", "IF Solutes IS Low and Solids IS Low and Solvents IS Medium THEN RawMaterials IS VeryLow");
            IS.NewRule("Rule 3", "IF Solutes IS Low and Solids IS Low and Solvents IS High THEN RawMaterials IS Low");
            IS.NewRule("Rule 4", "IF Solutes IS Low and Solids IS Medium and Solvents IS Low THEN RawMaterials IS VeryLow");
            IS.NewRule("Rule 5", "IF Solutes IS Low and Solids IS Medium and Solvents IS Medium THEN RawMaterials IS Low");
            IS.NewRule("Rule 6", "IF Solutes IS Low and Solids IS Medium and Solvents IS High THEN RawMaterials IS Middle");
            IS.NewRule("Rule 7", "IF Solutes IS Low and Solids IS High and Solvents IS Low THEN RawMaterials IS Low");
            IS.NewRule("Rule 8", "IF Solutes IS Low and Solids IS High and Solvents IS Medium THEN RawMaterials IS Middle");
            IS.NewRule("Rule 9", "IF Solutes IS Low and Solids IS High and Solvents IS High THEN RawMaterials IS High");
            IS.NewRule("Rule 10", "IF Solutes IS Medium and Solids IS Low and Solvents IS Low THEN RawMaterials IS VeryLow");
            IS.NewRule("Rule 11", "IF Solutes IS Medium and Solids IS Low and Solvents IS Medium THEN RawMaterials IS Low");
            IS.NewRule("Rule 12", "IF Solutes IS Medium and Solids IS Low and Solvents IS High THEN RawMaterials IS Middle");
            IS.NewRule("Rule 13", "IF Solutes IS Medium and Solids IS Medium and Solvents IS Low THEN RawMaterials IS Low");
            IS.NewRule("Rule 14", "IF Solutes IS Medium and Solids IS Medium and Solvents IS Medium THEN RawMaterials IS Middle");
            IS.NewRule("Rule 15", "IF Solutes IS Medium and Solids IS Medium and Solvents IS High THEN RawMaterials IS High");
            IS.NewRule("Rule 16", "IF Solutes IS Medium and Solids IS High and Solvents IS Low THEN RawMaterials IS Middle");
            IS.NewRule("Rule 17", "IF Solutes IS Medium and Solids IS High and Solvents IS Medium THEN RawMaterials IS High");
            IS.NewRule("Rule 18", "IF Solutes IS Medium and Solids IS High and Solvents IS High THEN RawMaterials IS VeryHigh");
            IS.NewRule("Rule 19", "IF Solutes IS High and Solids IS Low and Solvents IS Low THEN RawMaterials IS Low");
            IS.NewRule("Rule 20", "IF Solutes IS High and Solids IS Low and Solvents IS Medium THEN RawMaterials IS Middle");
            IS.NewRule("Rule 21", "IF Solutes IS High and Solids IS Low and Solvents IS High THEN RawMaterials IS High");
            IS.NewRule("Rule 22", "IF Solutes IS High and Solids IS Medium and Solvents IS Low THEN RawMaterials IS Middle");
            IS.NewRule("Rule 23", "IF Solutes IS High and Solids IS Medium and Solvents IS Medium THEN RawMaterials IS High");
            IS.NewRule("Rule 24", "IF Solutes IS High and Solids IS Medium and Solvents IS High THEN RawMaterials IS VeryHigh");
            IS.NewRule("Rule 25", "IF Solutes IS High and Solids IS High and Solvents IS Low THEN RawMaterials IS High");
            IS.NewRule("Rule 26", "IF Solutes IS High and Solids IS High and Solvents IS Medium THEN RawMaterials IS VeryHigh");
            IS.NewRule("Rule 27", "IF Solutes IS High and Solids IS High and Solvents IS High THEN RawMaterials IS VeryHigh");

            IS.SetInput("Solutes", (float)solutesValue);
            IS.SetInput("Solids", (float)solidsValue);
            IS.SetInput("Solvents", (float)solventsValue);

            double resultado = IS.Evaluate("RawMaterials");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Solutes", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Solids", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Solvents", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("RawMaterials");
            }
            double m = (IS.GetLinguisticVariable("RawMaterials").End - IS.GetLinguisticVariable("RawMaterials").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("RawMaterials").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateChemicalTreatment(double recyclingValue, double rawMaterialsValue, double wasteDisposalValue)
        {
            LinguisticVariable recycling = new( "Recycling", 0, 10 );
            recycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recycling.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable rawMaterials = new( "RawMaterials", 0, 10 );
            rawMaterials.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            rawMaterials.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            rawMaterials.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable wasteDisposal = new( "WasteDisposal", 0, 10 );
            wasteDisposal.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            wasteDisposal.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            wasteDisposal.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable chemicalTreatment = new( "ChemicalTreatment", 0, 10 );
            chemicalTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( recycling );
            fuzzyDB.AddVariable( rawMaterials );
            fuzzyDB.AddVariable( wasteDisposal );
            fuzzyDB.AddVariable( chemicalTreatment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Recycling IS Low and RawMaterials IS Low and WasteDisposal IS Low THEN ChemicalTreatment IS VeryLow");
            IS.NewRule("Rule 2", "IF Recycling IS Low and RawMaterials IS Low and WasteDisposal IS Medium THEN ChemicalTreatment IS VeryLow");
            IS.NewRule("Rule 3", "IF Recycling IS Low and RawMaterials IS Low and WasteDisposal IS High THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 4", "IF Recycling IS Low and RawMaterials IS Medium and WasteDisposal IS Low THEN ChemicalTreatment IS VeryLow");
            IS.NewRule("Rule 5", "IF Recycling IS Low and RawMaterials IS Medium and WasteDisposal IS Medium THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 6", "IF Recycling IS Low and RawMaterials IS Medium and WasteDisposal IS High THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 7", "IF Recycling IS Low and RawMaterials IS High and WasteDisposal IS Low THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 8", "IF Recycling IS Low and RawMaterials IS High and WasteDisposal IS Medium THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 9", "IF Recycling IS Low and RawMaterials IS High and WasteDisposal IS High THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 10", "IF Recycling IS Medium and RawMaterials IS Low and WasteDisposal IS Low THEN ChemicalTreatment IS VeryLow");
            IS.NewRule("Rule 11", "IF Recycling IS Medium and RawMaterials IS Low and WasteDisposal IS Medium THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 12", "IF Recycling IS Medium and RawMaterials IS Low and WasteDisposal IS High THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 13", "IF Recycling IS Medium and RawMaterials IS Medium and WasteDisposal IS Low THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 14", "IF Recycling IS Medium and RawMaterials IS Medium and WasteDisposal IS Medium THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 15", "IF Recycling IS Medium and RawMaterials IS Medium and WasteDisposal IS High THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 16", "IF Recycling IS Medium and RawMaterials IS High and WasteDisposal IS Low THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 17", "IF Recycling IS Medium and RawMaterials IS High and WasteDisposal IS Medium THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 18", "IF Recycling IS Medium and RawMaterials IS High and WasteDisposal IS High THEN ChemicalTreatment IS VeryHigh");
            IS.NewRule("Rule 19", "IF Recycling IS High and RawMaterials IS Low and WasteDisposal IS Low THEN ChemicalTreatment IS Low");
            IS.NewRule("Rule 20", "IF Recycling IS High and RawMaterials IS Low and WasteDisposal IS Medium THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 21", "IF Recycling IS High and RawMaterials IS Low and WasteDisposal IS High THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 22", "IF Recycling IS High and RawMaterials IS Medium and WasteDisposal IS Low THEN ChemicalTreatment IS Middle");
            IS.NewRule("Rule 23", "IF Recycling IS High and RawMaterials IS Medium and WasteDisposal IS Medium THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 24", "IF Recycling IS High and RawMaterials IS Medium and WasteDisposal IS High THEN ChemicalTreatment IS VeryHigh");
            IS.NewRule("Rule 25", "IF Recycling IS High and RawMaterials IS High and WasteDisposal IS Low THEN ChemicalTreatment IS High");
            IS.NewRule("Rule 26", "IF Recycling IS High and RawMaterials IS High and WasteDisposal IS Medium THEN ChemicalTreatment IS VeryHigh");
            IS.NewRule("Rule 27", "IF Recycling IS High and RawMaterials IS High and WasteDisposal IS High THEN ChemicalTreatment IS VeryHigh");

            IS.SetInput("Recycling", (float)recyclingValue);
            IS.SetInput("RawMaterials", (float)rawMaterialsValue);
            IS.SetInput("WasteDisposal", (float)wasteDisposalValue);

            double resultado = IS.Evaluate("ChemicalTreatment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Recycling", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RawMaterials", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WasteDisposal", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("ChemicalTreatment");
            }
            double m = (IS.GetLinguisticVariable("ChemicalTreatment").End - IS.GetLinguisticVariable("ChemicalTreatment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("ChemicalTreatment").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }
    }
}