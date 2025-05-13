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
    private readonly ILogger<MeasurementController> _logger;

    public MeasurementController(IMeasurementService measurementService, ILogger<MeasurementController> logger)
    {
        _measurementService = measurementService;
        _logger = logger;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("User ID claim missing in token.");
            throw new UnauthorizedAccessException("Invalid token: user ID not found.");
        }

        return int.Parse(userIdClaim);
    }

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
    public async Task<IActionResult> Create(int customerId, [FromBody] CreateMeasurementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetUserId();
        try
        {
            var created = await _measurementService.CreateAsync(userId, customerId, dto);
            return CreatedAtAction(nameof(GetById), new { customerId, id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create measurement.");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int customerId, int id, [FromBody] UpdateMeasurementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

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
