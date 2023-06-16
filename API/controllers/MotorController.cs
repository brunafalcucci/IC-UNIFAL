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
    public class MotorController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IMotorRepository _MotorRepository;
        public MotorController(IConfiguration configuration, IMotorRepository MotorRepository)
        {
            Configuration = configuration;
            _MotorRepository = MotorRepository;
        }

        [HttpPost]
        public ActionResult<Motor> InsertMotor([FromBody] Motor motor)
        {
            try
            {
                motor.Management = CalculateManagement(Convert.ToDouble(motor.Maintenance), Convert.ToDouble(motor.Cleaning), Convert.ToDouble(motor.Moisture));
                motor.Performance = CalculatePerformance(Convert.ToDouble(motor.Operation), Convert.ToDouble(motor.Noise), Convert.ToDouble(motor.Ventilation));
                motor.MotorValue = CalculateMotor(Convert.ToDouble(motor.Management), Convert.ToDouble(motor.Performance), Convert.ToDouble(motor.Temperature));

                return _MotorRepository.InsertMotor(motor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Motor> GetMotorById(int id)
        {
            try
            {
                return _MotorRepository.GetMotorById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<Motor>> GetMotorByIndustry(string industryName)
        {
            try
            {
                return _MotorRepository.GetMotorByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double maintenanceValue, double cleaningValue, double moistureValue)
        {
            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "Dirty", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            cleaning.AddLabel( new FuzzySet( "MediumCleaning", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            cleaning.AddLabel( new FuzzySet( "Clean", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable moisture = new( "Moisture", 0, 10 );
            moisture.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            moisture.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            moisture.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( moisture );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Maintenance IS Low and Cleaning IS Dirty and Moisture IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 2", "IF Maintenance IS Low and Cleaning IS Dirty and Moisture IS Medium THEN Management IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Maintenance IS Low and Cleaning IS Dirty and Moisture IS High THEN Management IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Maintenance IS Low and Cleaning IS MediumCleaning and Moisture IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 5", "IF Maintenance IS Low and Cleaning IS MediumCleaning and Moisture IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 6", "IF Maintenance IS Low and Cleaning IS MediumCleaning and Moisture IS High THEN Management IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Maintenance IS Low and Cleaning IS Clean and Moisture IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 8", "IF Maintenance IS Low and Cleaning IS Clean and Moisture IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 9", "IF Maintenance IS Low and Cleaning IS Clean and Moisture IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 10", "IF Maintenance IS Medium and Cleaning IS Dirty and Moisture IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 11", "IF Maintenance IS Medium and Cleaning IS Dirty and Moisture IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 12", "IF Maintenance IS Medium and Cleaning IS Dirty and Moisture IS High THEN Management IS VeryInadequate");
            IS.NewRule("Rule 13", "IF Maintenance IS Medium and Cleaning IS MediumCleaning and Moisture IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 14", "IF Maintenance IS Medium and Cleaning IS MediumCleaning and Moisture IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 15", "IF Maintenance IS Medium and Cleaning IS MediumCleaning and Moisture IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 16", "IF Maintenance IS Medium and Cleaning IS Clean and Moisture IS Low THEN Management IS VeryAdequate");
            IS.NewRule("Rule 17", "IF Maintenance IS Medium and Cleaning IS Clean and Moisture IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 18", "IF Maintenance IS Medium and Cleaning IS Clean and Moisture IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 19", "IF Maintenance IS High and Cleaning IS Dirty and Moisture IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF Maintenance IS High and Cleaning IS Dirty and Moisture IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 21", "IF Maintenance IS High and Cleaning IS Dirty and Moisture IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 22", "IF Maintenance IS High and Cleaning IS MediumCleaning and Moisture IS Low THEN Management IS VeryAdequate");
            IS.NewRule("Rule 23", "IF Maintenance IS High and Cleaning IS MediumCleaning and Moisture IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF Maintenance IS High and Cleaning IS MediumCleaning and Moisture IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 25", "IF Maintenance IS High and Cleaning IS Clean and Moisture IS Low THEN Management IS VeryAdequate");
            IS.NewRule("Rule 26", "IF Maintenance IS High and Cleaning IS Clean and Moisture IS Medium THEN Management IS VeryAdequate");
            IS.NewRule("Rule 27", "IF Maintenance IS High and Cleaning IS Clean and Moisture IS High THEN Management IS Adequate");

            IS.SetInput("Maintenance", (float)maintenanceValue);
            IS.SetInput("Cleaning", (float)cleaningValue);
            IS.SetInput("Moisture", (float)moistureValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Maintenance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Cleaning", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Moisture", i == 0 ? (float)9.99 : 0);
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

        public string CalculatePerformance(double operationValue, double noiseValue, double ventilationValue)
        {
            LinguisticVariable operation = new( "Operation", 0, 10 );
            operation.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            operation.AddLabel( new FuzzySet( "Moderate", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            operation.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable noise = new( "Noise", 0, 10 );
            noise.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            noise.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            noise.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable ventilation = new( "Ventilation", 0, 10 );
            ventilation.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            ventilation.AddLabel( new FuzzySet( "Moderate", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            ventilation.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( operation );
            fuzzyDB.AddVariable( noise );
            fuzzyDB.AddVariable( ventilation );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Operation IS Bad and Noise IS Low and Ventilation IS Bad THEN Performance IS Inadequate");
            IS.NewRule("Rule 2", "IF Operation IS Bad and Noise IS Low and Ventilation IS Moderate THEN Performance IS Acceptable");
            IS.NewRule("Rule 3", "IF Operation IS Bad and Noise IS Low and Ventilation IS Good THEN Performance IS Adequate");
            IS.NewRule("Rule 4", "IF Operation IS Bad and Noise IS Medium and Ventilation IS Bad THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Operation IS Bad and Noise IS Medium and Ventilation IS Moderate THEN Performance IS Inadequate");
            IS.NewRule("Rule 6", "IF Operation IS Bad and Noise IS Medium and Ventilation IS Good THEN Performance IS Acceptable");
            IS.NewRule("Rule 7", "IF Operation IS Bad and Noise IS High and Ventilation IS Bad THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Operation IS Bad and Noise IS High and Ventilation IS Moderate THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Operation IS Bad and Noise IS High and Ventilation IS Good THEN Performance IS Inadequate");
            IS.NewRule("Rule 10", "IF Operation IS Moderate and Noise IS Low and Ventilation IS Bad THEN Performance IS Acceptable");
            IS.NewRule("Rule 11", "IF Operation IS Moderate and Noise IS Low and Ventilation IS Moderate THEN Performance IS Adequate");
            IS.NewRule("Rule 12", "IF Operation IS Moderate and Noise IS Low and Ventilation IS Good THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 13", "IF Operation IS Moderate and Noise IS Medium and Ventilation IS Bad THEN Performance IS Inadequate");
            IS.NewRule("Rule 14", "IF Operation IS Moderate and Noise IS Medium and Ventilation IS Moderate THEN Performance IS Acceptable");
            IS.NewRule("Rule 15", "IF Operation IS Moderate and Noise IS Medium and Ventilation IS Good THEN Performance IS Adequate");
            IS.NewRule("Rule 16", "IF Operation IS Moderate and Noise IS High and Ventilation IS Bad THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Operation IS Moderate and Noise IS High and Ventilation IS Moderate THEN Performance IS Inadequate");
            IS.NewRule("Rule 18", "IF Operation IS Moderate and Noise IS High and Ventilation IS Good THEN Performance IS Acceptable");
            IS.NewRule("Rule 19", "IF Operation IS Good and Noise IS Low and Ventilation IS Bad THEN Performance IS Adequate");
            IS.NewRule("Rule 20", "IF Operation IS Good and Noise IS Low and Ventilation IS Moderate THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Operation IS Good and Noise IS Low and Ventilation IS Good THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 22", "IF Operation IS Good and Noise IS Medium and Ventilation IS Bad THEN Performance IS Acceptable");
            IS.NewRule("Rule 23", "IF Operation IS Good and Noise IS Medium and Ventilation IS Moderate THEN Performance IS Adequate");
            IS.NewRule("Rule 24", "IF Operation IS Good and Noise IS Medium and Ventilation IS Good THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Operation IS Good and Noise IS High and Ventilation IS Bad THEN Performance IS Inadequate");
            IS.NewRule("Rule 26", "IF Operation IS Good and Noise IS High and Ventilation IS Moderate THEN Performance IS Acceptable");
            IS.NewRule("Rule 27", "IF Operation IS Good and Noise IS High and Ventilation IS Good THEN Performance IS Adequate");

            IS.SetInput("Operation", (float)operationValue);
            IS.SetInput("Noise", (float)noiseValue);
            IS.SetInput("Ventilation", (float)ventilationValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Operation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Noise", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Ventilation", i == 0 ? (float)9.99 : 0);
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

        public string CalculateMotor(double managementValue, double performanceValue, double temperatureValue)
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

            LinguisticVariable temperature = new( "Temperature", 0, 10 );
            temperature.AddLabel( new FuzzySet( "Minimun", new TrapezoidalFunction(0, 0, 1, 3) ) );
            temperature.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(1, 3, 5) ) );
            temperature.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            temperature.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(5, 7, 9) ) );
            temperature.AddLabel( new FuzzySet( "Maximun", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable motor = new( "Motor", 0, 10 );
            motor.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            motor.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            motor.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            motor.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            motor.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( temperature );
            fuzzyDB.AddVariable( motor );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Management IS VeryInadequate and Performance IS VeryInadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Management IS VeryInadequate and Performance IS Inadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Management IS VeryInadequate and Performance IS Inadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Management IS VeryInadequate and Performance IS Inadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Management IS VeryInadequate and Performance IS Inadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 10", "IF Management IS VeryInadequate and Performance IS Inadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 11", "IF Management IS VeryInadequate and Performance IS Acceptable and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Management IS VeryInadequate and Performance IS Acceptable and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 13", "IF Management IS VeryInadequate and Performance IS Acceptable and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 14", "IF Management IS VeryInadequate and Performance IS Acceptable and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 15", "IF Management IS VeryInadequate and Performance IS Acceptable and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 16", "IF Management IS VeryInadequate and Performance IS Adequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Management IS VeryInadequate and Performance IS Adequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 18", "IF Management IS VeryInadequate and Performance IS Adequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 19", "IF Management IS VeryInadequate and Performance IS Adequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 20", "IF Management IS VeryInadequate and Performance IS Adequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 21", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 23", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 24", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 25", "IF Management IS VeryInadequate and Performance IS VeryAdequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 26", "IF Management IS Inadequate and Performance IS VeryInadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 27", "IF Management IS Inadequate and Performance IS VeryInadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 28", "IF Management IS Inadequate and Performance IS VeryInadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 29", "IF Management IS Inadequate and Performance IS VeryInadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 30", "IF Management IS Inadequate and Performance IS VeryInadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 31", "IF Management IS Inadequate and Performance IS Inadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 32", "IF Management IS Inadequate and Performance IS Inadequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 33", "IF Management IS Inadequate and Performance IS Inadequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 34", "IF Management IS Inadequate and Performance IS Inadequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 35", "IF Management IS Inadequate and Performance IS Inadequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 36", "IF Management IS Inadequate and Performance IS Acceptable and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 37", "IF Management IS Inadequate and Performance IS Acceptable and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 38", "IF Management IS Inadequate and Performance IS Acceptable and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 39", "IF Management IS Inadequate and Performance IS Acceptable and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 40", "IF Management IS Inadequate and Performance IS Acceptable and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 41", "IF Management IS Inadequate and Performance IS Adequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 42", "IF Management IS Inadequate and Performance IS Adequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 43", "IF Management IS Inadequate and Performance IS Adequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 44", "IF Management IS Inadequate and Performance IS Adequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 45", "IF Management IS Inadequate and Performance IS Adequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 46", "IF Management IS Inadequate and Performance IS VeryAdequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 47", "IF Management IS Inadequate and Performance IS VeryAdequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 48", "IF Management IS Inadequate and Performance IS VeryAdequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 49", "IF Management IS Inadequate and Performance IS VeryAdequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 50", "IF Management IS Inadequate and Performance IS VeryAdequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 51", "IF Management IS Acceptable and Performance IS VeryInadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 52", "IF Management IS Acceptable and Performance IS VeryInadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 53", "IF Management IS Acceptable and Performance IS VeryInadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 54", "IF Management IS Acceptable and Performance IS VeryInadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 55", "IF Management IS Acceptable and Performance IS VeryInadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 56", "IF Management IS Acceptable and Performance IS Inadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 57", "IF Management IS Acceptable and Performance IS Inadequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 58", "IF Management IS Acceptable and Performance IS Inadequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 59", "IF Management IS Acceptable and Performance IS Inadequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 60", "IF Management IS Acceptable and Performance IS Inadequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 61", "IF Management IS Acceptable and Performance IS Acceptable and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 62", "IF Management IS Acceptable and Performance IS Acceptable and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 63", "IF Management IS Acceptable and Performance IS Acceptable and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 64", "IF Management IS Acceptable and Performance IS Acceptable and Temperature IS Bad THEN Motor IS Acceptable");
            IS.NewRule("Rule 65", "IF Management IS Acceptable and Performance IS Acceptable and Temperature IS Minimun THEN Motor IS Acceptable");
            IS.NewRule("Rule 66", "IF Management IS Acceptable and Performance IS Adequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 67", "IF Management IS Acceptable and Performance IS Adequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 68", "IF Management IS Acceptable and Performance IS Adequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 69", "IF Management IS Acceptable and Performance IS Adequate and Temperature IS Bad THEN Motor IS Acceptable");
            IS.NewRule("Rule 70", "IF Management IS Acceptable and Performance IS Adequate and Temperature IS Minimun THEN Motor IS Acceptable");
            IS.NewRule("Rule 71", "IF Management IS Acceptable and Performance IS VeryAdequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 72", "IF Management IS Acceptable and Performance IS VeryAdequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 73", "IF Management IS Acceptable and Performance IS VeryAdequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 74", "IF Management IS Acceptable and Performance IS VeryAdequate and Temperature IS Bad THEN Motor IS Acceptable");
            IS.NewRule("Rule 75", "IF Management IS Acceptable and Performance IS VeryAdequate and Temperature IS Minimun THEN Motor IS Acceptable");
            IS.NewRule("Rule 76", "IF Management IS Adequate and Performance IS VeryInadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 77", "IF Management IS Adequate and Performance IS VeryInadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 78", "IF Management IS Adequate and Performance IS VeryInadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 79", "IF Management IS Adequate and Performance IS VeryInadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 80", "IF Management IS Adequate and Performance IS VeryInadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 81", "IF Management IS Adequate and Performance IS Inadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 82", "IF Management IS Adequate and Performance IS Inadequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 83", "IF Management IS Adequate and Performance IS Inadequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 84", "IF Management IS Adequate and Performance IS Inadequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 85", "IF Management IS Adequate and Performance IS Inadequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 86", "IF Management IS Adequate and Performance IS Acceptable and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 87", "IF Management IS Adequate and Performance IS Acceptable and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 88", "IF Management IS Adequate and Performance IS Acceptable and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 89", "IF Management IS Adequate and Performance IS Acceptable and Temperature IS Bad THEN Motor IS Acceptable");
            IS.NewRule("Rule 90", "IF Management IS Adequate and Performance IS Acceptable and Temperature IS Minimun THEN Motor IS Acceptable");
            IS.NewRule("Rule 91", "IF Management IS Adequate and Performance IS Adequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 92", "IF Management IS Adequate and Performance IS Adequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 93", "IF Management IS Adequate and Performance IS Adequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 94", "IF Management IS Adequate and Performance IS Adequate and Temperature IS Bad THEN Motor IS Adequate");
            IS.NewRule("Rule 95", "IF Management IS Adequate and Performance IS Adequate and Temperature IS Minimun THEN Motor IS Adequate");
            IS.NewRule("Rule 96", "IF Management IS Adequate and Performance IS VeryAdequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 97", "IF Management IS Adequate and Performance IS VeryAdequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 98", "IF Management IS Adequate and Performance IS VeryAdequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 99", "IF Management IS Adequate and Performance IS VeryAdequate and Temperature IS Bad THEN Motor IS Adequate");
            IS.NewRule("Rule 100", "IF Management IS Adequate and Performance IS VeryAdequate and Temperature IS Minimun THEN Motor IS Adequate");
            IS.NewRule("Rule 101", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 102", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Temperature IS Acceptable THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 103", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Temperature IS Medium THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 104", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Temperature IS Bad THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 105", "IF Management IS VeryAdequate and Performance IS VeryInadequate and Temperature IS Minimun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 106", "IF Management IS VeryAdequate and Performance IS Inadequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 107", "IF Management IS VeryAdequate and Performance IS Inadequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 108", "IF Management IS VeryAdequate and Performance IS Inadequate and Temperature IS Medium THEN Motor IS Inadequate");
            IS.NewRule("Rule 109", "IF Management IS VeryAdequate and Performance IS Inadequate and Temperature IS Bad THEN Motor IS Inadequate");
            IS.NewRule("Rule 110", "IF Management IS VeryAdequate and Performance IS Inadequate and Temperature IS Minimun THEN Motor IS Inadequate");
            IS.NewRule("Rule 111", "IF Management IS VeryAdequate and Performance IS Acceptable and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 112", "IF Management IS VeryAdequate and Performance IS Acceptable and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 113", "IF Management IS VeryAdequate and Performance IS Acceptable and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 114", "IF Management IS VeryAdequate and Performance IS Acceptable and Temperature IS Bad THEN Motor IS Acceptable");
            IS.NewRule("Rule 115", "IF Management IS VeryAdequate and Performance IS Acceptable and Temperature IS Minimun THEN Motor IS Acceptable");
            IS.NewRule("Rule 116", "IF Management IS VeryAdequate and Performance IS Adequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 117", "IF Management IS VeryAdequate and Performance IS Adequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 118", "IF Management IS VeryAdequate and Performance IS Adequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 119", "IF Management IS VeryAdequate and Performance IS Adequate and Temperature IS Bad THEN Motor IS Adequate");
            IS.NewRule("Rule 120", "IF Management IS VeryAdequate and Performance IS Adequate and Temperature IS Minimun THEN Motor IS Adequate");
            IS.NewRule("Rule 121", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Temperature IS Maximun THEN Motor IS VeryInadequate");
            IS.NewRule("Rule 122", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Temperature IS Acceptable THEN Motor IS Inadequate");
            IS.NewRule("Rule 123", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Temperature IS Medium THEN Motor IS Acceptable");
            IS.NewRule("Rule 124", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Temperature IS Bad THEN Motor IS Adequate");
            IS.NewRule("Rule 125", "IF Management IS VeryAdequate and Performance IS VeryAdequate and Temperature IS Minimun THEN Motor IS VeryAdequate");

            IS.SetInput("Management", (float)managementValue);
            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Temperature", (float)temperatureValue);

            double resultado = IS.Evaluate("Motor");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Performance", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Temperature", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Motor");
            }
            double m = (IS.GetLinguisticVariable("Motor").End - IS.GetLinguisticVariable("Motor").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Motor").Start;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - VeryInadequate";
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
                return resultado.ToString() + " - VeryAdequate";
            }
        }
    }
}