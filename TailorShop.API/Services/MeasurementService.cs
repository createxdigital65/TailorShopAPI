using Microsoft.EntityFrameworkCore;
using TailorShop.API.Data;
using TailorShop.API.DTOs;
using TailorShop.API.Models;

namespace TailorShop.API.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly TailorDbContext _context;

        public MeasurementService(TailorDbContext context)
        {
            _context = context;
        }

        public async Task<List<MeasurementDto>> GetByCustomerIdAsync(int userId, int customerId)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId && c.UserId == userId);
            if (!customerExists) return new List<MeasurementDto>();

            return await _context.Measurements
                .Where(m => m.CustomerId == customerId)
                .Select(m => new MeasurementDto
                {
                    Id = m.Id,
                    Shoulder = m.Shoulder,
                    Chest = m.Chest,
                    Waist = m.Waist,
                    Hips = m.Hips
                })
                .ToListAsync();
        }

        public async Task<MeasurementDto?> GetByIdAsync(int userId, int customerId, int measurementId)
        {
            var measurement = await _context.Measurements
                .Include(m => m.Customer)
                .FirstOrDefaultAsync(m => m.Id == measurementId && m.CustomerId == customerId && m.Customer.UserId == userId);

            if (measurement == null) return null;

            return new MeasurementDto
            {
                Id = measurement.Id,
                Shoulder = measurement.Shoulder,
                Chest = measurement.Chest,
                Waist = measurement.Waist,
                Hips = measurement.Hips
            };
        }

        public async Task<MeasurementDto> CreateAsync(int userId, int customerId, CreateMeasurementDto dto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId && c.UserId == userId);
            if (customer == null) throw new Exception("Customer not found");

            var measurement = new Measurement
            {
                CustomerId = customerId,
                Shoulder = dto.Shoulder,
                Chest = dto.Chest,
                Waist = dto.Waist,
                Hips = dto.Hips
            };

            _context.Measurements.Add(measurement);
            await _context.SaveChangesAsync();

            return new MeasurementDto
            {
                Id = measurement.Id,
                Shoulder = measurement.Shoulder,
                Chest = measurement.Chest,
                Waist = measurement.Waist,
                Hips = measurement.Hips
            };
        }

        public async Task<bool> UpdateAsync(int userId, int customerId, int measurementId, UpdateMeasurementDto dto)
        {
            var measurement = await _context.Measurements
                .Include(m => m.Customer)
                .FirstOrDefaultAsync(m => m.Id == measurementId && m.CustomerId == customerId && m.Customer.UserId == userId);

            if (measurement == null) return false;

            measurement.Shoulder = dto.Shoulder;
            measurement.Chest = dto.Chest;
            measurement.Waist = dto.Waist;
            measurement.Hips = dto.Hips;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int customerId, int measurementId)
        {
            var measurement = await _context.Measurements
                .Include(m => m.Customer)
                .FirstOrDefaultAsync(m => m.Id == measurementId && m.CustomerId == customerId && m.Customer.UserId == userId);

            if (measurement == null) return false;

            _context.Measurements.Remove(measurement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
