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
    public class AirConditioningController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IAirConditioningRepository _AirConditioningRepository;
        public AirConditioningController(IConfiguration configuration, IAirConditioningRepository AirConditioningRepository)
        {
            Configuration = configuration;
            _AirConditioningRepository = AirConditioningRepository;
        }

        [HttpPost]
        public ActionResult<AirConditioning> InsertAirConditioning([FromBody] AirConditioning airConditioning)
        {
            try
            {
                airConditioning.Management = CalculateManagement(Convert.ToDouble(airConditioning.Cleaning), Convert.ToDouble(airConditioning.Maintenance));
                airConditioning.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(airConditioning.Isolation), Convert.ToDouble(airConditioning.Acclimatized), Convert.ToDouble(airConditioning.Temperature));
                airConditioning.AirConditioningValue = CalculateAirConditioning(Convert.ToDouble(airConditioning.Thermodynamics), Convert.ToDouble(airConditioning.Management));

                return _AirConditioningRepository.InsertAirConditioning(airConditioning);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<AirConditioning> GetAirConditioningById(int id)
        {
            try
            {
                return _AirConditioningRepository.GetAirConditioningById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<AirConditioning>> GetAirConditioningByIndustry(string industryName)
        {
            try
            {
                return _AirConditioningRepository.GetAirConditioningByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double cleaningValue, double maintenanceValue)
        {
            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "VeryDirty", new TrapezoidalFunction(0, 0, 1, 3) ) );
            cleaning.AddLabel( new FuzzySet( "Dirty", new TrapezoidalFunction(1, 3, 5) ) );
            cleaning.AddLabel( new FuzzySet( "MediumClean", new TrapezoidalFunction(3, 5, 7) ) );
            cleaning.AddLabel( new FuzzySet( "Clean", new TrapezoidalFunction(5, 7, 9) ) );
            cleaning.AddLabel( new FuzzySet( "VeryClean", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            maintenance.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Cleaning IS VeryDirty and Maintenance IS VeryLow THEN Management IS VeryLow");
            IS.NewRule("Rule 2", "IF Cleaning IS VeryDirty and Maintenance IS Low THEN Management IS VeryLow");
            IS.NewRule("Rule 3", "IF Cleaning IS VeryDirty and Maintenance IS Medium THEN Management IS Low");
            IS.NewRule("Rule 4", "IF Cleaning IS VeryDirty and Maintenance IS High THEN Management IS Low");
            IS.NewRule("Rule 5", "IF Cleaning IS VeryDirty and Maintenance IS VeryHigh THEN Management IS Middle");
            IS.NewRule("Rule 6", "IF Cleaning IS Dirty and Maintenance IS VeryLow THEN Management IS VeryLow");
            IS.NewRule("Rule 7", "IF Cleaning IS Dirty and Maintenance IS Low THEN Management IS Low");
            IS.NewRule("Rule 8", "IF Cleaning IS Dirty and Maintenance IS Medium THEN Management IS Low");
            IS.NewRule("Rule 9", "IF Cleaning IS Dirty and Maintenance IS High THEN Management IS Middle");
            IS.NewRule("Rule 10", "IF Cleaning IS Dirty and Maintenance IS VeryHigh THEN Management IS High");
            IS.NewRule("Rule 11", "IF Cleaning IS MediumClean and Maintenance IS VeryLow THEN Management IS Low");
            IS.NewRule("Rule 12", "IF Cleaning IS MediumClean and Maintenance IS Low THEN Management IS Low");
            IS.NewRule("Rule 13", "IF Cleaning IS MediumClean and Maintenance IS Medium THEN Management IS Middle");
            IS.NewRule("Rule 14", "IF Cleaning IS MediumClean and Maintenance IS High THEN Management IS High");
            IS.NewRule("Rule 15", "IF Cleaning IS MediumClean and Maintenance IS VeryHigh THEN Management IS High");
            IS.NewRule("Rule 16", "IF Cleaning IS Clean and Maintenance IS VeryLow THEN Management IS Low");
            IS.NewRule("Rule 17", "IF Cleaning IS Clean and Maintenance IS Low THEN Management IS Middle");
            IS.NewRule("Rule 18", "IF Cleaning IS Clean and Maintenance IS Medium THEN Management IS High");
            IS.NewRule("Rule 19", "IF Cleaning IS Clean and Maintenance IS High THEN Management IS High");
            IS.NewRule("Rule 20", "IF Cleaning IS Clean and Maintenance IS VeryHigh THEN Management IS VeryHigh");
            IS.NewRule("Rule 21", "IF Cleaning IS VeryClean and Maintenance IS VeryLow THEN Management IS Middle");
            IS.NewRule("Rule 22", "IF Cleaning IS VeryClean and Maintenance IS Low THEN Management IS High");
            IS.NewRule("Rule 23", "IF Cleaning IS VeryClean and Maintenance IS Medium THEN Management IS High");
            IS.NewRule("Rule 24", "IF Cleaning IS VeryClean and Maintenance IS High THEN Management IS VeryHigh");
            IS.NewRule("Rule 25", "IF Cleaning IS VeryClean and Maintenance IS VeryHigh THEN Management IS VeryHigh");

            IS.SetInput("Cleaning", (float)cleaningValue);
            IS.SetInput("Maintenance", (float)maintenanceValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Cleaning", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Maintenance", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Management");
            }
            double m = (IS.GetLinguisticVariable("Management").End - IS.GetLinguisticVariable("Management").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Management").End;
            
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

        public string CalculateThermodynamics(double isolationValue, double acclimatizedValue, double temperatureValue)
        {
            LinguisticVariable isolation = new( "Isolation", 0, 10 );
            isolation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            isolation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            isolation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable acclimatized = new( "Acclimatized", 0, 10 );
            acclimatized.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            acclimatized.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            acclimatized.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable temperature = new( "Temperature", 0, 10 );
            temperature.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            temperature.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            temperature.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( isolation );
            fuzzyDB.AddVariable( acclimatized );
            fuzzyDB.AddVariable( temperature );
            fuzzyDB.AddVariable( thermodynamics );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Isolation IS Low and Acclimatized IS Low and Temperature IS Low THEN Thermodynamics IS Low");
            IS.NewRule("Rule 2", "IF Isolation IS Low and Acclimatized IS Low and Temperature IS Medium THEN Thermodynamics IS VeryLow");
            IS.NewRule("Rule 3", "IF Isolation IS Low and Acclimatized IS Low and Temperature IS High THEN Thermodynamics IS VeryLow");
            IS.NewRule("Rule 4", "IF Isolation IS Low and Acclimatized IS Medium and Temperature IS Low THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 5", "IF Isolation IS Low and Acclimatized IS Medium and Temperature IS Medium THEN Thermodynamics IS Low");
            IS.NewRule("Rule 6", "IF Isolation IS Low and Acclimatized IS Medium and Temperature IS High THEN Thermodynamics IS VeryLow");
            IS.NewRule("Rule 7", "IF Isolation IS Low and Acclimatized IS High and Temperature IS Low THEN Thermodynamics IS High");
            IS.NewRule("Rule 8", "IF Isolation IS Low and Acclimatized IS High and Temperature IS Medium THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 9", "IF Isolation IS Low and Acclimatized IS High and Temperature IS High THEN Thermodynamics IS Low");
            IS.NewRule("Rule 10", "IF Isolation IS Medium and Acclimatized IS Low and Temperature IS Low THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 11", "IF Isolation IS Medium and Acclimatized IS Low and Temperature IS Medium THEN Thermodynamics IS Low");
            IS.NewRule("Rule 12", "IF Isolation IS Medium and Acclimatized IS Low and Temperature IS High THEN Thermodynamics IS VeryLow");
            IS.NewRule("Rule 13", "IF Isolation IS Medium and Acclimatized IS Medium and Temperature IS Low THEN Thermodynamics IS High");
            IS.NewRule("Rule 14", "IF Isolation IS Medium and Acclimatized IS Medium and Temperature IS Medium THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 15", "IF Isolation IS Medium and Acclimatized IS Medium and Temperature IS High THEN Thermodynamics IS Low");
            IS.NewRule("Rule 16", "IF Isolation IS Medium and Acclimatized IS High and Temperature IS Low THEN Thermodynamics IS VeryHigh");
            IS.NewRule("Rule 17", "IF Isolation IS Medium and Acclimatized IS High and Temperature IS Medium THEN Thermodynamics IS High");
            IS.NewRule("Rule 18", "IF Isolation IS Medium and Acclimatized IS High and Temperature IS High THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 19", "IF Isolation IS High and Acclimatized IS Low and Temperature IS Low THEN Thermodynamics IS High");
            IS.NewRule("Rule 20", "IF Isolation IS High and Acclimatized IS Low and Temperature IS Medium THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 21", "IF Isolation IS High and Acclimatized IS Low and Temperature IS High THEN Thermodynamics IS Low");
            IS.NewRule("Rule 22", "IF Isolation IS High and Acclimatized IS Medium and Temperature IS Low THEN Thermodynamics IS VeryHigh");
            IS.NewRule("Rule 23", "IF Isolation IS High and Acclimatized IS Medium and Temperature IS Medium THEN Thermodynamics IS High");
            IS.NewRule("Rule 24", "IF Isolation IS High and Acclimatized IS Medium and Temperature IS High THEN Thermodynamics IS Medium");
            IS.NewRule("Rule 25", "IF Isolation IS High and Acclimatized IS High and Temperature IS Low THEN Thermodynamics IS VeryHigh");
            IS.NewRule("Rule 26", "IF Isolation IS High and Acclimatized IS High and Temperature IS Medium THEN Thermodynamics IS VeryHigh");
            IS.NewRule("Rule 27", "IF Isolation IS High and Acclimatized IS High and Temperature IS High THEN Thermodynamics IS High");

            IS.SetInput("Isolation", (float)isolationValue);
            IS.SetInput("Acclimatized", (float)acclimatizedValue);
            IS.SetInput("Temperature", (float)temperatureValue);

            double resultado = IS.Evaluate("Thermodynamics");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Isolation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Acclimatized", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Temperature", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Thermodynamics");
            }
            double m = (IS.GetLinguisticVariable("Thermodynamics").End - IS.GetLinguisticVariable("Thermodynamics").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Thermodynamics").Start;

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
                return resultado.ToString() + " - Medium";
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

        public string CalculateAirConditioning(double thermodynamicsValue, double managementValue)
        {
            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable airConditioning = new( "AirConditioning", 0, 10 );
            airConditioning.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            airConditioning.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            airConditioning.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            airConditioning.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            airConditioning.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( airConditioning );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Thermodynamics IS VeryLow and Management IS VeryLow THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 2", "IF Thermodynamics IS VeryLow and Management IS Low THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 3", "IF Thermodynamics IS VeryLow and Management IS Medium THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 4", "IF Thermodynamics IS VeryLow and Management IS High THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 5", "IF Thermodynamics IS VeryLow and Management IS VeryHigh THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 6", "IF Thermodynamics IS Low and Management IS VeryLow THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 7", "IF Thermodynamics IS Low and Management IS Low THEN AirConditioning IS Low");
            IS.NewRule("Rule 8", "IF Thermodynamics IS Low and Management IS Medium THEN AirConditioning IS Low");
            IS.NewRule("Rule 9", "IF Thermodynamics IS Low and Management IS High THEN AirConditioning IS Low");
            IS.NewRule("Rule 10", "IF Thermodynamics IS Low and Management IS VeryHigh THEN AirConditioning IS Low");
            IS.NewRule("Rule 11", "IF Thermodynamics IS Medium and Management IS VeryLow THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 12", "IF Thermodynamics IS Medium and Management IS Low THEN AirConditioning IS Low");
            IS.NewRule("Rule 13", "IF Thermodynamics IS Medium and Management IS Medium THEN AirConditioning IS Middle");
            IS.NewRule("Rule 14", "IF Thermodynamics IS Medium and Management IS High THEN AirConditioning IS Middle");
            IS.NewRule("Rule 15", "IF Thermodynamics IS Medium and Management IS VeryHigh THEN AirConditioning IS Middle");
            IS.NewRule("Rule 16", "IF Thermodynamics IS High and Management IS VeryLow THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 17", "IF Thermodynamics IS High and Management IS Low THEN AirConditioning IS Low");
            IS.NewRule("Rule 18", "IF Thermodynamics IS High and Management IS Medium THEN AirConditioning IS Middle");
            IS.NewRule("Rule 19", "IF Thermodynamics IS High and Management IS High THEN AirConditioning IS High");
            IS.NewRule("Rule 20", "IF Thermodynamics IS High and Management IS VeryHigh THEN AirConditioning IS High");
            IS.NewRule("Rule 21", "IF Thermodynamics IS VeryHigh and Management IS VeryLow THEN AirConditioning IS VeryLow");
            IS.NewRule("Rule 22", "IF Thermodynamics IS VeryHigh and Management IS Low THEN AirConditioning IS Low");
            IS.NewRule("Rule 23", "IF Thermodynamics IS VeryHigh and Management IS Medium THEN AirConditioning IS Middle");
            IS.NewRule("Rule 24", "IF Thermodynamics IS VeryHigh and Management IS High THEN AirConditioning IS High");
            IS.NewRule("Rule 25", "IF Thermodynamics IS VeryHigh and Management IS VeryHigh THEN AirConditioning IS VeryHigh");

            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);
            IS.SetInput("Management", (float)managementValue);

            double resultado = IS.Evaluate("AirConditioning");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("AirConditioning");
            }
            double m = (IS.GetLinguisticVariable("AirConditioning").End - IS.GetLinguisticVariable("AirConditioning").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("AirConditioning").End;
            
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