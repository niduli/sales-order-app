namespace Domain.Entities
{
    public class SalesOrderLine
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        // Navigation to SalesOrder (required by EF config)
        public SalesOrder? SalesOrder { get; set; }

        public int ItemId { get; set; }

        // Navigation to Item
        public Item? Item { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal TaxRate { get; set; }

        public decimal ExclAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InclAmount { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
