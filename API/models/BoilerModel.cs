using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Boiler
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
        public string? BoilerValue { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Thermodynamics { get; set; }

        [Column(TypeName = "varchar")]
        public string? Performance { get; set; }

        [Column(TypeName = "varchar")]
        public string? Pressure { get; set; }

        [Column(TypeName = "varchar")]
        public string? Condensed { get; set; }

        [Column(TypeName = "varchar")]
        public string? Heat { get; set; }

        [Column(TypeName = "varchar")]
        public string? Place { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }

        [Column(TypeName = "varchar")]
        public string? StudiesAndMeasures { get; set; }

        [Column(TypeName = "varchar")]
        public string? Inspection { get; set; }

        [Column(TypeName = "varchar")]
        public string? Maintenance { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}