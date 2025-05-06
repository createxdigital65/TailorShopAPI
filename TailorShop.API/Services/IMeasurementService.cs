using TailorShop.API.DTOs;

namespace TailorShop.API.Services;

public interface IMeasurementService
{
    Task<List<MeasurementDto>> GetByCustomerIdAsync(int userId, int customerId);
    Task<MeasurementDto?> GetByIdAsync(int userId, int customerId, int measurementId);
    Task<MeasurementDto> CreateAsync(int userId, int customerId, CreateMeasurementDto dto);
    Task<bool> UpdateAsync(int userId, int customerId, int measurementId, UpdateMeasurementDto dto);
    Task<bool> DeleteAsync(int userId, int customerId, int measurementId);
}
