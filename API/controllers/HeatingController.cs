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
    public class HeatingController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IHeatingRepository _HeatingRepository;
        public HeatingController(IConfiguration configuration, IHeatingRepository HeatingRepository)
        {
            Configuration = configuration;
            _HeatingRepository = HeatingRepository;
        }

        [HttpPost]
        public ActionResult<Heating> InsertHeating([FromBody] Heating heating)
        {
            try
            {
                heating.Performance = CalculatePerformance(Convert.ToDouble(heating.Use), Convert.ToDouble(heating.Place));
                heating.Management = CalculateManagement(Convert.ToDouble(heating.AirType), Convert.ToDouble(heating.Isolation), Convert.ToDouble(heating.Inspection));
                heating.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(heating.Heat), Convert.ToDouble(heating.Temperature), Convert.ToDouble(heating.Fluid));
                heating.HeatingValue = CalculateHeating(Convert.ToDouble(heating.Management), Convert.ToDouble(heating.Performance), Convert.ToDouble(heating.Thermodynamics));

                return _HeatingRepository.InsertHeating(heating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Heating> GetHeatingById(int id)
        {
            try
            {
                return _HeatingRepository.GetHeatingById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<Heating>> GetHeatingByIndustry(string industryName)
        {
            try
            {
                return _HeatingRepository.GetHeatingByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculatePerformance(double useValue, double placeValue)
        {
            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            use.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            use.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable place = new( "Place", 0, 10 );
            place.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            place.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            place.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            place.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            place.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( use );
            fuzzyDB.AddVariable( place );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Use IS VeryLow and Place IS VeryLow THEN Performance IS Acceptable");
            IS.NewRule("Rule 2", "IF Use IS VeryLow and Place IS Low THEN Performance IS Adequate");
            IS.NewRule("Rule 3", "IF Use IS VeryLow and Place IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 4", "IF Use IS VeryLow and Place IS High THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 5", "IF Use IS VeryLow and Place IS VeryHigh THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 6", "IF Use IS Low and Place IS VeryLow THEN Performance IS Inadequate");
            IS.NewRule("Rule 7", "IF Use IS Low and Place IS Low THEN Performance IS Acceptable");
            IS.NewRule("Rule 8", "IF Use IS Low and Place IS Middle THEN Performance IS Adequate");
            IS.NewRule("Rule 9", "IF Use IS Low and Place IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 10", "IF Use IS Low and Place IS VeryHigh THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Use IS Middle and Place IS VeryLow THEN Performance IS Inadequate");
            IS.NewRule("Rule 12", "IF Use IS Middle and Place IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 13", "IF Use IS Middle and Place IS Middle THEN Performance IS Acceptable");
            IS.NewRule("Rule 14", "IF Use IS Middle and Place IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 15", "IF Use IS Middle and Place IS VeryHigh THEN Performance IS Adequate");
            IS.NewRule("Rule 16", "IF Use IS High and Place IS VeryLow THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Use IS High and Place IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 18", "IF Use IS High and Place IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 19", "IF Use IS High and Place IS High THEN Performance IS Acceptable");
            IS.NewRule("Rule 20", "IF Use IS High and Place IS VeryHigh THEN Performance IS Adequate");
            IS.NewRule("Rule 21", "IF Use IS VeryHigh and Place IS VeryLow THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Use IS VeryHigh and Place IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Use IS VeryHigh and Place IS Middle THEN Performance IS Inadequate");
            IS.NewRule("Rule 24", "IF Use IS VeryHigh and Place IS High THEN Performance IS Inadequate");
            IS.NewRule("Rule 25", "IF Use IS VeryHigh and Place IS VeryHigh THEN Performance IS Acceptable");

            IS.SetInput("Use", (float)useValue);
            IS.SetInput("Place", (float)placeValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Use", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Place", i == 0 ? 0 : (float)9.99);
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

        public string CalculateManagement(double airTypeValue, double isolationValue, double inspectionValue)
        {
            LinguisticVariable airType = new( "AirType", 0, 10 );
            airType.AddLabel( new FuzzySet( "Little", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            airType.AddLabel( new FuzzySet( "Proper", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            airType.AddLabel( new FuzzySet( "Much", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable isolation = new( "Isolation", 0, 10 );
            isolation.AddLabel( new FuzzySet( "Few", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            isolation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            isolation.AddLabel( new FuzzySet( "Much", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable inspection = new( "Inspection", 0, 10 );
            inspection.AddLabel( new FuzzySet( "Little", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            inspection.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            inspection.AddLabel( new FuzzySet( "Much", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( airType );
            fuzzyDB.AddVariable( isolation );
            fuzzyDB.AddVariable( inspection );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF AirType IS Little and Isolation IS Few and Inspection IS Little THEN Management IS VeryInadequate");
            IS.NewRule("Rule 2", "IF AirType IS Little and Isolation IS Few and Inspection IS Medium THEN Management IS VeryInadequate");
            IS.NewRule("Rule 3", "IF AirType IS Little and Isolation IS Few and Inspection IS Much THEN Management IS Inadequate");
            IS.NewRule("Rule 4", "IF AirType IS Little and Isolation IS Medium and Inspection IS Little THEN Management IS VeryInadequate");
            IS.NewRule("Rule 5", "IF AirType IS Little and Isolation IS Medium and Inspection IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 6", "IF AirType IS Little and Isolation IS Medium and Inspection IS Much THEN Management IS Acceptable");
            IS.NewRule("Rule 7", "IF AirType IS Little and Isolation IS Much and Inspection IS Little THEN Management IS VeryInadequate");
            IS.NewRule("Rule 8", "IF AirType IS Little and Isolation IS Much and Inspection IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 9", "IF AirType IS Little and Isolation IS Much and Inspection IS Much THEN Management IS Adequate");
            IS.NewRule("Rule 10", "IF AirType IS Proper and Isolation IS Few and Inspection IS Little THEN Management IS VeryInadequate");
            IS.NewRule("Rule 11", "IF AirType IS Proper and Isolation IS Few and Inspection IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 12", "IF AirType IS Proper and Isolation IS Few and Inspection IS Much THEN Management IS Acceptable");
            IS.NewRule("Rule 13", "IF AirType IS Proper and Isolation IS Medium and Inspection IS Little THEN Management IS VeryInadequate");
            IS.NewRule("Rule 14", "IF AirType IS Proper and Isolation IS Medium and Inspection IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 15", "IF AirType IS Proper and Isolation IS Medium and Inspection IS Much THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF AirType IS Proper and Isolation IS Much and Inspection IS Little THEN Management IS Acceptable");
            IS.NewRule("Rule 17", "IF AirType IS Proper and Isolation IS Much and Inspection IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 18", "IF AirType IS Proper and Isolation IS Much and Inspection IS Much THEN Management IS VeryAdequate");
            IS.NewRule("Rule 19", "IF AirType IS Much and Isolation IS Few and Inspection IS Little THEN Management IS Inadequate");
            IS.NewRule("Rule 20", "IF AirType IS Much and Isolation IS Few and Inspection IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 21", "IF AirType IS Much and Isolation IS Few and Inspection IS Much THEN Management IS Adequate");
            IS.NewRule("Rule 22", "IF AirType IS Much and Isolation IS Medium and Inspection IS Little THEN Management IS Acceptable");
            IS.NewRule("Rule 23", "IF AirType IS Much and Isolation IS Medium and Inspection IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF AirType IS Much and Isolation IS Medium and Inspection IS Much THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF AirType IS Much and Isolation IS Much and Inspection IS Little THEN Management IS Adequate");
            IS.NewRule("Rule 26", "IF AirType IS Much and Isolation IS Much and Inspection IS Medium THEN Management IS VeryAdequate");
            IS.NewRule("Rule 27", "IF AirType IS Much and Isolation IS Much and Inspection IS Much THEN Management IS VeryAdequate");

            IS.SetInput("AirType", (float)airTypeValue);
            IS.SetInput("Isolation", (float)isolationValue);
            IS.SetInput("Inspection", (float)inspectionValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("AirType", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Isolation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Inspection", i == 0 ? 0 : (float)9.99);
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

        public string CalculateThermodynamics(double heatValue, double temperatureValue, double fluidValue)
        {
            LinguisticVariable heat = new( "Heat", 0, 10 );
            heat.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            heat.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            heat.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable temperature = new( "Temperature", 0, 10 );
            temperature.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            temperature.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            temperature.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable fluid = new( "Fluid", 0, 10 );
            fluid.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            fluid.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            fluid.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( heat );
            fuzzyDB.AddVariable( temperature );
            fuzzyDB.AddVariable( fluid );
            fuzzyDB.AddVariable( thermodynamics );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Heat IS Low and Temperature IS Low and Fluid IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 2", "IF Heat IS Low and Temperature IS Low and Fluid IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 3", "IF Heat IS Low and Temperature IS Low and Fluid IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 4", "IF Heat IS Low and Temperature IS Medium and Fluid IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 5", "IF Heat IS Low and Temperature IS Medium and Fluid IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 6", "IF Heat IS Low and Temperature IS Medium and Fluid IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Heat IS Low and Temperature IS High and Fluid IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 8", "IF Heat IS Low and Temperature IS High and Fluid IS Medium THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Heat IS Low and Temperature IS High and Fluid IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 10", "IF Heat IS Medium and Temperature IS Low and Fluid IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Heat IS Medium and Temperature IS Low and Fluid IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 12", "IF Heat IS Medium and Temperature IS Low and Fluid IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 13", "IF Heat IS Medium and Temperature IS Medium and Fluid IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 14", "IF Heat IS Medium and Temperature IS Medium and Fluid IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 15", "IF Heat IS Medium and Temperature IS Medium and Fluid IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 16", "IF Heat IS Medium and Temperature IS High and Fluid IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 17", "IF Heat IS Medium and Temperature IS High and Fluid IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 18", "IF Heat IS Medium and Temperature IS High and Fluid IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 19", "IF Heat IS High and Temperature IS Low and Fluid IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 20", "IF Heat IS High and Temperature IS Low and Fluid IS Medium THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Heat IS High and Temperature IS Low and Fluid IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 22", "IF Heat IS High and Temperature IS Medium and Fluid IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 23", "IF Heat IS High and Temperature IS Medium and Fluid IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 24", "IF Heat IS High and Temperature IS Medium and Fluid IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 25", "IF Heat IS High and Temperature IS High and Fluid IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 26", "IF Heat IS High and Temperature IS High and Fluid IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 27", "IF Heat IS High and Temperature IS High and Fluid IS High THEN Thermodynamics IS Inadequate");

            IS.SetInput("Heat", (float)heatValue);
            IS.SetInput("Temperature", (float)temperatureValue);
            IS.SetInput("Fluid", (float)fluidValue);

            double resultado = IS.Evaluate("Thermodynamics");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Heat", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Temperature", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Fluid", i == 0 ? (float)9.99 : 0);
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

        public string CalculateHeating(double managementValue, double performanceValue, double thermodynamicsValue)
        {
            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable heating = new( "Heating", 0, 10 );
            heating.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            heating.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            heating.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            heating.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            heating.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( heating );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Management IS VeryInadequate and Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Management IS VeryInadequate and Performance IS Inadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Management IS VeryInadequate and Performance IS Inadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Management IS VeryInadequate and Performance IS Inadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 10", "IF Management IS VeryInadequate and Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 11", "IF Management IS VeryInadequate and Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Management IS VeryInadequate and Performance IS Acceptable and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 13", "IF Management IS VeryInadequate and Performance IS Acceptable and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 14", "IF Management IS VeryInadequate and Performance IS Acceptable and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 15", "IF Management IS VeryInadequate and Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 16", "IF Management IS VeryInadequate and Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Management IS VeryInadequate and Performance IS Adequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 18", "IF Management IS VeryInadequate and Performance IS Adequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 19", "IF Management IS VeryInadequate and Performance IS Adequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 20", "IF Management IS VeryInadequate and Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 21", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 24", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 25", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 26", "IF Management IS Inadequate and Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 27", "IF Management IS Inadequate and Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 28", "IF Management IS Inadequate and Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 29", "IF Management IS Inadequate and Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 30", "IF Management IS Inadequate and Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 31", "IF Management IS Inadequate and Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 32", "IF Management IS Inadequate and Performance IS Inadequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 33", "IF Management IS Inadequate and Performance IS Inadequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 34", "IF Management IS Inadequate and Performance IS Inadequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 35", "IF Management IS Inadequate and Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 36", "IF Management IS Inadequate and Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 37", "IF Management IS Inadequate and Performance IS Acceptable and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 38", "IF Management IS Inadequate and Performance IS Acceptable and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 39", "IF Management IS Inadequate and Performance IS Acceptable and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 40", "IF Management IS Inadequate and Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 41", "IF Management IS Inadequate and Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 42", "IF Management IS Inadequate and Performance IS Adequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 43", "IF Management IS Inadequate and Performance IS Adequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 44", "IF Management IS Inadequate and Performance IS Adequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 45", "IF Management IS Inadequate and Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 46", "IF Management IS Inadequate and Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 47", "IF Management IS Inadequate and Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 48", "IF Management IS Inadequate and Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 49", "IF Management IS Inadequate and Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 50", "IF Management IS Inadequate and Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 51", "IF Management IS Acceptable and Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 52", "IF Management IS Acceptable and Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 53", "IF Management IS Acceptable and Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 54", "IF Management IS Acceptable and Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 55", "IF Management IS Acceptable and Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 56", "IF Management IS Acceptable and Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 57", "IF Management IS Acceptable and Performance IS Inadequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 58", "IF Management IS Acceptable and Performance IS Inadequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 59", "IF Management IS Acceptable and Performance IS Inadequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 60", "IF Management IS Acceptable and Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 61", "IF Management IS Acceptable and Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 62", "IF Management IS Acceptable and Performance IS Acceptable and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 63", "IF Management IS Acceptable and Performance IS Acceptable and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 64", "IF Management IS Acceptable and Performance IS Acceptable and Thermodynamics IS Adequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 65", "IF Management IS Acceptable and Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 66", "IF Management IS Acceptable and Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 67", "IF Management IS Acceptable and Performance IS Adequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 68", "IF Management IS Acceptable and Performance IS Adequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 69", "IF Management IS Acceptable and Performance IS Adequate and Thermodynamics IS Adequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 70", "IF Management IS Acceptable and Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 71", "IF Management IS Acceptable and Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 72", "IF Management IS Acceptable and Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 73", "IF Management IS Acceptable and Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 74", "IF Management IS Acceptable and Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 75", "IF Management IS Acceptable and Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 76", "IF Management IS Adequate and Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 77", "IF Management IS Adequate and Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 78", "IF Management IS Adequate and Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 79", "IF Management IS Adequate and Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 80", "IF Management IS Adequate and Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 81", "IF Management IS Adequate and Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 82", "IF Management IS Adequate and Performance IS Inadequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 83", "IF Management IS Adequate and Performance IS Inadequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 84", "IF Management IS Adequate and Performance IS Inadequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 85", "IF Management IS Adequate and Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 86", "IF Management IS Adequate and Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 87", "IF Management IS Adequate and Performance IS Acceptable and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 88", "IF Management IS Adequate and Performance IS Acceptable and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 89", "IF Management IS Adequate and Performance IS Acceptable and Thermodynamics IS Adequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 90", "IF Management IS Adequate and Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 91", "IF Management IS Adequate and Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 92", "IF Management IS Adequate and Performance IS Adequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 93", "IF Management IS Adequate and Performance IS Adequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 94", "IF Management IS Adequate and Performance IS Adequate and Thermodynamics IS Adequate THEN Heating IS Adequate");
            IS.NewRule("Rule 95", "IF Management IS Adequate and Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Heating IS Adequate");
            IS.NewRule("Rule 96", "IF Management IS Adequate and Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 97", "IF Management IS Adequate and Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 98", "IF Management IS Adequate and Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 99", "IF Management IS Adequate and Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Heating IS Adequate");
            IS.NewRule("Rule 100", "IF Management IS Adequate and Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Heating IS Adequate");
            IS.NewRule("Rule 101", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 102", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 103", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 104", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 105", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 106", "IF Management IS VeryAdequate and Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 107", "IF Management IS VeryAdequate and Performance IS Inadequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 108", "IF Management IS VeryAdequate and Performance IS Inadequate and Thermodynamics IS Acceptable THEN Heating IS Inadequate");
            IS.NewRule("Rule 109", "IF Management IS VeryAdequate and Performance IS Inadequate and Thermodynamics IS Adequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 110", "IF Management IS VeryAdequate and Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 111", "IF Management IS VeryAdequate and Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 112", "IF Management IS VeryAdequate and Performance IS Acceptable and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 113", "IF Management IS VeryAdequate and Performance IS Acceptable and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 114", "IF Management IS VeryAdequate and Performance IS Acceptable and Thermodynamics IS Adequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 115", "IF Management IS VeryAdequate and Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Heating IS Acceptable");
            IS.NewRule("Rule 116", "IF Management IS VeryAdequate and Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 117", "IF Management IS VeryAdequate and Performance IS Adequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 118", "IF Management IS VeryAdequate and Performance IS Adequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 119", "IF Management IS VeryAdequate and Performance IS Adequate and Thermodynamics IS Adequate THEN Heating IS Adequate");
            IS.NewRule("Rule 120", "IF Management IS VeryAdequate and Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Heating IS Adequate");
            IS.NewRule("Rule 121", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Heating IS VeryInadequate");
            IS.NewRule("Rule 122", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Heating IS Inadequate");
            IS.NewRule("Rule 123", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Heating IS Acceptable");
            IS.NewRule("Rule 124", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Heating IS Adequate");
            IS.NewRule("Rule 125", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Heating IS VeryAdequate");

            IS.SetInput("Management", (float)managementValue);
            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);

            double resultado = IS.Evaluate("Heating");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Heating");
            }
            double m = (IS.GetLinguisticVariable("Heating").End - IS.GetLinguisticVariable("Heating").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Heating").End;

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