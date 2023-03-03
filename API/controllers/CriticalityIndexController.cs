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
        public ActionResult<CriticalityIndex> InsertCriticalityIndex([FromBody] CriticalityIndex criticalityIndex)
        {
            try
            {
                criticalityIndex.Operation_Work = CalculateOperation(Convert.ToDouble(criticalityIndex.OperationSensitivity), Convert.ToDouble(criticalityIndex.MeanTimeBetweenFailures), Convert.ToDouble(criticalityIndex.AvailabilityOfRepairPersonnel), Convert.ToDouble(criticalityIndex.WorkLoad));
                criticalityIndex.Activities = CalculateActivities(Convert.ToDouble(criticalityIndex.AvailabilityOfRequiredParts), Convert.ToDouble(criticalityIndex.AverageRepairTime));
                criticalityIndex.Investiments = CalculateInvestiment(Convert.ToDouble(criticalityIndex.EnergyGeneration), Convert.ToDouble(criticalityIndex.Process), Convert.ToDouble(criticalityIndex.Technology));
                criticalityIndex.Predictive = CalculatePredictive(Convert.ToDouble(criticalityIndex.PredictiveOperation), Convert.ToDouble(criticalityIndex.TechnologyDataCollection), Convert.ToDouble(criticalityIndex.Instrumentation))
                criticalityIndex.Preventive = CalculatePreventive(Convert.ToDouble(criticalityIndex.Operation_Work.Split(" ")[0]), Convert.ToDouble(criticalityIndex.Activities.Split(" ")[0]));
                criticalityIndex.Governance = CalculateGovernance(Convert.ToDouble(criticalityIndex.SkillLevels), Convert.ToDouble(criticalityIndex.ManagementStrategy));
                criticalityIndex.EnvironmentalRisks = CalculateEnvironmentalRisk(Convert.ToDouble(criticalityIndex.Solubility), Convert.ToDouble(criticalityIndex.Toxicity));
                criticalityIndex.EnergyUse = CalculateEnergyUse(Convert.ToDouble(criticalityIndex.Renewable), Convert.ToDouble(criticalityIndex.No_Renewable));
                criticalityIndex.Maintenance = CalculateMaintenance(Convert.ToDouble(criticalityIndex.Corrective), Convert.ToDouble(criticalityIndex.Preventive.Split(" ")[0]), Convert.ToDouble(criticalityIndex.Predictive.Split(" ")[0]));
                criticalityIndex.CostsManagement = CalculateCostsManagement(Convert.ToDouble(criticalityIndex.ElectricityExpensives), Convert.ToDouble(criticalityIndex.Investments.Split(" ")[0]));
                criticalityIndex.IndustrialManagement = CalculateIndustrialManagement(Convert.ToDouble(criticalityIndex.Governance.Split(" ")[0]), Convert.ToDouble(criticalityIndex.Maintenance.Split(" ")[0]));
                criticalityIndex.EnvironmentalQuality = CalculateEnvironmentalQuality(Convert.ToDouble(criticalityIndex.EnvironmentalRisks.Split(" ")[0]), Convert.ToDouble(criticalityIndex.EnergyUse.Split(" ")[0]));
                criticalityIndex.CriticalityIndexValue = CalculateCriticalityIndex(Convert.ToDouble(criticalityIndex.EnvironmentalQuality.Split(" ")[0]), Convert.ToDouble(criticalityIndex.CostsManagement.Split(" ")[0]), Convert.ToDouble(criticalityIndex.IndustrialManagement.Split(" ")[0]));

                return _CriticalityIndexRepository.InsertCriticalityIndex(criticalityIndex);
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

        [HttpGet("{industryName}")]
        public ActionResult<List<CriticalityIndex>> GetCriticalityIndexByIndustry(string industryName)
        {
            try
            {
                return _CriticalityIndexRepository.GetCriticalityIndexByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateEnvironmentalRisk(double solubilityValue, double toxicityValue)
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

        public string CalculateEnergyUse(double renewableValue, double noRenewableValue)
        {
            LinguisticVariable renewable = new( "Renewable", 0, 100 );
            renewable.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            renewable.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            renewable.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(30, 50, 70) ) );
            renewable.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            renewable.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            LinguisticVariable noRenewable = new( "NoRenewable", 0, 100 );
            noRenewable.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            noRenewable.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            noRenewable.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(30, 50, 70) ) );
            noRenewable.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            noRenewable.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            LinguisticVariable energyUse = new( "EnergyUse", 0, 100 );
            energyUse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            energyUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            energyUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(30, 50, 70) ) );
            energyUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            energyUse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( renewable );
            fuzzyDB.AddVariable( noRenewable );
            fuzzyDB.AddVariable( energyUse );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Renewable IS VeryLow and NoRenewable IS VeryHigh THEN EnergyUse IS VeryLow");
            IS.NewRule("Rule 2", "IF Renewable IS Low and NoRenewable IS High THEN EnergyUse IS Low");
            IS.NewRule("Rule 3", "IF Renewable IS Medium and NoRenewable IS Medium THEN EnergyUse IS Medium");
            IS.NewRule("Rule 4", "IF Renewable IS High and NoRenewable IS Low THEN EnergyUse IS High");
            IS.NewRule("Rule 5", "IF Renewable IS VeryHigh and NoRenewable IS VeryLow THEN EnergyUse IS VeryHigh");

            IS.SetInput("Renewable", (float)renewableValue);
            IS.SetInput("NoRenewable", (float)noRenewableValue);

            double resultado = IS.Evaluate("EnergyUse");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Renewable", i == 0 ? (float)99.99 : 0);
                IS.SetInput("NoRenewable", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("EnergyUse");
            }
            double m = (IS.GetLinguisticVariable("EnergyUse").End - IS.GetLinguisticVariable("EnergyUse").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("EnergyUse").Start;

            if (resultado >= 0 && resultado <= 25)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 25 && resultado <= 40)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 40 && resultado <= 60)
            {
                return resultado.ToString() + " - Medium";
            }
            else if (resultado > 60 && resultado <= 80)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public string CalculateEnvironmentalQuality(double environmentalRiskValue, double energyUseValue)
        {
            LinguisticVariable environmentalRisk = new( "EnvironmentalRisk", 0, 10 );
            environmentalRisk.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            environmentalRisk.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            environmentalRisk.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            environmentalRisk.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            environmentalRisk.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable energyUse = new( "EnergyUse", 0, 100 );
            energyUse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            energyUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            energyUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(30, 50, 70) ) );
            energyUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            energyUse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            LinguisticVariable environmentalQuality = new( "EnvironmentalQuality", 0, 10 );
            environmentalQuality.AddLabel( new FuzzySet( "Inappropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            environmentalQuality.AddLabel( new FuzzySet( "PotentiallyInappropriate", new TrapezoidalFunction(1, 3, 5) ) );
            environmentalQuality.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            environmentalQuality.AddLabel( new FuzzySet( "PotentiallyAppropriate", new TrapezoidalFunction(5, 7, 9) ) );
            environmentalQuality.AddLabel( new FuzzySet( "Appropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( environmentalRisk );
            fuzzyDB.AddVariable( energyUse );
            fuzzyDB.AddVariable( environmentalQuality );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF EnvironmentalRisk IS VeryLow and EnergyUse IS VeryLow THEN EnvironmentalQuality IS Appropriate");
            IS.NewRule("Rule 2", "IF EnvironmentalRisk IS VeryLow and EnergyUse IS Low THEN EnvironmentalQuality IS Appropriate");
            IS.NewRule("Rule 3", "IF EnvironmentalRisk IS VeryLow and EnergyUse IS Medium THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 4", "IF EnvironmentalRisk IS VeryLow and EnergyUse IS High THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 5", "IF EnvironmentalRisk IS VeryLow and EnergyUse IS VeryHigh THEN EnvironmentalQuality IS Acceptable");
            IS.NewRule("Rule 6", "IF EnvironmentalRisk IS Low and EnergyUse IS VeryLow THEN EnvironmentalQuality IS Appropriate");
            IS.NewRule("Rule 7", "IF EnvironmentalRisk IS Low and EnergyUse IS Low THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 8", "IF EnvironmentalRisk IS Low and EnergyUse IS Medium THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 9", "IF EnvironmentalRisk IS Low and EnergyUse IS High THEN EnvironmentalQuality IS Acceptable");
            IS.NewRule("Rule 10", "IF EnvironmentalRisk IS Low and EnergyUse IS VeryHigh THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 11", "IF EnvironmentalRisk IS Medium and EnergyUse IS VeryLow THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 12", "IF EnvironmentalRisk IS Medium and EnergyUse IS Low THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 13", "IF EnvironmentalRisk IS Medium and EnergyUse IS Medium THEN EnvironmentalQuality IS Acceptable");
            IS.NewRule("Rule 14", "IF EnvironmentalRisk IS Medium and EnergyUse IS High THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 15", "IF EnvironmentalRisk IS Medium and EnergyUse IS VeryHigh THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 16", "IF EnvironmentalRisk IS High and EnergyUse IS VeryLow THEN EnvironmentalQuality IS PotentiallyAppropriate");
            IS.NewRule("Rule 17", "IF EnvironmentalRisk IS High and EnergyUse IS Low THEN EnvironmentalQuality IS Acceptable");
            IS.NewRule("Rule 18", "IF EnvironmentalRisk IS High and EnergyUse IS Medium THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 19", "IF EnvironmentalRisk IS High and EnergyUse IS High THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 20", "IF EnvironmentalRisk IS High and EnergyUse IS VeryHigh THEN EnvironmentalQuality IS Inappropriate");
            IS.NewRule("Rule 21", "IF EnvironmentalRisk IS VeryHigh and EnergyUse IS VeryLow THEN EnvironmentalQuality IS Acceptable");
            IS.NewRule("Rule 22", "IF EnvironmentalRisk IS VeryHigh and EnergyUse IS Low THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 23", "IF EnvironmentalRisk IS VeryHigh and EnergyUse IS Medium THEN EnvironmentalQuality IS PotentiallyInappropriate");
            IS.NewRule("Rule 24", "IF EnvironmentalRisk IS VeryHigh and EnergyUse IS High THEN EnvironmentalQuality IS Inappropriate");
            IS.NewRule("Rule 25", "IF EnvironmentalRisk IS VeryHigh and EnergyUse IS VeryHigh THEN EnvironmentalQuality IS Inappropriate");

            IS.SetInput("EnvironmentalRisk", (float)environmentalRiskValue);
            IS.SetInput("EnergyUse", (float)energyUseValue);

            double resultado = IS.Evaluate("EnvironmentalQuality");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("EnvironmentalRisk", i == 0 ? 0 : (float)9.99);
                IS.SetInput("EnergyUse", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("EnvironmentalQuality");
            }
            double m = (IS.GetLinguisticVariable("EnvironmentalQuality").End - IS.GetLinguisticVariable("EnvironmentalQuality").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("EnvironmentalQuality").Start;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Inappropriate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Potentially Inappropriate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Potentially Appropriate";
            }
            else
            {
                return resultado.ToString() + " - Appropriate";
            }
        }

        public string CalculateInvestiment(double energyGenerationValue, double processValue, double technologicalValue)
        {
            LinguisticVariable energyGeneration = new( "EnergyGeneration", 0, 100 );
            energyGeneration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            energyGeneration.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            energyGeneration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable process = new( "Process", 0, 100 );
            process.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            process.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            process.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable technological = new( "Technological", 0, 100 );
            technological.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            technological.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            technological.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable investiment = new( "Investiment", 0, 100 );
            investiment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            investiment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            investiment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(30, 50, 70) ) );
            investiment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            investiment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( energyGeneration );
            fuzzyDB.AddVariable( process );
            fuzzyDB.AddVariable( technological );
            fuzzyDB.AddVariable( investiment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF EnergyGeneration IS Low and Process IS Low and Technological IS Low THEN Investiment IS VeryLow");
            IS.NewRule("Rule 2", "IF EnergyGeneration IS Low and Process IS Low and Technological IS Medium THEN Investiment IS VeryLow");
            IS.NewRule("Rule 3", "IF EnergyGeneration IS Low and Process IS Low and Technological IS High THEN Investiment IS Low");
            IS.NewRule("Rule 4", "IF EnergyGeneration IS Low and Process IS Medium and Technological IS Low THEN Investiment IS VeryLow");
            IS.NewRule("Rule 5", "IF EnergyGeneration IS Low and Process IS Medium and Technological IS Medium THEN Investiment IS Low");
            IS.NewRule("Rule 6", "IF EnergyGeneration IS Low and Process IS Medium and Technological IS High THEN Investiment IS Middle");
            IS.NewRule("Rule 7", "IF EnergyGeneration IS Low and Process IS High and Technological IS Low THEN Investiment IS Low");
            IS.NewRule("Rule 8", "IF EnergyGeneration IS Low and Process IS High and Technological IS Medium THEN Investiment IS Middle");
            IS.NewRule("Rule 9", "IF EnergyGeneration IS Low and Process IS High and Technological IS High THEN Investiment IS High");
            IS.NewRule("Rule 10", "IF EnergyGeneration IS Medium and Process IS Low and Technological IS Low THEN Investiment IS VeryLow");
            IS.NewRule("Rule 11", "IF EnergyGeneration IS Medium and Process IS Low and Technological IS Medium THEN Investiment IS Low");
            IS.NewRule("Rule 12", "IF EnergyGeneration IS Medium and Process IS Low and Technological IS High THEN Investiment IS Middle");
            IS.NewRule("Rule 13", "IF EnergyGeneration IS Medium and Process IS Medium and Technological IS Low THEN Investiment IS Low");
            IS.NewRule("Rule 14", "IF EnergyGeneration IS Medium and Process IS Medium and Technological IS Medium THEN Investiment IS Middle");
            IS.NewRule("Rule 15", "IF EnergyGeneration IS Medium and Process IS Medium and Technological IS High THEN Investiment IS High");
            IS.NewRule("Rule 16", "IF EnergyGeneration IS Medium and Process IS High and Technological IS Low THEN Investiment IS Middle");
            IS.NewRule("Rule 17", "IF EnergyGeneration IS Medium and Process IS High and Technological IS Medium THEN Investiment IS High");
            IS.NewRule("Rule 18", "IF EnergyGeneration IS Medium and Process IS High and Technological IS High THEN Investiment IS VeryHigh");
            IS.NewRule("Rule 19", "IF EnergyGeneration IS High and Process IS Low and Technological IS Low THEN Investiment IS Low");
            IS.NewRule("Rule 20", "IF EnergyGeneration IS High and Process IS Low and Technological IS Medium THEN Investiment IS Middle");
            IS.NewRule("Rule 21", "IF EnergyGeneration IS High and Process IS Low and Technological IS High THEN Investiment IS High");
            IS.NewRule("Rule 22", "IF EnergyGeneration IS High and Process IS Medium and Technological IS Low THEN Investiment IS Middle");
            IS.NewRule("Rule 23", "IF EnergyGeneration IS High and Process IS Medium and Technological IS Medium THEN Investiment IS High");
            IS.NewRule("Rule 24", "IF EnergyGeneration IS High and Process IS Medium and Technological IS High THEN Investiment IS VeryHigh");
            IS.NewRule("Rule 25", "IF EnergyGeneration IS High and Process IS High and Technological IS Low THEN Investiment IS High");
            IS.NewRule("Rule 26", "IF EnergyGeneration IS High and Process IS High and Technological IS Medium THEN Investiment IS VeryHigh");
            IS.NewRule("Rule 27", "IF EnergyGeneration IS High and Process IS High and Technological IS High THEN Investiment IS VeryHigh");

            IS.SetInput("EnergyGeneration", (float)energyGenerationValue);
            IS.SetInput("Process", (float)processValue);
            IS.SetInput("Technological", (float)technologicalValue);

            double resultado = IS.Evaluate("Investiment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("EnergyGeneration", i == 0 ? 0 : (float)99.99);
                IS.SetInput("Process", i == 0 ? 0 : (float)99.99);
                IS.SetInput("Technological", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("Investiment");
            }
            double m = (IS.GetLinguisticVariable("Investiment").End - IS.GetLinguisticVariable("Investiment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Investiment").End;

            if (resultado >= 0 && resultado <= 25)
            {
                return resultado.ToString() + " - Very Low";
            }
            else if (resultado > 25 && resultado <= 40)
            {
                return resultado.ToString() + " - Low";
            }
            else if (resultado > 40 && resultado <= 60)
            {
                return resultado.ToString() + " - Middle";
            }
            else if (resultado > 60 && resultado <= 80)
            {
                return resultado.ToString() + " - High";
            }
            else
            {
                return resultado.ToString() + " - Very High";
            }
        }

        public double CalculateCostsManagement(double electricityCostsValue, double investimentValue)
        {
            LinguisticVariable electricityCosts = new( "ElectricityCosts", 0, 100 );
            electricityCosts.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            electricityCosts.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            electricityCosts.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(30, 50, 70) ) );
            electricityCosts.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            electricityCosts.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            LinguisticVariable investiment = new( "Investiment", 0, 100 );
            investiment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 10, 30) ) );
            investiment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(10, 30, 50) ) );
            investiment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(30, 50, 70) ) );
            investiment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 70, 90) ) );
            investiment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(70, 90, 100, 100) ) );

            LinguisticVariable costsManagement = new( "CostsManagement", 0, 10 );
            costsManagement.AddLabel( new FuzzySet( "Inappropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            costsManagement.AddLabel( new FuzzySet( "PotentiallyInappropriate", new TrapezoidalFunction(1, 3, 5) ) );
            costsManagement.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            costsManagement.AddLabel( new FuzzySet( "PotentiallyAppropriate", new TrapezoidalFunction(5, 7, 9) ) );
            costsManagement.AddLabel( new FuzzySet( "Appropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( electricityCosts );
            fuzzyDB.AddVariable( investiment );
            fuzzyDB.AddVariable( costsManagement );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ElectricityCosts IS VeryLow and Investiment IS VeryLow THEN CostsManagement IS PotentiallyAppropriate");
            IS.NewRule("Rule 2", "IF ElectricityCosts IS VeryLow and Investiment IS Low THEN CostsManagement IS Acceptable");
            IS.NewRule("Rule 3", "IF ElectricityCosts IS VeryLow and Investiment IS Middle THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 4", "IF ElectricityCosts IS VeryLow and Investiment IS High THEN CostsManagement IS Inappropriate");
            IS.NewRule("Rule 5", "IF ElectricityCosts IS VeryLow and Investiment IS VeryHigh THEN CostsManagement IS Inappropriate");
            IS.NewRule("Rule 6", "IF ElectricityCosts IS Low and Investiment IS VeryLow THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 7", "IF ElectricityCosts IS Low and Investiment IS Low THEN CostsManagement IS PotentiallyAppropriate");
            IS.NewRule("Rule 8", "IF ElectricityCosts IS Low and Investiment IS Middle THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 9", "IF ElectricityCosts IS Low and Investiment IS High THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 10", "IF ElectricityCosts IS Low and Investiment IS VeryHigh THEN CostsManagement IS Inappropriate");
            IS.NewRule("Rule 11", "IF ElectricityCosts IS Middle and Investiment IS VeryLow THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 12", "IF ElectricityCosts IS Middle and Investiment IS Low THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 13", "IF ElectricityCosts IS Middle and Investiment IS Middle THEN CostsManagement IS Acceptable");
            IS.NewRule("Rule 14", "IF ElectricityCosts IS Middle and Investiment IS High THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 15", "IF ElectricityCosts IS Middle and Investiment IS VeryHigh THEN CostsManagement IS Inappropriate");
            IS.NewRule("Rule 16", "IF ElectricityCosts IS High and Investiment IS VeryLow THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 17", "IF ElectricityCosts IS High and Investiment IS Low THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 18", "IF ElectricityCosts IS High and Investiment IS Middle THEN CostsManagement IS PotentiallyAppropriate");
            IS.NewRule("Rule 19", "IF ElectricityCosts IS High and Investiment IS High THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 20", "IF ElectricityCosts IS High and Investiment IS VeryHigh THEN CostsManagement IS PotentiallyInappropriate");
            IS.NewRule("Rule 21", "IF ElectricityCosts IS VeryHigh and Investiment IS VeryLow THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 22", "IF ElectricityCosts IS VeryHigh and Investiment IS Low THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 23", "IF ElectricityCosts IS VeryHigh and Investiment IS Middle THEN CostsManagement IS Appropriate");
            IS.NewRule("Rule 24", "IF ElectricityCosts IS VeryHigh and Investiment IS High THEN CostsManagement IS Acceptable");
            IS.NewRule("Rule 25", "IF ElectricityCosts IS VeryHigh and Investiment IS VeryHigh THEN CostsManagement IS PotentiallyInappropriate");

            IS.SetInput("ElectricityCosts", (float)electricityCostsValue);
            IS.SetInput("Investiment", (float)investimentValue);

            double resultado = IS.Evaluate("CostsManagement");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ElectricityCosts", i == 0 ? (float)99.99 : 0);
                IS.SetInput("Investiment", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("CostsManagement");
            }
            double m = (IS.GetLinguisticVariable("CostsManagement").End - IS.GetLinguisticVariable("CostsManagement").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("CostsManagement").Start;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Inappropriate";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Potentially Inappropriate";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Potentially Appropriate";
            }
            else
            {
                return resultado.ToString() + " - Appropriate";
            }
        }

        public string CalculateGovernance(double skillLevelValue, double managementStrategyValue)
        {
            LinguisticVariable skillLevel = new( "SkillLevel", 0, 5 );
            skillLevel.AddLabel( new FuzzySet( "VerySmall", new TrapezoidalFunction(0, 0, (float)0.5, (float)1.5) ) );
            skillLevel.AddLabel( new FuzzySet( "Small", new TrapezoidalFunction((float)0.5, (float)1.5, (float)2.5) ) );
            skillLevel.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)1.5, (float)2.5, (float)3.5) ) );
            skillLevel.AddLabel( new FuzzySet( "Big", new TrapezoidalFunction((float)2.5, (float)3.5, (float)4.5) ) );
            skillLevel.AddLabel( new FuzzySet( "VeryBig", new TrapezoidalFunction((float)3.5, (float)4.5, 5, 5) ) );

            LinguisticVariable managementStrategy = new( "ManagementStrategy", 0, 5 );
            managementStrategy.AddLabel( new FuzzySet( "VerySmall", new TrapezoidalFunction(0, 0, (float)0.5, (float)1.5) ) );
            managementStrategy.AddLabel( new FuzzySet( "Small", new TrapezoidalFunction((float)0.5, (float)1.5, (float)2.5) ) );
            managementStrategy.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)1.5, (float)2.5, (float)3.5) ) );
            managementStrategy.AddLabel( new FuzzySet( "Big", new TrapezoidalFunction((float)2.5, (float)3.5, (float)4.5) ) );
            managementStrategy.AddLabel( new FuzzySet( "VeryBig", new TrapezoidalFunction((float)3.5, (float)4.5, 5, 5) ) );

            LinguisticVariable governance = new( "Governance", 0, 25 );
            governance.AddLabel( new FuzzySet( "VerySmall", new TrapezoidalFunction(0, 0, (float)2.5, (float)7.5) ) );
            governance.AddLabel( new FuzzySet( "Small", new TrapezoidalFunction((float)2.5, (float)7.5, (float)12.5) ) );
            governance.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)7.5, (float)12.5, (float)17.5) ) );
            governance.AddLabel( new FuzzySet( "Big", new TrapezoidalFunction((float)12.5, (float)17.5, (float)22.5) ) );
            governance.AddLabel( new FuzzySet( "VeryBig", new TrapezoidalFunction((float)17.5, (float)22.5, 25, 25) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( SkillLevel );
            fuzzyDB.AddVariable( managementStrategy );
            fuzzyDB.AddVariable( governance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SkillLevel IS VerySmall and ManagementStrategy IS VerySmall THEN Governance IS VerySmall");
            IS.NewRule("Rule 2", "IF SkillLevel IS VerySmall and ManagementStrategy IS Small THEN Governance IS VerySmall");
            IS.NewRule("Rule 3", "IF SkillLevel IS VerySmall and ManagementStrategy IS Medium THEN Governance IS Small");
            IS.NewRule("Rule 4", "IF SkillLevel IS VerySmall and ManagementStrategy IS Big THEN Governance IS Small");
            IS.NewRule("Rule 5", "IF SkillLevel IS VerySmall and ManagementStrategy IS VeryBig THEN Governance IS Medium");
            IS.NewRule("Rule 6", "IF SkillLevel IS Small and ManagementStrategy IS VerySmall THEN Governance IS VerySmall");
            IS.NewRule("Rule 7", "IF SkillLevel IS Small and ManagementStrategy IS Small THEN Governance IS Small");
            IS.NewRule("Rule 8", "IF SkillLevel IS Small and ManagementStrategy IS Medium THEN Governance IS Small");
            IS.NewRule("Rule 9", "IF SkillLevel IS Small and ManagementStrategy IS Big THEN Governance IS Medium");
            IS.NewRule("Rule 10", "IF SkillLevel IS Small and ManagementStrategy IS VeryBig THEN Governance IS Big");
            IS.NewRule("Rule 11", "IF SkillLevel IS Medium and ManagementStrategy IS VerySmall THEN Governance IS Small");
            IS.NewRule("Rule 12", "IF SkillLevel IS Medium and ManagementStrategy IS Small THEN Governance IS Small");
            IS.NewRule("Rule 13", "IF SkillLevel IS Medium and ManagementStrategy IS Medium THEN Governance IS Medium");
            IS.NewRule("Rule 14", "IF SkillLevel IS Medium and ManagementStrategy IS Big THEN Governance IS Big");
            IS.NewRule("Rule 15", "IF SkillLevel IS Medium and ManagementStrategy IS VeryBig THEN Governance IS Big");
            IS.NewRule("Rule 16", "IF SkillLevel IS Big and ManagementStrategy IS VerySmall THEN Governance IS Small");
            IS.NewRule("Rule 17", "IF SkillLevel IS Big and ManagementStrategy IS Small THEN Governance IS Medium");
            IS.NewRule("Rule 18", "IF SkillLevel IS Big and ManagementStrategy IS Medium THEN Governance IS Big");
            IS.NewRule("Rule 19", "IF SkillLevel IS Big and ManagementStrategy IS Big THEN Governance IS Big");
            IS.NewRule("Rule 20", "IF SkillLevel IS Big and ManagementStrategy IS VeryBig THEN Governance IS VeryBig");
            IS.NewRule("Rule 21", "IF SkillLevel IS VeryBig and ManagementStrategy IS VerySmall THEN Governance IS Medium");
            IS.NewRule("Rule 22", "IF SkillLevel IS VeryBig and ManagementStrategy IS Small THEN Governance IS Big");
            IS.NewRule("Rule 23", "IF SkillLevel IS VeryBig and ManagementStrategy IS Medium THEN Governance IS Big");
            IS.NewRule("Rule 24", "IF SkillLevel IS VeryBig and ManagementStrategy IS Big THEN Governance IS VeryBig");
            IS.NewRule("Rule 25", "IF SkillLevel IS VeryBig and ManagementStrategy IS VeryBig THEN Governance IS VeryBig");

            IS.SetInput("SkillLevel", (float)skillLevelValue);
            IS.SetInput("ManagementStrategy", (float)managementStrategyValue);

            double resultado = IS.Evaluate("EnvironmentalQuality");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SkillLevel", i == 0 ? 0 : (float)4.99);
                IS.SetInput("ManagementStrategy", i == 0 ? 0 : (float)4.99);
                input[i] = IS.Evaluate("Governance");
            }
            double m = (IS.GetLinguisticVariable("Governance").End - IS.GetLinguisticVariable("Governance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Governance").End;

            if (resultado >= 0 && resultado <= 5.5)
            {
                return resultado.ToString() + " - Very Small";
            }
            else if (resultado > 5.5 && resultado <= 10)
            {
                return resultado.ToString() + " - Small";
            }
            else if (resultado > 10 && resultado <= 15)
            {
                return resultado.ToString() + " - Medium";
            }
            else if (resultado > 15 && resultado <= 20)
            {
                return resultado.ToString() + " - Big";
            }
            else
            {
                return resultado.ToString() + " - Very Big";
            }
        }

        public string CalculateOperation(double sensitiveOperationValue, double meanTimeBetweenFailuresValue, double availabilityOfRepairPersonneValue double workLoadValue)
        {
            LinguisticVariable sensitiveOperation = new( "SensitiveOperation", 0, 24 );
            sensitiveOperation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 6, 12) ) );
            sensitiveOperation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(6, 12, 18) ) );
            sensitiveOperation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(12, 18, 24, 24) ) );

            LinguisticVariable meanTimeBetweenFailures = new( "MeanTimeBetweenFailures", 0, 12 );
            meanTimeBetweenFailures.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 3, 6) ) );
            meanTimeBetweenFailures.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 6, 9) ) );
            meanTimeBetweenFailures.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(6, 9, 12, 12) ) );

            LinguisticVariable availabilityOfRepairPersonne = new( "AvailabilityOfRepairPersonne", 0, 100 );
            availabilityOfRepairPersonne.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            availabilityOfRepairPersonne.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            availabilityOfRepairPersonne.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable workLoad = new( "WorkLoad", 0, 100 );
            workLoad.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            workLoad.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            workLoad.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable Operation = new( "Operation", 0, 10 );
            Operation.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            Operation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            Operation.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            Operation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            Operation.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( sensitiveOperation );
            fuzzyDB.AddVariable( meanTimeBetweenFailures );
            fuzzyDB.AddVariable( availabilityOfRepairPersonne );
            fuzzyDB.AddVariable( workLoad );
            fuzzyDB.AddVariable( Operation );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 2", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 3", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 4", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 5", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 6", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 7", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 8", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 9", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS Middle");
            IS.NewRule("Rule 10", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS Low");
            IS.NewRule("Rule 11", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS VeryLow");
            IS.NewRule("Rule 12", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 13", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 14", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 15", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 16", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 17", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 18", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 19", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS VeryLow");
            IS.NewRule("Rule 20", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS VeryLow");
            IS.NewRule("Rule 21", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 22", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS Low");
            IS.NewRule("Rule 23", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS VeryLow");
            IS.NewRule("Rule 24", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 25", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 26", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 27", "IF SensitiveOperation IS Low and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 28", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 29", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 30", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 31", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS VeryLow");
            IS.NewRule("Rule 32", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 33", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS Middle");
            IS.NewRule("Rule 34", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 35", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS VeryHigh");
            IS.NewRule("Rule 36", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS High");
            IS.NewRule("Rule 37", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 38", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 39", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 40", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 41", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 42", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 43", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 44", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 45", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS Middle");
            IS.NewRule("Rule 46", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS Low");
            IS.NewRule("Rule 47", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS VeryLow");
            IS.NewRule("Rule 48", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 49", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 50", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 51", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 52", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 53", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 54", "IF SensitiveOperation IS Medium and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 55", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 56", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 57", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS Middle");
            IS.NewRule("Rule 58", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 59", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS VeryHigh");
            IS.NewRule("Rule 60", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS High");
            IS.NewRule("Rule 61", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 62", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS VeryHigh");
            IS.NewRule("Rule 63", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Low and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS VeryHigh");
            IS.NewRule("Rule 64", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 65", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 66", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 67", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 68", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 69", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS Middle");
            IS.NewRule("Rule 70", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 71", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS VeryHigh");
            IS.NewRule("Rule 72", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS Medium and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS High");
            IS.NewRule("Rule 73", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Low THEN Operation IS Middle");
            IS.NewRule("Rule 74", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS Medium THEN Operation IS Low");
            IS.NewRule("Rule 75", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Low and WorkLoad IS High THEN Operation IS VeryLow");
            IS.NewRule("Rule 76", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Low THEN Operation IS High");
            IS.NewRule("Rule 77", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS Medium THEN Operation IS Middle");
            IS.NewRule("Rule 78", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS Medium and WorkLoad IS High THEN Operation IS Low");
            IS.NewRule("Rule 79", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Low THEN Operation IS VeryHigh");
            IS.NewRule("Rule 80", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS Medium THEN Operation IS High");
            IS.NewRule("Rule 81", "IF SensitiveOperation IS High and MeanTimeBetweenFailures IS High and AvailabilityOfRepairPersonne IS High and WorkLoad IS High THEN Operation IS Middle");

            IS.SetInput("SensitiveOperation", (float)sensitiveOperationValue);
            IS.SetInput("MeanTimeBetweenFailures", (float)meanTimeBetweenFailuresValue);
            IS.SetInput("AvailabilityOfRepairPersonne", (float)availabilityOfRepairPersonneValue);
            IS.SetInput("WorkLoad", (float)workLoadValue);

            double resultado = IS.Evaluate("Operation");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("SensitiveOperation", i == 0 ? 0 : (float)23.99);
                IS.SetInput("MeanTimeBetweenFailures", i == 0 ? 0 : (float)11.99);
                IS.SetInput("AvailabilityOfRepairPersonne", i == 0 ? 0 : (float)99.99);
                IS.SetInput("WorkLoad", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("Operation");
            }
            double m = (IS.GetLinguisticVariable("Operation").End - IS.GetLinguisticVariable("Operation").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Operation").End;

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

        public double CalculateActivities(double availabilityOfRequiredPartsValue, double averageRepairTimeValue)
        {
            LinguisticVariable availabilityOfRequiredParts = new( "AvailabilityOfRequiredParts", 0, 24 );
            availabilityOfRequiredParts.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, (float)2.4, (float)7.2) ) );
            availabilityOfRequiredParts.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction((float)2.4, (float)7.2, 12) ) );
            availabilityOfRequiredParts.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)7.2, 12, (float)16.8) ) );
            availabilityOfRequiredParts.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(12, (float)16.8, (float)21.6) ) );
            availabilityOfRequiredParts.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction((float)16.8, (float)21.6, 24, 24) ) );

            LinguisticVariable averageRepairTime = new( "AverageRepairTime", 0, 24 );
            averageRepairTime.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, (float)2.4, (float)7.2) ) );
            averageRepairTime.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction((float)2.4, (float)7.2, 12) ) );
            averageRepairTime.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)7.2, 12, (float)16.8) ) );
            averageRepairTime.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(12, (float)16.8, (float)21.6) ) );
            averageRepairTime.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction((float)16.8, (float)21.6, 24, 24) ) );

            LinguisticVariable activities = new( "Activities", 0, 10 );
            activities.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            activities.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            activities.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            activities.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            activities.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( availabilityOfRequiredParts );
            fuzzyDB.AddVariable( averageRepairTime );
            fuzzyDB.AddVariable( activities );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF AvailabilityOfRequiredParts IS VeryLow and AverageRepairTime IS VeryLow THEN Activities IS VeryLow");
            IS.NewRule("Rule 2", "IF AvailabilityOfRequiredParts IS VeryLow and AverageRepairTime IS Low THEN Activities IS VeryLow");
            IS.NewRule("Rule 3", "IF AvailabilityOfRequiredParts IS VeryLow and AverageRepairTime IS Medium THEN Activities IS Low");
            IS.NewRule("Rule 4", "IF AvailabilityOfRequiredParts IS VeryLow and AverageRepairTime IS High THEN Activities IS Low");
            IS.NewRule("Rule 5", "IF AvailabilityOfRequiredParts IS VeryLow and AverageRepairTime IS VeryHigh THEN Activities IS Middle");
            IS.NewRule("Rule 6", "IF AvailabilityOfRequiredParts IS Low and AverageRepairTime IS VeryLow THEN Activities IS VeryLow");
            IS.NewRule("Rule 7", "IF AvailabilityOfRequiredParts IS Low and AverageRepairTime IS Low THEN Activities IS Low");
            IS.NewRule("Rule 8", "IF AvailabilityOfRequiredParts IS Low and AverageRepairTime IS Medium THEN Activities IS Low");
            IS.NewRule("Rule 9", "IF AvailabilityOfRequiredParts IS Low and AverageRepairTime IS High THEN Activities IS Middle");
            IS.NewRule("Rule 10", "IF AvailabilityOfRequiredParts IS Low and AverageRepairTime IS VeryHigh THEN Activities IS High");
            IS.NewRule("Rule 11", "IF AvailabilityOfRequiredParts IS Medium and AverageRepairTime IS VeryLow THEN Activities IS Low");
            IS.NewRule("Rule 12", "IF AvailabilityOfRequiredParts IS Medium and AverageRepairTime IS Low THEN Activities IS Low");
            IS.NewRule("Rule 13", "IF AvailabilityOfRequiredParts IS Medium and AverageRepairTime IS Medium THEN Activities IS Middle");
            IS.NewRule("Rule 14", "IF AvailabilityOfRequiredParts IS Medium and AverageRepairTime IS High THEN Activities IS High");
            IS.NewRule("Rule 15", "IF AvailabilityOfRequiredParts IS Medium and AverageRepairTime IS VeryHigh THEN Activities IS High");
            IS.NewRule("Rule 16", "IF AvailabilityOfRequiredParts IS High and AverageRepairTime IS VeryLow THEN Activities IS Low");
            IS.NewRule("Rule 17", "IF AvailabilityOfRequiredParts IS High and AverageRepairTime IS Low THEN Activities IS Middle");
            IS.NewRule("Rule 18", "IF AvailabilityOfRequiredParts IS High and AverageRepairTime IS Medium THEN Activities IS High");
            IS.NewRule("Rule 19", "IF AvailabilityOfRequiredParts IS High and AverageRepairTime IS High THEN Activities IS High");
            IS.NewRule("Rule 20", "IF AvailabilityOfRequiredParts IS High and AverageRepairTime IS VeryHigh THEN Activities IS VeryHigh");
            IS.NewRule("Rule 21", "IF AvailabilityOfRequiredParts IS VeryHigh and AverageRepairTime IS VeryLow THEN Activities IS Middle");
            IS.NewRule("Rule 22", "IF AvailabilityOfRequiredParts IS VeryHigh and AverageRepairTime IS Low THEN Activities IS High");
            IS.NewRule("Rule 23", "IF AvailabilityOfRequiredParts IS VeryHigh and AverageRepairTime IS Medium THEN Activities IS High");
            IS.NewRule("Rule 24", "IF AvailabilityOfRequiredParts IS VeryHigh and AverageRepairTime IS High THEN Activities IS VeryHigh");
            IS.NewRule("Rule 25", "IF AvailabilityOfRequiredParts IS VeryHigh and AverageRepairTime IS VeryHigh THEN Activities IS VeryHigh");

            IS.SetInput("AvailabilityOfRequiredParts", (float)availabilityOfRequiredPartsValue);
            IS.SetInput("AverageRepairTime", (float)averageRepairTimeValue);

            double resultado = IS.Evaluate("Activities");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("AvailabilityOfRequiredParts", i == 0 ? 0 : (float)23.99);
                IS.SetInput("AverageRepairTime", i == 0 ? 0 : (float)23.99);
                input[i] = IS.Evaluate("Activities");
            }
            double m = (IS.GetLinguisticVariable("Activities").End - IS.GetLinguisticVariable("Activities").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Activities").End;

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

        public double CalculatePreventive(double operationValue, double equipmentActivitiesValue)
        {
            LinguisticVariable operation = new( "Operation", 0, 10 );
            operation.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            operation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            operation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            operation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            operation.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable equipmentActivities = new( "EquipmentActivities", 0, 10 );
            equipmentActivities.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            equipmentActivities.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            equipmentActivities.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            equipmentActivities.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            equipmentActivities.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable preventive = new( "Preventive", 0, 10 );
            preventive.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            preventive.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            preventive.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            preventive.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            preventive.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( operation );
            fuzzyDB.AddVariable( equipmentActivities );
            fuzzyDB.AddVariable( preventive );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Operation IS VeryLow and EquipmentActivities IS VeryLow THEN Preventive IS VeryLow");
            IS.NewRule("Rule 2", "IF Operation IS VeryLow and EquipmentActivities IS Low THEN Preventive IS VeryLow");
            IS.NewRule("Rule 3", "IF Operation IS VeryLow and EquipmentActivities IS Medium THEN Preventive IS Low");
            IS.NewRule("Rule 4", "IF Operation IS VeryLow and EquipmentActivities IS High THEN Preventive IS Low");
            IS.NewRule("Rule 5", "IF Operation IS VeryLow and EquipmentActivities IS VeryHigh THEN Preventive IS Middle");
            IS.NewRule("Rule 6", "IF Operation IS Low and EquipmentActivities IS VeryLow THEN Preventive IS VeryLow");
            IS.NewRule("Rule 7", "IF Operation IS Low and EquipmentActivities IS Low THEN Preventive IS Low");
            IS.NewRule("Rule 8", "IF Operation IS Low and EquipmentActivities IS Medium THEN Preventive IS Low");
            IS.NewRule("Rule 9", "IF Operation IS Low and EquipmentActivities IS High THEN Preventive IS Middle");
            IS.NewRule("Rule 10", "IF Operation IS Low and EquipmentActivities IS VeryHigh THEN Preventive IS High");
            IS.NewRule("Rule 11", "IF Operation IS Medium and EquipmentActivities IS VeryLow THEN Preventive IS Low");
            IS.NewRule("Rule 12", "IF Operation IS Medium and EquipmentActivities IS Low THEN Preventive IS Low");
            IS.NewRule("Rule 13", "IF Operation IS Medium and EquipmentActivities IS Medium THEN Preventive IS Middle");
            IS.NewRule("Rule 14", "IF Operation IS Medium and EquipmentActivities IS High THEN Preventive IS High");
            IS.NewRule("Rule 15", "IF Operation IS Medium and EquipmentActivities IS VeryHigh THEN Preventive IS High");
            IS.NewRule("Rule 16", "IF Operation IS High and EquipmentActivities IS VeryLow THEN Preventive IS Low");
            IS.NewRule("Rule 17", "IF Operation IS High and EquipmentActivities IS Low THEN Preventive IS Middle");
            IS.NewRule("Rule 18", "IF Operation IS High and EquipmentActivities IS Medium THEN Preventive IS High");
            IS.NewRule("Rule 19", "IF Operation IS High and EquipmentActivities IS High THEN Preventive IS High");
            IS.NewRule("Rule 20", "IF Operation IS High and EquipmentActivities IS VeryHigh THEN Preventive IS VeryHigh");
            IS.NewRule("Rule 21", "IF Operation IS VeryHigh and EquipmentActivities IS VeryLow THEN Preventive IS Middle");
            IS.NewRule("Rule 22", "IF Operation IS VeryHigh and EquipmentActivities IS Low THEN Preventive IS High");
            IS.NewRule("Rule 23", "IF Operation IS VeryHigh and EquipmentActivities IS Medium THEN Preventive IS High");
            IS.NewRule("Rule 24", "IF Operation IS VeryHigh and EquipmentActivities IS High THEN Preventive IS VeryHigh");
            IS.NewRule("Rule 25", "IF Operation IS VeryHigh and EquipmentActivities IS VeryHigh THEN Preventive IS VeryHigh");

            IS.SetInput("Operation", (float)operationValue);
            IS.SetInput("EquipmentActivities", (float)equipmentActivitiesValue);

            double resultado = IS.Evaluate("Preventive");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Operation", i == 0 ? 0 : (float)9.99);
                IS.SetInput("EquipmentActivities", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Preventive");
            }
            double m = (IS.GetLinguisticVariable("Preventive").End - IS.GetLinguisticVariable("Preventive").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Preventive").End;

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

        public string CalculatePredictive(double predictiveOperationValue, double technologyDataCollectionValue, double instrumetationValue)
        {
            LinguisticVariable predictiveOperation = new( "PredictiveOperation", 0, 12 );
            predictiveOperation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 3, 6) ) );
            predictiveOperation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 6, 9) ) );
            predictiveOperation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(6, 9, 12, 12) ) );

            LinguisticVariable technologyDataCollection = new( "TechnologyDataCollection", 0, 12 );
            technologyDataCollection.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 3, 6) ) );
            technologyDataCollection.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 6, 9) ) );
            technologyDataCollection.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(6, 9, 12, 12) ) );

            LinguisticVariable Instrumentation = new( "Instrumentation", 0, 100 );
            instrumentation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, 25, 50) ) );
            instrumentation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(25, 50, 75) ) );
            instrumentation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(50, 75, 100, 100) ) );

            LinguisticVariable Predictive = new( "Predictive", 0, 10 );
            predictive.AddLabel( new FuzzySet( "VeryLimited", new TrapezoidalFunction(0, 0, 1, 3) ) );
            predictive.AddLabel( new FuzzySet( "Limited", new TrapezoidalFunction(1, 3, 5) ) );
            predictive.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            predictive.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            predictive.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( predictiveOperation );
            fuzzyDB.AddVariable( technologyDataCollection );
            fuzzyDB.AddVariable( instrumentation );
            fuzzyDB.AddVariable( predictive );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Low and Instrumentation IS Low THEN Predictive IS VeryLimited");
            IS.NewRule("Rule 2", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Low and Instrumentation IS Medium THEN Predictive IS VeryLimited");
            IS.NewRule("Rule 3", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Low and Instrumentation IS High THEN Predictive IS Limited");
            IS.NewRule("Rule 4", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Medium and Instrumentation IS Low THEN Predictive IS VeryLimited");
            IS.NewRule("Rule 5", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Medium and Instrumentation IS Medium THEN Predictive IS Limited");
            IS.NewRule("Rule 6", "IF PredictiveOperation IS Low and TechnologyDataCollection IS Medium and Instrumentation IS High THEN Predictive IS Medium");
            IS.NewRule("Rule 7", "IF PredictiveOperation IS Low and TechnologyDataCollection IS High and Instrumentation IS Low THEN Predictive IS Limited");
            IS.NewRule("Rule 8", "IF PredictiveOperation IS Low and TechnologyDataCollection IS High and Instrumentation IS Medium THEN Predictive IS Medium");
            IS.NewRule("Rule 9", "IF PredictiveOperation IS Low and TechnologyDataCollection IS High and Instrumentation IS High THEN Predictive IS High");
            IS.NewRule("Rule 10", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Low and Instrumentation IS Low THEN Predictive IS VeryLimited");
            IS.NewRule("Rule 11", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Low and Instrumentation IS Medium THEN Predictive IS Limited");
            IS.NewRule("Rule 12", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Low and Instrumentation IS High THEN Predictive IS Medium");
            IS.NewRule("Rule 13", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Medium and Instrumentation IS Low THEN Predictive IS Limited");
            IS.NewRule("Rule 14", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Medium and Instrumentation IS Medium THEN Predictive IS Medium");
            IS.NewRule("Rule 15", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS Medium and Instrumentation IS High THEN Predictive IS High");
            IS.NewRule("Rule 16", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS High and Instrumentation IS Low THEN Predictive IS Medium");
            IS.NewRule("Rule 17", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS High and Instrumentation IS Medium THEN Predictive IS High");
            IS.NewRule("Rule 18", "IF PredictiveOperation IS Medium and TechnologyDataCollection IS High and Instrumentation IS High THEN Predictive IS VeryHigh");
            IS.NewRule("Rule 19", "IF PredictiveOperation IS High and TechnologyDataCollection IS Low and Instrumentation IS Low THEN Predictive IS Limited");
            IS.NewRule("Rule 20", "IF PredictiveOperation IS High and TechnologyDataCollection IS Low and Instrumentation IS Medium THEN Predictive IS Medium");
            IS.NewRule("Rule 21", "IF PredictiveOperation IS High and TechnologyDataCollection IS Low and Instrumentation IS High THEN Predictive IS High");
            IS.NewRule("Rule 22", "IF PredictiveOperation IS High and TechnologyDataCollection IS Medium and Instrumentation IS Low THEN Predictive IS Medium");
            IS.NewRule("Rule 23", "IF PredictiveOperation IS High and TechnologyDataCollection IS Medium and Instrumentation IS Medium THEN Predictive IS High");
            IS.NewRule("Rule 24", "IF PredictiveOperation IS High and TechnologyDataCollection IS Medium and Instrumentation IS High THEN Predictive IS VeryHigh");
            IS.NewRule("Rule 25", "IF PredictiveOperation IS High and TechnologyDataCollection IS High and Instrumentation IS Low THEN Predictive IS High");
            IS.NewRule("Rule 26", "IF PredictiveOperation IS High and TechnologyDataCollection IS High and Instrumentation IS Medium THEN Predictive IS VeryHigh");
            IS.NewRule("Rule 27", "IF PredictiveOperation IS High and TechnologyDataCollection IS High and Instrumentation IS High THEN Predictive IS VeryHigh");

            IS.SetInput("PredictiveOperation", (float)predictiveOperationValue);
            IS.SetInput("TechnologyDataCollection", (float)technologyDataCollectionValue);
            IS.SetInput("Instrumentation", (float)instrumentationValue);

            double resultado = IS.Evaluate("Predictive");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("PredictiveOperation", i == 0 ? 0 : (float)11.99);
                IS.SetInput("TechnologyDataCollection", i == 0 ? 0 : (float)11.99);
                IS.SetInput("Instrumentation", i == 0 ? 0 : (float)99.99);
                input[i] = IS.Evaluate("Predictive");
            }
            double m = (IS.GetLinguisticVariable("Predictive").End - IS.GetLinguisticVariable("Predictive").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Predictive").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Limited";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Limited";
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

        public string CalculateMaintenance(double correctiveValue, double preventiveValue, double predictiveValue)
        {
            LinguisticVariable corrective = new( "Corrective", 0, 30 );
            corrective.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)7.5, 15 ) );
            corrective.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction((float)7.5, 15, (float)22.5) ) );
            corrective.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(15, (float)22.5, 30, 30) ) );

            LinguisticVariable preventive = new( "Preventive", 0, 10 );
            preventive.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            preventive.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            preventive.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable predictive = new( "Predictive", 0, 10 );
            predictive.AddLabel( new FuzzySet( "Limited", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            predictive.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            predictive.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "VeryBad", new TrapezoidalFunction(0, 0, 1, 3) ) );
            maintenance.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(1, 3, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Proper", new TrapezoidalFunction(3, 5, 7) ) );
            maintenance.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction(5, 7, 9) ) );
            maintenance.AddLabel( new FuzzySet( "VeryGood", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( corrective );
            fuzzyDB.AddVariable( preventive );
            fuzzyDB.AddVariable( predictive );
            fuzzyDB.AddVariable( maintenance );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Corrective IS Low and Preventive IS Low and Predictive IS Limited THEN Maintenance IS Bad");
            IS.NewRule("Rule 2", "IF Corrective IS Low and Preventive IS Low and Predictive IS Medium THEN Maintenance IS Proper");
            IS.NewRule("Rule 3", "IF Corrective IS Low and Preventive IS Low and Predictive IS High THEN Maintenance IS Good");
            IS.NewRule("Rule 4", "IF Corrective IS Low and Preventive IS Middle and Predictive IS Limited THEN Maintenance IS Proper");
            IS.NewRule("Rule 5", "IF Corrective IS Low and Preventive IS Middle and Predictive IS Medium THEN Maintenance IS Good");
            IS.NewRule("Rule 6", "IF Corrective IS Low and Preventive IS Middle and Predictive IS High THEN Maintenance IS VeryGood");
            IS.NewRule("Rule 7", "IF Corrective IS Low and Preventive IS High and Predictive IS Limited THEN Maintenance IS Good");
            IS.NewRule("Rule 8", "IF Corrective IS Low and Preventive IS High and Predictive IS Medium THEN Maintenance IS VeryGood");
            IS.NewRule("Rule 9", "IF Corrective IS Low and Preventive IS High and Predictive IS High THEN Maintenance IS VeryGood");
            IS.NewRule("Rule 10", "IF Corrective IS Middle and Preventive IS Low and Predictive IS Limited THEN Maintenance IS VeryBad");
            IS.NewRule("Rule 11", "IF Corrective IS Middle and Preventive IS Low and Predictive IS Medium THEN Maintenance IS Bad");
            IS.NewRule("Rule 12", "IF Corrective IS Middle and Preventive IS Low and Predictive IS High THEN Maintenance IS Proper");
            IS.NewRule("Rule 13", "IF Corrective IS Middle and Preventive IS Middle and Predictive IS Limited THEN Maintenance IS Bad");
            IS.NewRule("Rule 14", "IF Corrective IS Middle and Preventive IS Middle and Predictive IS Medium THEN Maintenance IS Proper");
            IS.NewRule("Rule 15", "IF Corrective IS Middle and Preventive IS Middle and Predictive IS High THEN Maintenance IS Good");
            IS.NewRule("Rule 16", "IF Corrective IS Middle and Preventive IS High and Predictive IS Limited THEN Maintenance IS Proper");
            IS.NewRule("Rule 17", "IF Corrective IS Middle and Preventive IS High and Predictive IS Medium THEN Maintenance IS Good");
            IS.NewRule("Rule 18", "IF Corrective IS Middle and Preventive IS High and Predictive IS High THEN Maintenance IS VeryGood");
            IS.NewRule("Rule 19", "IF Corrective IS High and Preventive IS Low and Predictive IS Limited THEN Maintenance IS VeryBad");
            IS.NewRule("Rule 20", "IF Corrective IS High and Preventive IS Low and Predictive IS Medium THEN Maintenance IS VeryBad");
            IS.NewRule("Rule 21", "IF Corrective IS High and Preventive IS Low and Predictive IS High THEN Maintenance IS Bad");
            IS.NewRule("Rule 22", "IF Corrective IS High and Preventive IS Middle and Predictive IS Limited THEN Maintenance IS VeryBad");
            IS.NewRule("Rule 23", "IF Corrective IS High and Preventive IS Middle and Predictive IS Medium THEN Maintenance IS Bad");
            IS.NewRule("Rule 24", "IF Corrective IS High and Preventive IS Middle and Predictive IS High THEN Maintenance IS Proper");
            IS.NewRule("Rule 25", "IF Corrective IS High and Preventive IS High and Predictive IS Limited THEN Maintenance IS Bad");
            IS.NewRule("Rule 26", "IF Corrective IS High and Preventive IS High and Predictive IS Medium THEN Maintenance IS Proper");
            IS.NewRule("Rule 27", "IF Corrective IS High and Preventive IS High and Predictive IS High THEN Maintenance IS Good");

            IS.SetInput("Corrective", (float)correctiveValue);
            IS.SetInput("Preventive", (float)preventiveValue);
            IS.SetInput("Predictive", (float)predictiveValue);

            double resultado = IS.Evaluate("Maintenance");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Corrective", i == 0 ? (float)29.99 : 0);
                IS.SetInput("Preventive", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Predictive", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Maintenance");
            }
            double m = (IS.GetLinguisticVariable("Maintenance").End - IS.GetLinguisticVariable("Maintenance").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[0]) + IS.GetLinguisticVariable("Maintenance").Start;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Very Bad";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Bad";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Proper";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Good";
            }
            else
            {
                return resultado.ToString() + " - Very Good";
            }
        }

        public double CalculateIndustrialManagement(double governanceValue, double maintenanceValue)
        {
            LinguisticVariable governance = new( "Governance", 0, 25 );
            governance.AddLabel( new FuzzySet( "VeryBad", new TrapezoidalFunction(0, 0, (float)2.5, (float)7.5) ) );
            governance.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction((float)2.5, (float)7.5, (float)12.5) ) );
            governance.AddLabel( new FuzzySet( "Moderate", new TrapezoidalFunction((float)7.5, (float)12.5, (float)17.5) ) );
            governance.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction((float)12.5, (float)17.5, (float)22.5) ) );
            governance.AddLabel( new FuzzySet( "VeryGood", new TrapezoidalFunction((float)17.5, (float)22.5, 25, 25) ) );

            LinguisticVariable maintenance = new( "Maintenance", 0, 10 );
            maintenance.AddLabel( new FuzzySet( "VeryBad", new TrapezoidalFunction(0, 0, 1, 3) ) );
            maintenance.AddLabel( new FuzzySet( "Bad", new TrapezoidalFunction(1, 3, 5) ) );
            maintenance.AddLabel( new FuzzySet( "Moderate", new TrapezoidalFunction(3, 5, 7) ) );
            maintenance.AddLabel( new FuzzySet( "Good", new TrapezoidalFunction(5, 7, 9) ) );
            maintenance.AddLabel( new FuzzySet( "VeryGood", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable industrialManagement = new( "IndustrialManagement", 0, 10 );
            industrialManagement.AddLabel( new FuzzySet( "Ineffective", new TrapezoidalFunction(0, 0, 1, 3) ) );
            industrialManagement.AddLabel( new FuzzySet( "PotentiallyIneffective", new TrapezoidalFunction(1, 3, 5) ) );
            industrialManagement.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            industrialManagement.AddLabel( new FuzzySet( "PotentiallyEffective", new TrapezoidalFunction(5, 7, 9) ) );
            industrialManagement.AddLabel( new FuzzySet( "Effective", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( governance );
            fuzzyDB.AddVariable( maintenance );
            fuzzyDB.AddVariable( industrialManagement );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Governance IS VeryBad and Maintenance IS VeryBad THEN IndustrialManagement IS Ineffective");
            IS.NewRule("Rule 2", "IF Governance IS VeryBad and Maintenance IS Bad THEN IndustrialManagement IS Ineffective");
            IS.NewRule("Rule 3", "IF Governance IS VeryBad and Maintenance IS Moderate THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 4", "IF Governance IS VeryBad and Maintenance IS Good THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 5", "IF Governance IS VeryBad and Maintenance IS VeryGood THEN IndustrialManagement IS Acceptable");
            IS.NewRule("Rule 6", "IF Governance IS Bad and Maintenance IS VeryBad THEN IndustrialManagement IS Ineffective");
            IS.NewRule("Rule 7", "IF Governance IS Bad and Maintenance IS Bad THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 8", "IF Governance IS Bad and Maintenance IS Moderate THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 9", "IF Governance IS Bad and Maintenance IS Good THEN IndustrialManagement IS Acceptable");
            IS.NewRule("Rule 10", "IF Governance IS Bad and Maintenance IS VeryGood THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 11", "IF Governance IS Moderate and Maintenance IS VeryBad THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 12", "IF Governance IS Moderate and Maintenance IS Bad THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 13", "IF Governance IS Moderate and Maintenance IS Moderate THEN IndustrialManagement IS Acceptable");
            IS.NewRule("Rule 14", "IF Governance IS Moderate and Maintenance IS Good THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 15", "IF Governance IS Moderate and Maintenance IS VeryGood THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 16", "IF Governance IS Good and Maintenance IS VeryBad THEN IndustrialManagement IS PotentiallyIneffective");
            IS.NewRule("Rule 17", "IF Governance IS Good and Maintenance IS Bad THEN IndustrialManagement IS Acceptable");
            IS.NewRule("Rule 18", "IF Governance IS Good and Maintenance IS Moderate THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 19", "IF Governance IS Good and Maintenance IS Good THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 20", "IF Governance IS Good and Maintenance IS VeryGood THEN IndustrialManagement IS Effective");
            IS.NewRule("Rule 21", "IF Governance IS VeryGood and Maintenance IS VeryBad THEN IndustrialManagement IS Acceptable");
            IS.NewRule("Rule 22", "IF Governance IS VeryGood and Maintenance IS Bad THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 23", "IF Governance IS VeryGood and Maintenance IS Moderate THEN IndustrialManagement IS PotentiallyEffective");
            IS.NewRule("Rule 24", "IF Governance IS VeryGood and Maintenance IS Good THEN IndustrialManagement IS Effective");
            IS.NewRule("Rule 25", "IF Governance IS VeryGood and Maintenance IS VeryGood THEN IndustrialManagement IS Effective");

            IS.SetInput("Governance", (float)governanceValue);
            IS.SetInput("Maintenance", (float)maintenanceValue);

            double resultado = IS.Evaluate("IndustrialManagement");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Governance", i == 0 ? 0 : (float)24.99);
                IS.SetInput("Maintenance", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("IndustrialManagement");
            }
            double m = (IS.GetLinguisticVariable("IndustrialManagement").End - IS.GetLinguisticVariable("IndustrialManagement").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("IndustrialManagement").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Ineffective";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - Potentially Ineffective";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - Potentially Effective";
            }
            else
            {
                return resultado.ToString() + " - Effective";
            }
        }

        public string CalculateCriticalityIndex(double environmentalQualityValue, double costsManagementValue, double industrialManagementValue)
        {
            LinguisticVariable environmentalQuality = new( "EnvironmentalQuality", 0, 10 );
            environmentalQuality.AddLabel( new FuzzySet( "Inappropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            environmentalQuality.AddLabel( new FuzzySet( "PotentiallyInappropriate", new TrapezoidalFunction(1, 3, 5) ) );
            environmentalQuality.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            environmentalQuality.AddLabel( new FuzzySet( "PotentiallyAppropriate", new TrapezoidalFunction(5, 7, 9) ) );
            environmentalQuality.AddLabel( new FuzzySet( "Appropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable costsManagement = new( "CostsManagement", 0, 10 );
            costsManagement.AddLabel( new FuzzySet( "Inappropriate", new TrapezoidalFunction(0, 0, 1, 3) ) );
            costsManagement.AddLabel( new FuzzySet( "PotentiallyInappropriate", new TrapezoidalFunction(1, 3, 5) ) );
            costsManagement.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            costsManagement.AddLabel( new FuzzySet( "PotentiallyAppropriate", new TrapezoidalFunction(5, 7, 9) ) );
            costsManagement.AddLabel( new FuzzySet( "Appropriate", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable industrialManagement = new( "IndustrialManagement", 0, 10 );
            industrialManagement.AddLabel( new FuzzySet( "Ineffective", new TrapezoidalFunction(0, 0, 1, 3) ) );
            industrialManagement.AddLabel( new FuzzySet( "PotentiallyIneffective", new TrapezoidalFunction(1, 3, 5) ) );
            industrialManagement.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            industrialManagement.AddLabel( new FuzzySet( "PotentiallyEffective", new TrapezoidalFunction(5, 7, 9) ) );
            industrialManagement.AddLabel( new FuzzySet( "Effective", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable criticalityIndex = new( "CriticalityIndex", 0, 10 );
            criticalityIndex.AddLabel( new FuzzySet( "Critical", new TrapezoidalFunction(0, 0, 1, 3) ) );
            criticalityIndex.AddLabel( new FuzzySet( "PotentiallyCritical", new TrapezoidalFunction(1, 3, 5) ) );
            criticalityIndex.AddLabel( new FuzzySet( "Acceptable", new TrapezoidalFunction(3, 5, 7) ) );
            criticalityIndex.AddLabel( new FuzzySet( "PotentiallyUncritical", new TrapezoidalFunction(5, 7, 9) ) );
            criticalityIndex.AddLabel( new FuzzySet( "Uncritical", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( environmentalQuality );
            fuzzyDB.AddVariable( costsManagement );
            fuzzyDB.AddVariable( industrialManagement );
            fuzzyDB.AddVariable( criticalityIndex );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 2", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 3", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 4", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 5", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 6", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 7", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 8", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 9", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 10", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 11", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 12", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 13", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 14", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 15", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 16", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 17", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 18", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 19", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 20", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 21", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 22", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 23", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 24", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 25", "IF EnvironmentalQuality IS Inappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 26", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 27", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 28", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 29", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 30", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 31", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 32", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 33", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 34", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 35", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 36", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 37", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 38", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 39", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 40", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Acceptable and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 41", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 42", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 43", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 44", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 45", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 46", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 47", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 48", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 49", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 50", "IF EnvironmentalQuality IS PotentiallyInappropriate and CostsManagement IS Appropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 51", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Inappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 52", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 53", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Inappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 54", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 55", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Inappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 56", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 57", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 58", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 59", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 60", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 61", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Acceptable and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 62", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 63", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Acceptable and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 64", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 65", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Acceptable and IndustrialManagement IS Effective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 66", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 67", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 68", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 69", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 70", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 71", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Appropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 72", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 73", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Appropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 74", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 75", "IF EnvironmentalQuality IS Acceptable and CostsManagement IS Appropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 76", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 77", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 78", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 79", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 80", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 81", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 82", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 83", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 84", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 85", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 86", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Acceptable and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 87", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 88", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Acceptable and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 89", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 90", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Acceptable and IndustrialManagement IS Effective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 91", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 92", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 93", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 94", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 95", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 96", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Appropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 97", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 98", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Appropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 99", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 100", "IF EnvironmentalQuality IS PotentiallyAppropriate and CostsManagement IS Appropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 101", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 102", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 103", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 104", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Inappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 105", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Inappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 106", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 107", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 108", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 109", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 110", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyInappropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 111", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Acceptable and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 112", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 113", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Acceptable and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 114", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Acceptable and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 115", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Acceptable and IndustrialManagement IS Effective THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 116", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 117", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 118", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 119", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 120", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS PotentiallyAppropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 121", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Appropriate and IndustrialManagement IS Ineffective THEN CriticalityIndex IS Critical");
            IS.NewRule("Rule 122", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyIneffective THEN CriticalityIndex IS PotentiallyCritical");
            IS.NewRule("Rule 123", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Appropriate and IndustrialManagement IS Acceptable THEN CriticalityIndex IS Acceptable");
            IS.NewRule("Rule 124", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Appropriate and IndustrialManagement IS PotentiallyEffective THEN CriticalityIndex IS PotentiallyUncritical");
            IS.NewRule("Rule 125", "IF EnvironmentalQuality IS Appropriate and CostsManagement IS Appropriate and IndustrialManagement IS Effective THEN CriticalityIndex IS Uncritical");

            IS.SetInput("EnvironmentalQuality", (float)environmentalQualityValue);
            IS.SetInput("CostsManagement", (float)costsManagementValue);
            IS.SetInput("IndustrialManagement", (float)industrialManagementValue);

            double resultado = IS.Evaluate("CriticalityIndex");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("EnvironmentalQuality", i == 0 ? 0 : (float)9.99);
                IS.SetInput("CostsManagement", i == 0 ? 0 : (float)9.99);
                IS.SetInput("IndustrialManagement", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("CriticalityIndex");
            }
            double m = (IS.GetLinguisticVariable("CriticalityIndex").End - IS.GetLinguisticVariable("CriticalityIndex").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("CriticalityIndex").End;

            if (resultado >= 0 && resultado <= 2.5)
            {
                return resultado.ToString() + " - Critical";
            }
            else if (resultado > 2.5 && resultado <= 4)
            {
                return resultado.ToString() + " - PotentiallyCritical";
            }
            else if (resultado > 4 && resultado <= 6)
            {
                return resultado.ToString() + " - Acceptable";
            }
            else if (resultado > 6 && resultado <= 8)
            {
                return resultado.ToString() + " - PotentiallyUncritical";
            }
            else
            {
                return resultado.ToString() + " - Uncritical";
            }
        }
    }
}