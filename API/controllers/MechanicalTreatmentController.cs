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
    public class MechanicalTreatmentController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IMechanicalTreatmentRepository _MechanicalTreatmentRepository;
        public MechanicalTreatmentController(IConfiguration configuration, IMechanicalTreatmentRepository MechanicalTreatmentRepository)
        {
            Configuration = configuration;
            _MechanicalTreatmentRepository = MechanicalTreatmentRepository;
        }

        [HttpPost]
        public ActionResult<MechanicalTreatment> InsertMechanicalTreatment([FromBody] MechanicalTreatment mechanicalTreatment)
        {
            try
            {

                return _MechanicalTreatmentRepository.InsertMechanicalTreatment(mechanicalTreatment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MechanicalTreatment> GetMechanicalTreatmentById(int id)
        {
            try
            {
                return _MechanicalTreatmentRepository.GetMechanicalTreatmentById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<MechanicalTreatment>> GetMechanicalTreatmentByIndustry(string industryName)
        {
            try
            {
                return _MechanicalTreatmentRepository.GetMechanicalTreatmentByIndustry(industryName);
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
    }
}