using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class WasteEnergyUse
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
        public string? WasteEnergyUseValue { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? ChemicalTreatment { get; set; }

        [Column(TypeName = "varchar")]
        public string? MechanicalTreatment { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}