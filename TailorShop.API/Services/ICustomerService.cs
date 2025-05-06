using TailorShop.API.DTOs;

namespace TailorShop.API.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync(int userId);
        Task<CustomerDto?> GetByIdAsync(int userId, int customerId);
        Task<CustomerDto> CreateAsync(int userId, CreateCustomerDto dto);
        Task<bool> UpdateAsync(int userId, int customerId, UpdateCustomerDto dto);
        Task<bool> DeleteAsync(int userId, int customerId);
    }
}
