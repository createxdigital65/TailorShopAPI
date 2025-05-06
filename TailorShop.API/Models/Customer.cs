using System.Diagnostics.Metrics;

namespace TailorShop.API.Models;

public class Customer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
}
