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
    public class AirCompressorController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IAirCompressorRepository _AirCompressorRepository;
        public AirCompressorController(IConfiguration configuration, IAirCompressorRepository AirCompressorRepository)
        {
            Configuration = configuration;
            _AirCompressorRepository = AirCompressorRepository;
        }

        [HttpPost]
        public ActionResult<AirCompressor> InsertAirCompressor([FromBody] AirCompressor airCompressor)
        {
            try
            {
                airCompressor.Performance = CalculatePerformance(Convert.ToDouble(airCompressor.Use), Convert.ToDouble(airCompressor.Local));
                airCompressor.Management = CalculateManagement(Convert.ToDouble(airCompressor.Cleaning), Convert.ToDouble(airCompressor.Moisture), Convert.ToDouble(airCompressor.Maintenance));
                airCompressor.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(airCompressor.Temperature), Convert.ToDouble(airCompressor.InletPressureControl));
                airCompressor.AirCompressorValue = CalculateAirCompressor(Convert.ToDouble(airCompressor.Management), Convert.ToDouble(airCompressor.Thermodynamics), Convert.ToDouble(airCompressor.Performance));

                return _AirCompressorRepository.InsertAirCompressor(airCompressor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AirCompressor> GetAirCompressorById(int id)
        {
            try
            {
                return _AirCompressorRepository.GetAirCompressorById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<AirCompressor>> GetAirCompressorByIndustry(string industryName)
        {
            try
            {
                return _AirCompressorRepository.GetAirCompressorByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculatePerformance(double useValue, double localValue)
        {
            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            use.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            use.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable local = new( "Local", 0, 10 );
            local.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            local.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            local.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            local.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            local.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( use );
            fuzzyDB.AddVariable( local );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Use IS VeryLow and Local IS VeryLow THEN Performance IS Acceptable");
            IS.NewRule("Rule 2", "IF Use IS VeryLow and Local IS Low THEN Performance IS Adequate");
            IS.NewRule("Rule 3", "IF Use IS VeryLow and Local IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 4", "IF Use IS VeryLow and Local IS High THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 5", "IF Use IS VeryLow and Local IS VeryHigh THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 6", "IF Use IS Low and Local IS VeryLow THEN Performance IS Inadequate");
            IS.NewRule("Rule 7", "IF Use IS Low and Local IS Low THEN Performance IS Acceptable");
            IS.NewRule("Rule 8", "IF Use IS Low and Local IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 9", "IF Use IS Low and Local IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 10", "IF Use IS Low and Local IS VeryHigh THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Use IS Middle and Local IS VeryLow THEN Performance IS Inadequate");
            IS.NewRule("Rule 12", "IF Use IS Middle and Local IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 13", "IF Use IS Middle and Local IS Middle THEN Performance IS Acceptable");
            IS.NewRule("Rule 14", "IF Use IS Middle and Local IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 15", "IF Use IS Middle and Local IS VeryHigh THEN Performance IS Adequate");
            IS.NewRule("Rule 16", "IF Use IS High and Local IS VeryLow THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Use IS High and Local IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 18", "IF Use IS High and Local IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 19", "IF Use IS High and Local IS High THEN Performance IS Acceptable");
            IS.NewRule("Rule 20", "IF Use IS High and Local IS VeryHigh THEN Performance IS Adequate");
            IS.NewRule("Rule 21", "IF Use IS VeryHigh and Local IS VeryLow THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Use IS VeryHigh and Local IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Use IS VeryHigh and Local IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 24", "IF Use IS VeryHigh and Local IS High THEN Performance IS Inadequate");
            IS.NewRule("Rule 25", "IF Use IS VeryHigh and Local IS VeryHigh THEN Performance IS Acceptable");

            IS.SetInput("Use", (float)useValue);
            IS.SetInput("Local", (float)localValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Use", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Local", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Performance");
            }
            double m = (IS.GetLinguisticVariable("Performance").End - IS.GetLinguisticVariable("Performance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Performance").Start;
            
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

        public string CalculateManagement(double cleaningValue, double moistureValue, double maintenanceValue)
        {
            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "Dirty", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            cleaning.AddLabel( new FuzzySet( "MediumClean", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            cleaning.AddLabel( new FuzzySet( "Clean", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable moisture = new( "Moisture", 0, 10 );
            moisture.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            moisture.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            moisture.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( moisture );
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Cleaning IS Dirty and Moisture IS Low and Maintenance IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Cleaning IS Dirty and Moisture IS Low and Maintenance IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 3", "IF Cleaning IS Dirty and Moisture IS Low and Maintenance IS High THEN Management IS Adequate");
            IS.NewRule("Rule 4", "IF Cleaning IS Dirty and Moisture IS Middle and Maintenance IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Cleaning IS Dirty and Moisture IS Middle and Maintenance IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 6", "IF Cleaning IS Dirty and Moisture IS Middle and Maintenance IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 7", "IF Cleaning IS Dirty and Moisture IS High and Maintenance IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Cleaning IS Dirty and Moisture IS High and Maintenance IS Medium THEN Management IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Cleaning IS Dirty and Moisture IS High and Maintenance IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 10", "IF Cleaning IS MediumClean and Moisture IS Low and Maintenance IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 11", "IF Cleaning IS MediumClean and Moisture IS Low and Maintenance IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 12", "IF Cleaning IS MediumClean and Moisture IS Low and Maintenance IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 13", "IF Cleaning IS MediumClean and Moisture IS Middle and Maintenance IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 14", "IF Cleaning IS MediumClean and Moisture IS Middle and Maintenance IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 15", "IF Cleaning IS MediumClean and Moisture IS Middle and Maintenance IS High THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF Cleaning IS MediumClean and Moisture IS High and Maintenance IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Cleaning IS MediumClean and Moisture IS High and Maintenance IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 18", "IF Cleaning IS MediumClean and Moisture IS High and Maintenance IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 19", "IF Cleaning IS Clean and Moisture IS Low and Maintenance IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF Cleaning IS Clean and Moisture IS Low and Maintenance IS Medium THEN Management IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Cleaning IS Clean and Moisture IS Low and Maintenance IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 22", "IF Cleaning IS Clean and Moisture IS Middle and Maintenance IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 23", "IF Cleaning IS Clean and Moisture IS Middle and Maintenance IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF Cleaning IS Clean and Moisture IS Middle and Maintenance IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Cleaning IS Clean and Moisture IS High and Maintenance IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 26", "IF Cleaning IS Clean and Moisture IS High and Maintenance IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 27", "IF Cleaning IS Clean and Moisture IS High and Maintenance IS High THEN Management IS Adequate");

            IS.SetInput("Cleaning", (float)cleaningValue);
            IS.SetInput("Moisture", (float)moistureValue);
            IS.SetInput("Maintenance", (float)maintenanceValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Cleaning", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Moisture", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Maintenance", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Management");
            }
            double m = (IS.GetLinguisticVariable("Management").End - IS.GetLinguisticVariable("Management").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Management").Start;

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

        public string CalculateThermodynamics(double temperatureValue, double inletPressureControlValue)
        {
            LinguisticVariable temperature = new( "Temperature", 0, 10 );
            temperature.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            temperature.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            temperature.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            temperature.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            temperature.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable inletPressureControl = new( "InletPressureControl", 0, 10 );
            inletPressureControl.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            inletPressureControl.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            inletPressureControl.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            inletPressureControl.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            inletPressureControl.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( temperature );
            fuzzyDB.AddVariable( inletPressureControl );
            fuzzyDB.AddVariable( thermodynamics );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Temperature IS VeryLow and InletPressureControl IS VeryLow THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 2", "IF Temperature IS VeryLow and InletPressureControl IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 3", "IF Temperature IS VeryLow and InletPressureControl IS Middle THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 4", "IF Temperature IS VeryLow and InletPressureControl IS High THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 5", "IF Temperature IS VeryLow and InletPressureControl IS VeryHigh THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 6", "IF Temperature IS Low and InletPressureControl IS VeryLow THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 7", "IF Temperature IS Low and InletPressureControl IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 8", "IF Temperature IS Low and InletPressureControl IS Middle THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 9", "IF Temperature IS Low and InletPressureControl IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 10", "IF Temperature IS Low and InletPressureControl IS VeryHigh THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Temperature IS Middle and InletPressureControl IS VeryLow THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 12", "IF Temperature IS Middle and InletPressureControl IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 13", "IF Temperature IS Middle and InletPressureControl IS Middle THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 14", "IF Temperature IS Middle and InletPressureControl IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 15", "IF Temperature IS Middle and InletPressureControl IS VeryHigh THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 16", "IF Temperature IS High and InletPressureControl IS VeryLow THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Temperature IS High and InletPressureControl IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 18", "IF Temperature IS High and InletPressureControl IS Middle THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 19", "IF Temperature IS High and InletPressureControl IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 20", "IF Temperature IS High and InletPressureControl IS VeryHigh THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 21", "IF Temperature IS VeryHigh and InletPressureControl IS VeryLow THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Temperature IS VeryHigh and InletPressureControl IS Low THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Temperature IS VeryHigh and InletPressureControl IS Middle THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 24", "IF Temperature IS VeryHigh and InletPressureControl IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 25", "IF Temperature IS VeryHigh and InletPressureControl IS VeryHigh THEN Thermodynamics IS Acceptable");

            IS.SetInput("Temperature", (float)temperatureValue);
            IS.SetInput("InletPressureControl", (float)inletPressureControlValue);

            double resultado = IS.Evaluate("Thermodynamics");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Temperature", i == 0 ? (float)9.99 : 0);
                IS.SetInput("InletPressureControl", i == 0 ? 0 : (float)9.99);
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

        public string CalculateAirCompressor(double managementValue, double thermodynamicsValue, double performanceValue)
        {
            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable airCompressor = new( "AirCompressor", 0, 10 );
            airCompressor.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            airCompressor.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            airCompressor.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            airCompressor.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            airCompressor.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( airCompressor );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 10", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 11", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 13", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 14", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 15", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 16", "IF Management IS VeryInadequate and Thermodynamics IS Adequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Management IS VeryInadequate and Thermodynamics IS Adequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 18", "IF Management IS VeryInadequate and Thermodynamics IS Adequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 19", "IF Management IS VeryInadequate and Thermodynamics IS Adequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 20", "IF Management IS VeryInadequate and Thermodynamics IS Adequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 21", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 24", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 25", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 26", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 27", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 28", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 29", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 30", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 31", "IF Management IS Inadequate and Thermodynamics IS Inadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 32", "IF Management IS Inadequate and Thermodynamics IS Inadequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 33", "IF Management IS Inadequate and Thermodynamics IS Inadequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 34", "IF Management IS Inadequate and Thermodynamics IS Inadequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 35", "IF Management IS Inadequate and Thermodynamics IS Inadequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 36", "IF Management IS Inadequate and Thermodynamics IS Acceptable and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 37", "IF Management IS Inadequate and Thermodynamics IS Acceptable and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 38", "IF Management IS Inadequate and Thermodynamics IS Acceptable and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 39", "IF Management IS Inadequate and Thermodynamics IS Acceptable and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 40", "IF Management IS Inadequate and Thermodynamics IS Acceptable and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 41", "IF Management IS Inadequate and Thermodynamics IS Adequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 42", "IF Management IS Inadequate and Thermodynamics IS Adequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 43", "IF Management IS Inadequate and Thermodynamics IS Adequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 44", "IF Management IS Inadequate and Thermodynamics IS Adequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 45", "IF Management IS Inadequate and Thermodynamics IS Adequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 46", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 47", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 48", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 49", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 50", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 51", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 52", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 53", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 54", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 55", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 56", "IF Management IS Acceptable and Thermodynamics IS Inadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 57", "IF Management IS Acceptable and Thermodynamics IS Inadequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 58", "IF Management IS Acceptable and Thermodynamics IS Inadequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 59", "IF Management IS Acceptable and Thermodynamics IS Inadequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 60", "IF Management IS Acceptable and Thermodynamics IS Inadequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 61", "IF Management IS Acceptable and Thermodynamics IS Acceptable and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 62", "IF Management IS Acceptable and Thermodynamics IS Acceptable and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 63", "IF Management IS Acceptable and Thermodynamics IS Acceptable and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 64", "IF Management IS Acceptable and Thermodynamics IS Acceptable and Performance IS Adequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 65", "IF Management IS Acceptable and Thermodynamics IS Acceptable and Performance IS VeryAdequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 66", "IF Management IS Acceptable and Thermodynamics IS Adequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 67", "IF Management IS Acceptable and Thermodynamics IS Adequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 68", "IF Management IS Acceptable and Thermodynamics IS Adequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 69", "IF Management IS Acceptable and Thermodynamics IS Adequate and Performance IS Adequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 70", "IF Management IS Acceptable and Thermodynamics IS Adequate and Performance IS VeryAdequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 71", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 72", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 73", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 74", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate and Performance IS Adequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 75", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate and Performance IS VeryAdequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 76", "IF Management IS Adequate and Thermodynamics IS VeryInadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 77", "IF Management IS Adequate and Thermodynamics IS VeryInadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 78", "IF Management IS Adequate and Thermodynamics IS VeryInadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 79", "IF Management IS Adequate and Thermodynamics IS VeryInadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 80", "IF Management IS Adequate and Thermodynamics IS VeryInadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 81", "IF Management IS Adequate and Thermodynamics IS Inadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 82", "IF Management IS Adequate and Thermodynamics IS Inadequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 83", "IF Management IS Adequate and Thermodynamics IS Inadequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 84", "IF Management IS Adequate and Thermodynamics IS Inadequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 85", "IF Management IS Adequate and Thermodynamics IS Inadequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 86", "IF Management IS Adequate and Thermodynamics IS Acceptable and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 87", "IF Management IS Adequate and Thermodynamics IS Acceptable and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 88", "IF Management IS Adequate and Thermodynamics IS Acceptable and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 89", "IF Management IS Adequate and Thermodynamics IS Acceptable and Performance IS Adequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 90", "IF Management IS Adequate and Thermodynamics IS Acceptable and Performance IS VeryAdequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 91", "IF Management IS Adequate and Thermodynamics IS Adequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 92", "IF Management IS Adequate and Thermodynamics IS Adequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 93", "IF Management IS Adequate and Thermodynamics IS Adequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 94", "IF Management IS Adequate and Thermodynamics IS Adequate and Performance IS Adequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 95", "IF Management IS Adequate and Thermodynamics IS Adequate and Performance IS VeryAdequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 96", "IF Management IS Adequate and Thermodynamics IS VeryAdequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 97", "IF Management IS Adequate and Thermodynamics IS VeryAdequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 98", "IF Management IS Adequate and Thermodynamics IS VeryAdequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 99", "IF Management IS Adequate and Thermodynamics IS VeryAdequate and Performance IS Adequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 100", "IF Management IS Adequate and Thermodynamics IS VeryAdequate and Performance IS VeryAdequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 101", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 102", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate and Performance IS Inadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 103", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate and Performance IS Acceptable THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 104", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate and Performance IS Adequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 105", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate and Performance IS VeryAdequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 106", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 107", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 108", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate and Performance IS Acceptable THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 109", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate and Performance IS Adequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 110", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate and Performance IS VeryAdequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 111", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 112", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 113", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 114", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable and Performance IS Adequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 115", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable and Performance IS VeryAdequate THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 116", "IF Management IS VeryAdequate and Thermodynamics IS Adequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 117", "IF Management IS VeryAdequate and Thermodynamics IS Adequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 118", "IF Management IS VeryAdequate and Thermodynamics IS Adequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 119", "IF Management IS VeryAdequate and Thermodynamics IS Adequate and Performance IS Adequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 120", "IF Management IS VeryAdequate and Thermodynamics IS Adequate and Performance IS VeryAdequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 121", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate and Performance IS VeryInadequate THEN AirCompressor IS VeryInadequate");
            IS.NewRule("Rule 122", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate and Performance IS Inadequate THEN AirCompressor IS Inadequate");
            IS.NewRule("Rule 123", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate and Performance IS Acceptable THEN AirCompressor IS Acceptable");
            IS.NewRule("Rule 124", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate and Performance IS Adequate THEN AirCompressor IS Adequate");
            IS.NewRule("Rule 125", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate and Performance IS VeryAdequate THEN AirCompressor IS VeryAdequate");

            IS.SetInput("Management", (float)managementValue);
            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);
            IS.SetInput("Performance", (float)performanceValue);

            double resultado = IS.Evaluate("AirCompressor");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("AirCompressor");
            }
            double m = (IS.GetLinguisticVariable("AirCompressor").End - IS.GetLinguisticVariable("AirCompressor").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("AirCompressor").End;

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