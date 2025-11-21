using Beneficiarios.Api.Infrastructure.Db;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;
using Dapper;
using Npgsql;

namespace Beneficiarios.Api.Infrastructure.Repositories;

public class PlanoRepository : IPlano
{
    private readonly string _connectionString;

    public PlanoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public Plano AddPlan(Plano plano)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var nomeExists = connection.ExecuteScalar<bool>(
                "SELECT EXISTS(SELECT 1 FROM planos WHERE nome = @Nome)", 
                new { plano.Nome });

        if (nomeExists)
            throw new InvalidOperationException($"Já existe um plano com o nome '{plano.Nome}'.");

            var codigoExists = connection.ExecuteScalar<bool>(
                "SELECT EXISTS(SELECT 1 FROM planos WHERE codigo_registro_ans = @Codigo)", 
                new { Codigo = plano.CodigoRegistroAns });

        if (codigoExists)
            throw new InvalidOperationException($"Já existe um plano com o código ANS '{plano.CodigoRegistroAns}'.");

            var sql = @"
                INSERT INTO planos (id, nome, codigo_registro_ans, created_at)
                VALUES (@Id, @Nome, @CodigoRegistroAns, NOW())
                RETURNING *";

            if (plano.Id == Guid.Empty) plano.Id = Guid.NewGuid();

            return connection.QuerySingle<Plano>(sql, plano);
        }
    public Plano GetById(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
            SELECT id, nome, codigo_registro_ans AS CodigoRegistroAns, 
                   created_at AS CreatedAt, updated_at AS UpdatedAt 
            FROM planos 
            WHERE id = @Id";

        var result = connection.QueryFirstOrDefault<Plano>(sql, new { Id = id, });

        if (result == null)
        {
            throw new KeyNotFoundException($"Plano com id '{id}' não encontrado.");
        }
        return result;
    }
  
    public IEnumerable<Plano> GetAll()
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
            SELECT id, nome, codigo_registro_ans AS CodigoRegistroAns, created_at, updated_at AS UpdatedAt
            FROM planos";

        return connection.Query<Plano>(sql);
    }

    public Plano UpdatePlan(Guid id, Plano plano)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var exists = connection.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM planos WHERE id = @Id)", new { Id = id });
        if (!exists)
        {
            throw new KeyNotFoundException($"Plano com id '{id}' não encontrado.");
        }
            var nomeExists = connection.ExecuteScalar<bool>(
                "SELECT EXISTS(SELECT 1 FROM planos WHERE nome = @Nome AND id != @Id)", 
                new { plano.Nome, Id = id });

        if (nomeExists)
        {
            throw new InvalidOperationException($"Já existe um plano com o nome '{plano.Nome}'.");            
        }

            
        var codigoExists = connection.ExecuteScalar<bool>(
            "SELECT EXISTS(SELECT 1 FROM planos WHERE codigo_registro_ans = @Codigo AND id != @Id)", 
            new { Codigo = plano.CodigoRegistroAns, Id = id });

        if (codigoExists)
        {
            throw new InvalidOperationException($"Já existe um plano com o código ANS '{plano.CodigoRegistroAns}'.");
        }

        var sql = @"
                UPDATE planos 
                SET nome = @Nome, 
                    codigo_registro_ans = @CodigoRegistroAns,
                    updated_at = NOW()
                WHERE id = @Id
                RETURNING id, 
                          nome, 
                          codigo_registro_ans AS CodigoRegistroAns, 
                          created_at AS CreatedAt, 
                          updated_at AS UpdatedAt";

        plano.Id = id;
        plano.UpdatedAt = DateTime.UtcNow;
        return connection.QuerySingle<Plano>(sql, plano);
    }
    public bool DeletePlan(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var exists = connection.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM planos WHERE id = @Id)", new { Id = id });
            if (!exists)
        {
            throw new KeyNotFoundException($"Plano com id '{id}' não encontrado.");
        }

        var temBeneficiarios = connection.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM beneficiarios WHERE plano_id = @Id)", new { Id = id });
        if (temBeneficiarios)
        {
            throw new InvalidOperationException("Não é possível excluir o plano pois existem beneficiários vinculados a ele.");
        }
        
        var rows = connection.Execute("DELETE FROM planos WHERE id = @Id", new { Id = id });
        return rows > 0;
    }
}
        