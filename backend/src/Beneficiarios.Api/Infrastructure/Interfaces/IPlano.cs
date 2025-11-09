using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beneficiarios.Api.Models;

namespace Beneficiarios.Api.Infrastructure.Interfaces
{
    public interface IPlano
    {
        void AddPlan(Plano plano);
        Plano GetById(Guid id);
    }
}