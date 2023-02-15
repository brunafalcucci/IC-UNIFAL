using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Application.Models
{
    public class Dados
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
        public double? Dado1 { get; set; }
        
        [Column(TypeName = "numeric(15, 15)")]
        public double? Dado2 { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Dado3 { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Dado4 { get; set; }

        [Column(TypeName = "numeric(15, 15)")]
        public double? Dado5 { get; set; }
        
        [Column(TypeName = "timestamp(0)")]
        public DateTime Ultima_atualizacao { get; set; }
    }
}