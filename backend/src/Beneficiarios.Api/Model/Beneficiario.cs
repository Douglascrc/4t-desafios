using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace Beneficiarios.Api.Model
{
    [Table("beneficiarios")]
    public class Beneficiario
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Status { get; set; }
        public int PlanoId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Beneficiario(int id, string nomeCompleto, string cpf, DateTime dataNascimento, bool status, int planoId, DateTime dataCadastro, DateTime createdAt, DateTime updatedAt)
        {
            this.Id = id;
            this.NomeCompleto = nomeCompleto;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            this.Status = status;
            this.PlanoId = planoId;
            this.DataCadastro = dataCadastro;
            this.CreatedAt = createdAt;
            this.UpdatedAt = updatedAt;
        }
    }

}