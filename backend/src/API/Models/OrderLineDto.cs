namespace API.Models
{
    public class OrderLineDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public string? Note { get; set; }
    }
}
