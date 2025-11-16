using Beneficiarios.Api.Enums;
using Beneficiarios.Api.Models;

namespace Beneficiarios.Api.Infrastructure.Interfaces
{
    public interface IBeneficiario
    {
        Beneficiario AddBeneficiario(Beneficiario beneficiario);
        Beneficiario GetById(Guid id);
        IEnumerable<Beneficiario> GetAll(Status? status, Guid? planoId);
        Beneficiario UpdateBeneficiario(Guid id, Beneficiario beneficiario);
        bool DeleteBeneficiario(Guid id);
    }
}