using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Beneficiarios.Api.Models
{
    [Table("planos")]
    public class Plano
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Nome é Obrigatório")]
        [Column("nome")]
        [MaxLength(255)]
        public string Nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "O código ANS é obrigatório")]
        [Column("codigo_registro_ans")]
        [JsonPropertyName("codigo_registro_ans")]
        [MaxLength(50)]
        public string CodigoRegistroAns { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public Plano()
        {
        }
        public Plano(Guid id, string nome, string codigoRegistroAns)
        {
            Id = id;
            Nome = nome;
            CodigoRegistroAns = codigoRegistroAns;
        }
    }
}