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
    public class MechanicalTreatmentController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IMechanicalTreatmentRepository _MechanicalTreatmentRepository;
        public MechanicalTreatmentController(IConfiguration configuration, IMechanicalTreatmentRepository MechanicalTreatmentRepository)
        {
            Configuration = configuration;
            _MechanicalTreatmentRepository = MechanicalTreatmentRepository;
        }

        [HttpPost]
        public ActionResult<MechanicalTreatment> InsertMechanicalTreatment([FromBody] MechanicalTreatment mechanicalTreatment)
        {
            try
            {
                mechanicalTreatment.Neutralization = CalculateNeutralization(Convert.ToDouble(mechanicalTreatment.PH), Convert.ToDouble(mechanicalTreatment.Redox), Convert.ToDouble(mechanicalTreatment.OtherMethods));
                mechanicalTreatment.MaterialConcentration = CalculateMaterialConcentration(Convert.ToDouble(mechanicalTreatment.BiologicalTreatment), Convert.ToDouble(mechanicalTreatment.ReverseOsmosis), Convert.ToDouble(mechanicalTreatment.Evaporator));
                mechanicalTreatment.Chemistry = CalculateChemistry(Convert.ToDouble(mechanicalTreatment.DistillerUse), Convert.ToDouble(mechanicalTreatment.UseOfAbsorption), Convert.ToDouble(mechanicalTreatment.UseOfAdsorption));
                mechanicalTreatment.Physicist = CalculatePhysicist(Convert.ToDouble(mechanicalTreatment.UseOfMagneticSieve), Convert.ToDouble(mechanicalTreatment.UseOfFiltering), Convert.ToDouble(mechanicalTreatment.UseOfDecanter), Convert.ToDouble(mechanicalTreatment.CycloneSeparation));
                mechanicalTreatment.Recover = CalculateRecover(Convert.ToDouble(mechanicalTreatment.RecoveryOfMetalsFromWater), Convert.ToDouble(mechanicalTreatment.WaterTreatmentAndReuse), Convert.ToDouble(mechanicalTreatment.RecoveryAndReuseOfCoolingWater));
                mechanicalTreatment.Equipments = CalculateEquipments(Convert.ToDouble(mechanicalTreatment.UseOfClosedProcessInTheProductionOfWasteWater), Convert.ToDouble(mechanicalTreatment.ReplacementOfCoolingWaterInIndustry), Convert.ToDouble(mechanicalTreatment.RecycledWaterMeasurementToReduceSewageFees));
                mechanicalTreatment.WaterDevelopment = CalculateWaterDevelopment(Convert.ToDouble(mechanicalTreatment.WaterTreatmentByMagneticTechnology), Convert.ToDouble(mechanicalTreatment.ImprovedProductionOfDeionizedWater));
                mechanicalTreatment.Chlorination = CalculateChlorination(Convert.ToDouble(mechanicalTreatment.RecyclingOfChlorinatedWater), Convert.ToDouble(mechanicalTreatment.UseTheChlorinationWashWater), Convert.ToDouble(mechanicalTreatment.UsingChlorineInTheGasPhase));
                mechanicalTreatment.Quality = CalculateQuality(Convert.ToDouble(mechanicalTreatment.DecreasedContaminationOfTreatmentWater), Convert.ToDouble(mechanicalTreatment.UseOfDeionizedWater), Convert.ToDouble(mechanicalTreatment.RegularCleaningOfDirtOnProductionLinesThatUseWater));
                mechanicalTreatment.Conversion = CalculateConversion(Convert.ToDouble(mechanicalTreatment.QuantificationOfWaterUse), Convert.ToDouble(mechanicalTreatment.UseOfValvesToControlEquipmentFlow), Convert.ToDouble(mechanicalTreatment.ReplacementOfTreatedWaterWithWellWater));
                mechanicalTreatment.Equipment = CalculateEquipment(Convert.ToDouble(mechanicalTreatment.WaterLevelControlInEquipment), Convert.ToDouble(mechanicalTreatment.EliminationOfLeaksInWaterLinesAndValves), Convert.ToDouble(mechanicalTreatment.ReplacementOfWaterRegretInProcesses));
                mechanicalTreatment.Use = CalculateUse(Convert.ToDouble(mechanicalTreatment.ReductionInWaterUse), Convert.ToDouble(mechanicalTreatment.UseOfCountercurrentRinsing), Convert.ToDouble(mechanicalTreatment.MinimalUseOfCoolingWater));
                mechanicalTreatment.RemovalOfContaminants = CalculateRemovalOfContaminants(Convert.ToDouble(mechanicalTreatment.Chemistry), Convert.ToDouble(mechanicalTreatment.Physicist));
                mechanicalTreatment.CCWE = CalculateCCWE(Convert.ToDouble(mechanicalTreatment.Equipments), Convert.ToDouble(mechanicalTreatment.Recover));
                mechanicalTreatment.WT = CalculateWT(Convert.ToDouble(mechanicalTreatment.Chlorination), Convert.ToDouble(mechanicalTreatment.WaterDevelopment), Convert.ToDouble(mechanicalTreatment.ReplacementOfChlorineByO2));
                mechanicalTreatment.Reduction = CalculateReduction(Convert.ToDouble(mechanicalTreatment.Use), Convert.ToDouble(mechanicalTreatment.Equipment), Convert.ToDouble(mechanicalTreatment.Conversion));
                mechanicalTreatment.PostGenerationTreatment = CalculatePostGenerationTreatment(Convert.ToDouble(mechanicalTreatment.RemovalOfContaminants), Convert.ToDouble(mechanicalTreatment.MaterialConcentration), Convert.ToDouble(mechanicalTreatment.Neutralization));
                mechanicalTreatment.Utility = CalculateUtility(Convert.ToDouble(mechanicalTreatment.WT), Convert.ToDouble(mechanicalTreatment.CCWE));
                mechanicalTreatment.WaterUse = CalculateWaterUse(Convert.ToDouble(mechanicalTreatment.Reduction), Convert.ToDouble(mechanicalTreatment.Utility), Convert.ToDouble(mechanicalTreatment.Quality));
                mechanicalTreatment.MechanicalTreatmentValue = CalculateMechanicalTreatment(Convert.ToDouble(mechanicalTreatment.PostGenerationTreatment), Convert.ToDouble(mechanicalTreatment.WaterUse));

                return _MechanicalTreatmentRepository.InsertMechanicalTreatment(mechanicalTreatment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MechanicalTreatment> GetMechanicalTreatmentById(int id)
        {
            try
            {
                return _MechanicalTreatmentRepository.GetMechanicalTreatmentById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<MechanicalTreatment>> GetMechanicalTreatmentByIndustry(string industryName)
        {
            try
            {
                return _MechanicalTreatmentRepository.GetMechanicalTreatmentByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        public string CalculateNeutralization(double PHValue, double redoxValue, double otherMethodsValue)
        {
            LinguisticVariable PH = new( "PH", 0, 14 );
            PH.AddLabel( new FuzzySet( "Acid", new TrapezoidalFunction(0, 0, (float)5.6, 7) ) );
            PH.AddLabel( new FuzzySet( "Neutral", new TrapezoidalFunction((float)5.6, 7, (float)8.2) ) );
            PH.AddLabel( new FuzzySet( "Alkali", new TrapezoidalFunction(7, (float)8.2, 14, 14) ) );

            LinguisticVariable redox = new( "Redox", 0, 10 );
            redox.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            redox.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            redox.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable otherMethods = new( "OtherMethods", 0, 10 );
            otherMethods.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            otherMethods.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            otherMethods.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable neutralization = new( "Neutralization", 0, 10 );
            neutralization.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            neutralization.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            neutralization.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            neutralization.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            neutralization.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( PH );
            fuzzyDB.AddVariable( redox );
            fuzzyDB.AddVariable( otherMethods );
            fuzzyDB.AddVariable( neutralization );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF PH IS Acid and Redox IS Low and OtherMethods IS Low THEN Neutralization IS VeryLow");
            IS.NewRule("Rule 2", "IF PH IS Acid and Redox IS Low and OtherMethods IS Medium THEN Neutralization IS VeryLow");
            IS.NewRule("Rule 3", "IF PH IS Acid and Redox IS Low and OtherMethods IS High THEN Neutralization IS Low");
            IS.NewRule("Rule 4", "IF PH IS Acid and Redox IS Medium and OtherMethods IS Low THEN Neutralization IS VeryLow");
            IS.NewRule("Rule 5", "IF PH IS Acid and Redox IS Medium and OtherMethods IS Medium THEN Neutralization IS Low");
            IS.NewRule("Rule 6", "IF PH IS Acid and Redox IS Medium and OtherMethods IS High THEN Neutralization IS Middle");
            IS.NewRule("Rule 7", "IF PH IS Acid and Redox IS High and OtherMethods IS Low THEN Neutralization IS Low");
            IS.NewRule("Rule 8", "IF PH IS Acid and Redox IS High and OtherMethods IS Medium THEN Neutralization IS Middle");
            IS.NewRule("Rule 9", "IF PH IS Acid and Redox IS High and OtherMethods IS High THEN Neutralization IS High");
            IS.NewRule("Rule 10", "IF PH IS Neutral and Redox IS Low and OtherMethods IS Low THEN Neutralization IS VeryLow");
            IS.NewRule("Rule 11", "IF PH IS Neutral and Redox IS Low and OtherMethods IS Medium THEN Neutralization IS Low");
            IS.NewRule("Rule 12", "IF PH IS Neutral and Redox IS Low and OtherMethods IS High THEN Neutralization IS Middle");
            IS.NewRule("Rule 13", "IF PH IS Neutral and Redox IS Medium and OtherMethods IS Low THEN Neutralization IS Low");
            IS.NewRule("Rule 14", "IF PH IS Neutral and Redox IS Medium and OtherMethods IS Medium THEN Neutralization IS Middle");
            IS.NewRule("Rule 15", "IF PH IS Neutral and Redox IS Medium and OtherMethods IS High THEN Neutralization IS High");
            IS.NewRule("Rule 16", "IF PH IS Neutral and Redox IS High and OtherMethods IS Low THEN Neutralization IS Middle");
            IS.NewRule("Rule 17", "IF PH IS Neutral and Redox IS High and OtherMethods IS Medium THEN Neutralization IS High");
            IS.NewRule("Rule 18", "IF PH IS Neutral and Redox IS High and OtherMethods IS High THEN Neutralization IS VeryHigh");
            IS.NewRule("Rule 19", "IF PH IS Alkali and Redox IS Low and OtherMethods IS Low THEN Neutralization IS Low");
            IS.NewRule("Rule 20", "IF PH IS Alkali and Redox IS Low and OtherMethods IS Medium THEN Neutralization IS Middle");
            IS.NewRule("Rule 21", "IF PH IS Alkali and Redox IS Low and OtherMethods IS High THEN Neutralization IS High");
            IS.NewRule("Rule 22", "IF PH IS Alkali and Redox IS Medium and OtherMethods IS Low THEN Neutralization IS Middle");
            IS.NewRule("Rule 23", "IF PH IS Alkali and Redox IS Medium and OtherMethods IS Medium THEN Neutralization IS High");
            IS.NewRule("Rule 24", "IF PH IS Alkali and Redox IS Medium and OtherMethods IS High THEN Neutralization IS VeryHigh");
            IS.NewRule("Rule 25", "IF PH IS Alkali and Redox IS High and OtherMethods IS Low THEN Neutralization IS High");
            IS.NewRule("Rule 26", "IF PH IS Alkali and Redox IS High and OtherMethods IS Medium THEN Neutralization IS VeryHigh");
            IS.NewRule("Rule 27", "IF PH IS Alkali and Redox IS High and OtherMethods IS High THEN Neutralization IS VeryHigh");

            IS.SetInput("PH", (float)PHValue);
            IS.SetInput("Redox", (float)redoxValue);
            IS.SetInput("OtherMethods", (float)otherMethodsValue);

            double resultado = IS.Evaluate("Neutralization");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("PH", i == 0 ? 0 : (float)13.99);
                IS.SetInput("Redox", i == 0 ? 0 : (float)9.99);
                IS.SetInput("OtherMethods", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Neutralization");
            }
            double m = (IS.GetLinguisticVariable("Neutralization").End - IS.GetLinguisticVariable("Neutralization").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Neutralization").End;

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

        public string CalculateMaterialConcentration(double biologicalTreatmentValue, double reverseOsmosisValue, double evaporatorValue)
        {
            LinguisticVariable biologicalTreatment = new( "BiologicalTreatment", 0, 10 );
            biologicalTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            biologicalTreatment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            biologicalTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reverseOsmosis = new( "ReverseOsmosis", 0, 10 );
            reverseOsmosis.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reverseOsmosis.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reverseOsmosis.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable evaporator = new( "Evaporator", 0, 10 );
            evaporator.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            evaporator.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            evaporator.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable materialConcentration = new( "MaterialConcentration", 0, 10 );
            materialConcentration.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            materialConcentration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            materialConcentration.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            materialConcentration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            materialConcentration.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( biologicalTreatment );
            fuzzyDB.AddVariable( reverseOsmosis );
            fuzzyDB.AddVariable( evaporator );
            fuzzyDB.AddVariable( materialConcentration );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Low and Evaporator IS Low THEN MaterialConcentration IS VeryLow");
            IS.NewRule("Rule 2", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Low and Evaporator IS Medium THEN MaterialConcentration IS VeryLow");
            IS.NewRule("Rule 3", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Low and Evaporator IS High THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 4", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Medium and Evaporator IS Low THEN MaterialConcentration IS VeryLow");
            IS.NewRule("Rule 5", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Medium and Evaporator IS Medium THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 6", "IF BiologicalTreatment IS Low and ReverseOsmosis IS Medium and Evaporator IS High THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 7", "IF BiologicalTreatment IS Low and ReverseOsmosis IS High and Evaporator IS Low THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 8", "IF BiologicalTreatment IS Low and ReverseOsmosis IS High and Evaporator IS Medium THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 9", "IF BiologicalTreatment IS Low and ReverseOsmosis IS High and Evaporator IS High THEN MaterialConcentration IS High");
            IS.NewRule("Rule 10", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Low and Evaporator IS Low THEN MaterialConcentration IS VeryLow");
            IS.NewRule("Rule 11", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Low and Evaporator IS Medium THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 12", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Low and Evaporator IS High THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 13", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Medium and Evaporator IS Low THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 14", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Medium and Evaporator IS Medium THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 15", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS Medium and Evaporator IS High THEN MaterialConcentration IS High");
            IS.NewRule("Rule 16", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS High and Evaporator IS Low THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 17", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS High and Evaporator IS Medium THEN MaterialConcentration IS High");
            IS.NewRule("Rule 18", "IF BiologicalTreatment IS Medium and ReverseOsmosis IS High and Evaporator IS High THEN MaterialConcentration IS VeryHigh");
            IS.NewRule("Rule 19", "IF BiologicalTreatment IS High and ReverseOsmosis IS Low and Evaporator IS Low THEN MaterialConcentration IS Low");
            IS.NewRule("Rule 20", "IF BiologicalTreatment IS High and ReverseOsmosis IS Low and Evaporator IS Medium THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 21", "IF BiologicalTreatment IS High and ReverseOsmosis IS Low and Evaporator IS High THEN MaterialConcentration IS High");
            IS.NewRule("Rule 22", "IF BiologicalTreatment IS High and ReverseOsmosis IS Medium and Evaporator IS Low THEN MaterialConcentration IS Middle");
            IS.NewRule("Rule 23", "IF BiologicalTreatment IS High and ReverseOsmosis IS Medium and Evaporator IS Medium THEN MaterialConcentration IS High");
            IS.NewRule("Rule 24", "IF BiologicalTreatment IS High and ReverseOsmosis IS Medium and Evaporator IS High THEN MaterialConcentration IS VeryHigh");
            IS.NewRule("Rule 25", "IF BiologicalTreatment IS High and ReverseOsmosis IS High and Evaporator IS Low THEN MaterialConcentration IS High");
            IS.NewRule("Rule 26", "IF BiologicalTreatment IS High and ReverseOsmosis IS High and Evaporator IS Medium THEN MaterialConcentration IS VeryHigh");
            IS.NewRule("Rule 27", "IF BiologicalTreatment IS High and ReverseOsmosis IS High and Evaporator IS High THEN MaterialConcentration IS VeryHigh");

            IS.SetInput("BiologicalTreatment", (float)biologicalTreatmentValue);
            IS.SetInput("ReverseOsmosis", (float)reverseOsmosisValue);
            IS.SetInput("Evaporator", (float)evaporatorValue);

            double resultado = IS.Evaluate("MaterialConcentration");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("BiologicalTreatment", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReverseOsmosis", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Evaporator", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("MaterialConcentration");
            }
            double m = (IS.GetLinguisticVariable("MaterialConcentration").End - IS.GetLinguisticVariable("MaterialConcentration").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("MaterialConcentration").End;

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

        public string CalculateChemistry(double distillerUseValue, double useOfAbsorptionValue, double useOfAdsorptionValue)
        {
            LinguisticVariable distillerUse = new( "DistillerUse", 0, 10 );
            distillerUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            distillerUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            distillerUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfAbsorption = new( "UseOfAbsorption", 0, 10 );
            useOfAbsorption.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfAbsorption.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfAbsorption.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfAdsorption = new( "UseOfAdsorption", 0, 10 );
            useOfAdsorption.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfAdsorption.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfAdsorption.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable chemistry = new( "Chemistry", 0, 10 );
            chemistry.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chemistry.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chemistry.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            chemistry.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chemistry.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( distillerUse );
            fuzzyDB.AddVariable( useOfAbsorption );
            fuzzyDB.AddVariable( useOfAdsorption );
            fuzzyDB.AddVariable( chemistry );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DistillerUse IS Low and UseOfAbsorption IS Low and UseOfAdsorption IS Low THEN Chemistry IS VeryLow");
            IS.NewRule("Rule 2", "IF DistillerUse IS Low and UseOfAbsorption IS Low and UseOfAdsorption IS Medium THEN Chemistry IS VeryLow");
            IS.NewRule("Rule 3", "IF DistillerUse IS Low and UseOfAbsorption IS Low and UseOfAdsorption IS High THEN Chemistry IS Low");
            IS.NewRule("Rule 4", "IF DistillerUse IS Low and UseOfAbsorption IS Medium and UseOfAdsorption IS Low THEN Chemistry IS VeryLow");
            IS.NewRule("Rule 5", "IF DistillerUse IS Low and UseOfAbsorption IS Medium and UseOfAdsorption IS Medium THEN Chemistry IS Low");
            IS.NewRule("Rule 6", "IF DistillerUse IS Low and UseOfAbsorption IS Medium and UseOfAdsorption IS High THEN Chemistry IS Middle");
            IS.NewRule("Rule 7", "IF DistillerUse IS Low and UseOfAbsorption IS High and UseOfAdsorption IS Low THEN Chemistry IS Low");
            IS.NewRule("Rule 8", "IF DistillerUse IS Low and UseOfAbsorption IS High and UseOfAdsorption IS Medium THEN Chemistry IS Middle");
            IS.NewRule("Rule 9", "IF DistillerUse IS Low and UseOfAbsorption IS High and UseOfAdsorption IS High THEN Chemistry IS High");
            IS.NewRule("Rule 10", "IF DistillerUse IS Medium and UseOfAbsorption IS Low and UseOfAdsorption IS Low THEN Chemistry IS VeryLow");
            IS.NewRule("Rule 11", "IF DistillerUse IS Medium and UseOfAbsorption IS Low and UseOfAdsorption IS Medium THEN Chemistry IS Low");
            IS.NewRule("Rule 12", "IF DistillerUse IS Medium and UseOfAbsorption IS Low and UseOfAdsorption IS High THEN Chemistry IS Middle");
            IS.NewRule("Rule 13", "IF DistillerUse IS Medium and UseOfAbsorption IS Medium and UseOfAdsorption IS Low THEN Chemistry IS Low");
            IS.NewRule("Rule 14", "IF DistillerUse IS Medium and UseOfAbsorption IS Medium and UseOfAdsorption IS Medium THEN Chemistry IS Middle");
            IS.NewRule("Rule 15", "IF DistillerUse IS Medium and UseOfAbsorption IS Medium and UseOfAdsorption IS High THEN Chemistry IS High");
            IS.NewRule("Rule 16", "IF DistillerUse IS Medium and UseOfAbsorption IS High and UseOfAdsorption IS Low THEN Chemistry IS Middle");
            IS.NewRule("Rule 17", "IF DistillerUse IS Medium and UseOfAbsorption IS High and UseOfAdsorption IS Medium THEN Chemistry IS High");
            IS.NewRule("Rule 18", "IF DistillerUse IS Medium and UseOfAbsorption IS High and UseOfAdsorption IS High THEN Chemistry IS VeryHigh");
            IS.NewRule("Rule 19", "IF DistillerUse IS High and UseOfAbsorption IS Low and UseOfAdsorption IS Low THEN Chemistry IS Low");
            IS.NewRule("Rule 20", "IF DistillerUse IS High and UseOfAbsorption IS Low and UseOfAdsorption IS Medium THEN Chemistry IS Middle");
            IS.NewRule("Rule 21", "IF DistillerUse IS High and UseOfAbsorption IS Low and UseOfAdsorption IS High THEN Chemistry IS High");
            IS.NewRule("Rule 22", "IF DistillerUse IS High and UseOfAbsorption IS Medium and UseOfAdsorption IS Low THEN Chemistry IS Middle");
            IS.NewRule("Rule 23", "IF DistillerUse IS High and UseOfAbsorption IS Medium and UseOfAdsorption IS Medium THEN Chemistry IS High");
            IS.NewRule("Rule 24", "IF DistillerUse IS High and UseOfAbsorption IS Medium and UseOfAdsorption IS High THEN Chemistry IS VeryHigh");
            IS.NewRule("Rule 25", "IF DistillerUse IS High and UseOfAbsorption IS High and UseOfAdsorption IS Low THEN Chemistry IS High");
            IS.NewRule("Rule 26", "IF DistillerUse IS High and UseOfAbsorption IS High and UseOfAdsorption IS Medium THEN Chemistry IS VeryHigh");
            IS.NewRule("Rule 27", "IF DistillerUse IS High and UseOfAbsorption IS High and UseOfAdsorption IS High THEN Chemistry IS VeryHigh");

            IS.SetInput("DistillerUse", (float)distillerUseValue);
            IS.SetInput("UseOfAbsorption", (float)useOfAbsorptionValue);
            IS.SetInput("UseOfAdsorption", (float)useOfAdsorptionValue);

            double resultado = IS.Evaluate("Chemistry");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DistillerUse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfAbsorption", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfAdsorption", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Chemistry");
            }
            double m = (IS.GetLinguisticVariable("Chemistry").End - IS.GetLinguisticVariable("Chemistry").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Chemistry").End;

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

        public string CalculatePhysicist(double useOfMagneticSieveValue, double useOfFilteringValue, double useOfDecanterValue, double cycloneSeparationValue)
        {
            LinguisticVariable useOfMagneticSieve = new( "UseOfMagneticSieve", 0, 10 );
            useOfMagneticSieve.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfMagneticSieve.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfMagneticSieve.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfFiltering = new( "UseOfFiltering", 0, 10 );
            useOfFiltering.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfFiltering.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfFiltering.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfDecanter = new( "UseOfDecanter", 0, 10 );
            useOfDecanter.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfDecanter.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfDecanter.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable cycloneSeparation = new( "CycloneSeparation", 0, 10 );
            cycloneSeparation.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            cycloneSeparation.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            cycloneSeparation.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable physicist = new( "Physicist", 0, 10 );
            physicist.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            physicist.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            physicist.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            physicist.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            physicist.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfMagneticSieve );
            fuzzyDB.AddVariable( useOfFiltering );
            fuzzyDB.AddVariable( useOfDecanter );
            fuzzyDB.AddVariable( cycloneSeparation );
            fuzzyDB.AddVariable( physicist );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Low");
            IS.NewRule("Rule 4", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 6", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS Low");
            IS.NewRule("Rule 7", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 8", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 9", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 10", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 12", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Low");
            IS.NewRule("Rule 13", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 14", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 15", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 16", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 17", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 18", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 19", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 20", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 21", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 22", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 23", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 24", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 25", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 26", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 27", "IF UseOfMagneticSieve IS Low and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 28", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS VeryLow");
            IS.NewRule("Rule 29", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 30", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Low");
            IS.NewRule("Rule 31", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 32", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 33", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 34", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 35", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 36", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 37", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 38", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 39", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 40", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 41", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 42", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 43", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 44", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 45", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 46", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 47", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 48", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 49", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 50", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 51", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 52", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS High");
            IS.NewRule("Rule 53", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 54", "IF UseOfMagneticSieve IS Medium and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS VeryHigh");
            IS.NewRule("Rule 55", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 56", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Low");
            IS.NewRule("Rule 57", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS Middle");
            IS.NewRule("Rule 58", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 59", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 60", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 61", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 62", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 63", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Low and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 64", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Low");
            IS.NewRule("Rule 65", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS Middle");
            IS.NewRule("Rule 66", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 67", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 68", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 69", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 70", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS High");
            IS.NewRule("Rule 71", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 72", "IF UseOfMagneticSieve IS High and UseOfFiltering IS Medium and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS VeryHigh");
            IS.NewRule("Rule 73", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Low THEN Physicist IS Middle");
            IS.NewRule("Rule 74", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 75", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Low and CycloneSeparation IS High THEN Physicist IS High");
            IS.NewRule("Rule 76", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Low THEN Physicist IS High");
            IS.NewRule("Rule 77", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS Medium THEN Physicist IS High");
            IS.NewRule("Rule 78", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS Medium and CycloneSeparation IS High THEN Physicist IS VeryHigh");
            IS.NewRule("Rule 79", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Low THEN Physicist IS High");
            IS.NewRule("Rule 80", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS Medium THEN Physicist IS VeryHigh");
            IS.NewRule("Rule 81", "IF UseOfMagneticSieve IS High and UseOfFiltering IS High and UseOfDecanter IS High and CycloneSeparation IS High THEN Physicist IS VeryHigh");

            IS.SetInput("UseOfMagneticSieve", (float)useOfMagneticSieveValue);
            IS.SetInput("UseOfFiltering", (float)useOfFilteringValue);
            IS.SetInput("UseOfDecanter", (float)useOfDecanterValue);
            IS.SetInput("CycloneSeparation", (float)cycloneSeparationValue);

            double resultado = IS.Evaluate("Physicist");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfMagneticSieve", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfFiltering", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfDecanter", i == 0 ? 0 : (float)9.99);
                IS.SetInput("CycloneSeparation", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Physicist");
            }
            double m = (IS.GetLinguisticVariable("Physicist").End - IS.GetLinguisticVariable("Physicist").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Physicist").End;

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

        public string CalculateRecover(double recoveryOfMetalsFromWaterValue, double waterTreatmentAndReuseValue, double recoveryAndReuseOfCoolingWaterValue)
        {
            LinguisticVariable recoveryOfMetalsFromWater = new( "RecoveryOfMetalsFromWater", 0, 10 );
            recoveryOfMetalsFromWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recoveryOfMetalsFromWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recoveryOfMetalsFromWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable waterTreatmentAndReuse = new( "WaterTreatmentAndReuse", 0, 10 );
            waterTreatmentAndReuse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            waterTreatmentAndReuse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            waterTreatmentAndReuse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recoveryAndReuseOfCoolingWater = new( "RecoveryAndReuseOfCoolingWater", 0, 10 );
            recoveryAndReuseOfCoolingWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recoveryAndReuseOfCoolingWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recoveryAndReuseOfCoolingWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recover = new( "Recover", 0, 10 );
            recover.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recover.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recover.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            recover.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recover.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( recoveryOfMetalsFromWater );
            fuzzyDB.AddVariable( waterTreatmentAndReuse );
            fuzzyDB.AddVariable( recoveryAndReuseOfCoolingWater );
            fuzzyDB.AddVariable( recover );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS VeryLow");
            IS.NewRule("Rule 2", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS VeryLow");
            IS.NewRule("Rule 3", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS Low");
            IS.NewRule("Rule 4", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS VeryLow");
            IS.NewRule("Rule 5", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS Low");
            IS.NewRule("Rule 6", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS Middle");
            IS.NewRule("Rule 7", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS Low");
            IS.NewRule("Rule 8", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS Middle");
            IS.NewRule("Rule 9", "IF RecoveryOfMetalsFromWater IS Low and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS High");
            IS.NewRule("Rule 10", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS VeryLow");
            IS.NewRule("Rule 11", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS Low");
            IS.NewRule("Rule 12", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS Middle");
            IS.NewRule("Rule 13", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS Low");
            IS.NewRule("Rule 14", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS Middle");
            IS.NewRule("Rule 15", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS High");
            IS.NewRule("Rule 16", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS Middle");
            IS.NewRule("Rule 17", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS High");
            IS.NewRule("Rule 18", "IF RecoveryOfMetalsFromWater IS Medium and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS VeryHigh");
            IS.NewRule("Rule 19", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS Low");
            IS.NewRule("Rule 20", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS Middle");
            IS.NewRule("Rule 21", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Low and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS High");
            IS.NewRule("Rule 22", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS Middle");
            IS.NewRule("Rule 23", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS High");
            IS.NewRule("Rule 24", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS Medium and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS VeryHigh");
            IS.NewRule("Rule 25", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Low THEN Recover IS High");
            IS.NewRule("Rule 26", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS Medium THEN Recover IS VeryHigh");
            IS.NewRule("Rule 27", "IF RecoveryOfMetalsFromWater IS High and WaterTreatmentAndReuse IS High and RecoveryAndReuseOfCoolingWater IS High THEN Recover IS VeryHigh");

            IS.SetInput("RecoveryOfMetalsFromWater", (float)recoveryOfMetalsFromWaterValue);
            IS.SetInput("WaterTreatmentAndReuse", (float)waterTreatmentAndReuseValue);
            IS.SetInput("RecoveryAndReuseOfCoolingWater", (float)recoveryAndReuseOfCoolingWaterValue);

            double resultado = IS.Evaluate("Recover");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("RecoveryOfMetalsFromWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WaterTreatmentAndReuse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RecoveryAndReuseOfCoolingWater", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Recover");
            }
            double m = (IS.GetLinguisticVariable("Recover").End - IS.GetLinguisticVariable("Recover").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Recover").End;

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

        public string CalculateEquipments(double useOfClosedProcessInTheProductionOfWasteWaterValue, double replacementOfCoolingWaterInIndustryValue, double recycledWaterMeasurementToReduceSewageFeesValue)
        {
            LinguisticVariable useOfClosedProcessInTheProductionOfWasteWater = new( "UseOfClosedProcessInTheProductionOfWasteWater", 0, 10 );
            useOfClosedProcessInTheProductionOfWasteWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfClosedProcessInTheProductionOfWasteWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfClosedProcessInTheProductionOfWasteWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable replacementOfCoolingWaterInIndustry = new( "ReplacementOfCoolingWaterInIndustry", 0, 10 );
            replacementOfCoolingWaterInIndustry.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            replacementOfCoolingWaterInIndustry.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            replacementOfCoolingWaterInIndustry.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable recycledWaterMeasurementToReduceSewageFees = new( "RecycledWaterMeasurementToReduceSewageFees", 0, 10 );
            recycledWaterMeasurementToReduceSewageFees.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recycledWaterMeasurementToReduceSewageFees.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recycledWaterMeasurementToReduceSewageFees.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable equipments = new( "Equipments", 0, 10 );
            equipments.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            equipments.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            equipments.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            equipments.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            equipments.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( useOfClosedProcessInTheProductionOfWasteWater );
            fuzzyDB.AddVariable( replacementOfCoolingWaterInIndustry );
            fuzzyDB.AddVariable( recycledWaterMeasurementToReduceSewageFees );
            fuzzyDB.AddVariable( equipments );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS VeryLow");
            IS.NewRule("Rule 2", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS VeryLow");
            IS.NewRule("Rule 3", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS Low");
            IS.NewRule("Rule 4", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS VeryLow");
            IS.NewRule("Rule 5", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS Low");
            IS.NewRule("Rule 6", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS Middle");
            IS.NewRule("Rule 7", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS Low");
            IS.NewRule("Rule 8", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS Middle");
            IS.NewRule("Rule 9", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Low and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS High");
            IS.NewRule("Rule 10", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS VeryLow");
            IS.NewRule("Rule 11", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS Low");
            IS.NewRule("Rule 12", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS Middle");
            IS.NewRule("Rule 13", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS Low");
            IS.NewRule("Rule 14", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS Middle");
            IS.NewRule("Rule 15", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS High");
            IS.NewRule("Rule 16", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS Middle");
            IS.NewRule("Rule 17", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS High");
            IS.NewRule("Rule 18", "IF UseOfClosedProcessInTheProductionOfWasteWater IS Medium and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS VeryHigh");
            IS.NewRule("Rule 19", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS Low");
            IS.NewRule("Rule 20", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS Middle");
            IS.NewRule("Rule 21", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Low and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS High");
            IS.NewRule("Rule 22", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS Middle");
            IS.NewRule("Rule 23", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS High");
            IS.NewRule("Rule 24", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS Medium and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS VeryHigh");
            IS.NewRule("Rule 25", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Low THEN Equipments IS High");
            IS.NewRule("Rule 26", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS Medium THEN Equipments IS VeryHigh");
            IS.NewRule("Rule 27", "IF UseOfClosedProcessInTheProductionOfWasteWater IS High and ReplacementOfCoolingWaterInIndustry IS High and RecycledWaterMeasurementToReduceSewageFees IS High THEN Equipments IS VeryHigh");

            IS.SetInput("UseOfClosedProcessInTheProductionOfWasteWater", (float)useOfClosedProcessInTheProductionOfWasteWaterValue);
            IS.SetInput("ReplacementOfCoolingWaterInIndustry", (float)replacementOfCoolingWaterInIndustryValue);
            IS.SetInput("RecycledWaterMeasurementToReduceSewageFees", (float)recycledWaterMeasurementToReduceSewageFeesValue);

            double resultado = IS.Evaluate("Equipments");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("UseOfClosedProcessInTheProductionOfWasteWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReplacementOfCoolingWaterInIndustry", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RecycledWaterMeasurementToReduceSewageFees", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Equipments");
            }
            double m = (IS.GetLinguisticVariable("Equipments").End - IS.GetLinguisticVariable("Equipments").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Equipments").End;

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

        public string CalculateWaterDevelopment(double waterTreatmentByMagneticTechnologyValue, double improvedProductionOfDeionizedWaterValue)
        {
            LinguisticVariable waterTreatmentByMagneticTechnology = new( "WaterTreatmentByMagneticTechnology", 0, 10 );
            waterTreatmentByMagneticTechnology.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterTreatmentByMagneticTechnology.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterTreatmentByMagneticTechnology.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterTreatmentByMagneticTechnology.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterTreatmentByMagneticTechnology.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable improvedProductionOfDeionizedWater = new( "ImprovedProductionOfDeionizedWater", 0, 10 );
            improvedProductionOfDeionizedWater.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            improvedProductionOfDeionizedWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            improvedProductionOfDeionizedWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            improvedProductionOfDeionizedWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            improvedProductionOfDeionizedWater.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable waterDevelopment = new( "WaterDevelopment", 0, 10 );
            waterDevelopment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterDevelopment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterDevelopment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterDevelopment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterDevelopment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( waterTreatmentByMagneticTechnology );
            fuzzyDB.AddVariable( improvedProductionOfDeionizedWater );
            fuzzyDB.AddVariable( waterDevelopment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF WaterTreatmentByMagneticTechnology IS VeryLow and ImprovedProductionOfDeionizedWater IS VeryLow THEN WaterDevelopment IS VeryLow");
            IS.NewRule("Rule 2", "IF WaterTreatmentByMagneticTechnology IS VeryLow and ImprovedProductionOfDeionizedWater IS Low THEN WaterDevelopment IS VeryLow");
            IS.NewRule("Rule 3", "IF WaterTreatmentByMagneticTechnology IS VeryLow and ImprovedProductionOfDeionizedWater IS Medium THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 4", "IF WaterTreatmentByMagneticTechnology IS VeryLow and ImprovedProductionOfDeionizedWater IS High THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 5", "IF WaterTreatmentByMagneticTechnology IS VeryLow and ImprovedProductionOfDeionizedWater IS VeryHigh THEN WaterDevelopment IS Middle");
            IS.NewRule("Rule 6", "IF WaterTreatmentByMagneticTechnology IS Low and ImprovedProductionOfDeionizedWater IS VeryLow THEN WaterDevelopment IS VeryLow");
            IS.NewRule("Rule 7", "IF WaterTreatmentByMagneticTechnology IS Low and ImprovedProductionOfDeionizedWater IS Low THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 8", "IF WaterTreatmentByMagneticTechnology IS Low and ImprovedProductionOfDeionizedWater IS Medium THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 9", "IF WaterTreatmentByMagneticTechnology IS Low and ImprovedProductionOfDeionizedWater IS High THEN WaterDevelopment IS Middle");
            IS.NewRule("Rule 10", "IF WaterTreatmentByMagneticTechnology IS Low and ImprovedProductionOfDeionizedWater IS VeryHigh THEN WaterDevelopment IS High");
            IS.NewRule("Rule 11", "IF WaterTreatmentByMagneticTechnology IS Middle and ImprovedProductionOfDeionizedWater IS VeryLow THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 12", "IF WaterTreatmentByMagneticTechnology IS Middle and ImprovedProductionOfDeionizedWater IS Low THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 13", "IF WaterTreatmentByMagneticTechnology IS Middle and ImprovedProductionOfDeionizedWater IS Medium THEN WaterDevelopment IS Middle");
            IS.NewRule("Rule 14", "IF WaterTreatmentByMagneticTechnology IS Middle and ImprovedProductionOfDeionizedWater IS High THEN WaterDevelopment IS High");
            IS.NewRule("Rule 15", "IF WaterTreatmentByMagneticTechnology IS Middle and ImprovedProductionOfDeionizedWater IS VeryHigh THEN WaterDevelopment IS High");
            IS.NewRule("Rule 16", "IF WaterTreatmentByMagneticTechnology IS High and ImprovedProductionOfDeionizedWater IS VeryLow THEN WaterDevelopment IS Low");
            IS.NewRule("Rule 17", "IF WaterTreatmentByMagneticTechnology IS High and ImprovedProductionOfDeionizedWater IS Low THEN WaterDevelopment IS Middle");
            IS.NewRule("Rule 18", "IF WaterTreatmentByMagneticTechnology IS High and ImprovedProductionOfDeionizedWater IS Medium THEN WaterDevelopment IS High");
            IS.NewRule("Rule 19", "IF WaterTreatmentByMagneticTechnology IS High and ImprovedProductionOfDeionizedWater IS High THEN WaterDevelopment IS High");
            IS.NewRule("Rule 20", "IF WaterTreatmentByMagneticTechnology IS High and ImprovedProductionOfDeionizedWater IS VeryHigh THEN WaterDevelopment IS VeryHigh");
            IS.NewRule("Rule 21", "IF WaterTreatmentByMagneticTechnology IS VeryHigh and ImprovedProductionOfDeionizedWater IS VeryLow THEN WaterDevelopment IS Middle");
            IS.NewRule("Rule 22", "IF WaterTreatmentByMagneticTechnology IS VeryHigh and ImprovedProductionOfDeionizedWater IS Low THEN WaterDevelopment IS High");
            IS.NewRule("Rule 23", "IF WaterTreatmentByMagneticTechnology IS VeryHigh and ImprovedProductionOfDeionizedWater IS Medium THEN WaterDevelopment IS High");
            IS.NewRule("Rule 24", "IF WaterTreatmentByMagneticTechnology IS VeryHigh and ImprovedProductionOfDeionizedWater IS High THEN WaterDevelopment IS VeryHigh");
            IS.NewRule("Rule 25", "IF WaterTreatmentByMagneticTechnology IS VeryHigh and ImprovedProductionOfDeionizedWater IS VeryHigh THEN WaterDevelopment IS VeryHigh");

            IS.SetInput("WaterTreatmentByMagneticTechnology", (float)waterTreatmentByMagneticTechnologyValue);
            IS.SetInput("ImprovedProductionOfDeionizedWater", (float)improvedProductionOfDeionizedWaterValue);

            double resultado = IS.Evaluate("WaterDevelopment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("WaterTreatmentByMagneticTechnology", i == 0 ? (float)9.99 : 0);
                IS.SetInput("ImprovedProductionOfDeionizedWater", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("WaterDevelopment");
            }
            double m = (IS.GetLinguisticVariable("WaterDevelopment").End - IS.GetLinguisticVariable("WaterDevelopment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WaterDevelopment").End;
            
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

        public string CalculateChlorination(double recyclingOfChlorinatedWaterValue, double useTheChlorinationWashWaterValue, double usingChlorineInTheGasPhaseValue)
        {
            LinguisticVariable recyclingOfChlorinatedWater = new( "RecyclingOfChlorinatedWater", 0, 10 );
            recyclingOfChlorinatedWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            recyclingOfChlorinatedWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            recyclingOfChlorinatedWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useTheChlorinationWashWater = new( "UseTheChlorinationWashWater", 0, 10 );
            useTheChlorinationWashWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useTheChlorinationWashWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useTheChlorinationWashWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable UusingChlorineInTheGasPhase = new( "UsingChlorineInTheGasPhase", 0, 10 );
            UusingChlorineInTheGasPhase.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            UusingChlorineInTheGasPhase.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            UusingChlorineInTheGasPhase.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable chlorination = new( "Chlorination", 0, 10 );
            chlorination.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chlorination.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chlorination.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            chlorination.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chlorination.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( recyclingOfChlorinatedWater );
            fuzzyDB.AddVariable( useTheChlorinationWashWater );
            fuzzyDB.AddVariable( UusingChlorineInTheGasPhase );
            fuzzyDB.AddVariable( chlorination );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS VeryLow");
            IS.NewRule("Rule 2", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS VeryLow");
            IS.NewRule("Rule 3", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS High THEN Chlorination IS Low");
            IS.NewRule("Rule 4", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS VeryLow");
            IS.NewRule("Rule 5", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS Low");
            IS.NewRule("Rule 6", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS High THEN Chlorination IS Middle");
            IS.NewRule("Rule 7", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS Low");
            IS.NewRule("Rule 8", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS Middle");
            IS.NewRule("Rule 9", "IF RecyclingOfChlorinatedWater IS Low and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS High THEN Chlorination IS High");
            IS.NewRule("Rule 10", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS VeryLow");
            IS.NewRule("Rule 11", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS Low");
            IS.NewRule("Rule 12", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS High THEN Chlorination IS Middle");
            IS.NewRule("Rule 13", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS Low");
            IS.NewRule("Rule 14", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS Middle");
            IS.NewRule("Rule 15", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS High THEN Chlorination IS High");
            IS.NewRule("Rule 16", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS Middle");
            IS.NewRule("Rule 17", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS High");
            IS.NewRule("Rule 18", "IF RecyclingOfChlorinatedWater IS Medium and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS High THEN Chlorination IS VeryHigh");
            IS.NewRule("Rule 19", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS Low");
            IS.NewRule("Rule 20", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS Middle");
            IS.NewRule("Rule 21", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Low and UsingChlorineInTheGasPhase IS High THEN Chlorination IS High");
            IS.NewRule("Rule 22", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS Middle");
            IS.NewRule("Rule 23", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS High");
            IS.NewRule("Rule 24", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS Medium and UsingChlorineInTheGasPhase IS High THEN Chlorination IS VeryHigh");
            IS.NewRule("Rule 25", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Low THEN Chlorination IS High");
            IS.NewRule("Rule 26", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS Medium THEN Chlorination IS VeryHigh");
            IS.NewRule("Rule 27", "IF RecyclingOfChlorinatedWater IS High and UseTheChlorinationWashWater IS High and UsingChlorineInTheGasPhase IS High THEN Chlorination IS VeryHigh");

            IS.SetInput("RecyclingOfChlorinatedWater", (float)recyclingOfChlorinatedWaterValue);
            IS.SetInput("UseTheChlorinationWashWater", (float)useTheChlorinationWashWaterValue);
            IS.SetInput("UsingChlorineInTheGasPhase", (float)usingChlorineInTheGasPhaseValue);

            double resultado = IS.Evaluate("Chlorination");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("RecyclingOfChlorinatedWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseTheChlorinationWashWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UsingChlorineInTheGasPhase", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Chlorination");
            }
            double m = (IS.GetLinguisticVariable("Chlorination").End - IS.GetLinguisticVariable("Chlorination").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Chlorination").End;

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

        public string CalculateQuality(double decreasedContaminationOfTreatmentWaterValue, double useOfDeionizedWaterValue, double regularCleaningOfDirtOnProductionLinesThatUseWaterValue)
        {
            LinguisticVariable decreasedContaminationOfTreatmentWater = new( "DecreasedContaminationOfTreatmentWater", 0, 10 );
            decreasedContaminationOfTreatmentWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            decreasedContaminationOfTreatmentWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            decreasedContaminationOfTreatmentWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfDeionizedWater = new( "UseOfDeionizedWater", 0, 10 );
            useOfDeionizedWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfDeionizedWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfDeionizedWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable regularCleaningOfDirtOnProductionLinesThatUseWater = new( "RegularCleaningOfDirtOnProductionLinesThatUseWater", 0, 10 );
            regularCleaningOfDirtOnProductionLinesThatUseWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            regularCleaningOfDirtOnProductionLinesThatUseWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            regularCleaningOfDirtOnProductionLinesThatUseWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable quality = new( "Quality", 0, 10 );
            quality.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            quality.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            quality.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            quality.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            quality.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( decreasedContaminationOfTreatmentWater );
            fuzzyDB.AddVariable( useOfDeionizedWater );
            fuzzyDB.AddVariable( regularCleaningOfDirtOnProductionLinesThatUseWater );
            fuzzyDB.AddVariable( quality );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS VeryLow");
            IS.NewRule("Rule 2", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS VeryLow");
            IS.NewRule("Rule 3", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS Low");
            IS.NewRule("Rule 4", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS VeryLow");
            IS.NewRule("Rule 5", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS Low");
            IS.NewRule("Rule 6", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS Middle");
            IS.NewRule("Rule 7", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS Low");
            IS.NewRule("Rule 8", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS Middle");
            IS.NewRule("Rule 9", "IF DecreasedContaminationOfTreatmentWater IS Low and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS High");
            IS.NewRule("Rule 10", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS VeryLow");
            IS.NewRule("Rule 11", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS Low");
            IS.NewRule("Rule 12", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS Middle");
            IS.NewRule("Rule 13", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS Low");
            IS.NewRule("Rule 14", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS Middle");
            IS.NewRule("Rule 15", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS High");
            IS.NewRule("Rule 16", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS Middle");
            IS.NewRule("Rule 17", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS High");
            IS.NewRule("Rule 18", "IF DecreasedContaminationOfTreatmentWater IS Medium and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS VeryHigh");
            IS.NewRule("Rule 19", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS Low");
            IS.NewRule("Rule 20", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS Middle");
            IS.NewRule("Rule 21", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Low and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS High");
            IS.NewRule("Rule 22", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS Middle");
            IS.NewRule("Rule 23", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS High");
            IS.NewRule("Rule 24", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS Medium and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS VeryHigh");
            IS.NewRule("Rule 25", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Low THEN Quality IS High");
            IS.NewRule("Rule 26", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS Medium THEN Quality IS VeryHigh");
            IS.NewRule("Rule 27", "IF DecreasedContaminationOfTreatmentWater IS High and UseOfDeionizedWater IS High and RegularCleaningOfDirtOnProductionLinesThatUseWater IS High THEN Quality IS VeryHigh");

            IS.SetInput("DecreasedContaminationOfTreatmentWater", (float)decreasedContaminationOfTreatmentWaterValue);
            IS.SetInput("UseOfDeionizedWater", (float)useOfDeionizedWaterValue);
            IS.SetInput("RegularCleaningOfDirtOnProductionLinesThatUseWater", (float)regularCleaningOfDirtOnProductionLinesThatUseWaterValue);

            double resultado = IS.Evaluate("Quality");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("DecreasedContaminationOfTreatmentWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfDeionizedWater", i == 0 ? 0 : (float)9.99);
                IS.SetInput("RegularCleaningOfDirtOnProductionLinesThatUseWater", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Quality");
            }
            double m = (IS.GetLinguisticVariable("Quality").End - IS.GetLinguisticVariable("Quality").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Quality").End;

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

        public string CalculateConversion(double quantificationOfWaterUseValue, double useOfValvesToControlEquipmentFlowValue, double replacementOfTreatedWaterWithWellWaterValue)
        {
            LinguisticVariable quantificationOfWaterUse = new( "QuantificationOfWaterUse", 0, 10 );
            quantificationOfWaterUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            quantificationOfWaterUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            quantificationOfWaterUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfValvesToControlEquipmentFlow = new( "UseOfValvesToControlEquipmentFlow", 0, 10 );
            useOfValvesToControlEquipmentFlow.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfValvesToControlEquipmentFlow.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfValvesToControlEquipmentFlow.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable replacementOfTreatedWaterWithWellWater = new( "ReplacementOfTreatedWaterWithWellWater", 0, 10 );
            replacementOfTreatedWaterWithWellWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            replacementOfTreatedWaterWithWellWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            replacementOfTreatedWaterWithWellWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable conversion = new( "Conversion", 0, 10 );
            conversion.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            conversion.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            conversion.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            conversion.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            conversion.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( quantificationOfWaterUse );
            fuzzyDB.AddVariable( useOfValvesToControlEquipmentFlow );
            fuzzyDB.AddVariable( replacementOfTreatedWaterWithWellWater );
            fuzzyDB.AddVariable( conversion );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS VeryLow");
            IS.NewRule("Rule 2", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS VeryLow");
            IS.NewRule("Rule 3", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS Low");
            IS.NewRule("Rule 4", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS VeryLow");
            IS.NewRule("Rule 5", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS Low");
            IS.NewRule("Rule 6", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS Middle");
            IS.NewRule("Rule 7", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS Low");
            IS.NewRule("Rule 8", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS Middle");
            IS.NewRule("Rule 9", "IF QuantificationOfWaterUse IS Low and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS High");
            IS.NewRule("Rule 10", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS VeryLow");
            IS.NewRule("Rule 11", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS Low");
            IS.NewRule("Rule 12", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS Middle");
            IS.NewRule("Rule 13", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS Low");
            IS.NewRule("Rule 14", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS Middle");
            IS.NewRule("Rule 15", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS High");
            IS.NewRule("Rule 16", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS Middle");
            IS.NewRule("Rule 17", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS High");
            IS.NewRule("Rule 18", "IF QuantificationOfWaterUse IS Medium and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS VeryHigh");
            IS.NewRule("Rule 19", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS Low");
            IS.NewRule("Rule 20", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS Middle");
            IS.NewRule("Rule 21", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Low and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS High");
            IS.NewRule("Rule 22", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS Middle");
            IS.NewRule("Rule 23", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS High");
            IS.NewRule("Rule 24", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS Medium and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS VeryHigh");
            IS.NewRule("Rule 25", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Low THEN Conversion IS High");
            IS.NewRule("Rule 26", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS Medium THEN Conversion IS VeryHigh");
            IS.NewRule("Rule 27", "IF QuantificationOfWaterUse IS High and UseOfValvesToControlEquipmentFlow IS High and ReplacementOfTreatedWaterWithWellWater IS High THEN Conversion IS VeryHigh");

            IS.SetInput("QuantificationOfWaterUse", (float)quantificationOfWaterUseValue);
            IS.SetInput("UseOfValvesToControlEquipmentFlow", (float)useOfValvesToControlEquipmentFlowValue);
            IS.SetInput("ReplacementOfTreatedWaterWithWellWater", (float)replacementOfTreatedWaterWithWellWaterValue);

            double resultado = IS.Evaluate("Conversion");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("QuantificationOfWaterUse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfValvesToControlEquipmentFlow", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReplacementOfTreatedWaterWithWellWater", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Conversion");
            }
            double m = (IS.GetLinguisticVariable("Conversion").End - IS.GetLinguisticVariable("Conversion").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Conversion").End;

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

        public string CalculateEquipment(double waterLevelControlInEquipmentValue, double eliminationOfLeaksInWaterLinesAndValvesValue, double replacementOfWaterRegretInProcessesValue)
        {
            LinguisticVariable waterLevelControlInEquipment = new( "WaterLevelControlInEquipment", 0, 10 );
            waterLevelControlInEquipment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            waterLevelControlInEquipment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            waterLevelControlInEquipment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable eliminationOfLeaksInWaterLinesAndValves = new( "EliminationOfLeaksInWaterLinesAndValves", 0, 10 );
            eliminationOfLeaksInWaterLinesAndValves.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            eliminationOfLeaksInWaterLinesAndValves.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            eliminationOfLeaksInWaterLinesAndValves.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable replacementOfWaterRegretInProcesses = new( "ReplacementOfWaterRegretInProcesses", 0, 10 );
            replacementOfWaterRegretInProcesses.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            replacementOfWaterRegretInProcesses.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            replacementOfWaterRegretInProcesses.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable equipment = new( "Equipment", 0, 10 );
            equipment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            equipment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            equipment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            equipment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            equipment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( waterLevelControlInEquipment );
            fuzzyDB.AddVariable( eliminationOfLeaksInWaterLinesAndValves );
            fuzzyDB.AddVariable( replacementOfWaterRegretInProcesses );
            fuzzyDB.AddVariable( equipment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS VeryLow");
            IS.NewRule("Rule 2", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS VeryLow");
            IS.NewRule("Rule 3", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS Low");
            IS.NewRule("Rule 4", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS VeryLow");
            IS.NewRule("Rule 5", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS Low");
            IS.NewRule("Rule 6", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS Middle");
            IS.NewRule("Rule 7", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS Low");
            IS.NewRule("Rule 8", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS Middle");
            IS.NewRule("Rule 9", "IF WaterLevelControlInEquipment IS Low and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS High");
            IS.NewRule("Rule 10", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS VeryLow");
            IS.NewRule("Rule 11", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS Low");
            IS.NewRule("Rule 12", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS Middle");
            IS.NewRule("Rule 13", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS Low");
            IS.NewRule("Rule 14", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS Middle");
            IS.NewRule("Rule 15", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS High");
            IS.NewRule("Rule 16", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS Middle");
            IS.NewRule("Rule 17", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS High");
            IS.NewRule("Rule 18", "IF WaterLevelControlInEquipment IS Medium and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS VeryHigh");
            IS.NewRule("Rule 19", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS Low");
            IS.NewRule("Rule 20", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS Middle");
            IS.NewRule("Rule 21", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Low and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS High");
            IS.NewRule("Rule 22", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS Middle");
            IS.NewRule("Rule 23", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS High");
            IS.NewRule("Rule 24", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS Medium and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS VeryHigh");
            IS.NewRule("Rule 25", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Low THEN Equipment IS High");
            IS.NewRule("Rule 26", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS Medium THEN Equipment IS VeryHigh");
            IS.NewRule("Rule 27", "IF WaterLevelControlInEquipment IS High and EliminationOfLeaksInWaterLinesAndValves IS High and ReplacementOfWaterRegretInProcesses IS High THEN Equipment IS VeryHigh");

            IS.SetInput("WaterLevelControlInEquipment", (float)waterLevelControlInEquipmentValue);
            IS.SetInput("EliminationOfLeaksInWaterLinesAndValves", (float)eliminationOfLeaksInWaterLinesAndValvesValue);
            IS.SetInput("ReplacementOfWaterRegretInProcesses", (float)replacementOfWaterRegretInProcessesValue);

            double resultado = IS.Evaluate("Equipment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("WaterLevelControlInEquipment", i == 0 ? 0 : (float)9.99);
                IS.SetInput("EliminationOfLeaksInWaterLinesAndValves", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReplacementOfWaterRegretInProcesses", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Equipment");
            }
            double m = (IS.GetLinguisticVariable("Equipment").End - IS.GetLinguisticVariable("Equipment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Equipment").End;

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

        public string CalculateUse(double reductionInWaterUseValue, double useOfCountercurrentRinsingValue, double minimalUseOfCoolingWaterValue)
        {
            LinguisticVariable reductionInWaterUse = new( "ReductionInWaterUse", 0, 10 );
            reductionInWaterUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reductionInWaterUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reductionInWaterUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable useOfCountercurrentRinsing = new( "UseOfCountercurrentRinsing", 0, 10 );
            useOfCountercurrentRinsing.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            useOfCountercurrentRinsing.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            useOfCountercurrentRinsing.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable minimalUseOfCoolingWater = new( "MinimalUseOfCoolingWater", 0, 10 );
            minimalUseOfCoolingWater.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            minimalUseOfCoolingWater.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            minimalUseOfCoolingWater.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            use.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            use.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reductionInWaterUse );
            fuzzyDB.AddVariable( useOfCountercurrentRinsing );
            fuzzyDB.AddVariable( minimalUseOfCoolingWater );
            fuzzyDB.AddVariable( use );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Low THEN Use IS VeryLow");
            IS.NewRule("Rule 2", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Medium THEN Use IS VeryLow");
            IS.NewRule("Rule 3", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS High THEN Use IS Low");
            IS.NewRule("Rule 4", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Low THEN Use IS VeryLow");
            IS.NewRule("Rule 5", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Medium THEN Use IS Low");
            IS.NewRule("Rule 6", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS High THEN Use IS Middle");
            IS.NewRule("Rule 7", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Low THEN Use IS Low");
            IS.NewRule("Rule 8", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Medium THEN Use IS Middle");
            IS.NewRule("Rule 9", "IF ReductionInWaterUse IS Low and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS High THEN Use IS High");
            IS.NewRule("Rule 10", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Low THEN Use IS VeryLow");
            IS.NewRule("Rule 11", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Medium THEN Use IS Low");
            IS.NewRule("Rule 12", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS High THEN Use IS Middle");
            IS.NewRule("Rule 13", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Low THEN Use IS Low");
            IS.NewRule("Rule 14", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Medium THEN Use IS Middle");
            IS.NewRule("Rule 15", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS High THEN Use IS High");
            IS.NewRule("Rule 16", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Low THEN Use IS Middle");
            IS.NewRule("Rule 17", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Medium THEN Use IS High");
            IS.NewRule("Rule 18", "IF ReductionInWaterUse IS Medium and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS High THEN Use IS VeryHigh");
            IS.NewRule("Rule 19", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Low THEN Use IS Low");
            IS.NewRule("Rule 20", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS Medium THEN Use IS Middle");
            IS.NewRule("Rule 21", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Low and MinimalUseOfCoolingWater IS High THEN Use IS High");
            IS.NewRule("Rule 22", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Low THEN Use IS Middle");
            IS.NewRule("Rule 23", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS Medium THEN Use IS High");
            IS.NewRule("Rule 24", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS Medium and MinimalUseOfCoolingWater IS High THEN Use IS VeryHigh");
            IS.NewRule("Rule 25", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Low THEN Use IS High");
            IS.NewRule("Rule 26", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS Medium THEN Use IS VeryHigh");
            IS.NewRule("Rule 27", "IF ReductionInWaterUse IS High and UseOfCountercurrentRinsing IS High and MinimalUseOfCoolingWater IS High THEN Use IS VeryHigh");

            IS.SetInput("ReductionInWaterUse", (float)reductionInWaterUseValue);
            IS.SetInput("UseOfCountercurrentRinsing", (float)useOfCountercurrentRinsingValue);
            IS.SetInput("MinimalUseOfCoolingWater", (float)minimalUseOfCoolingWaterValue);

            double resultado = IS.Evaluate("Use");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ReductionInWaterUse", i == 0 ? 0 : (float)9.99);
                IS.SetInput("UseOfCountercurrentRinsing", i == 0 ? 0 : (float)9.99);
                IS.SetInput("MinimalUseOfCoolingWater", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Use");
            }
            double m = (IS.GetLinguisticVariable("Use").End - IS.GetLinguisticVariable("Use").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Use").End;

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

        public string CalculateRemovalOfContaminants(double chemistryValue, double physicistValue)
        {
            LinguisticVariable chemistry = new( "Chemistry", 0, 10 );
            chemistry.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chemistry.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chemistry.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            chemistry.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chemistry.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable physicist = new( "Physicist", 0, 10 );
            physicist.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            physicist.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            physicist.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            physicist.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            physicist.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable removalOfContaminants = new( "RemovalOfContaminants", 0, 10 );
            removalOfContaminants.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( chemistry );
            fuzzyDB.AddVariable( physicist );
            fuzzyDB.AddVariable( removalOfContaminants );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Chemistry IS VeryLow and Physicist IS VeryLow THEN RemovalOfContaminants IS VeryLow");
            IS.NewRule("Rule 2", "IF Chemistry IS VeryLow and Physicist IS Low THEN RemovalOfContaminants IS VeryLow");
            IS.NewRule("Rule 3", "IF Chemistry IS VeryLow and Physicist IS Medium THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 4", "IF Chemistry IS VeryLow and Physicist IS High THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 5", "IF Chemistry IS VeryLow and Physicist IS VeryHigh THEN RemovalOfContaminants IS Middle");
            IS.NewRule("Rule 6", "IF Chemistry IS Low and Physicist IS VeryLow THEN RemovalOfContaminants IS VeryLow");
            IS.NewRule("Rule 7", "IF Chemistry IS Low and Physicist IS Low THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 8", "IF Chemistry IS Low and Physicist IS Medium THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 9", "IF Chemistry IS Low and Physicist IS High THEN RemovalOfContaminants IS Middle");
            IS.NewRule("Rule 10", "IF Chemistry IS Low and Physicist IS VeryHigh THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 11", "IF Chemistry IS Middle and Physicist IS VeryLow THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 12", "IF Chemistry IS Middle and Physicist IS Low THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 13", "IF Chemistry IS Middle and Physicist IS Medium THEN RemovalOfContaminants IS Middle");
            IS.NewRule("Rule 14", "IF Chemistry IS Middle and Physicist IS High THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 15", "IF Chemistry IS Middle and Physicist IS VeryHigh THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 16", "IF Chemistry IS High and Physicist IS VeryLow THEN RemovalOfContaminants IS Low");
            IS.NewRule("Rule 17", "IF Chemistry IS High and Physicist IS Low THEN RemovalOfContaminants IS Middle");
            IS.NewRule("Rule 18", "IF Chemistry IS High and Physicist IS Medium THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 19", "IF Chemistry IS High and Physicist IS High THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 20", "IF Chemistry IS High and Physicist IS VeryHigh THEN RemovalOfContaminants IS VeryHigh");
            IS.NewRule("Rule 21", "IF Chemistry IS VeryHigh and Physicist IS VeryLow THEN RemovalOfContaminants IS Middle");
            IS.NewRule("Rule 22", "IF Chemistry IS VeryHigh and Physicist IS Low THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 23", "IF Chemistry IS VeryHigh and Physicist IS Medium THEN RemovalOfContaminants IS High");
            IS.NewRule("Rule 24", "IF Chemistry IS VeryHigh and Physicist IS High THEN RemovalOfContaminants IS VeryHigh");
            IS.NewRule("Rule 25", "IF Chemistry IS VeryHigh and Physicist IS VeryHigh THEN RemovalOfContaminants IS VeryHigh");

            IS.SetInput("Chemistry", (float)chemistryValue);
            IS.SetInput("Physicist", (float)physicistValue);

            double resultado = IS.Evaluate("RemovalOfContaminants");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Chemistry", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Physicist", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("RemovalOfContaminants");
            }
            double m = (IS.GetLinguisticVariable("RemovalOfContaminants").End - IS.GetLinguisticVariable("RemovalOfContaminants").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("RemovalOfContaminants").End;
            
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

        public string CalculateCCWE(double equipmentsValue, double recoverValue)
        {
            LinguisticVariable equipments = new( "Equipments", 0, 10 );
            equipments.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            equipments.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            equipments.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            equipments.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            equipments.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable recover = new( "Recover", 0, 10 );
            recover.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            recover.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            recover.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            recover.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            recover.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable CCWE = new( "CCWE", 0, 10 );
            CCWE.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            CCWE.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            CCWE.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            CCWE.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            CCWE.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( equipments );
            fuzzyDB.AddVariable( recover );
            fuzzyDB.AddVariable( CCWE );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Equipments IS VeryLow and Recover IS VeryLow THEN CCWE IS VeryLow");
            IS.NewRule("Rule 2", "IF Equipments IS VeryLow and Recover IS Low THEN CCWE IS VeryLow");
            IS.NewRule("Rule 3", "IF Equipments IS VeryLow and Recover IS Medium THEN CCWE IS Low");
            IS.NewRule("Rule 4", "IF Equipments IS VeryLow and Recover IS High THEN CCWE IS Low");
            IS.NewRule("Rule 5", "IF Equipments IS VeryLow and Recover IS VeryHigh THEN CCWE IS Middle");
            IS.NewRule("Rule 6", "IF Equipments IS Low and Recover IS VeryLow THEN CCWE IS VeryLow");
            IS.NewRule("Rule 7", "IF Equipments IS Low and Recover IS Low THEN CCWE IS Low");
            IS.NewRule("Rule 8", "IF Equipments IS Low and Recover IS Medium THEN CCWE IS Low");
            IS.NewRule("Rule 9", "IF Equipments IS Low and Recover IS High THEN CCWE IS Middle");
            IS.NewRule("Rule 10", "IF Equipments IS Low and Recover IS VeryHigh THEN CCWE IS High");
            IS.NewRule("Rule 11", "IF Equipments IS Middle and Recover IS VeryLow THEN CCWE IS Low");
            IS.NewRule("Rule 12", "IF Equipments IS Middle and Recover IS Low THEN CCWE IS Low");
            IS.NewRule("Rule 13", "IF Equipments IS Middle and Recover IS Medium THEN CCWE IS Middle");
            IS.NewRule("Rule 14", "IF Equipments IS Middle and Recover IS High THEN CCWE IS High");
            IS.NewRule("Rule 15", "IF Equipments IS Middle and Recover IS VeryHigh THEN CCWE IS High");
            IS.NewRule("Rule 16", "IF Equipments IS High and Recover IS VeryLow THEN CCWE IS Low");
            IS.NewRule("Rule 17", "IF Equipments IS High and Recover IS Low THEN CCWE IS Middle");
            IS.NewRule("Rule 18", "IF Equipments IS High and Recover IS Medium THEN CCWE IS High");
            IS.NewRule("Rule 19", "IF Equipments IS High and Recover IS High THEN CCWE IS High");
            IS.NewRule("Rule 20", "IF Equipments IS High and Recover IS VeryHigh THEN CCWE IS VeryHigh");
            IS.NewRule("Rule 21", "IF Equipments IS VeryHigh and Recover IS VeryLow THEN CCWE IS Middle");
            IS.NewRule("Rule 22", "IF Equipments IS VeryHigh and Recover IS Low THEN CCWE IS High");
            IS.NewRule("Rule 23", "IF Equipments IS VeryHigh and Recover IS Medium THEN CCWE IS High");
            IS.NewRule("Rule 24", "IF Equipments IS VeryHigh and Recover IS High THEN CCWE IS VeryHigh");
            IS.NewRule("Rule 25", "IF Equipments IS VeryHigh and Recover IS VeryHigh THEN CCWE IS VeryHigh");

            IS.SetInput("Equipments", (float)equipmentsValue);
            IS.SetInput("Recover", (float)recoverValue);

            double resultado = IS.Evaluate("CCWE");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Equipments", i == 0 ? (float)9.99 : 0);
                IS.SetInput("Recover", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("CCWE");
            }
            double m = (IS.GetLinguisticVariable("CCWE").End - IS.GetLinguisticVariable("CCWE").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("CCWE").End;
            
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

        public string CalculateWT(double chlorinationValue, double waterDevelopmentValue, double replacementOfChlorineByO2Value)
        {
            LinguisticVariable chlorination = new( "Chlorination", 0, 10 );
            chlorination.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            chlorination.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            chlorination.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable waterDevelopment = new( "WaterDevelopment", 0, 10 );
            waterDevelopment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            waterDevelopment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            waterDevelopment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable replacementOfChlorineByO2 = new( "ReplacementOfChlorineByO2", 0, 10 );
            replacementOfChlorineByO2.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            replacementOfChlorineByO2.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            replacementOfChlorineByO2.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable WT = new( "WT", 0, 10 );
            WT.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            WT.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            WT.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            WT.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            WT.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( chlorination );
            fuzzyDB.AddVariable( waterDevelopment );
            fuzzyDB.AddVariable( replacementOfChlorineByO2 );
            fuzzyDB.AddVariable( WT );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Chlorination IS Low and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Low THEN WT IS VeryLow");
            IS.NewRule("Rule 2", "IF Chlorination IS Low and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Medium THEN WT IS VeryLow");
            IS.NewRule("Rule 3", "IF Chlorination IS Low and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS High THEN WT IS Low");
            IS.NewRule("Rule 4", "IF Chlorination IS Low and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Low THEN WT IS VeryLow");
            IS.NewRule("Rule 5", "IF Chlorination IS Low and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Medium THEN WT IS Low");
            IS.NewRule("Rule 6", "IF Chlorination IS Low and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS High THEN WT IS Middle");
            IS.NewRule("Rule 7", "IF Chlorination IS Low and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Low THEN WT IS Low");
            IS.NewRule("Rule 8", "IF Chlorination IS Low and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Medium THEN WT IS Middle");
            IS.NewRule("Rule 9", "IF Chlorination IS Low and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS High THEN WT IS High");
            IS.NewRule("Rule 10", "IF Chlorination IS Medium and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Low THEN WT IS VeryLow");
            IS.NewRule("Rule 11", "IF Chlorination IS Medium and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Medium THEN WT IS Low");
            IS.NewRule("Rule 12", "IF Chlorination IS Medium and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS High THEN WT IS Middle");
            IS.NewRule("Rule 13", "IF Chlorination IS Medium and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Low THEN WT IS Low");
            IS.NewRule("Rule 14", "IF Chlorination IS Medium and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Medium THEN WT IS Middle");
            IS.NewRule("Rule 15", "IF Chlorination IS Medium and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS High THEN WT IS High");
            IS.NewRule("Rule 16", "IF Chlorination IS Medium and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Low THEN WT IS Middle");
            IS.NewRule("Rule 17", "IF Chlorination IS Medium and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Medium THEN WT IS High");
            IS.NewRule("Rule 18", "IF Chlorination IS Medium and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS High THEN WT IS VeryHigh");
            IS.NewRule("Rule 19", "IF Chlorination IS High and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Low THEN WT IS Low");
            IS.NewRule("Rule 20", "IF Chlorination IS High and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS Medium THEN WT IS Middle");
            IS.NewRule("Rule 21", "IF Chlorination IS High and WaterDevelopment IS Low and ReplacementOfChlorineByO2 IS High THEN WT IS High");
            IS.NewRule("Rule 22", "IF Chlorination IS High and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Low THEN WT IS Middle");
            IS.NewRule("Rule 23", "IF Chlorination IS High and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS Medium THEN WT IS High");
            IS.NewRule("Rule 24", "IF Chlorination IS High and WaterDevelopment IS Medium and ReplacementOfChlorineByO2 IS High THEN WT IS VeryHigh");
            IS.NewRule("Rule 25", "IF Chlorination IS High and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Low THEN WT IS High");
            IS.NewRule("Rule 26", "IF Chlorination IS High and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS Medium THEN WT IS VeryHigh");
            IS.NewRule("Rule 27", "IF Chlorination IS High and WaterDevelopment IS High and ReplacementOfChlorineByO2 IS High THEN WT IS VeryHigh");

            IS.SetInput("Chlorination", (float)chlorinationValue);
            IS.SetInput("WaterDevelopment", (float)waterDevelopmentValue);
            IS.SetInput("ReplacementOfChlorineByO2", (float)replacementOfChlorineByO2Value);

            double resultado = IS.Evaluate("WT");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Chlorination", i == 0 ? 0 : (float)9.99);
                IS.SetInput("WaterDevelopment", i == 0 ? 0 : (float)9.99);
                IS.SetInput("ReplacementOfChlorineByO2", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("WT");
            }
            double m = (IS.GetLinguisticVariable("WT").End - IS.GetLinguisticVariable("WT").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WT").End;

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

        public string CalculateReduction(double useValue, double equipmentValue, double conversionValue)
        {
            LinguisticVariable use = new( "Use", 0, 10 );
            use.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            use.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            use.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable equipment = new( "Equipment", 0, 10 );
            equipment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            equipment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            equipment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable conversion = new( "Conversion", 0, 10 );
            conversion.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            conversion.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            conversion.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable reduction = new( "Reduction", 0, 10 );
            reduction.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            reduction.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            reduction.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            reduction.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            reduction.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( use );
            fuzzyDB.AddVariable( equipment );
            fuzzyDB.AddVariable( conversion );
            fuzzyDB.AddVariable( reduction );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Use IS Low and Equipment IS Low and Conversion IS Low THEN Reduction IS VeryLow");
            IS.NewRule("Rule 2", "IF Use IS Low and Equipment IS Low and Conversion IS Medium THEN Reduction IS VeryLow");
            IS.NewRule("Rule 3", "IF Use IS Low and Equipment IS Low and Conversion IS High THEN Reduction IS Low");
            IS.NewRule("Rule 4", "IF Use IS Low and Equipment IS Medium and Conversion IS Low THEN Reduction IS VeryLow");
            IS.NewRule("Rule 5", "IF Use IS Low and Equipment IS Medium and Conversion IS Medium THEN Reduction IS Low");
            IS.NewRule("Rule 6", "IF Use IS Low and Equipment IS Medium and Conversion IS High THEN Reduction IS Middle");
            IS.NewRule("Rule 7", "IF Use IS Low and Equipment IS High and Conversion IS Low THEN Reduction IS Low");
            IS.NewRule("Rule 8", "IF Use IS Low and Equipment IS High and Conversion IS Medium THEN Reduction IS Middle");
            IS.NewRule("Rule 9", "IF Use IS Low and Equipment IS High and Conversion IS High THEN Reduction IS High");
            IS.NewRule("Rule 10", "IF Use IS Medium and Equipment IS Low and Conversion IS Low THEN Reduction IS VeryLow");
            IS.NewRule("Rule 11", "IF Use IS Medium and Equipment IS Low and Conversion IS Medium THEN Reduction IS Low");
            IS.NewRule("Rule 12", "IF Use IS Medium and Equipment IS Low and Conversion IS High THEN Reduction IS Middle");
            IS.NewRule("Rule 13", "IF Use IS Medium and Equipment IS Medium and Conversion IS Low THEN Reduction IS Low");
            IS.NewRule("Rule 14", "IF Use IS Medium and Equipment IS Medium and Conversion IS Medium THEN Reduction IS Middle");
            IS.NewRule("Rule 15", "IF Use IS Medium and Equipment IS Medium and Conversion IS High THEN Reduction IS High");
            IS.NewRule("Rule 16", "IF Use IS Medium and Equipment IS High and Conversion IS Low THEN Reduction IS Middle");
            IS.NewRule("Rule 17", "IF Use IS Medium and Equipment IS High and Conversion IS Medium THEN Reduction IS High");
            IS.NewRule("Rule 18", "IF Use IS Medium and Equipment IS High and Conversion IS High THEN Reduction IS VeryHigh");
            IS.NewRule("Rule 19", "IF Use IS High and Equipment IS Low and Conversion IS Low THEN Reduction IS Low");
            IS.NewRule("Rule 20", "IF Use IS High and Equipment IS Low and Conversion IS Medium THEN Reduction IS Middle");
            IS.NewRule("Rule 21", "IF Use IS High and Equipment IS Low and Conversion IS High THEN Reduction IS High");
            IS.NewRule("Rule 22", "IF Use IS High and Equipment IS Medium and Conversion IS Low THEN Reduction IS Middle");
            IS.NewRule("Rule 23", "IF Use IS High and Equipment IS Medium and Conversion IS Medium THEN Reduction IS High");
            IS.NewRule("Rule 24", "IF Use IS High and Equipment IS Medium and Conversion IS High THEN Reduction IS VeryHigh");
            IS.NewRule("Rule 25", "IF Use IS High and Equipment IS High and Conversion IS Low THEN Reduction IS High");
            IS.NewRule("Rule 26", "IF Use IS High and Equipment IS High and Conversion IS Medium THEN Reduction IS VeryHigh");
            IS.NewRule("Rule 27", "IF Use IS High and Equipment IS High and Conversion IS High THEN Reduction IS VeryHigh");

            IS.SetInput("Use", (float)useValue);
            IS.SetInput("Equipment", (float)equipmentValue);
            IS.SetInput("Conversion", (float)conversionValue);

            double resultado = IS.Evaluate("Reduction");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Use", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Equipment", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Conversion", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("Reduction");
            }
            double m = (IS.GetLinguisticVariable("Reduction").End - IS.GetLinguisticVariable("Reduction").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Reduction").End;

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

        public string CalculatePostGenerationTreatment(double removalOfContaminantsValue, double materialConcentrationValue, double neutralizationValue)
        {
            LinguisticVariable removalOfContaminants = new( "RemovalOfContaminants", 0, 10 );
            removalOfContaminants.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            removalOfContaminants.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable materialConcentration = new( "MaterialConcentration", 0, 10 );
            materialConcentration.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            materialConcentration.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            materialConcentration.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable neutralization = new( "Neutralization", 0, 10 );
            neutralization.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            neutralization.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            neutralization.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable postGenerationTreatment = new( "PostGenerationTreatment", 0, 10 );
            postGenerationTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( removalOfContaminants );
            fuzzyDB.AddVariable( materialConcentration );
            fuzzyDB.AddVariable( neutralization );
            fuzzyDB.AddVariable( postGenerationTreatment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Low and Neutralization IS Low THEN PostGenerationTreatment IS VeryLow");
            IS.NewRule("Rule 2", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Low and Neutralization IS Medium THEN PostGenerationTreatment IS VeryLow");
            IS.NewRule("Rule 3", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Low and Neutralization IS High THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 4", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Medium and Neutralization IS Low THEN PostGenerationTreatment IS VeryLow");
            IS.NewRule("Rule 5", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Medium and Neutralization IS Medium THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 6", "IF RemovalOfContaminants IS Low and MaterialConcentration IS Medium and Neutralization IS High THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 7", "IF RemovalOfContaminants IS Low and MaterialConcentration IS High and Neutralization IS Low THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 8", "IF RemovalOfContaminants IS Low and MaterialConcentration IS High and Neutralization IS Medium THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 9", "IF RemovalOfContaminants IS Low and MaterialConcentration IS High and Neutralization IS High THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 10", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Low and Neutralization IS Low THEN PostGenerationTreatment IS VeryLow");
            IS.NewRule("Rule 11", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Low and Neutralization IS Medium THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 12", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Low and Neutralization IS High THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 13", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Medium and Neutralization IS Low THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 14", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Medium and Neutralization IS Medium THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 15", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS Medium and Neutralization IS High THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 16", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS High and Neutralization IS Low THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 17", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS High and Neutralization IS Medium THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 18", "IF RemovalOfContaminants IS Medium and MaterialConcentration IS High and Neutralization IS High THEN PostGenerationTreatment IS VeryHigh");
            IS.NewRule("Rule 19", "IF RemovalOfContaminants IS High and MaterialConcentration IS Low and Neutralization IS Low THEN PostGenerationTreatment IS Low");
            IS.NewRule("Rule 20", "IF RemovalOfContaminants IS High and MaterialConcentration IS Low and Neutralization IS Medium THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 21", "IF RemovalOfContaminants IS High and MaterialConcentration IS Low and Neutralization IS High THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 22", "IF RemovalOfContaminants IS High and MaterialConcentration IS Medium and Neutralization IS Low THEN PostGenerationTreatment IS Middle");
            IS.NewRule("Rule 23", "IF RemovalOfContaminants IS High and MaterialConcentration IS Medium and Neutralization IS Medium THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 24", "IF RemovalOfContaminants IS High and MaterialConcentration IS Medium and Neutralization IS High THEN PostGenerationTreatment IS VeryHigh");
            IS.NewRule("Rule 25", "IF RemovalOfContaminants IS High and MaterialConcentration IS High and Neutralization IS Low THEN PostGenerationTreatment IS High");
            IS.NewRule("Rule 26", "IF RemovalOfContaminants IS High and MaterialConcentration IS High and Neutralization IS Medium THEN PostGenerationTreatment IS VeryHigh");
            IS.NewRule("Rule 27", "IF RemovalOfContaminants IS High and MaterialConcentration IS High and Neutralization IS High THEN PostGenerationTreatment IS VeryHigh");

            IS.SetInput("RemovalOfContaminants", (float)removalOfContaminantsValue);
            IS.SetInput("MaterialConcentration", (float)materialConcentrationValue);
            IS.SetInput("Neutralization", (float)neutralizationValue);

            double resultado = IS.Evaluate("PostGenerationTreatment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("RemovalOfContaminants", i == 0 ? 0 : (float)9.99);
                IS.SetInput("MaterialConcentration", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Neutralization", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("PostGenerationTreatment");
            }
            double m = (IS.GetLinguisticVariable("PostGenerationTreatment").End - IS.GetLinguisticVariable("PostGenerationTreatment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("PostGenerationTreatment").End;

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

        public string CalculateUtility(double WTValue, double CCWEValue)
        {
            LinguisticVariable WT = new( "WT", 0, 10 );
            WT.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            WT.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            WT.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            WT.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            WT.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable CCWE = new( "CCWE", 0, 10 );
            CCWE.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            CCWE.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            CCWE.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            CCWE.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            CCWE.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable utility = new( "Utility", 0, 10 );
            utility.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            utility.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            utility.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            utility.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            utility.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( WT );
            fuzzyDB.AddVariable( CCWE );
            fuzzyDB.AddVariable( utility );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF WT IS VeryLow and CCWE IS VeryLow THEN Utility IS VeryLow");
            IS.NewRule("Rule 2", "IF WT IS VeryLow and CCWE IS Low THEN Utility IS VeryLow");
            IS.NewRule("Rule 3", "IF WT IS VeryLow and CCWE IS Medium THEN Utility IS Low");
            IS.NewRule("Rule 4", "IF WT IS VeryLow and CCWE IS High THEN Utility IS Low");
            IS.NewRule("Rule 5", "IF WT IS VeryLow and CCWE IS VeryHigh THEN Utility IS Middle");
            IS.NewRule("Rule 6", "IF WT IS Low and CCWE IS VeryLow THEN Utility IS VeryLow");
            IS.NewRule("Rule 7", "IF WT IS Low and CCWE IS Low THEN Utility IS Low");
            IS.NewRule("Rule 8", "IF WT IS Low and CCWE IS Medium THEN Utility IS Low");
            IS.NewRule("Rule 9", "IF WT IS Low and CCWE IS High THEN Utility IS Middle");
            IS.NewRule("Rule 10", "IF WT IS Low and CCWE IS VeryHigh THEN Utility IS High");
            IS.NewRule("Rule 11", "IF WT IS Middle and CCWE IS VeryLow THEN Utility IS Low");
            IS.NewRule("Rule 12", "IF WT IS Middle and CCWE IS Low THEN Utility IS Low");
            IS.NewRule("Rule 13", "IF WT IS Middle and CCWE IS Medium THEN Utility IS Middle");
            IS.NewRule("Rule 14", "IF WT IS Middle and CCWE IS High THEN Utility IS High");
            IS.NewRule("Rule 15", "IF WT IS Middle and CCWE IS VeryHigh THEN Utility IS High");
            IS.NewRule("Rule 16", "IF WT IS High and CCWE IS VeryLow THEN Utility IS Low");
            IS.NewRule("Rule 17", "IF WT IS High and CCWE IS Low THEN Utility IS Middle");
            IS.NewRule("Rule 18", "IF WT IS High and CCWE IS Medium THEN Utility IS High");
            IS.NewRule("Rule 19", "IF WT IS High and CCWE IS High THEN Utility IS High");
            IS.NewRule("Rule 20", "IF WT IS High and CCWE IS VeryHigh THEN Utility IS VeryHigh");
            IS.NewRule("Rule 21", "IF WT IS VeryHigh and CCWE IS VeryLow THEN Utility IS Middle");
            IS.NewRule("Rule 22", "IF WT IS VeryHigh and CCWE IS Low THEN Utility IS High");
            IS.NewRule("Rule 23", "IF WT IS VeryHigh and CCWE IS Medium THEN Utility IS High");
            IS.NewRule("Rule 24", "IF WT IS VeryHigh and CCWE IS High THEN Utility IS VeryHigh");
            IS.NewRule("Rule 25", "IF WT IS VeryHigh and CCWE IS VeryHigh THEN Utility IS VeryHigh");

            IS.SetInput("WT", (float)WTValue);
            IS.SetInput("CCWE", (float)CCWEValue);

            double resultado = IS.Evaluate("Utility");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("WT", i == 0 ? (float)9.99 : 0);
                IS.SetInput("CCWE", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("Utility");
            }
            double m = (IS.GetLinguisticVariable("Utility").End - IS.GetLinguisticVariable("Utility").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("Utility").End;
            
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

        public string CalculateWaterUse(double reductionValue, double utilityValue, double qualityValue)
        {
            LinguisticVariable reduction = new( "Reduction", 0, 10 );
            reduction.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            reduction.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            reduction.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable utility = new( "Utility", 0, 10 );
            utility.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            utility.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            utility.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable quality = new( "Quality", 0, 10 );
            quality.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(0, 0, (float)2.5, 5) ) );
            quality.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction((float)2.5, 5, (float)7.5) ) );
            quality.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, (float)7.5, 10, 10) ) );

            LinguisticVariable waterUse = new( "WaterUse", 0, 10 );
            waterUse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterUse.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            waterUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterUse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( reduction );
            fuzzyDB.AddVariable( utility );
            fuzzyDB.AddVariable( quality );
            fuzzyDB.AddVariable( waterUse );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF Reduction IS Low and Utility IS Low and Quality IS Low THEN WaterUse IS VeryLow");
            IS.NewRule("Rule 2", "IF Reduction IS Low and Utility IS Low and Quality IS Medium THEN WaterUse IS VeryLow");
            IS.NewRule("Rule 3", "IF Reduction IS Low and Utility IS Low and Quality IS High THEN WaterUse IS Low");
            IS.NewRule("Rule 4", "IF Reduction IS Low and Utility IS Medium and Quality IS Low THEN WaterUse IS VeryLow");
            IS.NewRule("Rule 5", "IF Reduction IS Low and Utility IS Medium and Quality IS Medium THEN WaterUse IS Low");
            IS.NewRule("Rule 6", "IF Reduction IS Low and Utility IS Medium and Quality IS High THEN WaterUse IS Middle");
            IS.NewRule("Rule 7", "IF Reduction IS Low and Utility IS High and Quality IS Low THEN WaterUse IS Low");
            IS.NewRule("Rule 8", "IF Reduction IS Low and Utility IS High and Quality IS Medium THEN WaterUse IS Middle");
            IS.NewRule("Rule 9", "IF Reduction IS Low and Utility IS High and Quality IS High THEN WaterUse IS High");
            IS.NewRule("Rule 10", "IF Reduction IS Medium and Utility IS Low and Quality IS Low THEN WaterUse IS VeryLow");
            IS.NewRule("Rule 11", "IF Reduction IS Medium and Utility IS Low and Quality IS Medium THEN WaterUse IS Low");
            IS.NewRule("Rule 12", "IF Reduction IS Medium and Utility IS Low and Quality IS High THEN WaterUse IS Middle");
            IS.NewRule("Rule 13", "IF Reduction IS Medium and Utility IS Medium and Quality IS Low THEN WaterUse IS Low");
            IS.NewRule("Rule 14", "IF Reduction IS Medium and Utility IS Medium and Quality IS Medium THEN WaterUse IS Middle");
            IS.NewRule("Rule 15", "IF Reduction IS Medium and Utility IS Medium and Quality IS High THEN WaterUse IS High");
            IS.NewRule("Rule 16", "IF Reduction IS Medium and Utility IS High and Quality IS Low THEN WaterUse IS Middle");
            IS.NewRule("Rule 17", "IF Reduction IS Medium and Utility IS High and Quality IS Medium THEN WaterUse IS High");
            IS.NewRule("Rule 18", "IF Reduction IS Medium and Utility IS High and Quality IS High THEN WaterUse IS VeryHigh");
            IS.NewRule("Rule 19", "IF Reduction IS High and Utility IS Low and Quality IS Low THEN WaterUse IS Low");
            IS.NewRule("Rule 20", "IF Reduction IS High and Utility IS Low and Quality IS Medium THEN WaterUse IS Middle");
            IS.NewRule("Rule 21", "IF Reduction IS High and Utility IS Low and Quality IS High THEN WaterUse IS High");
            IS.NewRule("Rule 22", "IF Reduction IS High and Utility IS Medium and Quality IS Low THEN WaterUse IS Middle");
            IS.NewRule("Rule 23", "IF Reduction IS High and Utility IS Medium and Quality IS Medium THEN WaterUse IS High");
            IS.NewRule("Rule 24", "IF Reduction IS High and Utility IS Medium and Quality IS High THEN WaterUse IS VeryHigh");
            IS.NewRule("Rule 25", "IF Reduction IS High and Utility IS High and Quality IS Low THEN WaterUse IS High");
            IS.NewRule("Rule 26", "IF Reduction IS High and Utility IS High and Quality IS Medium THEN WaterUse IS VeryHigh");
            IS.NewRule("Rule 27", "IF Reduction IS High and Utility IS High and Quality IS High THEN WaterUse IS VeryHigh");

            IS.SetInput("Reduction", (float)reductionValue);
            IS.SetInput("Utility", (float)utilityValue);
            IS.SetInput("Quality", (float)qualityValue);

            double resultado = IS.Evaluate("WaterUse");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("Reduction", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Utility", i == 0 ? 0 : (float)9.99);
                IS.SetInput("Quality", i == 0 ? 0 : (float)9.99);
                input[i] = IS.Evaluate("WaterUse");
            }
            double m = (IS.GetLinguisticVariable("WaterUse").End - IS.GetLinguisticVariable("WaterUse").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WaterUse").End;

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

        public string CalculateMechanicalTreatment(double postGenerationTreatmentValue, double waterUseValue)
        {
            LinguisticVariable postGenerationTreatment = new( "PostGenerationTreatment", 0, 10 );
            postGenerationTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            postGenerationTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable waterUse = new( "WaterUse", 0, 10 );
            waterUse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            waterUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            waterUse.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            waterUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            waterUse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable mechanicalTreatment = new( "MechanicalTreatment", 0, 10 );
            mechanicalTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( postGenerationTreatment );
            fuzzyDB.AddVariable( waterUse );
            fuzzyDB.AddVariable( mechanicalTreatment );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF PostGenerationTreatment IS VeryLow and WaterUse IS VeryLow THEN MechanicalTreatment IS VeryLow");
            IS.NewRule("Rule 2", "IF PostGenerationTreatment IS VeryLow and WaterUse IS Low THEN MechanicalTreatment IS VeryLow");
            IS.NewRule("Rule 3", "IF PostGenerationTreatment IS VeryLow and WaterUse IS Medium THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 4", "IF PostGenerationTreatment IS VeryLow and WaterUse IS High THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 5", "IF PostGenerationTreatment IS VeryLow and WaterUse IS VeryHigh THEN MechanicalTreatment IS Middle");
            IS.NewRule("Rule 6", "IF PostGenerationTreatment IS Low and WaterUse IS VeryLow THEN MechanicalTreatment IS VeryLow");
            IS.NewRule("Rule 7", "IF PostGenerationTreatment IS Low and WaterUse IS Low THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 8", "IF PostGenerationTreatment IS Low and WaterUse IS Medium THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 9", "IF PostGenerationTreatment IS Low and WaterUse IS High THEN MechanicalTreatment IS Middle");
            IS.NewRule("Rule 10", "IF PostGenerationTreatment IS Low and WaterUse IS VeryHigh THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 11", "IF PostGenerationTreatment IS Middle and WaterUse IS VeryLow THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 12", "IF PostGenerationTreatment IS Middle and WaterUse IS Low THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 13", "IF PostGenerationTreatment IS Middle and WaterUse IS Medium THEN MechanicalTreatment IS Middle");
            IS.NewRule("Rule 14", "IF PostGenerationTreatment IS Middle and WaterUse IS High THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 15", "IF PostGenerationTreatment IS Middle and WaterUse IS VeryHigh THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 16", "IF PostGenerationTreatment IS High and WaterUse IS VeryLow THEN MechanicalTreatment IS Low");
            IS.NewRule("Rule 17", "IF PostGenerationTreatment IS High and WaterUse IS Low THEN MechanicalTreatment IS Middle");
            IS.NewRule("Rule 18", "IF PostGenerationTreatment IS High and WaterUse IS Medium THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 19", "IF PostGenerationTreatment IS High and WaterUse IS High THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 20", "IF PostGenerationTreatment IS High and WaterUse IS VeryHigh THEN MechanicalTreatment IS VeryHigh");
            IS.NewRule("Rule 21", "IF PostGenerationTreatment IS VeryHigh and WaterUse IS VeryLow THEN MechanicalTreatment IS Middle");
            IS.NewRule("Rule 22", "IF PostGenerationTreatment IS VeryHigh and WaterUse IS Low THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 23", "IF PostGenerationTreatment IS VeryHigh and WaterUse IS Medium THEN MechanicalTreatment IS High");
            IS.NewRule("Rule 24", "IF PostGenerationTreatment IS VeryHigh and WaterUse IS High THEN MechanicalTreatment IS VeryHigh");
            IS.NewRule("Rule 25", "IF PostGenerationTreatment IS VeryHigh and WaterUse IS VeryHigh THEN MechanicalTreatment IS VeryHigh");

            IS.SetInput("PostGenerationTreatment", (float)postGenerationTreatmentValue);
            IS.SetInput("WaterUse", (float)waterUseValue);

            double resultado = IS.Evaluate("MechanicalTreatment");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("PostGenerationTreatment", i == 0 ? (float)9.99 : 0);
                IS.SetInput("WaterUse", i == 0 ? (float)9.99 : 0);
                input[i] = IS.Evaluate("MechanicalTreatment");
            }
            double m = (IS.GetLinguisticVariable("MechanicalTreatment").End - IS.GetLinguisticVariable("MechanicalTreatment").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("MechanicalTreatment").End;
            
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