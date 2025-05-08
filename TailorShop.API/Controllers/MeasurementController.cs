using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TailorShop.API.DTOs;
using TailorShop.API.Services;

namespace TailorShop.API.Controllers;

[ApiController]
[Route("api/customers/{customerId}/measurements")]
[Authorize]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue("userId")!);


    [HttpGet]
    public async Task<IActionResult> GetAll(int customerId)
    {
        var userId = GetUserId();
        var measurements = await _measurementService.GetByCustomerIdAsync(userId, customerId);
        return Ok(measurements);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int customerId, int id)
    {
        var userId = GetUserId();
        var measurement = await _measurementService.GetByIdAsync(userId, customerId, id);
        if (measurement == null) return NotFound();
        return Ok(measurement);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int customerId, CreateMeasurementDto dto)
    {
        var userId = GetUserId();
        try
        {
            var created = await _measurementService.CreateAsync(userId, customerId, dto);
            return CreatedAtAction(nameof(GetById), new { customerId, id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int customerId, int id, UpdateMeasurementDto dto)
    {
        var userId = GetUserId();
        var updated = await _measurementService.UpdateAsync(userId, customerId, id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int customerId, int id)
    {
        var userId = GetUserId();
        var deleted = await _measurementService.DeleteAsync(userId, customerId, id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
