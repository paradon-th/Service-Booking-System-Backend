using Microsoft.AspNetCore.Mvc;
using SBS.Domain.Entities;
using SBS.Domain.Interfaces;

namespace SBS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync();
        return Ok(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
    {
        await _unitOfWork.Repository<Booking>().AddAsync(booking);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
    }
}
