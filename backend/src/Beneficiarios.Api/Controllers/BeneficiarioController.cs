using Beneficiarios.Api.Enums;
using Beneficiarios.Api.Infrastructure.Interfaces;
using Beneficiarios.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.Api.Controllers;

[ApiController]
[Route("api/beneficiarios")]
public class BeneficiarioController : ControllerBase
{
    private readonly IBeneficiario _beneficiarioRepository;

    public BeneficiarioController(IBeneficiario beneficiarioRepository)
    {
        _beneficiarioRepository = beneficiarioRepository;
    }

    [HttpPost]
    public IActionResult AddBeneficiario([FromBody] Beneficiario beneficiario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = _beneficiarioRepository.AddBeneficiario(beneficiario);
        return CreatedAtAction(nameof(GetBeneficiarioById), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public IActionResult GetBeneficiarioById(Guid id)
    {
        try
        {
            var beneficiario = _beneficiarioRepository.GetById(id);
            if (beneficiario == null)
            {
                return NotFound(new { message = "Beneficiário não encontrado." });
            }

            return Ok(beneficiario);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAllBeneficiarios([FromQuery] Status? status = null, [FromQuery] Guid? planoId = null)
    {
        var beneficiarios = _beneficiarioRepository.GetAll(status, planoId);
        return Ok(beneficiarios);
        
    }

[HttpPut("{id}")]
public async Task<ActionResult<Beneficiario>> UpdateBeneficiario(Guid id, [FromBody] Beneficiario beneficiario)
{
    if (beneficiario == null)
    {
        return BadRequest("Dados do beneficiário são obrigatórios.");
    }

    var existingBeneficiario = _beneficiarioRepository.GetById(id);
    if (existingBeneficiario == null)
    {
        return NotFound($"Beneficiário com ID {id} não encontrado.");
    }

    var updatedBeneficiario = _beneficiarioRepository.UpdateBeneficiario(id, beneficiario);
    return Ok(updatedBeneficiario);
}

    [HttpDelete("{id}")]
    public IActionResult DeleteBeneficiario(Guid id)
    {
       
        var success = _beneficiarioRepository.DeleteBeneficiario(id);
        if (!success)
        {
            return NotFound(new { message = "Beneficiário não encontrado." });
        }

        return NoContent();       
    }
}
    