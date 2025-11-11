using Beneficiarios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Beneficiarios.Api.Infrastructure.Db;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {     
    }

    public DbSet<Plano> Planos { get; set; } = null!;
    public DbSet<Beneficiario> Beneficiarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Plano>()
            .Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Plano>()
        .HasIndex(p => p.CodigoRegistroAns)
        .IsUnique();

        modelBuilder.Entity<Beneficiario>()
            .Property(b => b.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Beneficiario>()
        .HasIndex(b => b.Cpf)
        .IsUnique();
    }
}

