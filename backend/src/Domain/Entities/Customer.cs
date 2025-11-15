namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public ICollection<SalesOrder>? Orders { get; set; }
}
