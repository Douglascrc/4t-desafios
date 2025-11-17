using Npgsql;
 
namespace Beneficiarios.Api.Infrastructure.Db
{
    public class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();    
        try
        {
            dbContext.Database.EnsureCreated();
        }
        catch (PostgresException pgEx)
        {
            throw new Exception($"Erro PostgreSQL ({pgEx.SqlState}): {pgEx.MessageText}", pgEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao inicializar o banco de dados: {ex.Message}", ex);
        }
        }
    }
}