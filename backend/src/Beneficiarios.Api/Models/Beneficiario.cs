using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Beneficiarios.Api.Enums;

namespace Beneficiarios.Api.Models
{
    [Table("beneficiarios")]
    public class Beneficiario
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("nome_completo")]
        [MaxLength(255)]
        public string NomeCompleto { get; set; } = string.Empty;
        
        [Required]
        [Column("cpf")]
        [MaxLength(11)]
        public required string Cpf { get; set; }
        
        [Required]
        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }
        
        [Required]
        [Column("plano_id")]
        public Guid PlanoId { get; set; }
        
        [Column("status")]
        [MaxLength(20)]
        public Status Status { get; set; } = Status.ATIVO;
        
        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public Beneficiario()
        {
        }

        public Beneficiario(string nomeCompleto, string cpf, DateTime dataNascimento, Guid planoId)
        {
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            PlanoId = planoId;
        }
    }
}