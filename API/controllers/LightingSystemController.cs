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
    public class LightingSystemController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ILightingSystemRepository _LightingSystemRepository;
        public LightingSystemController(IConfiguration configuration, ILightingSystemRepository LightingSystemRepository)
        {
            Configuration = configuration;
            _LightingSystemRepository = LightingSystemRepository;
        }

        [HttpPost]
        public ActionResult<LightingSystem> InsertLightingSystem([FromBody] LightingSystem lightingSystem)
        {
            try
            {
                lightingSystem.Management = CalculateManagement(Convert.ToDouble(lightingSystem.Cleaning), Convert.ToDouble(lightingSystem.ConstructionStructure));
                lightingSystem.Performance = CalculatePerformance(Convert.ToDouble(lightingSystem.Reactor), Convert.ToDouble(lightingSystem.Operation));
                lightingSystem.LightingSystemValue = CalculateLightingSystem(Convert.ToDouble(lightingSystem.Performance), Convert.ToDouble(lightingSystem.Management));

                return _LightingSystemRepository.InsertLightingSystem(lightingSystem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<LightingSystem> GetLightingSystemById(int id)
        {
            try
            {
                return _LightingSystemRepository.GetLightingSystemById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<LightingSystem>> GetLightingSystemByIndustry(string industryName)
        {
            try
            {
                return _LightingSystemRepository.GetLightingSystemByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateManagement(double cleaningValue, double constructionStructureValue)
        {
            LinguisticVariable cleaning = new( "Cleaning", 0, 10 );
            cleaning.AddLabel( new FuzzySet( "VeryDirty", new TrapezoidalFunction(0, 0, 1, 3) ) );
            cleaning.AddLabel( new FuzzySet( "Dirty", new TrapezoidalFunction(1, 3, 5) ) );
            cleaning.AddLabel( new FuzzySet( "MiddleCleaning", new TrapezoidalFunction(3, 5, 7) ) );
            cleaning.AddLabel( new FuzzySet( "Clean", new TrapezoidalFunction(5, 7, 9) ) );
            cleaning.AddLabel( new FuzzySet( "VeryClean", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable constructionStructure = new( "ConstructionStructure", 0, 10 );
            constructionStructure.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            constructionStructure.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            constructionStructure.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            constructionStructure.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            constructionStructure.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable management = new( "Management", 0, 10 );
            management.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            management.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            management.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            management.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            management.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( cleaning );
            fuzzyDB.AddVariable( constructionStructure );
            fuzzyDB.AddVariable( management );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Cleaning IS VeryDirty and ConstructionStructure IS VeryLow THEN Management IS VeryLow");
            IS.NewRule("Rule 2", "IF Cleaning IS VeryDirty and ConstructionStructure IS Low THEN Management IS VeryLow");
            IS.NewRule("Rule 3", "IF Cleaning IS VeryDirty and ConstructionStructure IS Middle THEN Management IS Low");
            IS.NewRule("Rule 4", "IF Cleaning IS VeryDirty and ConstructionStructure IS High THEN Management IS Low");
            IS.NewRule("Rule 5", "IF Cleaning IS VeryDirty and ConstructionStructure IS VeryHigh THEN Management IS Middle");
            IS.NewRule("Rule 6", "IF Cleaning IS Dirty and ConstructionStructure IS VeryLow THEN Management IS VeryLow");
            IS.NewRule("Rule 7", "IF Cleaning IS Dirty and ConstructionStructure IS Low THEN Management IS Low");
            IS.NewRule("Rule 8", "IF Cleaning IS Dirty and ConstructionStructure IS Middle THEN Management IS Low");
            IS.NewRule("Rule 9", "IF Cleaning IS Dirty and ConstructionStructure IS High THEN Management IS Middle");
            IS.NewRule("Rule 10", "IF Cleaning IS Dirty and ConstructionStructure IS VeryHigh THEN Management IS High");
            IS.NewRule("Rule 11", "IF Cleaning IS MiddleCleaning and ConstructionStructure IS VeryLow THEN Management IS Low");
            IS.NewRule("Rule 12", "IF Cleaning IS MiddleCleaning and ConstructionStructure IS Low THEN Management IS Low");
            IS.NewRule("Rule 13", "IF Cleaning IS MiddleCleaning and ConstructionStructure IS Middle THEN Management IS Middle");
            IS.NewRule("Rule 14", "IF Cleaning IS MiddleCleaning and ConstructionStructure IS High THEN Management IS High");
            IS.NewRule("Rule 15", "IF Cleaning IS MiddleCleaning and ConstructionStructure IS VeryHigh THEN Management IS High");
            IS.NewRule("Rule 16", "IF Cleaning IS Clean and ConstructionStructure IS VeryLow THEN Management IS Low");
            IS.NewRule("Rule 17", "IF Cleaning IS Clean and ConstructionStructure IS Low THEN Management IS Middle");
            IS.NewRule("Rule 18", "IF Cleaning IS Clean and ConstructionStructure IS Middle THEN Management IS High");
            IS.NewRule("Rule 19", "IF Cleaning IS Clean and ConstructionStructure IS High THEN Management IS High");
            IS.NewRule("Rule 20", "IF Cleaning IS Clean and ConstructionStructure IS VeryHigh THEN Management IS VeryHigh");
            IS.NewRule("Rule 21", "IF Cleaning IS VeryClean and ConstructionStructure IS VeryLow THEN Management IS Middle");
            IS.NewRule("Rule 22", "IF Cleaning IS VeryClean and ConstructionStructure IS Low THEN Management IS High");
            IS.NewRule("Rule 23", "IF Cleaning IS VeryClean and ConstructionStructure IS Middle THEN Management IS High");
            IS.NewRule("Rule 24", "IF Cleaning IS VeryClean and ConstructionStructure IS High THEN Management IS VeryHigh");
            IS.NewRule("Rule 25", "IF Cleaning IS VeryClean and ConstructionStructure IS VeryHigh THEN Management IS VeryHigh");

            IS.SetInput("Cleaning", (float)cleaningValue);
            IS.SetInput("ConstructionStructure", (float)constructionStructureValue);

            double resultado = IS.Evaluate("Management");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Cleaning", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ConstructionStructure", i == 0 ? 0 : (float)9.99);
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

        public string CalculatePerformance(double reactorValue, double operationValue)
        {
            LinguisticVariable reactor = new( "Reactor", 0, 10 );
            reactor.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reactor.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reactor.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reactor.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reactor.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable operation = new( "Operation", 0, 10 );
            operation.AddLabel( new FuzzySet( "VeryInapropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            operation.AddLabel( new FuzzySet( "Inapropriate", new TrapezoidalFunction(1, 3, 5) ) );
            operation.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            operation.AddLabel( new FuzzySet( "Apropriate", new TrapezoidalFunction(5, 7, 9) ) );
            operation.AddLabel( new FuzzySet( "VeryApropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable performance = new( "Performance", 0, 10 );
            performance.AddLabel( new FuzzySet( "VeryInapropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            performance.AddLabel( new FuzzySet( "Inapropriate", new TrapezoidalFunction(1, 3, 5) ) );
            performance.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            performance.AddLabel( new FuzzySet( "Apropriate", new TrapezoidalFunction(5, 7, 9) ) );
            performance.AddLabel( new FuzzySet( "VeryApropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reactor );
            fuzzyDB.AddVariable( operation );
            fuzzyDB.AddVariable( performance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Reactor IS VeryLow and Operation IS VeryInapropriate THEN Performance IS Acceptable");
            IS.NewRule("Rule 2", "IF Reactor IS VeryLow and Operation IS Inapropriate THEN Performance IS Apropriate");
            IS.NewRule("Rule 3", "IF Reactor IS VeryLow and Operation IS Acceptable THEN Performance IS Apropriate");
            IS.NewRule("Rule 4", "IF Reactor IS VeryLow and Operation IS Apropriate THEN Performance IS VeryApropriate");
            IS.NewRule("Rule 5", "IF Reactor IS VeryLow and Operation IS VeryApropriate THEN Performance IS VeryApropriate");
            IS.NewRule("Rule 6", "IF Reactor IS Low and Operation IS VeryInapropriate THEN Performance IS Inapropriate");
            IS.NewRule("Rule 7", "IF Reactor IS Low and Operation IS Inapropriate THEN Performance IS Acceptable");
            IS.NewRule("Rule 8", "IF Reactor IS Low and Operation IS Acceptable THEN Performance IS Apropriate");
            IS.NewRule("Rule 9", "IF Reactor IS Low and Operation IS Apropriate THEN Performance IS Apropriate");
            IS.NewRule("Rule 10", "IF Reactor IS Low and Operation IS VeryApropriate THEN Performance IS VeryApropriate");
            IS.NewRule("Rule 11", "IF Reactor IS Middle and Operation IS VeryInapropriate THEN Performance IS Inapropriate");
            IS.NewRule("Rule 12", "IF Reactor IS Middle and Operation IS Inapropriate THEN Performance IS Inapropriate");
            IS.NewRule("Rule 13", "IF Reactor IS Middle and Operation IS Acceptable THEN Performance IS Acceptable");
            IS.NewRule("Rule 14", "IF Reactor IS Middle and Operation IS Apropriate THEN Performance IS Apropriate");
            IS.NewRule("Rule 15", "IF Reactor IS Middle and Operation IS VeryApropriate THEN Performance IS Apropriate");
            IS.NewRule("Rule 16", "IF Reactor IS High and Operation IS VeryInapropriate THEN Performance IS VeryInapropriate");
            IS.NewRule("Rule 17", "IF Reactor IS High and Operation IS Inapropriate THEN Performance IS Inapropriate");
            IS.NewRule("Rule 18", "IF Reactor IS High and Operation IS Acceptable THEN Performance IS Inapropriate");
            IS.NewRule("Rule 19", "IF Reactor IS High and Operation IS Apropriate THEN Performance IS Acceptable");
            IS.NewRule("Rule 20", "IF Reactor IS High and Operation IS VeryApropriate THEN Performance IS Apropriate");
            IS.NewRule("Rule 21", "IF Reactor IS VeryHigh and Operation IS VeryInapropriate THEN Performance IS VeryInapropriate");
            IS.NewRule("Rule 22", "IF Reactor IS VeryHigh and Operation IS Inapropriate THEN Performance IS VeryInapropriate");
            IS.NewRule("Rule 23", "IF Reactor IS VeryHigh and Operation IS Acceptable THEN Performance IS Inapropriate");
            IS.NewRule("Rule 24", "IF Reactor IS VeryHigh and Operation IS Apropriate THEN Performance IS Inapropriate");
            IS.NewRule("Rule 25", "IF Reactor IS VeryHigh and Operation IS VeryApropriate THEN Performance IS Acceptable");

            IS.SetInput("Reactor", (float)reactorValue);
            IS.SetInput("Operation", (float)operationValue);

            double resultado = IS.Evaluate("Performance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Reactor", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Operation", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Performance");
            }
            double m = (IS.GetLinguisticVariable("Performance").End - IS.GetLinguisticVariable("Performance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Performance").Start;
            
            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Inapropriate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Inapropriate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Apropriate";
            }
            else
            {
                return resultado.ToString() + " - Very Apropriate";
            }
        }

        public string CalculateLightingSystem(double performanceValue, double managementValue)
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

            LinguisticVariable lightingSystem = new( "LightingSystem", 0, 10 );
            lightingSystem.AddLabel( new FuzzySet( "VeryInadequate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            lightingSystem.AddLabel( new FuzzySet( "Inadequate", new TrapezoidalFunction(1, 3, 5) ) );
            lightingSystem.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            lightingSystem.AddLabel( new FuzzySet( "Adequate", new TrapezoidalFunction(5, 7, 9) ) );
            lightingSystem.AddLabel( new FuzzySet( "VeryAdequate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( performance );
            fuzzyDB.AddVariable( management );
            fuzzyDB.AddVariable( lightingSystem );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Performance IS VeryInapropriate and Management IS VeryInadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 2", "IF Performance IS VeryInapropriate and Management IS Inadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 3", "IF Performance IS VeryInapropriate and Management IS Acceptable THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 4", "IF Performance IS VeryInapropriate and Management IS Adequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 5", "IF Performance IS VeryInapropriate and Management IS VeryAdequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 6", "IF Performance IS Inapropriate and Management IS VeryInadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 7", "IF Performance IS Inapropriate and Management IS Inadequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 8", "IF Performance IS Inapropriate and Management IS Acceptable THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 9", "IF Performance IS Inapropriate and Management IS Adequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 10", "IF Performance IS Inapropriate and Management IS VeryAdequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 11", "IF Performance IS Acceptable and Management IS VeryInadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 12", "IF Performance IS Acceptable and Management IS Inadequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 13", "IF Performance IS Acceptable and Management IS Acceptable THEN LightingSystem IS Acceptable");
            IS.NewRule("Rule 14", "IF Performance IS Acceptable and Management IS Adequate THEN LightingSystem IS Acceptable");
            IS.NewRule("Rule 15", "IF Performance IS Acceptable and Management IS VeryAdequate THEN LightingSystem IS Acceptable");
            IS.NewRule("Rule 16", "IF Performance IS Apropriate and Management IS VeryInadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 17", "IF Performance IS Apropriate and Management IS Inadequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 18", "IF Performance IS Apropriate and Management IS Acceptable THEN LightingSystem IS Acceptable");
            IS.NewRule("Rule 19", "IF Performance IS Apropriate and Management IS Adequate THEN LightingSystem IS Adequate");
            IS.NewRule("Rule 20", "IF Performance IS Apropriate and Management IS VeryAdequate THEN LightingSystem IS Adequate");
            IS.NewRule("Rule 21", "IF Performance IS VeryApropriate and Management IS VeryInadequate THEN LightingSystem IS VeryInadequate");
            IS.NewRule("Rule 22", "IF Performance IS VeryApropriate and Management IS Inadequate THEN LightingSystem IS Inadequate");
            IS.NewRule("Rule 23", "IF Performance IS VeryApropriate and Management IS Acceptable THEN LightingSystem IS Acceptable");
            IS.NewRule("Rule 24", "IF Performance IS VeryApropriate and Management IS Adequate THEN LightingSystem IS Adequate");
            IS.NewRule("Rule 25", "IF Performance IS VeryApropriate and Management IS VeryAdequate THEN LightingSystem IS VeryAdequate");

            IS.SetInput("Performance", (float)performanceValue);
            IS.SetInput("Management", (float)managementValue);

            double resultado = IS.Evaluate("LightingSystem");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Performance", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Management", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("LightingSystem");
            }
            double m = (IS.GetLinguisticVariable("LightingSystem").End - IS.GetLinguisticVariable("LightingSystem").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("LightingSystem").End;
            
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