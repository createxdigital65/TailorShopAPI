namespace TailorShop.API.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
