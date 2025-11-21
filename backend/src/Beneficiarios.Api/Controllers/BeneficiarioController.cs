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
        try
        {
            var created = _beneficiarioRepository.AddBeneficiario(beneficiario);
            return CreatedAtAction(nameof(GetBeneficiarioById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao criar beneficiário.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetBeneficiarioById(Guid id)
    {
        try
        {
            var beneficiario = _beneficiarioRepository.GetById(id);
            return Ok(beneficiario);
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao buscar beneficiário.", details = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAllBeneficiarios([FromQuery] Status? status = null, [FromQuery(Name = "plano_id")] Guid? planoId = null)
    {
        try
        {
            var beneficiarios = _beneficiarioRepository.GetAll(status, planoId);
            return Ok(beneficiarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao buscar beneficiários.", details = ex.Message });
        }
        
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBeneficiario(Guid id, [FromBody] Beneficiario beneficiario)
    {
        try
        {
        var updatedBeneficiario = _beneficiarioRepository.UpdateBeneficiario(id, beneficiario);
        return Ok(updatedBeneficiario);       
        } catch(KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao atualizar beneficiário.", details = ex.Message });
        }

    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBeneficiario(Guid id)
    {
        try
        {
            _beneficiarioRepository.DeleteBeneficiario(id);
            return Ok();
        } catch(KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        } catch(Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao deletar beneficiário.", details = ex.Message });
        }      
    }
}
    