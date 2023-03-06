using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class CriticalityIndex
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
        public string? CriticalityIndexValue { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? CostsManagement { get; set; }

        [Column(TypeName = "varchar")]
        public string? IndustrialManagement { get; set; }

        [Column(TypeName = "varchar")]
        public string? EnvironmentalQuality { get; set; }

        [Column(TypeName = "varchar")]
        public string? Investiments { get; set; }

        [Column(TypeName = "varchar")]
        public string? ElectricityExpensives { get; set; }

        [Column(TypeName = "varchar")]
        public string? Predictive { get; set; }

        [Column(TypeName = "varchar")]
        public string? Maintenance { get; set; }

        [Column(TypeName = "varchar")]
        public string? Preventive { get; set; }

        [Column(TypeName = "varchar")]
        public string? Corrective { get; set; }

        [Column(TypeName = "varchar")]
        public string? Governance { get; set; }

        [Column(TypeName = "varchar")]
        public string? EnvironmentalRisks { get; set; }

        [Column(TypeName = "varchar")]
        public string? EnergyUse { get; set; }

        [Column(TypeName = "varchar")]
        public string? EnergyGeneration { get; set; }

        [Column(TypeName = "varchar")]
        public string? Technology { get; set; }

        [Column(TypeName = "varchar")]
        public string? Process { get; set; }

        [Column(TypeName = "varchar")]
        public string? TechnologyDataCollection { get; set; }

        [Column(TypeName = "varchar")]
        public string? Instrumentation { get; set; }

        [Column(TypeName = "varchar")]
        public string? PredictiveOperation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Operation_Work { get; set; }

        [Column(TypeName = "varchar")]
        public string? AvailabilityOfRepairPersonnel { get; set; }

        [Column(TypeName = "varchar")]
        public string? OperationSensitivity { get; set; }

        [Column(TypeName = "varchar")]
        public string? MeanTimeBetweenFailures { get; set; }

        [Column(TypeName = "varchar")]
        public string? WorkLoad { get; set; }

        [Column(TypeName = "varchar")]
        public string? Activities { get; set; }

        [Column(TypeName = "varchar")]
        public string? AverageRepairTime { get; set; }

        [Column(TypeName = "varchar")]
        public string? AvailabilityOfRequiredParts { get; set; }

        [Column(TypeName = "varchar")]
        public string? SkillLevels { get; set; }

        [Column(TypeName = "varchar")]
        public string? ManagementStrategy { get; set; }

        [Column(TypeName = "varchar")]
        public string? Toxicity { get; set; }

        [Column(TypeName = "varchar")]
        public string? Solubility { get; set; }

        [Column(TypeName = "varchar")]
        public string? No_Renewable { get; set; }

        [Column(TypeName = "varchar")]
        public string? Renewable { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}