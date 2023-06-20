using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class LightingSystem
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
        public string? LightingSystemValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? Management { get; set; }

        [Column(TypeName = "varchar")]
        public string? Performance { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Cleaning { get; set; }

        [Column(TypeName = "varchar")]
        public string? ConstructionStructure { get; set; }

        [Column(TypeName = "varchar")]
        public string? Operation { get; set; }

        [Column(TypeName = "varchar")]
        public string? Reactor { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}