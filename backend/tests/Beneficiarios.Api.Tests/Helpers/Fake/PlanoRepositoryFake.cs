using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;

namespace Beneficiarios.Api.Tests.Helpers.Fake
{
    public class PlanoRepositoryFake : IPlano
    {
        private readonly List<Plano> _planos = new List<Plano>();

        public Plano AddPlan(Plano plano)
        {
            if (_planos.Any(p => p.CodigoRegistroAns == plano.CodigoRegistroAns))
            {
                throw new InvalidOperationException($"Já existe um plano com o código ANS '{plano.CodigoRegistroAns}'.");
            }
            if (_planos.Any(p => p.Nome == plano.Nome))
            {
                throw new InvalidOperationException($"Já existe um plano com o nome '{plano.Nome}'.");
            }
            plano.Id = Guid.NewGuid();
            _planos.Add(plano);
            return plano;
        }

        public bool DeletePlan(Guid id)
        {
            var plano = _planos.FirstOrDefault(p => p.Id == id);
            if (plano != null)
            {
                _planos.Remove(plano);
                return true;
            }
            return false;
        }

        public IEnumerable<Plano> GetAll()
        {
            var planos = _planos.ToList();
            return planos;
        }

        public Plano GetById(Guid id)
        {
             var plano = _planos.FirstOrDefault(p => p.Id == id);

            if (plano == null)
            {
                throw new KeyNotFoundException($"Plano com id '{id}' não encontrado.");
            }

            return plano;
        }
        public Plano UpdatePlan(Guid id,Plano plano)
        {
            var existing = _planos.FirstOrDefault(p => p.Id == plano.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Plano com id '{id}' não encontrado.");
            }

            existing.Nome = plano.Nome;
            existing.CodigoRegistroAns = plano.CodigoRegistroAns;       
            return existing;
        }
    }
}