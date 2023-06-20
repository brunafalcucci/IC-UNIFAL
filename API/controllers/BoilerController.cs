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
    public class BoilerController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IBoilerRepository _BoilerRepository;
        public BoilerController(IConfiguration configuration, IBoilerRepository BoilerRepository)
        {
            Configuration = configuration;
            _BoilerRepository = BoilerRepository;
        }

        [HttpPost]
        public ActionResult<Boiler> InsertBoiler([FromBody] Boiler boiler)
        {
            try
            {
                boiler.Management = CalculateManagement(Convert.ToDouble(boiler.Inspection), Convert.ToDouble(boiler.Maintenance));
                boiler.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(boiler.Heat), Convert.ToDouble(boiler.Condensed), Convert.ToDouble(boiler.Pressure));
                boiler.Performance = CalculatePerformance(Convert.ToDouble(boiler.StudiesAndMeasures), Convert.ToDouble(boiler.Place), Convert.ToDouble(boiler.Management));
                boiler.BoilerValue = CalculateBoiler(Convert.ToDouble(boiler.Performance), Convert.ToDouble(boiler.Thermodynamics));

                return _BoilerRepository.InsertBoiler(boiler);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Boiler> GetBoilerById(int id)
        {
            try
            {
                return _BoilerRepository.GetBoilerById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<Boiler>> GetBoilerByIndustry(string industryName)
        {
            try
            {
                return _BoilerRepository.GetBoilerByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double inspectionValue, double maintenanceValue)
        {
            LinguisticVariable inspection = new( "Inspection", 0, 10 );
            inspection.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            inspection.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            inspection.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            inspection.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            inspection.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            maintenance.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            maintenance.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            maintenance.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( inspection );
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Inspection IS VeryLow and Maintenance IS VeryLow THEN Management IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Inspection IS VeryLow and Maintenance IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Inspection IS VeryLow and Maintenance IS Middle THEN Management IS Inadequate");
            IS.NewRule("Rule 4", "IF Inspection IS VeryLow and Maintenance IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 5", "IF Inspection IS VeryLow and Maintenance IS VeryHigh THEN Management IS Acceptable");
            IS.NewRule("Rule 6", "IF Inspection IS Low and Maintenance IS VeryLow THEN Management IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Inspection IS Low and Maintenance IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 8", "IF Inspection IS Low and Maintenance IS Middle THEN Management IS Inadequate");
            IS.NewRule("Rule 9", "IF Inspection IS Low and Maintenance IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 10", "IF Inspection IS Low and Maintenance IS VeryHigh THEN Management IS Adequate");
            IS.NewRule("Rule 11", "IF Inspection IS Middle and Maintenance IS VeryLow THEN Management IS Inadequate");
            IS.NewRule("Rule 12", "IF Inspection IS Middle and Maintenance IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 13", "IF Inspection IS Middle and Maintenance IS Middle THEN Management IS Acceptable");
            IS.NewRule("Rule 14", "IF Inspection IS Middle and Maintenance IS High THEN Management IS Adequate");
            IS.NewRule("Rule 15", "IF Inspection IS Middle and Maintenance IS VeryHigh THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF Inspection IS High and Maintenance IS VeryLow THEN Management IS Inadequate");
            IS.NewRule("Rule 17", "IF Inspection IS High and Maintenance IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 18", "IF Inspection IS High and Maintenance IS Middle THEN Management IS Adequate");
            IS.NewRule("Rule 19", "IF Inspection IS High and Maintenance IS High THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF Inspection IS High and Maintenance IS VeryHigh THEN Management IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Inspection IS VeryHigh and Maintenance IS VeryLow THEN Management IS Acceptable");
            IS.NewRule("Rule 22", "IF Inspection IS VeryHigh and Maintenance IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 23", "IF Inspection IS VeryHigh and Maintenance IS Middle THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF Inspection IS VeryHigh and Maintenance IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Inspection IS VeryHigh and Maintenance IS VeryHigh THEN Management IS VeryAdequate");

            IS.SetInput("Inspection", (float)inspectionValue);
            IS.SetInput("Maintenance", (float)maintenanceValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Inspection", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Maintenance", i == 0 ? 0 : (float)9.99);
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

        public string CalculateThermodynamics(double heatValue, double condensedValue, double pressureValue)
        {
            LinguisticVariable heat = new( "Heat", 0, 10 );
            heat.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            heat.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            heat.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable condensed = new( "Condensed", 0, 10 );
            condensed.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            condensed.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            condensed.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable pressure = new( "Pressure", 0, 10 );
            pressure.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            pressure.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            pressure.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable thermodynamics = new( "Thermodynamics", 0, 10 );
            thermodynamics.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            thermodynamics.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            thermodynamics.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            thermodynamics.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            thermodynamics.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( heat );
            fuzzyDB.AddVariable( condensed );
            fuzzyDB.AddVariable( pressure );
            fuzzyDB.AddVariable( thermodynamics );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Heat IS Low and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 2", "IF Heat IS Low and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 3", "IF Heat IS Low and Condensed IS Low and Pressure IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 4", "IF Heat IS Low and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 5", "IF Heat IS Low and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 6", "IF Heat IS Low and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Heat IS Low and Condensed IS High and Pressure IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 8", "IF Heat IS Low and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Heat IS Low and Condensed IS High and Pressure IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 10", "IF Heat IS Medium and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 11", "IF Heat IS Medium and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 12", "IF Heat IS Medium and Condensed IS Low and Pressure IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 13", "IF Heat IS Medium and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 14", "IF Heat IS Medium and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 15", "IF Heat IS Medium and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 16", "IF Heat IS Medium and Condensed IS High and Pressure IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 17", "IF Heat IS Medium and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 18", "IF Heat IS Medium and Condensed IS High and Pressure IS High THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 19", "IF Heat IS High and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 20", "IF Heat IS High and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Heat IS High and Condensed IS Low and Pressure IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 22", "IF Heat IS High and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 23", "IF Heat IS High and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 24", "IF Heat IS High and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 25", "IF Heat IS High and Condensed IS High and Pressure IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 26", "IF Heat IS High and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 27", "IF Heat IS High and Condensed IS High and Pressure IS High THEN Thermodynamics IS Inadequate");

            IS.SetInput("Heat", (float)heatValue);
            IS.SetInput("Condensed", (float)condensedValue);
            IS.SetInput("Pressure", (float)pressureValue);

            double resultado = IS.Evaluate("Thermodynamics");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Heat", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Condensed", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Pressure", i == 0 ? (float)9.99 : 0);
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

        public string CalculatePerformance(double studiesAndMeasuresValue, double placeValue, double managementValue)
        {
            LinguisticVariable studiesAndMeasures = new( "StudiesAndMeasures", 0, 10 );
            studiesAndMeasures.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            studiesAndMeasures.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            studiesAndMeasures.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable place = new( "Place", 0, 10 );
            place.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            place.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            place.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            management.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            management.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( studiesAndMeasures );
            fuzzyDB.AddVariable( place );
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF StudiesAndMeasures IS Low and Place IS Low and Management IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 2", "IF StudiesAndMeasures IS Low and Place IS Low and Management IS Medium THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 3", "IF StudiesAndMeasures IS Low and Place IS Low and Management IS High THEN Performance IS Inadequate");
            IS.NewRule("Rule 4", "IF StudiesAndMeasures IS Low and Place IS Medium and Management IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 5", "IF StudiesAndMeasures IS Low and Place IS Medium and Management IS Medium THEN Performance IS Inadequate");
            IS.NewRule("Rule 6", "IF StudiesAndMeasures IS Low and Place IS Medium and Management IS High THEN Performance IS Acceptable");
            IS.NewRule("Rule 7", "IF StudiesAndMeasures IS Low and Place IS High and Management IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 8", "IF StudiesAndMeasures IS Low and Place IS High and Management IS Medium THEN Performance IS Acceptable");
            IS.NewRule("Rule 9", "IF StudiesAndMeasures IS Low and Place IS High and Management IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 10", "IF StudiesAndMeasures IS Medium and Place IS Low and Management IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 11", "IF StudiesAndMeasures IS Medium and Place IS Low and Management IS Medium THEN Performance IS Inadequate");
            IS.NewRule("Rule 12", "IF StudiesAndMeasures IS Medium and Place IS Low and Management IS High THEN Performance IS Acceptable");
            IS.NewRule("Rule 13", "IF StudiesAndMeasures IS Medium and Place IS Medium and Management IS Low THEN Performance IS VeryInadequate");
            IS.NewRule("Rule 14", "IF StudiesAndMeasures IS Medium and Place IS Medium and Management IS Medium THEN Performance IS Acceptable");
            IS.NewRule("Rule 15", "IF StudiesAndMeasures IS Medium and Place IS Medium and Management IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 16", "IF StudiesAndMeasures IS Medium and Place IS High and Management IS Low THEN Performance IS Acceptable");
            IS.NewRule("Rule 17", "IF StudiesAndMeasures IS Medium and Place IS High and Management IS Medium THEN Performance IS Adequate");
            IS.NewRule("Rule 18", "IF StudiesAndMeasures IS Medium and Place IS High and Management IS High THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 19", "IF StudiesAndMeasures IS High and Place IS Low and Management IS Low THEN Performance IS Inadequate");
            IS.NewRule("Rule 20", "IF StudiesAndMeasures IS High and Place IS Low and Management IS Medium THEN Performance IS Acceptable");
            IS.NewRule("Rule 21", "IF StudiesAndMeasures IS High and Place IS Low and Management IS High THEN Performance IS Adequate");
            IS.NewRule("Rule 22", "IF StudiesAndMeasures IS High and Place IS Medium and Management IS Low THEN Performance IS Acceptable");
            IS.NewRule("Rule 23", "IF StudiesAndMeasures IS High and Place IS Medium and Management IS Medium THEN Performance IS Adequate");
            IS.NewRule("Rule 24", "IF StudiesAndMeasures IS High and Place IS Medium and Management IS High THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 25", "IF StudiesAndMeasures IS High and Place IS High and Management IS Low THEN Performance IS Adequate");
            IS.NewRule("Rule 26", "IF StudiesAndMeasures IS High and Place IS High and Management IS Medium THEN Performance IS VeryAdequate");
            IS.NewRule("Rule 27", "IF StudiesAndMeasures IS High and Place IS High and Management IS High THEN Performance IS VeryAdequate");

            IS.SetInput("StudiesAndMeasures", (float)studiesAndMeasuresValue);
            IS.SetInput("Place", (float)placeValue);
            IS.SetInput("Management", (float)managementValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("StudiesAndMeasures", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Place", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
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

        public string CalculateBoiler(double performanceValue, double thermodynamicsValue)
        {
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

            LinguisticVariable boiler = new( "Boiler", 0, 10 );
            boiler.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            boiler.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            boiler.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            boiler.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            boiler.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( boiler );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Performance IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Performance IS VeryInadequate and Thermodynamics IS Inadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Performance IS VeryInadequate and Thermodynamics IS Acceptable THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Performance IS VeryInadequate and Thermodynamics IS Adequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Performance IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Performance IS Inadequate and Thermodynamics IS VeryInadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Performance IS Inadequate and Thermodynamics IS Inadequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 8", "IF Performance IS Inadequate and Thermodynamics IS Acceptable THEN Boiler IS Inadequate");
            IS.NewRule("Rule 9", "IF Performance IS Inadequate and Thermodynamics IS Adequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 10", "IF Performance IS Inadequate and Thermodynamics IS VeryAdequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 11", "IF Performance IS Acceptable and Thermodynamics IS VeryInadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Performance IS Acceptable and Thermodynamics IS Inadequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 13", "IF Performance IS Acceptable and Thermodynamics IS Acceptable THEN Boiler IS Acceptable");
            IS.NewRule("Rule 14", "IF Performance IS Acceptable and Thermodynamics IS Adequate THEN Boiler IS Acceptable");
            IS.NewRule("Rule 15", "IF Performance IS Acceptable and Thermodynamics IS VeryAdequate THEN Boiler IS Acceptable");
            IS.NewRule("Rule 16", "IF Performance IS Adequate and Thermodynamics IS VeryInadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Performance IS Adequate and Thermodynamics IS Inadequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 18", "IF Performance IS Adequate and Thermodynamics IS Acceptable THEN Boiler IS Acceptable");
            IS.NewRule("Rule 19", "IF Performance IS Adequate and Thermodynamics IS Adequate THEN Boiler IS Adequate");
            IS.NewRule("Rule 20", "IF Performance IS Adequate and Thermodynamics IS VeryAdequate THEN Boiler IS Adequate");
            IS.NewRule("Rule 21", "IF Performance IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Boiler IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Performance IS VeryAdequate and Thermodynamics IS Inadequate THEN Boiler IS Inadequate");
            IS.NewRule("Rule 23", "IF Performance IS VeryAdequate and Thermodynamics IS Acceptable THEN Boiler IS Acceptable");
            IS.NewRule("Rule 24", "IF Performance IS VeryAdequate and Thermodynamics IS Adequate THEN Boiler IS Adequate");
            IS.NewRule("Rule 25", "IF Performance IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Boiler IS VeryAdequate");

            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);

            double resultado = IS.Evaluate("Boiler");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Boiler");
            }
            double m = (IS.GetLinguisticVariable("Boiler").End - IS.GetLinguisticVariable("Boiler").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Boiler").End;
            
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