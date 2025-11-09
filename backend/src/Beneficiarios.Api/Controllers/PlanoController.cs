using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.Api.Controllers;

[ApiController]
[Route("/api/planos")]
public class PlanoController : ControllerBase
{
    private readonly IPlano _planoRepository;

    public PlanoController(IPlano planoRepository)
    {
        _planoRepository = planoRepository;
    }
    [HttpPost]
    public IActionResult PostPlan([FromBody] Plano plano)
    {
        _planoRepository.AddPlan(plano);
        return CreatedAtAction(nameof(GetById), new { id = plano.Id }, plano);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var plano = _planoRepository.GetById(id);
        if(plano == null)
        {
            return NotFound();
        }
        return Ok(plano);
    }
}