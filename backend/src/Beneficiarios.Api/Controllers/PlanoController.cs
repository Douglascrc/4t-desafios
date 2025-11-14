using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.Api.Controllers;

[ApiController]
[Route("/api/planos")]
public class PlanoController(IPlano planoRepository) : ControllerBase
{
    private readonly IPlano _planoRepository = planoRepository;

    [HttpPost]
    public IActionResult PostPlan([FromBody] Plano plano)
    {
        var novoPlano = _planoRepository.AddPlan(plano);
        return CreatedAtAction(nameof(GetById), new { id = novoPlano.Id }, novoPlano);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var plano = _planoRepository.GetById(id);
        if (plano == null)
        {
            return NotFound();
        }
        return Ok(plano);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var planos = _planoRepository.GetAll();
        return Ok(planos);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdatePlan(Guid id, [FromBody] Plano plano)
    {
        var updatedPlano = _planoRepository.UpdatePlan(id, plano);
        if (updatedPlano == null)
        {
            return NotFound();
        }
        return Ok(updatedPlano);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeletePlan(Guid id)
    {
        var deleted = _planoRepository.DeletePlan(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}