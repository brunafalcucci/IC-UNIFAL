using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class MechanicalTreatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column(TypeName = "int8")]
        public int? Id { get; set; }
        
        [Column(TypeName = "varchar")]
        [Required]
        public string? IndustrialSector { get; set; }

        [Column(TypeName = "varchar")]
        [Required]
        public string? IndustryName { get; set; }

        [Column(TypeName = "varchar")]
        public string? MechanicalTreatmentValue { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? PostGenerationTreatment { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? Neutralization { get; set; }

        [Column(TypeName = "varchar")]
        public string? MaterialContamination { get; set; }

        [Column(TypeName = "varchar")]
        public string? RemovalOfContaminants { get; set; }

        [Column(TypeName = "varchar")]
        public string? Utility { get; set; }

        [Column(TypeName = "varchar")]
        public string? Quality { get; set; }

        [Column(TypeName = "varchar")]
        public string? Reduction { get; set; }

        [Column(TypeName = "varchar")]
        public string? PH7 { get; set; }

        [Column(TypeName = "varchar")]
        public string? Redox { get; set; }

        [Column(TypeName = "varchar")]
        public string? OtherMethods { get; set; }

        [Column(TypeName = "varchar")]
        public string? MaterialContamination33131 { get; set; }

        [Column(TypeName = "varchar")]
        public string? MaterialContamination33132 { get; set; }

        [Column(TypeName = "varchar")]
        public string? MaterialContamination33133 { get; set; }

        [Column(TypeName = "varchar")]
        public string? Chemistry { get; set; }

        [Column(TypeName = "varchar")]
        public string? Physicist { get; set; }

        [Column(TypeName = "varchar")]
        public string? CCWE { get; set; }

        [Column(TypeName = "varchar")]
        public string? WT { get; set; }

        [Column(TypeName = "varchar")]
        public string? DecreasedContaminationOfTreatmentWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfDeionizedWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? RegularCleaningOfDirtOnProductionLinesThatUseWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? Conversion { get; set; }

        [Column(TypeName = "varchar")]
        public string? Equipment { get; set; }

        [Column(TypeName = "varchar")]
        public string? Use { get; set; }

        [Column(TypeName = "varchar")]
        public string? DistillerUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfAbsorption { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfAdsorption { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfMagneticSieve { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfFiltering { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfDecanter { get; set; }

        [Column(TypeName = "varchar")]
        public string? CycloneSeparation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Recover { get; set; }

        [Column(TypeName = "varchar")]
        public string? Equipments { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReplacementOfChlorineByO2 { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterDevelopment { get; set; }

        [Column(TypeName = "varchar")]
        public string? Chlorination { get; set; }

        [Column(TypeName = "varchar")]
        public string? QuantificationOfWaterUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfValvesToControlEquipmentFlow { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReplacementOfTreatedWaterWithWellWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterLevelControlInEquipment { get; set; }

        [Column(TypeName = "varchar")]
        public string? EliminationOfLeaksInWaterLinesAndValves { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReplacementOfWaterCoolingInProcesses { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReductionInWaterUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfCountercurrentRinsing { get; set; }

        [Column(TypeName = "varchar")]
        public string? MinimalUseOfCoolingWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecoveryOfMetalsFromWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterTreatmentAndReuse { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecoveryAndReuseOfCoolingWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseOfClosedProcessInTheProductionOfWasteWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? ReplacementOfCoolingWaterInIndustry { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecycledWaterMeasurementToReduceSewageFees { get; set; }

        [Column(TypeName = "varchar")]
        public string? WaterTreatmentByMagneticTechnology { get; set; }

        [Column(TypeName = "varchar")]
        public string? ImprovedProductionOfDeionizedWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? RecyclingOfChlorinatedWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? UseTheChlorinationWashWater { get; set; }

        [Column(TypeName = "varchar")]
        public string? UsingChlorineInTheGasPhase { get; set; }

        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}