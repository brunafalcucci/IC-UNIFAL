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
        
        [Column(TypeName = "varchar(100)")]
        [Required]
        public string? City { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? CriticalityIndexValue { get; set; }
        
        [Column(TypeName = "numeric(15, 15)")]
        public double? CostsManagement { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? IndustrialManagement { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? EnvironmentalQuality { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Investments { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? ElectricityExpensives { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Predictive { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Maintenance { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Preventive { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Corrective { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Governance { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? EnvironmentalRisks { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? EnergyUse { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? EnergyGeneration { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Technology { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Process { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? TechnologyDataCollection { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Instrumentation { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? PredictiveOperation { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Operation_Work { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? AvailabilityOfRepairPersonnel { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? OperationSensitivity { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? MeanTimeBetweenFailures { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? WorkLoad { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Activities { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? AverageRepairTime { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? AvailabilityOfRequiredParts { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? SkillLevels { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? ManagementStrategy { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Toxicity { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Solubility { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? No_Renewable { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Renewable { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_atualizacao { get; set; }
    }
}