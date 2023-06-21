using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Ventilation
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
        public string? VentilationValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }

        [Column(TypeName = "varchar")]
        public string? Performance { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Use { get; set; }

        [Column(TypeName = "varchar")]
        public string? FanControl { get; set; }

        [Column(TypeName = "varchar")]
        public string? Functionality { get; set; }

        [Column(TypeName = "varchar")]
        public string? AirReduction { get; set; }

        [Column(TypeName = "varchar")]
        public string? AirRecycling { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}