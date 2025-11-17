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

        var sql = @"
            INSERT INTO planos  (nome, codigo_registro_ans, created_at) 
            VALUES (@Nome, @CodigoRegistroAns, NOW())
            RETURNING *";

        var inserted = connection.QuerySingle<Plano>(sql, new { Nome = plano.Nome, CodigoRegistroAns = plano.CodigoRegistroAns });
        plano.Id = inserted.Id;
        return plano;
    }
    public Plano GetById(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
            SELECT id, nome, codigo_registro_ans AS CodigoRegistroAns, created_at, updated_at 
            FROM planos 
            WHERE id = @Id";

        var result = connection.QueryFirstOrDefault<Plano>(sql, new { Id = id, });

        return result;
    }
  
    public IEnumerable<Plano> GetAll()
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
            SELECT id, nome, codigo_registro_ans AS CodigoRegistroAns, created_at, updated_at 
            FROM planos";

        return connection.Query<Plano>(sql);
    }

    public Plano UpdatePlan(Guid id, Plano plano)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = @"
        UPDATE planos 
        SET nome = @Nome, 
            codigo_registro_ans = @CodigoRegistroAns, 
            updated_at = NOW()
        WHERE id = @Id
        RETURNING id, 
                  nome, 
                  codigo_registro_ans AS CodigoRegistroAns, 
                  created_at, 
                  updated_at AS UpdatedAt";

        var result = connection.QueryFirstOrDefault<Plano>(sql, new
        {
            Id = id,
            Nome = plano.Nome,
            CodigoRegistroAns = plano.CodigoRegistroAns
        });
        if (result == null)
        {
            throw new KeyNotFoundException("Plano não encontrado.");
        }

        return result;
    }
    public bool DeletePlan(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        
        var beneficiarioCheck = @"
            SELECT COUNT(*) 
            FROM beneficiarios 
            WHERE plano_id = @Id";

        var beneficiaryCount = connection.QuerySingle<int>(beneficiarioCheck, new { Id = id });

        if (beneficiaryCount > 0)
        {
            throw new InvalidOperationException("Não é possível excluir um plano que possui beneficiários vinculados.");
        }

        var sql = @"
            DELETE FROM planos 
            WHERE id = @Id";

        var rowsAffected = connection.Execute(sql, new { Id = id });
        return rowsAffected > 0;
    }
}
        