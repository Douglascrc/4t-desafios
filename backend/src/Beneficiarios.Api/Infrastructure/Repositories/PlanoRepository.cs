using System;
using System.Linq;
using Beneficiarios.Api.Infrastructure.Db;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Model;

namespace Beneficiarios.Api.Infrastructure.Repositories;

public class PlanoRepository(ApplicationDbContext context) : IPlano
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public void AddPlan(Plano plano)
    {
        _context.Add(plano);
        _context.SaveChanges();
    }
    public Plano GetById(Guid id)
    {
        return _context.Set<Plano>().FirstOrDefault(p => p.Id == id);
    }
    
}