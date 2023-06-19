using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class CoolingSystem
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
        public string? CoolingSystemValue { get; set; }

        [Column(TypeName = "varchar")]
        public string? SystemOperationCooling { get; set; }

        [Column(TypeName = "varchar")]
        public string? HeatTransferCooling { get; set; }
        
        [Column(TypeName = "varchar")]
        public string? Management { get; set; }

        [Column(TypeName = "varchar")]
        public string? Performance { get; set; }

        [Column(TypeName = "varchar")]
        public string? Condenser { get; set; }

        [Column(TypeName = "varchar")]
        public string? Thermodynamics { get; set; }

        [Column(TypeName = "varchar")]
        public string? Cleaning { get; set; }

        [Column(TypeName = "varchar")]
        public string? ChillerWaste { get; set; }

        [Column(TypeName = "varchar")]
        public string? Air { get; set; }

        [Column(TypeName = "varchar")]
        public string? Pressure { get; set; }

        [Column(TypeName = "varchar")]
        public string? Refrigeration { get; set; }

        [Column(TypeName = "varchar")]
        public string? Water { get; set; }

        [Column(TypeName = "varchar")]
        public string? Temperature { get; set; }

        [Column(TypeName = "varchar")]
        public string? Heat { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_Atualizacao { get; set; }
    }
}