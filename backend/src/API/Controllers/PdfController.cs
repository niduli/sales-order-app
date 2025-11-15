using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/pdf")]
    public class PdfController : ControllerBase
    {
        private readonly PdfGenerator _pdf;

        public PdfController(PdfGenerator pdf)
        {
            _pdf = pdf;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetInvoice(int orderId)
        {
            var pdfBytes = await _pdf.GenerateInvoicePdf(orderId);

            return File(pdfBytes, "application/pdf", $"Invoice_{orderId}.pdf");
        }
    }
}
