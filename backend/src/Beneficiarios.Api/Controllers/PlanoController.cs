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
        try
        {
            var novoPlano = _planoRepository.AddPlan(plano);
            return CreatedAtAction(nameof(GetById), new { id = novoPlano.Id }, novoPlano);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao criar plano.", details = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var plano = _planoRepository.GetById(id);
            return Ok(plano);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao buscar plano.", details = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var planos = _planoRepository.GetAll();
            return Ok(planos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao listar planos.", details = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdatePlan(Guid id, [FromBody] Plano plano)
    {
        try
        {
            var updatedPlano = _planoRepository.UpdatePlan(id, plano);
            return Ok(updatedPlano);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao atualizar plano.", details = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeletePlan(Guid id)
    {
        try
        {
            _planoRepository.DeletePlan(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao remover plano.", details = ex.Message });
        }
    }
}