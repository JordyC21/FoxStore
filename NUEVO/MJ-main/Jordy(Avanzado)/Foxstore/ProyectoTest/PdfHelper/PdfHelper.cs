using Foxstore.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Foxstore.Helpers
{
    public class PdfHelper
    {
        public static byte[] GenerarFactura(Compra compra)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Agregar el contenido de la factura
                doc.Add(new Paragraph("Factura de Compra"));
                doc.Add(new Paragraph("Id de Compra: " + compra.IdCompra));
                doc.Add(new Paragraph("Fecha de Compra: " + compra.FechaTexto));
                doc.Add(new Paragraph("Total: $" + compra.Total));
                doc.Add(new Paragraph("Detalles de la Compra:"));
                foreach (var detalle in compra.oDetalleCompra)
                {
                    doc.Add(new Paragraph("Producto: " + detalle.oProducto.Nombre));
                    doc.Add(new Paragraph("Cantidad: " + detalle.Cantidad));
                    doc.Add(new Paragraph("Total: $" + detalle.Total));
                    doc.Add(new Paragraph("----------------------"));
                }

                doc.Close();
                writer.Close();

                return ms.ToArray();
            }
        }
    }
}
