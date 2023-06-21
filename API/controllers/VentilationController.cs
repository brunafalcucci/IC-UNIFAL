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
    public class VentilationController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IVentilationRepository _VentilationRepository;
        public VentilationController(IConfiguration configuration, IVentilationRepository VentilationRepository)
        {
            Configuration = configuration;
            _VentilationRepository = VentilationRepository;
        }

        [HttpPost]
        public ActionResult<Ventilation> InsertVentilation([FromBody] Ventilation ventilation)
        {
            try
            {
                ventilation.Management = CalculateManagement(Convert.ToDouble(ventilation.Functionality), Convert.ToDouble(ventilation.Use), Convert.ToDouble(ventilation.FanControl));
                ventilation.Performance = CalculatePerformance(Convert.ToDouble(ventilation.AirReduction), Convert.ToDouble(ventilation.AirRecycling));
                ventilation.VentilationValue = CalculateVentilation(Convert.ToDouble(ventilation.Performance), Convert.ToDouble(ventilation.Management));

                return _VentilationRepository.InsertVentilation(ventilation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Ventilation> GetVentilationById(int id)
        {
            try
            {
                return _VentilationRepository.GetVentilationById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<Ventilation>> GetVentilationByIndustry(string industryName)
        {
            try
            {
                return _VentilationRepository.GetVentilationByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double functionalityValue, double useValue, double fanControlValue)
        {
            LinguisticVariable functionality = new( "Functionality", 0, 10 );
            functionality.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            functionality.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            functionality.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            use.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable fanControl = new( "FanControl", 0, 10 );
            fanControl.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            fanControl.AddLabel( new FuzzySet( "Moderate", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            fanControl.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( functionality );
            fuzzyDB.AddVariable( use );
            fuzzyDB.AddVariable( fanControl );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Functionality IS Low and Use IS Low and FanControl IS Bad THEN Management IS Inadequate");
            IS.NewRule("Rule 2", "IF Functionality IS Low and Use IS Low and FanControl IS Moderate THEN Management IS Acceptable");
            IS.NewRule("Rule 3", "IF Functionality IS Low and Use IS Low and FanControl IS Good THEN Management IS Adequate");
            IS.NewRule("Rule 4", "IF Functionality IS Low and Use IS Medium and FanControl IS Bad THEN Management IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Functionality IS Low and Use IS Medium and FanControl IS Moderate THEN Management IS Inadequate");
            IS.NewRule("Rule 6", "IF Functionality IS Low and Use IS Medium and FanControl IS Good THEN Management IS Acceptable");
            IS.NewRule("Rule 7", "IF Functionality IS Low and Use IS High and FanControl IS Bad THEN Management IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Functionality IS Low and Use IS High and FanControl IS Moderate THEN Management IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Functionality IS Low and Use IS High and FanControl IS Good THEN Management IS Inadequate");
            IS.NewRule("Rule 10", "IF Functionality IS Medium and Use IS Low and FanControl IS Bad THEN Management IS Acceptable");
            IS.NewRule("Rule 11", "IF Functionality IS Medium and Use IS Low and FanControl IS Moderate THEN Management IS Adequate");
            IS.NewRule("Rule 12", "IF Functionality IS Medium and Use IS Low and FanControl IS Good THEN Management IS VeryAdequate");
            IS.NewRule("Rule 13", "IF Functionality IS Medium and Use IS Medium and FanControl IS Bad THEN Management IS Inadequate");
            IS.NewRule("Rule 14", "IF Functionality IS Medium and Use IS Medium and FanControl IS Moderate THEN Management IS Acceptable");
            IS.NewRule("Rule 15", "IF Functionality IS Medium and Use IS Medium and FanControl IS Good THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF Functionality IS Medium and Use IS High and FanControl IS Bad THEN Management IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Functionality IS Medium and Use IS High and FanControl IS Moderate THEN Management IS Inadequate");
            IS.NewRule("Rule 18", "IF Functionality IS Medium and Use IS High and FanControl IS Good THEN Management IS Acceptable");
            IS.NewRule("Rule 19", "IF Functionality IS High and Use IS Low and FanControl IS Bad THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF Functionality IS High and Use IS Low and FanControl IS Moderate THEN Management IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Functionality IS High and Use IS Low and FanControl IS Good THEN Management IS VeryAdequate");
            IS.NewRule("Rule 22", "IF Functionality IS High and Use IS Medium and FanControl IS Bad THEN Management IS Acceptable");
            IS.NewRule("Rule 23", "IF Functionality IS High and Use IS Medium and FanControl IS Moderate THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF Functionality IS High and Use IS Medium and FanControl IS Good THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Functionality IS High and Use IS High and FanControl IS Bad THEN Management IS Inadequate");
            IS.NewRule("Rule 26", "IF Functionality IS High and Use IS High and FanControl IS Moderate THEN Management IS Acceptable");
            IS.NewRule("Rule 27", "IF Functionality IS High and Use IS High and FanControl IS Good THEN Management IS Adequate");

            IS.SetInput("Functionality", (float)functionalityValue);
            IS.SetInput("Use", (float)useValue);
            IS.SetInput("FanControl", (float)fanControlValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Functionality", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Use", i == 0 ? (float)9.99 : 0);
                IS.SetInput("FanControl", i == 0 ? (float)9.99 : 0);
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
        public string CalculatePerformance(double airReductionValue, double airRecyclingValue)
        {
            LinguisticVariable airReduction = new( "AirReduction", 0, 10 );
            airReduction.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            airReduction.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            airReduction.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            airReduction.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            airReduction.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable airRecycling = new( "AirRecycling", 0, 10 );
            airRecycling.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            airRecycling.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            airRecycling.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            airRecycling.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            airRecycling.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( airReduction );
            fuzzyDB.AddVariable( airRecycling );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF AirReduction IS VeryLow and AirRecycling IS VeryLow THEN Performance IS VeryLow");
            IS.NewRule("Rule 2", "IF AirReduction IS VeryLow and AirRecycling IS Low THEN Performance IS VeryLow");
            IS.NewRule("Rule 3", "IF AirReduction IS VeryLow and AirRecycling IS Middle THEN Performance IS Low");
            IS.NewRule("Rule 4", "IF AirReduction IS VeryLow and AirRecycling IS High THEN Performance IS Low");
            IS.NewRule("Rule 5", "IF AirReduction IS VeryLow and AirRecycling IS VeryHigh THEN Performance IS Middle");
            IS.NewRule("Rule 6", "IF AirReduction IS Low and AirRecycling IS VeryLow THEN Performance IS VeryLow");
            IS.NewRule("Rule 7", "IF AirReduction IS Low and AirRecycling IS Low THEN Performance IS Low");
            IS.NewRule("Rule 8", "IF AirReduction IS Low and AirRecycling IS Middle THEN Performance IS Low");
            IS.NewRule("Rule 9", "IF AirReduction IS Low and AirRecycling IS High THEN Performance IS Middle");
            IS.NewRule("Rule 10", "IF AirReduction IS Low and AirRecycling IS VeryHigh THEN Performance IS High");
            IS.NewRule("Rule 11", "IF AirReduction IS Middle and AirRecycling IS VeryLow THEN Performance IS Low");
            IS.NewRule("Rule 12", "IF AirReduction IS Middle and AirRecycling IS Low THEN Performance IS Low");
            IS.NewRule("Rule 13", "IF AirReduction IS Middle and AirRecycling IS Middle THEN Performance IS Middle");
            IS.NewRule("Rule 14", "IF AirReduction IS Middle and AirRecycling IS High THEN Performance IS High");
            IS.NewRule("Rule 15", "IF AirReduction IS Middle and AirRecycling IS VeryHigh THEN Performance IS High");
            IS.NewRule("Rule 16", "IF AirReduction IS High and AirRecycling IS VeryLow THEN Performance IS Low");
            IS.NewRule("Rule 17", "IF AirReduction IS High and AirRecycling IS Low THEN Performance IS Middle");
            IS.NewRule("Rule 18", "IF AirReduction IS High and AirRecycling IS Middle THEN Performance IS High");
            IS.NewRule("Rule 19", "IF AirReduction IS High and AirRecycling IS High THEN Performance IS High");
            IS.NewRule("Rule 20", "IF AirReduction IS High and AirRecycling IS VeryHigh THEN Performance IS VeryHigh");
            IS.NewRule("Rule 21", "IF AirReduction IS VeryHigh and AirRecycling IS VeryLow THEN Performance IS Middle");
            IS.NewRule("Rule 22", "IF AirReduction IS VeryHigh and AirRecycling IS Low THEN Performance IS High");
            IS.NewRule("Rule 23", "IF AirReduction IS VeryHigh and AirRecycling IS Middle THEN Performance IS High");
            IS.NewRule("Rule 24", "IF AirReduction IS VeryHigh and AirRecycling IS High THEN Performance IS VeryHigh");
            IS.NewRule("Rule 25", "IF AirReduction IS VeryHigh and AirRecycling IS VeryHigh THEN Performance IS VeryHigh");

            IS.SetInput("AirReduction", (float)airReductionValue);
            IS.SetInput("AirRecycling", (float)airRecyclingValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("AirReduction", i == 0 ? 0 : (float)9.99);
                IS.SetInput("AirRecycling", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Performance");
            }
            double m = (IS.GetLinguisticVariable("Performance").End - IS.GetLinguisticVariable("Performance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Performance").End;
            
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

        public string CalculateVentilation(double performanceValue, double managementValue)
        {
            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInapropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inapropriate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Apropriate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryApropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable ventilation = new( "Ventilation", 0, 10 );
            ventilation.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            ventilation.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            ventilation.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            ventilation.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            ventilation.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( ventilation );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Performance IS VeryInapropriate and Management IS VeryInadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Performance IS VeryInapropriate and Management IS Inadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Performance IS VeryInapropriate and Management IS Acceptable THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Performance IS VeryInapropriate and Management IS Adequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Performance IS VeryInapropriate and Management IS VeryAdequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Performance IS Inapropriate and Management IS VeryInadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Performance IS Inapropriate and Management IS Inadequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 8", "IF Performance IS Inapropriate and Management IS Acceptable THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 9", "IF Performance IS Inapropriate and Management IS Adequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 10", "IF Performance IS Inapropriate and Management IS VeryAdequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 11", "IF Performance IS Acceptable and Management IS VeryInadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Performance IS Acceptable and Management IS Inadequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 13", "IF Performance IS Acceptable and Management IS Acceptable THEN Ventilation IS Acceptable");
            IS.NewRule("Rule 14", "IF Performance IS Acceptable and Management IS Adequate THEN Ventilation IS Acceptable");
            IS.NewRule("Rule 15", "IF Performance IS Acceptable and Management IS VeryAdequate THEN Ventilation IS Acceptable");
            IS.NewRule("Rule 16", "IF Performance IS Apropriate and Management IS VeryInadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Performance IS Apropriate and Management IS Inadequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 18", "IF Performance IS Apropriate and Management IS Acceptable THEN Ventilation IS Acceptable");
            IS.NewRule("Rule 19", "IF Performance IS Apropriate and Management IS Adequate THEN Ventilation IS Adequate");
            IS.NewRule("Rule 20", "IF Performance IS Apropriate and Management IS VeryAdequate THEN Ventilation IS Adequate");
            IS.NewRule("Rule 21", "IF Performance IS VeryApropriate and Management IS VeryInadequate THEN Ventilation IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Performance IS VeryApropriate and Management IS Inadequate THEN Ventilation IS Inadequate");
            IS.NewRule("Rule 23", "IF Performance IS VeryApropriate and Management IS Acceptable THEN Ventilation IS Acceptable");
            IS.NewRule("Rule 24", "IF Performance IS VeryApropriate and Management IS Adequate THEN Ventilation IS Adequate");
            IS.NewRule("Rule 25", "IF Performance IS VeryApropriate and Management IS VeryAdequate THEN Ventilation IS VeryAdequate");

            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Management", (float)managementValue);

            double resultado = IS.Evaluate("Ventilation");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Ventilation");
            }
            double m = (IS.GetLinguisticVariable("Ventilation").End - IS.GetLinguisticVariable("Ventilation").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Ventilation").End;
            
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