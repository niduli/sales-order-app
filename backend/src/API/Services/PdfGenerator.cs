using iTextSharp.text;
using iTextSharp.text.pdf;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class PdfGenerator
    {
        private readonly AppDbContext _db;

        public PdfGenerator(AppDbContext db)
        {
            _db = db;
        }

        public async Task<byte[]> GenerateInvoicePdf(int orderId)
        {
            var order = await _db.SalesOrders
                .Include(o => o.Customer)
                .Include(o => o.Lines)
                .ThenInclude(l => l.Item)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found");

            using (var stream = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                doc.Add(new Paragraph("SALES ORDER / INVOICE", titleFont));
                doc.Add(new Paragraph("\n"));

                // Order info
                var normal = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                doc.Add(new Paragraph($"Order No: SO-{order.Id}", normal));
                doc.Add(new Paragraph($"Date: {DateTime.Now:yyyy-MM-dd}", normal));
                doc.Add(new Paragraph($"Customer: {order.Customer.Name}", normal));
                doc.Add(new Paragraph($"Address: {order.Customer.Address1}", normal));
                doc.Add(new Paragraph($"{order.Customer.Address2}", normal));
                doc.Add(new Paragraph($"{order.Customer.City}", normal));
                doc.Add(new Paragraph("\n"));

                // Table
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                var headers = new string[] { "Item Code", "Description", "Qty", "Price", "Tax%", "Total" };

                foreach (var h in headers)
                {
                    var cell = new PdfPCell(new Phrase(h, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                    cell.BackgroundColor = new BaseColor(230, 230, 230);
                    table.AddCell(cell);
                }

                foreach (var line in order.Lines)
                {
                    table.AddCell(line.Item.ItemCode);
                    table.AddCell(line.Item.Description);
                    table.AddCell(line.Quantity.ToString());
                    table.AddCell(line.Price.ToString("0.00"));
                    table.AddCell(line.TaxRate.ToString("0.##"));
                    table.AddCell(line.InclAmount.ToString("0.00"));
                }

                doc.Add(table);
                doc.Add(new Paragraph("\n"));

                // Totals
                doc.Add(new Paragraph($"Total Excl: {order.TotalExcl:0.00}", normal));
                doc.Add(new Paragraph($"Total Tax: {order.TotalTax:0.00}", normal));
                doc.Add(new Paragraph($"Total Incl: {order.TotalIncl:0.00}", normal));

                doc.Close();

                return stream.ToArray();
            }
        }
    }
}
