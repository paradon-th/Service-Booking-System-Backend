using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SBS.Application.DTOs;
using SBS.Application.Interfaces;
using SBS.Domain.Entities;
using SBS.Domain.Interfaces;

namespace SBS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register(UserRegisterDto registerDto)
    {
        var existingUser = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == registerDto.Email)).FirstOrDefault();
        if (existingUser != null) return BadRequest(new ApiResponse<AuthResponseDto> { StatusCode = 400, Message = "User already exists" });

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _authService.HashPassword(registerDto.Password),
            RoleId = 2 // Default to Customer
        };

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.CompleteAsync();

        // Reload user with Role info
        var savedUser = await _unitOfWork.Repository<User>().GetByIdAsync(user.Id);

        var responseData = new AuthResponseDto
        {
            AccessToken = _authService.GenerateToken(user),
            Username = user.Username,
            Role = "Customer"
        };

        return Ok(new ApiResponse<AuthResponseDto>
        {
            StatusCode = 200,
            Message = "Registration successful",
            Data = responseData
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(UserLoginDto loginDto)
    {
        var user = (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == loginDto.Email, "Role")).FirstOrDefault();
        if (user == null || !_authService.VerifyPassword(loginDto.Password, user.PasswordHash))
            return Unauthorized(new ApiResponse<AuthResponseDto> { StatusCode = 401, Message = "Invalid credentials" });

        var responseData = new AuthResponseDto
        {
            AccessToken = _authService.GenerateToken(user),
            Username = user.Username,
            Role = user.Role.Name
        };

        return Ok(new ApiResponse<AuthResponseDto>
        {
            StatusCode = 200,
            Message = "Login successful",
            Data = responseData
        });
    }

    [Authorize]
    [HttpGet("user-permissions")]
    public async Task<ActionResult<ApiResponse<IEnumerable<int>>>> GetUserPermissions()
    {
        try
        {
            Console.WriteLine("[Backend] GetUserPermissions reached.");
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) 
            {
                Console.WriteLine("[Backend] No NameIdentifier claim found.");
                return Unauthorized();
            }
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = int.Parse(userIdStr);
            Console.WriteLine($"[Backend] GetUserPermissions reached for User ID: {userId}");

            var user = await _unitOfWork.Repository<User>().FindAsync(
                u => u.Id == userId,
                "Role.RolePermissions.Permission"
            );

            var firstUser = user.FirstOrDefault();
            if (firstUser == null || firstUser.Role == null) return NotFound();

            var permissions = firstUser.Role.RolePermissions.Select(rp => new
            {
                FunctionId = rp.Permission.FunctionId,
                CanRead = rp.CanRead,
                CanCreate = rp.CanCreate,
                CanUpdate = rp.CanUpdate,
                CanDelete = rp.CanDelete
            });

            Console.WriteLine($"[Backend] Returning {permissions.Count()} permissions for role {firstUser.Role.Name}");

            return Ok(new ApiResponse<IEnumerable<object>>
            {
                StatusCode = 200,
                Message = "Permissions retrieved",
                Data = permissions
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Backend ERROR] GetUserPermissions: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return StatusCode(500, new ApiResponse<string> { StatusCode = 500, Message = ex.Message });
        }
    }
}
