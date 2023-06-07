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
    public class WasteEnergyUseController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IWasteEnergyUseRepository _WasteEnergyUseRepository;
        public WasteEnergyUseController(IConfiguration configuration, IWasteEnergyUseRepository WasteEnergyUseRepository)
        {
            Configuration = configuration;
            _WasteEnergyUseRepository = WasteEnergyUseRepository;
        }

        [HttpPost]
        public ActionResult<WasteEnergyUse> InsertWasteEnergyUse([FromBody] WasteEnergyUse wasteEnergyUse)
        {
            try
            {
                wasteEnergyUse.WasteEnergyUseValue = CalculateWasteEnergyUse(Convert.ToDouble(wasteEnergyUse.ChemicalTreatment), Convert.ToDouble(wasteEnergyUse.MechanicalTreatment));

                return _WasteEnergyUseRepository.InsertWasteEnergyUse(wasteEnergyUse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<WasteEnergyUse> GetWasteEnergyUseById(int id)
        {
            try
            {
                return _WasteEnergyUseRepository.GetWasteEnergyUseById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{industryName}")]
        public ActionResult<List<WasteEnergyUse>> GetWasteEnergyUseByIndustry(string industryName)
        {
            try
            {
                return _WasteEnergyUseRepository.GetWasteEnergyUseByIndustry(industryName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public string CalculateWasteEnergyUse(double chemicalTreatmentValue, double mechanicalTreatmentValue)
        {
            LinguisticVariable chemicalTreatment = new( "ChemicalTreatment", 0, 10 );
            chemicalTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            chemicalTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable mechanicalTreatment = new( "MechanicalTreatment", 0, 10 );
            mechanicalTreatment.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "Medium", new TrapezoidalFunction(3, 5, 7) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            mechanicalTreatment.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            LinguisticVariable wasteEnergyUse = new( "WasteEnergyUse", 0, 10 );
            wasteEnergyUse.AddLabel( new FuzzySet( "VeryLow", new TrapezoidalFunction(0, 0, 1, 3) ) );
            wasteEnergyUse.AddLabel( new FuzzySet( "Low", new TrapezoidalFunction(1, 3, 5) ) );
            wasteEnergyUse.AddLabel( new FuzzySet( "Middle", new TrapezoidalFunction(3, 5, 7) ) );
            wasteEnergyUse.AddLabel( new FuzzySet( "High", new TrapezoidalFunction(5, 7, 9) ) );
            wasteEnergyUse.AddLabel( new FuzzySet( "VeryHigh", new TrapezoidalFunction(7, 9, 10, 10) ) );

            Database fuzzyDB = new();
            fuzzyDB.AddVariable( chemicalTreatment );
            fuzzyDB.AddVariable( mechanicalTreatment );
            fuzzyDB.AddVariable( wasteEnergyUse );

            InferenceSystem IS = new ( fuzzyDB, new CentroidDefuzzifier( 1000 ) );

            IS.NewRule("Rule 1", "IF ChemicalTreatment IS VeryLow and MechanicalTreatment IS VeryLow THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 2", "IF ChemicalTreatment IS VeryLow and MechanicalTreatment IS Low THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 3", "IF ChemicalTreatment IS VeryLow and MechanicalTreatment IS Medium THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 4", "IF ChemicalTreatment IS VeryLow and MechanicalTreatment IS High THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 5", "IF ChemicalTreatment IS VeryLow and MechanicalTreatment IS VeryHigh THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 6", "IF ChemicalTreatment IS Low and MechanicalTreatment IS VeryLow THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 7", "IF ChemicalTreatment IS Low and MechanicalTreatment IS Low THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 8", "IF ChemicalTreatment IS Low and MechanicalTreatment IS Medium THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 9", "IF ChemicalTreatment IS Low and MechanicalTreatment IS High THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 10", "IF ChemicalTreatment IS Low and MechanicalTreatment IS VeryHigh THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 11", "IF ChemicalTreatment IS Medium and MechanicalTreatment IS VeryLow THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 12", "IF ChemicalTreatment IS Medium and MechanicalTreatment IS Low THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 13", "IF ChemicalTreatment IS Medium and MechanicalTreatment IS Medium THEN WasteEnergyUse IS Middle");
            IS.NewRule("Rule 14", "IF ChemicalTreatment IS Medium and MechanicalTreatment IS High THEN WasteEnergyUse IS Middle");
            IS.NewRule("Rule 15", "IF ChemicalTreatment IS Medium and MechanicalTreatment IS VeryHigh THEN WasteEnergyUse IS Middle");
            IS.NewRule("Rule 16", "IF ChemicalTreatment IS High and MechanicalTreatment IS VeryLow THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 17", "IF ChemicalTreatment IS High and MechanicalTreatment IS Low THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 18", "IF ChemicalTreatment IS High and MechanicalTreatment IS Medium THEN WasteEnergyUse IS Middle");
            IS.NewRule("Rule 19", "IF ChemicalTreatment IS High and MechanicalTreatment IS High THEN WasteEnergyUse IS High");
            IS.NewRule("Rule 20", "IF ChemicalTreatment IS High and MechanicalTreatment IS VeryHigh THEN WasteEnergyUse IS High");
            IS.NewRule("Rule 21", "IF ChemicalTreatment IS VeryHigh and MechanicalTreatment IS VeryLow THEN WasteEnergyUse IS VeryLow");
            IS.NewRule("Rule 22", "IF ChemicalTreatment IS VeryHigh and MechanicalTreatment IS Low THEN WasteEnergyUse IS Low");
            IS.NewRule("Rule 23", "IF ChemicalTreatment IS VeryHigh and MechanicalTreatment IS Medium THEN WasteEnergyUse IS Middle");
            IS.NewRule("Rule 24", "IF ChemicalTreatment IS VeryHigh and MechanicalTreatment IS High THEN WasteEnergyUse IS High");
            IS.NewRule("Rule 25", "IF ChemicalTreatment IS VeryHigh and MechanicalTreatment IS VeryHigh THEN WasteEnergyUse IS VeryHigh");

            IS.SetInput("ChemicalTreatment", (float)chemicalTreatmentValue);
            IS.SetInput("MechanicalTreatment", (float)mechanicalTreatmentValue);

            double resultado = IS.Evaluate("WasteEnergyUse");

            double[] input = new double[2];
            for (int i = 0; i < 2; i++)
            {
                IS.SetInput("ChemicalTreatment", i == 0 ? 0 : (float)9.99);
                IS.SetInput("MechanicalTreatment", i == 0 ? 0: (float)9.99);
                input[i] = IS.Evaluate("WasteEnergyUse");
            }
            double m = (IS.GetLinguisticVariable("WasteEnergyUse").End - IS.GetLinguisticVariable("WasteEnergyUse").Start) / (input[1] - input[0]);
            resultado = m * (resultado - input[1]) + IS.GetLinguisticVariable("WasteEnergyUse").End;
            
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