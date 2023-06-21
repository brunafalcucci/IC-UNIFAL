using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class AirConditioning
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
        public string? AirConditioningValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Thermodynamics { get; set; }

        [Column(TypeName = "varchar")]
        public string? Maintenance { get; set; }

        [Column(TypeName = "varchar")]
        public string? Cleaning { get; set; }

        [Column(TypeName = "varchar")]
        public string? Temperature { get; set; }

        [Column(TypeName = "varchar")]
        public string? Acclimatized { get; set; }

        [Column(TypeName = "varchar")]
        public string? Isolation { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}