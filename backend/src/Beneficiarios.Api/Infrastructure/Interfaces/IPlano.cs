using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beneficiarios.Api.Models;

namespace Beneficiarios.Api.Infrastructure.Interfaces
{
    public interface IPlano
    {
        Plano AddPlan(Plano plano);
        Plano GetById(Guid id);
        IEnumerable<Plano> GetAll();
        Plano UpdatePlan(Guid id, Plano plano);
        bool DeletePlan(Guid id);

    }
}