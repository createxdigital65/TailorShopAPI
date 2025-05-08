using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TailorShop.API.DTOs;
using TailorShop.API.Services;

namespace TailorShop.API.Controllers
{
    [ApiController]
   [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private int GetUserId() =>
           int.Parse(User.FindFirstValue("userId")!);


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var customers = await _customerService.GetAllAsync(userId);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var customer = await _customerService.GetByIdAsync(userId, id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            var userId = GetUserId();
            var newCustomer = await _customerService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = newCustomer.Id }, newCustomer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
        {
            var userId = GetUserId();
            var updated = await _customerService.UpdateAsync(userId, id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var deleted = await _customerService.DeleteAsync(userId, id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
