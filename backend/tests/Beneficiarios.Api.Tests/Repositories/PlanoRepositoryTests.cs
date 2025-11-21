using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beneficiarios.Api.Models;
using Beneficiarios.Api.Tests.Helpers.Fake;

namespace Beneficiarios.Api.Tests.Repositories
{
    public class PlanoRepositoryTests
    {
        private readonly PlanoRepositoryFake _repository;

        public PlanoRepositoryTests()
        {
            _repository = new PlanoRepositoryFake();
        }

        [Fact]
        public void AddPlan_ShouldThrowException_WhenNomeIsDuplicated()
        {
            var plano1 = new Plano { Nome = "Plano Duplicado", CodigoRegistroAns = "ANS-001" };
            _repository.AddPlan(plano1);

            var plano2 = Assert.Throws<InvalidOperationException>(() =>
            {
                _repository.AddPlan(new Plano 
                { 
                    Nome = "Plano Duplicado", 
                    CodigoRegistroAns = "ANS-002" 
                });
            });

            Assert.Equal($"Já existe um plano com o nome '{plano1.Nome}'.", plano2.Message);
        }

        [Fact]
        public void AddPlan_ShouldThrowException_WhenCodigoAnsIsDuplicated()
        {
            var plano1 = new Plano { Nome = "Plano A", CodigoRegistroAns = "ANS-DUPLICADO" };
            _repository.AddPlan(plano1);

            var plano2 = Assert.Throws<InvalidOperationException>(() =>
            { _repository.AddPlan(new Plano 
                { 
                    Nome = "Plano B",
                    CodigoRegistroAns = "ANS-DUPLICADO"       
                 });
            });

            Assert.Equal($"Já existe um plano com o código ANS '{plano1.CodigoRegistroAns}'.", plano2.Message);
        }

        [Fact]
        public void UpdatePlan_ShouldUpdateData_WhenIdExists()
        {
            var plano = new Plano { Nome = "Plano Antigo", CodigoRegistroAns = "ANS-OLD" };
            var created = _repository.AddPlan(plano);

            var updateData = new Plano 
            { 
                Id = created.Id,
                Nome = "Plano Novo", 
                CodigoRegistroAns = "ANS-NEW" 
            };
            
            var result = _repository.UpdatePlan(created.Id, updateData);
            
            Assert.Equal("Plano Novo", result.Nome);
            Assert.Equal("ANS-NEW", result.CodigoRegistroAns);
            
            var fromDb = _repository.GetById(created.Id);
            Assert.Equal("Plano Novo", fromDb.Nome);
        }

        [Fact]
        public void UpdatePlan_ShouldThrowException_WhenIdDoesNotExist()
        {
            var plano = new Plano { Id = Guid.NewGuid(), Nome = "Fantasma", CodigoRegistroAns = "ANS-000" };

            var ex = Assert.Throws<KeyNotFoundException>(() => _repository.UpdatePlan(plano.Id, plano));

            Assert.Equal($"Plano com id '{plano.Id}' não encontrado.", ex.Message);
        }
    }
}