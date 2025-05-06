namespace TailorShop.API.Models;

public class User
{
    public int Id { get; set; }
    public string BusinessName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
