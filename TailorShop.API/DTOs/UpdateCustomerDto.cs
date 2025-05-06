namespace TailorShop.API.DTOs;

public class UpdateCustomerDto
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Gender { get; set; } = null!;
}
