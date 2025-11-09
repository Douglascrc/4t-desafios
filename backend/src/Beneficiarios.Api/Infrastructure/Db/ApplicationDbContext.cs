using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beneficiarios.Api.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

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
    }
}

