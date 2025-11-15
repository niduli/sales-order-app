namespace API.Models
{
    public class OrderDto
    {
        public int CustomerId { get; set; }
        public List<OrderLineDto> Lines { get; set; } = new();
    }
}
