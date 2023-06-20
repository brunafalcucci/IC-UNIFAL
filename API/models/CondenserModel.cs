using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Condenser
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
        public string? CondenserValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Thermodynamics { get; set; }

        [Column(TypeName = "varchar")]
        public string? Ventilation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Isolation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Local { get; set; }

        [Column(TypeName = "varchar")]
        public string? Heat { get; set; }

        [Column(TypeName = "varchar")]
        public string? Condensed { get; set; }

        [Column(TypeName = "varchar")]
        public string? Pressure { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}