using Microsoft.AspNetCore.Mvc;
using SBS.Domain.Entities;
using SBS.Domain.Interfaces;

namespace SBS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ServicesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        var services = await _unitOfWork.Repository<Service>().GetAllAsync();
        return Ok(services);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(int id)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id);
        if (service == null) return NotFound();
        return Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<Service>> CreateService(Service service)
    {
        await _unitOfWork.Repository<Service>().AddAsync(service);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetService), new { id = service.Id }, service);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateService(int id, Service service)
    {
        if (id != service.Id) return BadRequest();
        
        _unitOfWork.Repository<Service>().Update(service);
        await _unitOfWork.CompleteAsync();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id);
        if (service == null) return NotFound();

        _unitOfWork.Repository<Service>().Remove(service);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}
