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

            var planoExists = connection.ExecuteScalar<bool>(
                "SELECT EXISTS(SELECT 1 FROM planos WHERE id = @PlanoId)", 
                new { beneficiario.PlanoId }
            );

            if (!planoExists)
            {
                throw new KeyNotFoundException($"Plano com id '{beneficiario.PlanoId}' não encontrado.");
            }

            var existingCpf = connection.ExecuteScalar<bool>(
                "SELECT EXISTS(SELECT 1 FROM beneficiarios WHERE cpf = @Cpf)", 
                new { beneficiario.Cpf }
            );

            if (existingCpf)
            {
            throw new InvalidOperationException("Beneficiário com esse CPF já existe");
            }

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
            SELECT id, 
                   nome_completo AS NomeCompleto, 
                   cpf, 
                   data_nascimento AS DataNascimento, 
                   plano_id AS PlanoId, 
                   status, 
                   data_cadastro AS DataCadastro,
                   updated_at AS UpdatedAt,
                   deleted_at AS DeletedAt
            FROM beneficiarios
            WHERE id = @Id AND deleted_at IS NULL";

            var result = connection.QueryFirstOrDefault<Beneficiario>(sql, new { Id = id });

            if (result == null) 
            {
                throw new KeyNotFoundException($"Beneficiario with id '{id}' was not found.");
            }
            return result;
        }

        public IEnumerable<Beneficiario> GetAll(Status? status = null, Guid? planoId = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var parameters = new DynamicParameters();

            var sql = @"SELECT id,
                        nome_completo AS NomeCompleto,
                        cpf,
                        data_nascimento AS DataNascimento,
                        plano_id AS PlanoId,
                        status,
                        data_cadastro AS DataCadastro
                        FROM beneficiarios
                        WHERE deleted_at IS NULL";
            
            if (status.HasValue)
            {
                sql += " AND status = @Status";
                parameters.Add("Status", status.Value);
            }

            if (planoId.HasValue)
            {
                sql += " AND plano_id = @PlanoId";
                parameters.Add("PlanoId", planoId.Value);
            }

            sql += " ORDER BY data_cadastro DESC";

            return connection.Query<Beneficiario>(sql, parameters);
        
        }

        public Beneficiario UpdateBeneficiario(Guid id, Beneficiario beneficiario)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            if (!string.IsNullOrEmpty(beneficiario.Cpf))
            {
                var cpfExists = connection.ExecuteScalar<bool>(
                    "SELECT EXISTS(SELECT 1 FROM beneficiarios WHERE cpf = @Cpf AND id != @Id)", 
                    new { Cpf = beneficiario.Cpf, Id = id }
                );

                if (cpfExists)
                {
                    throw new InvalidOperationException("Beneficiário com esse CPF já existe.");
                }
            }

            var sql = @"
            UPDATE beneficiarios 
            SET nome_completo = @NomeCompleto, 
                cpf = @Cpf, 
                data_nascimento = @DataNascimento, 
                plano_id = @PlanoId, 
                status = @Status, 
                updated_at = NOW()
            WHERE id = @Id AND deleted_at IS NULL
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
                throw new KeyNotFoundException("Beneficiário não encontrado ou deletado.");
            }

            return result;
        }

        public bool DeleteBeneficiario(Guid id)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var sql = @"
            UPDATE beneficiarios 
            SET status = @Status,
            updated_at = NOW(),
            deleted_at = NOW()
            WHERE id = @Id AND deleted_at IS NULL";

            var rowsAffected = connection.Execute(sql, new { 
                Id = id,
                Status = Status.INATIVO
                });
            return rowsAffected > 0;
        }

    }
}