using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Heating
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
        public string? HeatingValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? Performance { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Thermodynamics { get; set; }

        [Column(TypeName = "varchar")]
        public string? Heat { get; set; }

        [Column(TypeName = "varchar")]
        public string? Temperature { get; set; }

        [Column(TypeName = "varchar")]
        public string? Fluid { get; set; }

        [Column(TypeName = "varchar")]
        public string? AirType { get; set; }

        [Column(TypeName = "varchar")]
        public string? Inspection { get; set; }

        [Column(TypeName = "varchar")]
        public string? Isolation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Use { get; set; }

        [Column(TypeName = "varchar")]
        public string? Place { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}