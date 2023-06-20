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
    public class CondenserController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ICondenserRepository _CondenserRepository;
        public CondenserController(IConfiguration configuration, ICondenserRepository CondenserRepository)
        {
            Configuration = configuration;
            _CondenserRepository = CondenserRepository;
        }

        [HttpPost]
        public ActionResult<Condenser> InsertCondenser([FromBody] Condenser condenser)
        {
            try
            {
                condenser.Management = CalculateManagement(Convert.ToDouble(condenser.Local), Convert.ToDouble(condenser.Ventilation), Convert.ToDouble(condenser.Isolation));
                condenser.Thermodynamics = CalculateThermodynamics(Convert.ToDouble(condenser.Heat), Convert.ToDouble(condenser.Condensed), Convert.ToDouble(condenser.Pressure));
                condenser.CondenserValue = CalculateCondenser(Convert.ToDouble(condenser.Management), Convert.ToDouble(condenser.Thermodynamics));

                return _CondenserRepository.InsertCondenser(condenser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Condenser> GetCondenserById(int id)
        {
            try
            {
                return _CondenserRepository.GetCondenserById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<Condenser>> GetCondenserByIndustry(string industryName)
        {
            try
            {
                return _CondenserRepository.GetCondenserByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double localValue, double ventilationValue, double isolationValue)
        {
            LinguisticVariable local = new( "Local", 0, 10 );
            local.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            local.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            local.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable ventilation = new( "Ventilation", 0, 10 );
            ventilation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            ventilation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            ventilation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable isolation = new( "Isolation", 0, 10 );
            isolation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            isolation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            isolation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( local );
            fuzzyDB.AddVariable( ventilation );
            fuzzyDB.AddVariable( isolation );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Local IS Low and Ventilation IS Low and Isolation IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 2", "IF Local IS Low and Ventilation IS Low and Isolation IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 3", "IF Local IS Low and Ventilation IS Low and Isolation IS High THEN Management IS Adequate");
            IS.NewRule("Rule 4", "IF Local IS Low and Ventilation IS Medium and Isolation IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Local IS Low and Ventilation IS Medium and Isolation IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 6", "IF Local IS Low and Ventilation IS Medium and Isolation IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 7", "IF Local IS Low and Ventilation IS High and Isolation IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Local IS Low and Ventilation IS High and Isolation IS Medium THEN Management IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Local IS Low and Ventilation IS High and Isolation IS High THEN Management IS Inadequate");
            IS.NewRule("Rule 10", "IF Local IS Medium and Ventilation IS Low and Isolation IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 11", "IF Local IS Medium and Ventilation IS Low and Isolation IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 12", "IF Local IS Medium and Ventilation IS Low and Isolation IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 13", "IF Local IS Medium and Ventilation IS Medium and Isolation IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 14", "IF Local IS Medium and Ventilation IS Medium and Isolation IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 15", "IF Local IS Medium and Ventilation IS Medium and Isolation IS High THEN Management IS Adequate");
            IS.NewRule("Rule 16", "IF Local IS Medium and Ventilation IS High and Isolation IS Low THEN Management IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Local IS Medium and Ventilation IS High and Isolation IS Medium THEN Management IS Inadequate");
            IS.NewRule("Rule 18", "IF Local IS Medium and Ventilation IS High and Isolation IS High THEN Management IS Acceptable");
            IS.NewRule("Rule 19", "IF Local IS High and Ventilation IS Low and Isolation IS Low THEN Management IS Adequate");
            IS.NewRule("Rule 20", "IF Local IS High and Ventilation IS Low and Isolation IS Medium THEN Management IS VeryAdequate");
            IS.NewRule("Rule 21", "IF Local IS High and Ventilation IS Low and Isolation IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 22", "IF Local IS High and Ventilation IS Medium and Isolation IS Low THEN Management IS Acceptable");
            IS.NewRule("Rule 23", "IF Local IS High and Ventilation IS Medium and Isolation IS Medium THEN Management IS Adequate");
            IS.NewRule("Rule 24", "IF Local IS High and Ventilation IS Medium and Isolation IS High THEN Management IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Local IS High and Ventilation IS High and Isolation IS Low THEN Management IS Inadequate");
            IS.NewRule("Rule 26", "IF Local IS High and Ventilation IS High and Isolation IS Medium THEN Management IS Acceptable");
            IS.NewRule("Rule 27", "IF Local IS High and Ventilation IS High and Isolation IS High THEN Management IS Adequate");

            IS.SetInput("Local", (float)localValue);
            IS.SetInput("Ventilation", (float)ventilationValue);
            IS.SetInput("Isolation", (float)isolationValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Local", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Ventilation", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Isolation", i == 0 ? (float)9.99 : 0);
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

            IS.NewRule("Rule 1", "IF Heat IS Low and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 2", "IF Heat IS Low and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 3", "IF Heat IS Low and Condensed IS Low and Pressure IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 4", "IF Heat IS Low and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Heat IS Low and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 6", "IF Heat IS Low and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 7", "IF Heat IS Low and Condensed IS High and Pressure IS Low THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 8", "IF Heat IS Low and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 9", "IF Heat IS Low and Condensed IS High and Pressure IS High THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 10", "IF Heat IS Medium and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 11", "IF Heat IS Medium and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 12", "IF Heat IS Medium and Condensed IS Low and Pressure IS High THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 13", "IF Heat IS Medium and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 14", "IF Heat IS Medium and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 15", "IF Heat IS Medium and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 16", "IF Heat IS Medium and Condensed IS High and Pressure IS Low THEN Thermodynamics IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Heat IS Medium and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 18", "IF Heat IS Medium and Condensed IS High and Pressure IS High THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 19", "IF Heat IS High and Condensed IS Low and Pressure IS Low THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 20", "IF Heat IS High and Condensed IS Low and Pressure IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 21", "IF Heat IS High and Condensed IS Low and Pressure IS High THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 22", "IF Heat IS High and Condensed IS Medium and Pressure IS Low THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 23", "IF Heat IS High and Condensed IS Medium and Pressure IS Medium THEN Thermodynamics IS Adequate");
            IS.NewRule("Rule 24", "IF Heat IS High and Condensed IS Medium and Pressure IS High THEN Thermodynamics IS VeryAdequate");
            IS.NewRule("Rule 25", "IF Heat IS High and Condensed IS High and Pressure IS Low THEN Thermodynamics IS Inadequate");
            IS.NewRule("Rule 26", "IF Heat IS High and Condensed IS High and Pressure IS Medium THEN Thermodynamics IS Acceptable");
            IS.NewRule("Rule 27", "IF Heat IS High and Condensed IS High and Pressure IS High THEN Thermodynamics IS Adequate");

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
        
        public string CalculateCondenser(double managementValue, double thermodynamicsValue)
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

            LinguisticVariable condenser = new( "Condenser", 0, 10 );
            condenser.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            condenser.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            condenser.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            condenser.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            condenser.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( thermodynamics );
            fuzzyDB.AddVariable( condenser );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Management IS VeryInadequate and Thermodynamics IS VeryInadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Management IS VeryInadequate and Thermodynamics IS Inadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Management IS VeryInadequate and Thermodynamics IS Acceptable THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Management IS VeryInadequate and Thermodynamics IS Adequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Management IS VeryInadequate and Thermodynamics IS VeryAdequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Management IS Inadequate and Thermodynamics IS VeryInadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Management IS Inadequate and Thermodynamics IS Inadequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 8", "IF Management IS Inadequate and Thermodynamics IS Acceptable THEN Condenser IS Inadequate");
            IS.NewRule("Rule 9", "IF Management IS Inadequate and Thermodynamics IS Adequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 10", "IF Management IS Inadequate and Thermodynamics IS VeryAdequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 11", "IF Management IS Acceptable and Thermodynamics IS VeryInadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Management IS Acceptable and Thermodynamics IS Inadequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 13", "IF Management IS Acceptable and Thermodynamics IS Acceptable THEN Condenser IS Acceptable");
            IS.NewRule("Rule 14", "IF Management IS Acceptable and Thermodynamics IS Adequate THEN Condenser IS Acceptable");
            IS.NewRule("Rule 15", "IF Management IS Acceptable and Thermodynamics IS VeryAdequate THEN Condenser IS Acceptable");
            IS.NewRule("Rule 16", "IF Management IS Adequate and Thermodynamics IS VeryInadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Management IS Adequate and Thermodynamics IS Inadequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 18", "IF Management IS Adequate and Thermodynamics IS Acceptable THEN Condenser IS Acceptable");
            IS.NewRule("Rule 19", "IF Management IS Adequate and Thermodynamics IS Adequate THEN Condenser IS Adequate");
            IS.NewRule("Rule 20", "IF Management IS Adequate and Thermodynamics IS VeryAdequate THEN Condenser IS Adequate");
            IS.NewRule("Rule 21", "IF Management IS VeryAdequate and Thermodynamics IS VeryInadequate THEN Condenser IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Management IS VeryAdequate and Thermodynamics IS Inadequate THEN Condenser IS Inadequate");
            IS.NewRule("Rule 23", "IF Management IS VeryAdequate and Thermodynamics IS Acceptable THEN Condenser IS Acceptable");
            IS.NewRule("Rule 24", "IF Management IS VeryAdequate and Thermodynamics IS Adequate THEN Condenser IS Adequate");
            IS.NewRule("Rule 25", "IF Management IS VeryAdequate and Thermodynamics IS VeryAdequate THEN Condenser IS VeryAdequate");

            IS.SetInput("Management", (float)managementValue);
            IS.SetInput("Thermodynamics", (float)thermodynamicsValue);

            double resultado = IS.Evaluate("Condenser");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Thermodynamics", i == 0 ? 0 : (float)9.99);
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
    }
}