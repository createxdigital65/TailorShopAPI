namespace TailorShop.API.Models;

public class Measurement
{
    public int Id { get; set; }
    public int CustomerId { get; set; }

    public double Shoulder { get; set; }
    public double Chest { get; set; }
    public double Waist { get; set; }
    public double Hips { get; set; }

    public Customer Customer { get; set; } = null!;
}
