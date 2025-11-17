using Beneficiarios.Api.Enums;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;
using Dapper;
using Npgsql;

namespace Beneficiarios.Api.Infrastructure.Repositories
{
    public class BeneficiarioRepository : IBeneficiario
    {
        private readonly string _connectionString;

        public BeneficiarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Beneficiario AddBeneficiario(Beneficiario beneficiario)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
            INSERT INTO beneficiarios (nome_completo, cpf, data_nascimento, plano_id, status, data_cadastro) 
            VALUES (@NomeCompleto, @Cpf, @DataNascimento, @PlanoId, @Status, NOW())
            RETURNING *";

            var result = connection.QuerySingle<Beneficiario>(sql, new
            {
                NomeCompleto = beneficiario.NomeCompleto,
                Cpf = beneficiario.Cpf,
                DataNascimento = beneficiario.DataNascimento,
                PlanoId = beneficiario.PlanoId,
                Status = beneficiario.Status
            });

            beneficiario.Id = result.Id;
            return beneficiario;
        }

        public Beneficiario GetById(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
            SELECT b.id, b.nome_completo, b.cpf, b.data_nascimento, 
                   b.plano_id , b.status, 
                   b.data_cadastro, b.updated_at,
                   p.nome , p.codigo_registro_ans
            FROM beneficiarios b
            LEFT JOIN planos p ON b.plano_id = p.id
            WHERE b.id = @Id";

            var result = connection.QueryFirstOrDefault<dynamic>(sql, new { Id = id });

            if (result == null) return null;

            return new Beneficiario
            {
                Id = result.id,
                NomeCompleto = result.nome_completo,
                Cpf = result.cpf,
                DataNascimento = result.data_nascimento,
                PlanoId = result.plano_id,
                Status = (Status)result.status,
                DataCadastro = result.data_cadastro,
                UpdatedAt = result.updated_at,
            };
        }

        public IEnumerable<Beneficiario> GetAll(Status? status = null, Guid? planoId = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (status.HasValue)
            {
                conditions.Add("b.status = @Status");
                parameters.Add("Status", status.Value);
            }

            if (planoId.HasValue)
            {
                conditions.Add("b.plano_id = @PlanoId");
                parameters.Add("PlanoId", planoId.Value);
            }

            var whereClause = conditions.Any() ? "WHERE " + string.Join(" AND ", conditions) : "";

            var sql = $@"
            SELECT b.id, b.nome_completo, b.cpf, b.data_nascimento, 
                   b.plano_id, b.status, 
                   b.data_cadastro, b.updated_at,
                   p.nome, p.codigo_registro_ans
            FROM beneficiarios b
            LEFT JOIN planos p ON b.plano_id = p.id
            {whereClause}
            ORDER BY b.data_cadastro DESC";

            var results = connection.Query<dynamic>(sql, parameters);

            return results.Select(result => new Beneficiario
            {
                Id = result.id,
                NomeCompleto = result.nome_completo,
                Cpf = result.cpf,
                DataNascimento = result.data_nascimento,
                PlanoId = result.plano_id,
                Status = (Status)result.status,
                DataCadastro = result.data_cadastro,
                UpdatedAt = result.updated_at,
            });
        }

        public Beneficiario UpdateBeneficiario(Guid id, Beneficiario beneficiario)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
            UPDATE beneficiarios 
            SET nome_completo = @NomeCompleto, 
                cpf = @Cpf, 
                data_nascimento = @DataNascimento, 
                plano_id = @PlanoId, 
                status = @Status, 
                updated_at = NOW()
            WHERE id = @Id
            RETURNING id, nome_completo AS NomeCompleto, cpf, data_nascimento AS DataNascimento, 
                      plano_id AS PlanoId, status, 
                      data_cadastro AS DataCadastro, updated_at AS UpdatedAt";

            var result = connection.QueryFirstOrDefault<Beneficiario>(sql, new
            {
                Id = id,
                NomeCompleto = beneficiario.NomeCompleto,
                Cpf = beneficiario.Cpf,
                DataNascimento = beneficiario.DataNascimento,
                PlanoId = beneficiario.PlanoId,
                Status = beneficiario.Status
            });

            if (result == null)
            {
                throw new KeyNotFoundException("Beneficiário não encontrado.");
            }

            return result;
        }

        public bool DeleteBeneficiario(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
            DELETE FROM beneficiarios 
            WHERE id = @Id";

            var rowsAffected = connection.Execute(sql, new { Id = id });
            return rowsAffected > 0;
        }

    }
}