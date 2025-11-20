using Beneficiarios.Api.Enums;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;

namespace Beneficiarios.Api.Tests.Helpers.Fake
{
    public class BeneficiarioRepositoryFake : IBeneficiario
    {
        private readonly List<Beneficiario> _beneficiarios = new List<Beneficiario>();
        private readonly IPlano? _planoRepository;

        public BeneficiarioRepositoryFake(IPlano? planoRepository = null)
        {
            _planoRepository = planoRepository;
        }
        public Beneficiario AddBeneficiario(Beneficiario beneficiario)
        {
            if (_beneficiarios.Any(b => b.Cpf == beneficiario.Cpf))
            {
                throw new InvalidOperationException("Beneficiário com esse CPF já existe");
            }
            if (_planoRepository != null)
            {
                var plano = _planoRepository.GetById(beneficiario.PlanoId);
                if (plano == null)
                {
                    throw new KeyNotFoundException($"Plano com ID {beneficiario.PlanoId} não encontrado");
                }
            }
            beneficiario.Id = Guid.NewGuid();
            _beneficiarios.Add(beneficiario);
            return beneficiario;
        }

        public bool DeleteBeneficiario(Guid id)
        {
            var beneficiario = _beneficiarios.FirstOrDefault(b => b.Id == id);
            if(beneficiario != null)
            {
                beneficiario.Status = Status.INATIVO;
                beneficiario.UpdatedAt = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public IEnumerable<Beneficiario> GetAll(Status? status = null, Guid? planoId= null)
        {
            var query = _beneficiarios.AsEnumerable();

            if (status.HasValue)
                query = query.Where(b => b.Status == status.Value);

            if (planoId.HasValue)
                query = query.Where(b => b.PlanoId == planoId.Value);

            return query.OrderByDescending(b => b.DataCadastro).ToList();
        }

        public Beneficiario GetById(Guid id)
        {
            var beneficiario = _beneficiarios.FirstOrDefault(b => b.Id == id);
            if (beneficiario == null)
            {
                throw new KeyNotFoundException($"Beneficiario com o '{id}' não foi encontrado.");
            }
            return beneficiario;
        }

        public Beneficiario UpdateBeneficiario(Guid id, Beneficiario beneficiario)
        {
            var existing = _beneficiarios.FirstOrDefault(b => b.Id == id);
            if (existing == null)
            {
            throw new KeyNotFoundException("Beneficiário não encontrado.");
            }

            existing.NomeCompleto = beneficiario.NomeCompleto;
            existing.Cpf = beneficiario.Cpf;
            existing.DataNascimento = beneficiario.DataNascimento;
            existing.PlanoId = beneficiario.PlanoId;
            existing.Status = beneficiario.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            return existing;
        }

    }
}