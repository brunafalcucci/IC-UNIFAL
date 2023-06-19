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
    public class CoolingSystemController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ICoolingSystemRepository _CoolingSystemRepository;
        public CoolingSystemController(IConfiguration configuration, ICoolingSystemRepository CoolingSystemRepository)
        {
            Configuration = configuration;
            _CoolingSystemRepository = CoolingSystemRepository;
        }

        [HttpPost]
        public ActionResult<CoolingSystem> InsertCoolingSystem([FromBody] CoolingSystem coolingSystem)
        {
            try
            {
                coolingSystem.Management = CalculateManagement(Convert.ToDouble(coolingSystem.ChillerWaste), Convert.ToDouble(coolingSystem.Cleaning));
                coolingSystem.Performance = CalculatePerformance(Convert.ToDouble(coolingSystem.Air), Convert.ToDouble(coolingSystem.Pressure));
                coolingSystem.Condenser = CalculateCondenser(Convert.ToDouble(coolingSystem.Refrigeration), Convert.ToDouble(coolingSystem.Water));
                coolingSystem.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(coolingSystem.Heat), Convert.ToDouble(coolingSystem.Temperature));
                coolingSystem.SystemOperationCooling = CalculateSystemOperationCooling(Convert.ToDouble(coolingSystem.Performance), Convert.ToDouble(coolingSystem.Management));
                coolingSystem.HeatTransferCooling = CalculateHeatTransferCooling(Convert.ToDouble(coolingSystem.Thermodynamics), Convert.ToDouble(coolingSystem.Condenser));
                coolingSystem.CoolingSystemValue = CalculateCoolingSystem(Convert.ToDouble(coolingSystem.SystemOperationCooling), Convert.ToDouble(coolingSystem.HeatTransferCooling));

                return _CoolingSystemRepository.InsertCoolingSystem(coolingSystem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CoolingSystem> GetCoolingSystemById(int id)
        {
            try
            {
                return _CoolingSystemRepository.GetCoolingSystemById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<CoolingSystem>> GetCoolingSystemByIndustry(string industryName)
        {
            try
            {
                return _CoolingSystemRepository.GetCoolingSystemByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double chillerWasteValue, double cleaningValue)
        {
            LinguisticVariable chillerWaste = new( "ChillerWaste", 0, 10 );
            chillerWaste.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chillerWaste.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chillerWaste.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            chillerWaste.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chillerWaste.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "VeryDirty", new TrapezoidalFunction(0, 0, 1, 3) ) );
            cleaning.AddLabel( new FuzzySet( "Dirty", new TrapezoidalFunction(1, 3, 5) ) );
            cleaning.AddLabel( new FuzzySet( "MediumClean", new TrapezoidalFunction(3, 5, 7) ) );
            cleaning.AddLabel( new FuzzySet( "Clean", new TrapezoidalFunction(5, 7, 9) ) );
            cleaning.AddLabel( new FuzzySet( "VeryClean", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( chillerWaste );
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ChillerWaste IS VeryLow and Cleaning IS VeryDirty THEN Management IS VeryInadequate");
            IS.NewRule("Rule 2", "IF ChillerWaste IS VeryLow and Cleaning IS Dirty THEN Management IS VeryInadequate");
            IS.NewRule("Rule 3", "IF ChillerWaste IS VeryLow and Cleaning IS MediumClean THEN Management IS Inadequate");
            IS.NewRule("Rule 4", "IF ChillerWaste IS VeryLow and Cleaning IS Clean THEN Management IS Inadequate");
            IS.NewRule("Rule 5", "IF ChillerWaste IS VeryLow and Cleaning IS VeryClean THEN Management IS Acceptable");
            IS.NewRule("Rule 6", "IF ChillerWaste IS Low and Cleaning IS VeryDirty THEN Management IS VeryInadequate");
            IS.NewRule("Rule 7", "IF ChillerWaste IS Low and Cleaning IS Dirty THEN Management IS Inadequate");
            IS.NewRule("Rule 8", "IF ChillerWaste IS Low and Cleaning IS MediumClean THEN Management IS Inadequate");
            IS.NewRule("Rule 9", "IF ChillerWaste IS Low and Cleaning IS Clean THEN Management IS Acceptable");
            IS.NewRule("Rule 10", "IF ChillerWaste IS Low and Cleaning IS VeryClean THEN Management IS Adequate");
            IS.NewRule("Rule 11", "IF ChillerWaste IS Middle and Cleaning IS VeryDirty THEN Management IS Inadequate");
            IS.NewRule("Rule 12", "IF ChillerWaste IS Middle and Cleaning IS Dirty THEN Management IS Inadequate");
            IS.NewRule("Rule 13", "IF ChillerWaste IS Middle and Cleaning IS MediumClean THEN Management IS Acceptable");
            IS.NewRule("Rule 14", "IF ChillerWaste IS Middle and Cleaning IS Clean THEN Management IS Adequate");
            IS.NewRule("Rule 15", "IF ChillerWaste IS Middle and Cleaning IS VeryClean THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF ChillerWaste IS High and Cleaning IS VeryDirty THEN Management IS Inadequate");
            IS.NewRule("Rule 17", "IF ChillerWaste IS High and Cleaning IS Dirty THEN Management IS Acceptable");
            IS.NewRule("Rule 18", "IF ChillerWaste IS High and Cleaning IS MediumClean THEN Management IS Adequate");
            IS.NewRule("Rule 19", "IF ChillerWaste IS High and Cleaning IS Clean THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF ChillerWaste IS High and Cleaning IS VeryClean THEN Management IS VeryAdequate");
            IS.NewRule("Rule 21", "IF ChillerWaste IS VeryHigh and Cleaning IS VeryDirty THEN Management IS Acceptable");
            IS.NewRule("Rule 22", "IF ChillerWaste IS VeryHigh and Cleaning IS Dirty THEN Management IS Adequate");
            IS.NewRule("Rule 23", "IF ChillerWaste IS VeryHigh and Cleaning IS MediumClean THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF ChillerWaste IS VeryHigh and Cleaning IS Clean THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF ChillerWaste IS VeryHigh and Cleaning IS VeryClean THEN Management IS VeryAdequate");

            IS.SetInput("ChillerWaste", (float)chillerWasteValue);
            IS.SetInput("Cleaning", (float)cleaningValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ChillerWaste", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Cleaning", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Management");
            }
            double m = (IS.GetLinguisticVariable("Management").End - IS.GetLinguisticVariable("Management").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Management").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculatePerformance(double airValue, double pressureValue)
        {
            LinguisticVariable air = new( "Air", 0, 10 );
            air.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            air.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            air.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            air.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            air.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable pressure = new( "Pressure", 0, 10 );
            pressure.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            pressure.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            pressure.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            pressure.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            pressure.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( air );
            fuzzyDB.AddVariable( pressure );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Air IS VeryLow and Pressure IS VeryLow THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 2", "IF Air IS VeryLow and Pressure IS Low THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 3", "IF Air IS VeryLow and Pressure IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 4", "IF Air IS VeryLow and Pressure IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 5", "IF Air IS VeryLow and Pressure IS VeryHigh THEN Performance IS Acceptable");
            IS.NewRule("Rule 6", "IF Air IS Low and Pressure IS VeryLow THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 7", "IF Air IS Low and Pressure IS Low THEN Performance IS Adequate");
            IS.NewRule("Rule 8", "IF Air IS Low and Pressure IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 9", "IF Air IS Low and Pressure IS High THEN Performance IS Acceptable");
            IS.NewRule("Rule 10", "IF Air IS Low and Pressure IS VeryHigh THEN Performance IS Inadequate");
            IS.NewRule("Rule 11", "IF Air IS Middle and Pressure IS VeryLow THEN Performance IS Adequate");
            IS.NewRule("Rule 12", "IF Air IS Middle and Pressure IS Low THEN Performance IS Adequate");
            IS.NewRule("Rule 13", "IF Air IS Middle and Pressure IS Middle THEN Performance IS Acceptable");
            IS.NewRule("Rule 14", "IF Air IS Middle and Pressure IS High THEN Performance IS Inadequate");
            IS.NewRule("Rule 15", "IF Air IS Middle and Pressure IS VeryHigh THEN Performance IS Inadequate");
            IS.NewRule("Rule 16", "IF Air IS High and Pressure IS VeryLow THEN Performance IS Adequate");
            IS.NewRule("Rule 17", "IF Air IS High and Pressure IS Low THEN Performance IS Acceptable");
            IS.NewRule("Rule 18", "IF Air IS High and Pressure IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 19", "IF Air IS High and Pressure IS High THEN Performance IS Inadequate");
            IS.NewRule("Rule 20", "IF Air IS High and Pressure IS VeryHigh THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 21", "IF Air IS VeryHigh and Pressure IS VeryLow THEN Performance IS Acceptable");
            IS.NewRule("Rule 22", "IF Air IS VeryHigh and Pressure IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 23", "IF Air IS VeryHigh and Pressure IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 24", "IF Air IS VeryHigh and Pressure IS High THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 25", "IF Air IS VeryHigh and Pressure IS VeryHigh THEN Performance IS VeryInadequate");

            IS.SetInput("Air", (float)airValue);
            IS.SetInput("Pressure", (float)pressureValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Air", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Pressure", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Performance");
            }
            double m = (IS.GetLinguisticVariable("Performance").End - IS.GetLinguisticVariable("Performance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Performance").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculateCondenser(double refrigerationValue, double waterValue)
        {
            LinguisticVariable refrigeration = new( "Refrigeration", 0, 10 );
            refrigeration.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            refrigeration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            refrigeration.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            refrigeration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            refrigeration.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable water = new( "Water", 0, 10 );
            water.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            water.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            water.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            water.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            water.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable condenser = new( "Condenser", 0, 10 );
            condenser.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            condenser.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            condenser.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            condenser.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            condenser.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( refrigeration );
            fuzzyDB.AddVariable( water );
            fuzzyDB.AddVariable( condenser );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Refrigeration IS VeryLow and Water IS VeryLow THEN Condenser IS VeryAdequate");
            IS.NewRule("Rule 2", "IF Refrigeration IS VeryLow and Water IS Low THEN Condenser IS VeryAdequate");
            IS.NewRule("Rule 3", "IF Refrigeration IS VeryLow and Water IS Middle THEN Condenser IS Adequate");
            IS.NewRule("Rule 4", "IF Refrigeration IS VeryLow and Water IS High THEN Condenser IS Adequate");
            IS.NewRule("Rule 5", "IF Refrigeration IS VeryLow and Water IS VeryHigh THEN Condenser IS Acceptable");
            IS.NewRule("Rule 6", "IF Refrigeration IS Low and Water IS VeryLow THEN Condenser IS VeryAdequate");
            IS.NewRule("Rule 7", "IF Refrigeration IS Low and Water IS Low THEN Condenser IS Adequate");
            IS.NewRule("Rule 8", "IF Refrigeration IS Low and Water IS Middle THEN Condenser IS Adequate");
            IS.NewRule("Rule 9", "IF Refrigeration IS Low and Water IS High THEN Condenser IS Acceptable");
            IS.NewRule("Rule 10", "IF Refrigeration IS Low and Water IS VeryHigh THEN Condenser IS Inadequate");
            IS.NewRule("Rule 11", "IF Refrigeration IS Middle and Water IS VeryLow THEN Condenser IS Adequate");
            IS.NewRule("Rule 12", "IF Refrigeration IS Middle and Water IS Low THEN Condenser IS Adequate");
            IS.NewRule("Rule 13", "IF Refrigeration IS Middle and Water IS Middle THEN Condenser IS Acceptable");
            IS.NewRule("Rule 14", "IF Refrigeration IS Middle and Water IS High THEN Condenser IS Inadequate");
            IS.NewRule("Rule 15", "IF Refrigeration IS Middle and Water IS VeryHigh THEN Condenser IS Inadequate");
            IS.NewRule("Rule 16", "IF Refrigeration IS High and Water IS VeryLow THEN Condenser IS Adequate");
            IS.NewRule("Rule 17", "IF Refrigeration IS High and Water IS Low THEN Condenser IS Acceptable");
            IS.NewRule("Rule 18", "IF Refrigeration IS High and Water IS Middle THEN Condenser IS Inadequate");
            IS.NewRule("Rule 19", "IF Refrigeration IS High and Water IS High THEN Condenser IS Inadequate");
            IS.NewRule("Rule 20", "IF Refrigeration IS High and Water IS VeryHigh THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 21", "IF Refrigeration IS VeryHigh and Water IS VeryLow THEN Condenser IS Acceptable");
            IS.NewRule("Rule 22", "IF Refrigeration IS VeryHigh and Water IS Low THEN Condenser IS Inadequate");
            IS.NewRule("Rule 23", "IF Refrigeration IS VeryHigh and Water IS Middle THEN Condenser IS Inadequate");
            IS.NewRule("Rule 24", "IF Refrigeration IS VeryHigh and Water IS High THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 25", "IF Refrigeration IS VeryHigh and Water IS VeryHigh THEN Condenser IS VeryInadequate");

            IS.SetInput("Refrigeration", (float)refrigerationValue);
            IS.SetInput("Water", (float)waterValue);

            double resultado = IS.Evaluate("Condenser");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Refrigeration", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Water", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Condenser");
            }
            double m = (IS.GetLinguisticVariable("Condenser").End - IS.GetLinguisticVariable("Condenser").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Condenser").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculateThermodynamics(double heatValue, double temperatureValue)
        {
            LinguisticVariable heat = new( "Heat", 0, 10 );
            heat.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            heat.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            heat.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            heat.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            heat.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable temperature = new( "Temperature", 0, 10 );
            temperature.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            temperature.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            temperature.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            temperature.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            temperature.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( heat );
            fuzzyDB.AddVariable( temperature );
            fuzzyDB.AddVariable( thermodynamics );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Heat IS VeryLow and Temperature IS VeryLow THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 2", "IF Heat IS VeryLow and Temperature IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 3", "IF Heat IS VeryLow and Temperature IS Middle THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 4", "IF Heat IS VeryLow and Temperature IS High THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 5", "IF Heat IS VeryLow and Temperature IS VeryHigh THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 6", "IF Heat IS Low and Temperature IS VeryLow THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 7", "IF Heat IS Low and Temperature IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 8", "IF Heat IS Low and Temperature IS Middle THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 9", "IF Heat IS Low and Temperature IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 10", "IF Heat IS Low and Temperature IS VeryHigh THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Heat IS Middle and Temperature IS VeryLow THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 12", "IF Heat IS Middle and Temperature IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 13", "IF Heat IS Middle and Temperature IS Middle THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 14", "IF Heat IS Middle and Temperature IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 15", "IF Heat IS Middle and Temperature IS VeryHigh THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 16", "IF Heat IS High and Temperature IS VeryLow THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Heat IS High and Temperature IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 18", "IF Heat IS High and Temperature IS Middle THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 19", "IF Heat IS High and Temperature IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 20", "IF Heat IS High and Temperature IS VeryHigh THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 21", "IF Heat IS VeryHigh and Temperature IS VeryLow THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Heat IS VeryHigh and Temperature IS Low THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Heat IS VeryHigh and Temperature IS Middle THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 24", "IF Heat IS VeryHigh and Temperature IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 25", "IF Heat IS VeryHigh and Temperature IS VeryHigh THEN Thermodynamics IS Acceptable");

            IS.SetInput("Heat", (float)heatValue);
            IS.SetInput("Temperature", (float)temperatureValue);

            double resultado = IS.Evaluate("Thermodynamics");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Heat", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Temperature", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Thermodynamics");
            }
            double m = (IS.GetLinguisticVariable("Thermodynamics").End - IS.GetLinguisticVariable("Thermodynamics").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Thermodynamics").Start;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculateSystemOperationCooling(double performanceValue, double managementValue)
        {
            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable systemOperationCooling = new( "SystemOperationCooling", 0, 10 );
            systemOperationCooling.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( systemOperationCooling );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Performance IS VeryInadequate and Management IS VeryInadequate THEN SystemOperationCooling IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Performance IS VeryInadequate and Management IS Inadequate THEN SystemOperationCooling IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Performance IS VeryInadequate and Management IS Acceptable THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 4", "IF Performance IS VeryInadequate and Management IS Adequate THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 5", "IF Performance IS VeryInadequate and Management IS VeryAdequate THEN SystemOperationCooling IS Acceptable");
            IS.NewRule("Rule 6", "IF Performance IS Inadequate and Management IS VeryInadequate THEN SystemOperationCooling IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Performance IS Inadequate and Management IS Inadequate THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 8", "IF Performance IS Inadequate and Management IS Acceptable THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 9", "IF Performance IS Inadequate and Management IS Adequate THEN SystemOperationCooling IS Acceptable");
            IS.NewRule("Rule 10", "IF Performance IS Inadequate and Management IS VeryAdequate THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 11", "IF Performance IS Acceptable and Management IS VeryInadequate THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 12", "IF Performance IS Acceptable and Management IS Inadequate THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 13", "IF Performance IS Acceptable and Management IS Acceptable THEN SystemOperationCooling IS Acceptable");
            IS.NewRule("Rule 14", "IF Performance IS Acceptable and Management IS Adequate THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 15", "IF Performance IS Acceptable and Management IS VeryAdequate THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 16", "IF Performance IS Adequate and Management IS VeryInadequate THEN SystemOperationCooling IS Inadequate");
            IS.NewRule("Rule 17", "IF Performance IS Adequate and Management IS Inadequate THEN SystemOperationCooling IS Acceptable");
            IS.NewRule("Rule 18", "IF Performance IS Adequate and Management IS Acceptable THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 19", "IF Performance IS Adequate and Management IS Adequate THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 20", "IF Performance IS Adequate and Management IS VeryAdequate THEN SystemOperationCooling IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Performance IS VeryAdequate and Management IS VeryInadequate THEN SystemOperationCooling IS Acceptable");
            IS.NewRule("Rule 22", "IF Performance IS VeryAdequate and Management IS Inadequate THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 23", "IF Performance IS VeryAdequate and Management IS Acceptable THEN SystemOperationCooling IS Adequate");
            IS.NewRule("Rule 24", "IF Performance IS VeryAdequate and Management IS Adequate THEN SystemOperationCooling IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Performance IS VeryAdequate and Management IS VeryAdequate THEN SystemOperationCooling IS VeryAdequate");

            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Management", (float)managementValue);

            double resultado = IS.Evaluate("SystemOperationCooling");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("SystemOperationCooling");
            }
            double m = (IS.GetLinguisticVariable("SystemOperationCooling").End - IS.GetLinguisticVariable("SystemOperationCooling").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("SystemOperationCooling").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculateHeatTransferCooling(double thermodynamicsValue, double condenserValue)
        {
            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable condenser = new( "Condenser", 0, 10 );
            condenser.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            condenser.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            condenser.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            condenser.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            condenser.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable heatTransferCooling = new( "HeatTransferCooling", 0, 10 );
            heatTransferCooling.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( condenser );
            fuzzyDB.AddVariable( heatTransferCooling );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Thermodynamics IS VeryInadequate and Condenser IS VeryInadequate THEN HeatTransferCooling IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Thermodynamics IS VeryInadequate and Condenser IS Inadequate THEN HeatTransferCooling IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Thermodynamics IS VeryInadequate and Condenser IS Acceptable THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 4", "IF Thermodynamics IS VeryInadequate and Condenser IS Adequate THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 5", "IF Thermodynamics IS VeryInadequate and Condenser IS VeryAdequate THEN HeatTransferCooling IS Acceptable");
            IS.NewRule("Rule 6", "IF Thermodynamics IS Inadequate and Condenser IS VeryInadequate THEN HeatTransferCooling IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Thermodynamics IS Inadequate and Condenser IS Inadequate THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 8", "IF Thermodynamics IS Inadequate and Condenser IS Acceptable THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 9", "IF Thermodynamics IS Inadequate and Condenser IS Adequate THEN HeatTransferCooling IS Acceptable");
            IS.NewRule("Rule 10", "IF Thermodynamics IS Inadequate and Condenser IS VeryAdequate THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 11", "IF Thermodynamics IS Acceptable and Condenser IS VeryInadequate THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 12", "IF Thermodynamics IS Acceptable and Condenser IS Inadequate THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 13", "IF Thermodynamics IS Acceptable and Condenser IS Acceptable THEN HeatTransferCooling IS Acceptable");
            IS.NewRule("Rule 14", "IF Thermodynamics IS Acceptable and Condenser IS Adequate THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 15", "IF Thermodynamics IS Acceptable and Condenser IS VeryAdequate THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 16", "IF Thermodynamics IS Adequate and Condenser IS VeryInadequate THEN HeatTransferCooling IS Inadequate");
            IS.NewRule("Rule 17", "IF Thermodynamics IS Adequate and Condenser IS Inadequate THEN HeatTransferCooling IS Acceptable");
            IS.NewRule("Rule 18", "IF Thermodynamics IS Adequate and Condenser IS Acceptable THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 19", "IF Thermodynamics IS Adequate and Condenser IS Adequate THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 20", "IF Thermodynamics IS Adequate and Condenser IS VeryAdequate THEN HeatTransferCooling IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Thermodynamics IS VeryAdequate and Condenser IS VeryInadequate THEN HeatTransferCooling IS Acceptable");
            IS.NewRule("Rule 22", "IF Thermodynamics IS VeryAdequate and Condenser IS Inadequate THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 23", "IF Thermodynamics IS VeryAdequate and Condenser IS Acceptable THEN HeatTransferCooling IS Adequate");
            IS.NewRule("Rule 24", "IF Thermodynamics IS VeryAdequate and Condenser IS Adequate THEN HeatTransferCooling IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Thermodynamics IS VeryAdequate and Condenser IS VeryAdequate THEN HeatTransferCooling IS VeryAdequate");

            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);
            IS.SetInput("Condenser", (float)condenserValue);

            double resultado = IS.Evaluate("HeatTransferCooling");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Condenser", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("HeatTransferCooling");
            }
            double m = (IS.GetLinguisticVariable("HeatTransferCooling").End - IS.GetLinguisticVariable("HeatTransferCooling").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("HeatTransferCooling").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }

        public string CalculateCoolingSystem(double systemOperationCoolingValue, double heatTransferCoolingValue)
        {
            LinguisticVariable systemOperationCooling = new( "SystemOperationCooling", 0, 10 );
            systemOperationCooling.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            systemOperationCooling.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable heatTransferCooling = new( "HeatTransferCooling", 0, 10 );
            heatTransferCooling.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            heatTransferCooling.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable coolingSystem = new( "CoolingSystem", 0, 10 );
            coolingSystem.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            coolingSystem.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            coolingSystem.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            coolingSystem.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            coolingSystem.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( systemOperationCooling );
            fuzzyDB.AddVariable( heatTransferCooling );
            fuzzyDB.AddVariable( coolingSystem );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SystemOperationCooling IS VeryInadequate and HeatTransferCooling IS VeryInadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 2", "IF SystemOperationCooling IS VeryInadequate and HeatTransferCooling IS Inadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 3", "IF SystemOperationCooling IS VeryInadequate and HeatTransferCooling IS Acceptable THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 4", "IF SystemOperationCooling IS VeryInadequate and HeatTransferCooling IS Adequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 5", "IF SystemOperationCooling IS VeryInadequate and HeatTransferCooling IS VeryAdequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 6", "IF SystemOperationCooling IS Inadequate and HeatTransferCooling IS VeryInadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 7", "IF SystemOperationCooling IS Inadequate and HeatTransferCooling IS Inadequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 8", "IF SystemOperationCooling IS Inadequate and HeatTransferCooling IS Acceptable THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 9", "IF SystemOperationCooling IS Inadequate and HeatTransferCooling IS Adequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 10", "IF SystemOperationCooling IS Inadequate and HeatTransferCooling IS VeryAdequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 11", "IF SystemOperationCooling IS Acceptable and HeatTransferCooling IS VeryInadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 12", "IF SystemOperationCooling IS Acceptable and HeatTransferCooling IS Inadequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 13", "IF SystemOperationCooling IS Acceptable and HeatTransferCooling IS Acceptable THEN CoolingSystem IS Acceptable");
            IS.NewRule("Rule 14", "IF SystemOperationCooling IS Acceptable and HeatTransferCooling IS Adequate THEN CoolingSystem IS Acceptable");
            IS.NewRule("Rule 15", "IF SystemOperationCooling IS Acceptable and HeatTransferCooling IS VeryAdequate THEN CoolingSystem IS Acceptable");
            IS.NewRule("Rule 16", "IF SystemOperationCooling IS Adequate and HeatTransferCooling IS VeryInadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 17", "IF SystemOperationCooling IS Adequate and HeatTransferCooling IS Inadequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 18", "IF SystemOperationCooling IS Adequate and HeatTransferCooling IS Acceptable THEN CoolingSystem IS Acceptable");
            IS.NewRule("Rule 19", "IF SystemOperationCooling IS Adequate and HeatTransferCooling IS Adequate THEN CoolingSystem IS Adequate");
            IS.NewRule("Rule 20", "IF SystemOperationCooling IS Adequate and HeatTransferCooling IS VeryAdequate THEN CoolingSystem IS Adequate");
            IS.NewRule("Rule 21", "IF SystemOperationCooling IS VeryAdequate and HeatTransferCooling IS VeryInadequate THEN CoolingSystem IS VeryInadequate");
            IS.NewRule("Rule 22", "IF SystemOperationCooling IS VeryAdequate and HeatTransferCooling IS Inadequate THEN CoolingSystem IS Inadequate");
            IS.NewRule("Rule 23", "IF SystemOperationCooling IS VeryAdequate and HeatTransferCooling IS Acceptable THEN CoolingSystem IS Acceptable");
            IS.NewRule("Rule 24", "IF SystemOperationCooling IS VeryAdequate and HeatTransferCooling IS Adequate THEN CoolingSystem IS Adequate");
            IS.NewRule("Rule 25", "IF SystemOperationCooling IS VeryAdequate and HeatTransferCooling IS VeryAdequate THEN CoolingSystem IS VeryAdequate");

            IS.SetInput("SystemOperationCooling", (float)systemOperationCoolingValue);
            IS.SetInput("HeatTransferCooling", (float)heatTransferCoolingValue);

            double resultado = IS.Evaluate("CoolingSystem");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SystemOperationCooling", i == 0 ? 0 : (float)9.99);
                IS.SetInput("HeatTransferCooling", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("CoolingSystem");
            }
            double m = (IS.GetLinguisticVariable("CoolingSystem").End - IS.GetLinguisticVariable("CoolingSystem").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("CoolingSystem").End;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inadequate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inadequate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Adequate";
            }
            else
            {
                return resultado.ToString() + " - Very Adequate";
            }
        }
    }
}