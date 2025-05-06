namespace TailorShop.API.DTOs;

public class CreateCustomerDto
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Gender { get; set; } = null!;
}
