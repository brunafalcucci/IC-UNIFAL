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
    public class CriticalityIndexController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly ICriticalityIndexRepository _CriticalityIndexRepository;
        public CriticalityIndexController(IConfiguration configuration, ICriticalityIndexRepository CriticalityIndexRepository)
        {
            Configuration = configuration;
            _CriticalityIndexRepository = CriticalityIndexRepository;
        }

        [HttpPost]
        public ActionResult<bool> InsertCriticalityIndex([FromBody] CriticalityIndex criticalityIndex)
        {
            try
            {
                return _CriticalityIndexRepository.InsertCriticalityIndex(criticalityIndex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteCriticalityIndex(int id)
        {
            try
            {
                return _CriticalityIndexRepository.DeleteCriticalityIndex(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CriticalityIndex> GetCriticalityIndexById(int id)
        {
            try
            {
                return _CriticalityIndexRepository.GetCriticalityIndexById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{city}")]
        public ActionResult<List<CriticalityIndex>> GetCriticalityIndexByCity(string city)
        {
            try
            {
                return _CriticalityIndexRepository.GetCriticalityIndexByCity(city);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public double CalculateEnvironmentalRisk(double solubilityValue, double toxicityValue)
        {
            LinguisticVariable solubility = new( "Solubility", 0, 33 );
            solubility.AddLabel( new FuzzySet( "VeryInsoluble", new TrapezoidalFunction(0, 0, (float)3.3, (float)9.9) ) );
            solubility.AddLabel( new FuzzySet( "Insoluble", new TrapezoidalFunction((float)3.3, (float)9.9, (float)16.5) ) );
            solubility.AddLabel( new FuzzySet( "AverageSoluble", new TrapezoidalFunction((float)9.9, (float)16.5, (float)23.1) ) );
            solubility.AddLabel( new FuzzySet( "Soluble", new TrapezoidalFunction((float)16.5, (float)23.1, (float)29.7) ) );
            solubility.AddLabel( new FuzzySet( "VerySoluble", new TrapezoidalFunction((float)23.1, (float)29.7, 33, 33) ) );

            LinguisticVariable toxicity = new( "Toxicity", 0, 45 );
            toxicity.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, (float)4.5, (float)13.5) ) );
            toxicity.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction((float)4.5, (float)13.5, (float)22.5) ) );
            toxicity.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)13.5, (float)22.5, (float)31.5) ) );
            toxicity.AddLabel( new FuzzySet( "High", new TrapezoidalFunction((float)22.5, (float)31.5, (float)40.5) ) );
            toxicity.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction((float)31.5, (float)40.5, 45, 45) ) );

            LinguisticVariable environmentalRisk = new( "EnvironmentalRisk", 0, 10 );
            environmentalRisk.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            environmentalRisk.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            environmentalRisk.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            environmentalRisk.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            environmentalRisk.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( solubility );
            fuzzyDB.AddVariable( toxicity );
            fuzzyDB.AddVariable( environmentalRisk );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Solubility IS VeryInsoluble and Toxicity IS VeryLow THEN EnvironmentalRisk IS VeryLow");
            IS.NewRule("Rule 2", "IF Solubility IS VeryInsoluble and Toxicity IS Low THEN EnvironmentalRisk IS VeryLow");
            IS.NewRule("Rule 3", "IF Solubility IS VeryInsoluble and Toxicity IS Medium THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 4", "IF Solubility IS VeryInsoluble and Toxicity IS High THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 5", "IF Solubility IS VeryInsoluble and Toxicity IS VeryHigh THEN EnvironmentalRisk IS Middle");
            IS.NewRule("Rule 6", "IF Solubility IS Insoluble and Toxicity IS VeryLow THEN EnvironmentalRisk IS VeryLow");
            IS.NewRule("Rule 7", "IF Solubility IS Insoluble and Toxicity IS Low THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 8", "IF Solubility IS Insoluble and Toxicity IS Medium THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 9", "IF Solubility IS Insoluble and Toxicity IS High THEN EnvironmentalRisk IS Middle");
            IS.NewRule("Rule 10", "IF Solubility IS Insoluble and Toxicity IS VeryHigh THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 11", "IF Solubility IS AverageSoluble and Toxicity IS VeryLow THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 12", "IF Solubility IS AverageSoluble and Toxicity IS Low THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 13", "IF Solubility IS AverageSoluble and Toxicity IS Medium THEN EnvironmentalRisk IS Middle");
            IS.NewRule("Rule 14", "IF Solubility IS AverageSoluble and Toxicity IS High THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 15", "IF Solubility IS AverageSoluble and Toxicity IS VeryHigh THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 16", "IF Solubility IS Soluble and Toxicity IS VeryLow THEN EnvironmentalRisk IS Low");
            IS.NewRule("Rule 17", "IF Solubility IS Soluble and Toxicity IS Low THEN EnvironmentalRisk IS Middle");
            IS.NewRule("Rule 18", "IF Solubility IS Soluble and Toxicity IS Medium THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 19", "IF Solubility IS Soluble and Toxicity IS High THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 20", "IF Solubility IS Soluble and Toxicity IS VeryHigh THEN EnvironmentalRisk IS VeryHigh");
            IS.NewRule("Rule 21", "IF Solubility IS VerySoluble and Toxicity IS VeryLow THEN EnvironmentalRisk IS Middle");
            IS.NewRule("Rule 22", "IF Solubility IS VerySoluble and Toxicity IS Low THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 23", "IF Solubility IS VerySoluble and Toxicity IS Medium THEN EnvironmentalRisk IS High");
            IS.NewRule("Rule 24", "IF Solubility IS VerySoluble and Toxicity IS High THEN EnvironmentalRisk IS VeryHigh");
            IS.NewRule("Rule 25", "IF Solubility IS VerySoluble and Toxicity IS VeryHigh THEN EnvironmentalRisk IS VeryHigh");

            IS.SetInput("Solubility", (float)solubilityValue);
            IS.SetInput("Toxicity", (float)toxicityValue);

            double resultado = IS.Evaluate("EnvironmentalRisk");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Solubility", i == 0 ? (float)32.99 : 0);
                IS.SetInput("Toxicity", i == 0 ? (float)44.99 : 0);
                input[i] = IS.Evaluate("EnvironmentalRisk");
            }
            double m = (IS.GetLinguisticVariable("EnvironmentalRisk").End - IS.GetLinguisticVariable("EnvironmentalRisk").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("EnvironmentalRisk").End;

            return resultado;
        }

        public static double NormalizeV2(double x, InferenceSystem fis, List<string> variablesName, int numberInputVariables, string outputVariableName, List<bool> boolValues)
        {
            if (boolValues.Count == 1)
            {
                boolValues = Enumerable.Repeat(boolValues[0], numberInputVariables).ToList();
            }
            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < numberInputVariables; j++)
                {
                    fis.SetInput(variablesName[j], i == 0 ? fis.GetLinguisticVariable(variablesName[j]).End : fis.GetLinguisticVariable(variablesName[j]).Start);
                }
                input[i] = fis.Evaluate(outputVariableName);
            }
            double m = (fis.GetLinguisticVariable(outputVariableName).End - fis.GetLinguisticVariable(outputVariableName).Start) / (input[1] - input[0]);
            double y = m * (x - input[0]) + fis.GetLinguisticVariable(outputVariableName).Start;
            return y;
        }
    }
}