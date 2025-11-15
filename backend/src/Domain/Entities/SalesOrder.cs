namespace Domain.Entities;

public class SalesOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }  

    public string CustomerName => Customer?.Name ?? "";

    public string? InvoiceNo { get; set; }
    public DateTime InvoiceDate { get; set; }
    

    public DateTime OrderDate { get; set; }


    public string? ReferenceNo { get; set; }
    public string? Note { get; set; }

    public decimal TotalExcl { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalIncl { get; set; }

    public List<SalesOrderLine> Lines { get; set; } = new();
}

