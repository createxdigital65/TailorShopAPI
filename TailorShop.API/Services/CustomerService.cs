using Microsoft.EntityFrameworkCore;
using TailorShop.API.Data;
using TailorShop.API.DTOs;
using TailorShop.API.Models;

namespace TailorShop.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TailorDbContext _context;

        public CustomerService(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> GetAllAsync(int userId)
        {
            return await _context.Customers
                .Where(c => c.UserId == userId)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Gender = c.Gender,
                    CreatedAt = c.CreatedAt

                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int userId, int customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId && c.UserId == userId);

            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                Gender = customer.Gender,
                CreatedAt = customer.CreatedAt

            };
        }

        public async Task<CustomerDto> CreateAsync(int userId, CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                UserId = userId,
                Gender = dto.Gender,

            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                Gender = customer.Gender,
                CreatedAt = customer.CreatedAt

            };
        }

        public async Task<bool> UpdateAsync(int userId, int customerId, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId && c.UserId == userId);

            if (customer == null) return false;

            customer.Name = dto.Name;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;
            customer.Gender = dto.Gender;


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int customerId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId && c.UserId == userId);

            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
