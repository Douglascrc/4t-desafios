using Beneficiarios.Api.Models;
using Beneficiarios.Api.Enums;
using Beneficiarios.Api.Tests.Helpers.Fake;

namespace Beneficiarios.Api.Tests
{
    public class BeneficiarioRepositoryTests
    {
        [Fact]
        public void Add_Beneficiario_Should_Return_Beneficiario()
        {
            var repository = new BeneficiarioRepositoryFake();
            var planoFake = new PlanoRepositoryFake();
            var planoCriado = planoFake.AddPlan(new Plano { Nome = "Plano Teste", CodigoRegistroAns = "ANS-0001" });

            var beneficiario = new Beneficiario
            {
                NomeCompleto = "João Silva",
                Cpf = "11144477735",
                PlanoId = planoCriado.Id
            };

            var result = repository.AddBeneficiario(beneficiario);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("João Silva", result.NomeCompleto);
            Assert.Equal("11144477735", result.Cpf);
            Assert.Equal(Status.ATIVO, result.Status);
        }
   
        [Fact]
        public void Delete_Beneficiario_ShouldUpdate_Status()
        {
            var repository = new BeneficiarioRepositoryFake();
            var planoFake = new PlanoRepositoryFake();
            var planoCriado = planoFake.AddPlan(new Plano { Nome = "Plano Teste", CodigoRegistroAns = "ANS-0001" });

            var beneficiario = new Beneficiario
            {
                NomeCompleto = "João Silva",
                Cpf = "11144477735",
                Status = Status.ATIVO,
                PlanoId = planoCriado.Id
            };

            var userAdd = repository.AddBeneficiario(beneficiario);
            var result = repository.DeleteBeneficiario(beneficiario.Id);

            Assert.True(result);

            var beneficiarioDeleted = repository.GetById(userAdd.Id);
            Assert.NotNull(beneficiarioDeleted);
            Assert.Equal(Status.INATIVO, beneficiarioDeleted.Status);
        }
        
        [Fact]
        public void GetAll_With_Filter_Status_Should_Return_Only_Active()
        {
            var repository = new BeneficiarioRepositoryFake();
            var planoFake = new PlanoRepositoryFake();
            var planoCriado = planoFake.AddPlan(new Plano { Nome = "Plano Teste", CodigoRegistroAns = "ANS-0001" });

            repository.AddBeneficiario(new Beneficiario
            {
                NomeCompleto = "João Silva",
                Cpf = "11144477735",
                DataNascimento = DateTime.Now.AddYears(-10),
                PlanoId = planoCriado.Id
            });

            repository.AddBeneficiario(new Beneficiario
            {
                NomeCompleto = "Caio",
                Cpf = "11111111111",
                DataNascimento = DateTime.Now.AddYears(-30),
                PlanoId = planoCriado.Id,
                Status = Status.INATIVO
            });

            var result = repository.GetAll(Status.ATIVO);

            Assert.Single(result);
            Assert.All(result, b => Assert.Equal(Status.ATIVO, b.Status));     
        }
  
        [Fact]
        public void Add_Beneficiaro_With_Cpf_AlreadyExists_Should_ThrowException()
        {
            var repository = new BeneficiarioRepositoryFake();
            var planoFake = new PlanoRepositoryFake();
            var planoCriado = planoFake.AddPlan(new Plano { Nome = "Plano Teste", CodigoRegistroAns = "ANS-0001" });
            
            var user1 = repository.AddBeneficiario(new Beneficiario
            {
                NomeCompleto = "João Silva",
                Cpf = "11144477735",
                DataNascimento = DateTime.Now,
                PlanoId = planoCriado.Id
            });

            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                repository.AddBeneficiario(new Beneficiario
                {
                    NomeCompleto = "Douglas Campos",
                    Cpf = "11144477735",
                    DataNascimento = DateTime.Now,
                    Status = Status.ATIVO
                });
            });

            Assert.Equal("Beneficiário com esse CPF já existe", exception.Message);
        }

        [Fact]
        public void Add_Beneficiario_ShouldThrowException_WhenPlano_NotExist()
    {

        var planoRepoFake = new PlanoRepositoryFake(); 
        var repo = new BeneficiarioRepositoryFake(planoRepoFake);
    
        var beneficiario = new Beneficiario 
        { 
            NomeCompleto = "Teste", 
            Cpf = "12345678901", 
            PlanoId = Guid.NewGuid() 
        };
    
        var ex = Assert.Throws<KeyNotFoundException>(() => repo.AddBeneficiario(beneficiario));
        Assert.Contains($"Plano com id '{beneficiario.PlanoId}' não encontrado.", ex.Message);
    }
   }
}
