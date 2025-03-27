using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.File;

namespace PureLifeClinic.Application.Services.FileGenerator;

public class InvoiceGenerator : FileGeneratorBase<InvoiceFileCreateViewModel>
{
    protected override async Task CreatePdfAsync(Stream stream, InvoiceFileCreateViewModel invoice, CancellationToken cancellationToken)
    {
        using (PdfWriter writer = new PdfWriter(stream))
        using (PdfDocument pdfDocument = new PdfDocument(writer))
        using (Document document = new Document(pdfDocument))
        {
            writer.SetCloseStream(false);

            if (!string.IsNullOrEmpty(invoice.CLinicLogoUrl))
            {
                try
                {
                    var logo = new Image(
                        iText.IO.Image.ImageDataFactory.Create(invoice.CLinicLogoUrl))
                        .ScaleToFit(100, 100)
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    document.Add(logo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Logo image could not be loaded: " + ex.Message);
                }
            }

            // Header
            var header = new Paragraph("INVOICE")
                .SetFontSize(18)
                .SimulateBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(10);
            document.Add(header);

            var datePara = new Paragraph($"Date: {invoice.InvoiceDate:dd/MM/yyyy}")
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(20);
            document.Add(datePara);

            // Clinic Information
            var clinicHeader = new Paragraph("CLINIC INFORMATION")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginBottom(5);
            document.Add(clinicHeader);

            var clinicInfo = new Paragraph()
                .Add($"{invoice.ClinicInfo.ClinicName}\n")
                .Add($"{invoice.ClinicInfo.Address}\n")
                .Add($"Phone: {invoice.ClinicInfo.PhoneNumber}");
            document.Add(clinicInfo);

            // Doctor Information
            var doctorHeader = new Paragraph("DOCTOR INFORMATION")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);
            document.Add(doctorHeader);

            var doctorInfo = new Paragraph()
                .Add($"Doctor: {invoice.DoctorInfo.DoctorName} ({invoice.DoctorInfo.Specialization})");
            document.Add(doctorInfo);

            // Patient Information
            var patientHeader = new Paragraph("PATIENT INFORMATION")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);
            document.Add(patientHeader);

            var patientInfo = new Paragraph()
                .Add($"Name: {invoice.PatientInfo.PatientName}\n")
                .Add($"Date of Birth: {invoice.PatientInfo.DateOfBirth:dd/MM/yyyy}\n")
                .Add($"Gender: {invoice.PatientInfo.Gender}\n")
                .Add($"Address: {invoice.PatientInfo.Address}\n")
                .Add($"Phone: {invoice.PatientInfo.PhoneNumber}");
            document.Add(patientInfo);

            // Medications Table
            var medHeader = new Paragraph("MEDICATIONS")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);
            document.Add(medHeader);

            var medTable = new Table(new float[] { 4, 2, 2 });
            medTable.SetWidth(UnitValue.CreatePercentValue(100));
            medTable.AddHeaderCell(new Cell().Add(new Paragraph("Medication Name").SimulateBold()));
            medTable.AddHeaderCell(new Cell().Add(new Paragraph("Dosage").SimulateBold()));
            medTable.AddHeaderCell(new Cell().Add(new Paragraph("Price").SimulateBold()).SetTextAlignment(TextAlignment.RIGHT));

            foreach (var med in invoice.Medications)
            {
                medTable.AddCell(new Cell().Add(new Paragraph(med.MedicationName)));
                medTable.AddCell(new Cell().Add(new Paragraph($"{med.Dosage} mg")));
                medTable.AddCell(new Cell().Add(new Paragraph($"{med.Price:C}")).SetTextAlignment(TextAlignment.RIGHT));
            }
            document.Add(medTable);

            // Services Table
            var serviceHeader = new Paragraph("SERVICES")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);
            document.Add(serviceHeader);

            var serviceTable = new Table(new float[] { 4, 1, 2 });
            serviceTable.SetWidth(UnitValue.CreatePercentValue(100));
            serviceTable.AddHeaderCell(new Cell().Add(new Paragraph("Service Name").SimulateBold()));
            serviceTable.AddHeaderCell(new Cell().Add(new Paragraph("Quantity").SimulateBold()).SetTextAlignment(TextAlignment.RIGHT));
            serviceTable.AddHeaderCell(new Cell().Add(new Paragraph("Price").SimulateBold()).SetTextAlignment(TextAlignment.RIGHT));

            foreach (var service in invoice.Services)
            {
                serviceTable.AddCell(new Cell().Add(new Paragraph(service.ServiceName)));
                serviceTable.AddCell(new Cell().Add(new Paragraph(service.Quantity.ToString())).SetTextAlignment(TextAlignment.RIGHT));
                serviceTable.AddCell(new Cell().Add(new Paragraph($"{service.Price:C}")).SetTextAlignment(TextAlignment.RIGHT));
            }
            document.Add(serviceTable);

            // Invoice Breakdown
            var breakdownHeader = new Paragraph("PAYMENT DETAILS")
                .SetFontSize(14)
                .SimulateBold()
                .SetMarginTop(10)
                .SetMarginBottom(5);
            document.Add(breakdownHeader);

            var breakdown = new Paragraph()
                .Add($"Medication Total: {invoice.InvoiceBreakdown.MedicationTotal:C}\n")
                .Add($"Service Total: {invoice.InvoiceBreakdown.ServiceTotal:C}\n")
                .Add($"Discount: -{invoice.InvoiceBreakdown.DiscountAmount:C}\n")
                .Add($"Tax (VAT): {invoice.InvoiceBreakdown.TaxAmount:C}\n")
                .Add(new Paragraph($"GRAND TOTAL: {invoice.InvoiceBreakdown.GrandTotal:C}")
                    .SetFontSize(16)
                    .SimulateBold());
            document.Add(breakdown);

            // Payment status and method
            var status = invoice.IsPaid ? "Paid" : "Unpaid";
            var statusPara = new Paragraph($"Payment Status: {status}")
                .SimulateItalic()
                .SetMarginTop(10);
            document.Add(statusPara);

            var methodPara = new Paragraph($"Payment Method: {invoice.PaymentMethod}")
                .SimulateItalic()
                .SetMarginTop(5);
            document.Add(methodPara);
        }

        await Task.CompletedTask;
    }
}
