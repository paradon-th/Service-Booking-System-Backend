using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs;
using SBS.Domain.Entities;
using SBS.Domain.Interfaces;

namespace SBS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UsersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetUsers()
    {
        // Include Role for display
        var users = await _unitOfWork.Repository<User>().FindAsync(u => true, "Role");

        return Ok(new ApiResponse<IEnumerable<User>>
        {
            StatusCode = 200,
            Message = "Users retrieved",
            Data = users
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
        if (user == null) return NotFound();

        _unitOfWork.Repository<User>().Remove(user);
        await _unitOfWork.CompleteAsync();

        return Ok(new ApiResponse<bool>
        {
            StatusCode = 200,
            Message = "User deleted",
            Data = true
        });
    }
}
