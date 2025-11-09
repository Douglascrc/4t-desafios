using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beneficiarios.Api.Models
{
    [Table("planos")]
    public class Plano
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Codigo_registro_ans { get; set; }
        public Plano()
        {
        }
        public Plano(Guid id, string nome, string codigo_registro_ans)
        {
            this.Id = id;
            this.Nome = nome;
            this.Codigo_registro_ans = codigo_registro_ans;
        }
    }
}