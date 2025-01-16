using Asp.Versioning;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace PureLifeClinic.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IConverter _converter;
        public FileController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpPost("create-pdf")]
        public IActionResult CreatePdf()
        {
            var templateContent = @"
            <!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Medical Record</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 20px;
                        background-color: #f4f4f4;
                    }
                    .container {
                        background-color: #fff;
                        padding: 20px;
                        margin: 0 auto;
                        border-radius: 5px;
                        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        max-width: 900px;
                    }
                    .header, .footer {
                        text-align: center;
                        color: #333;
                    }
                    .header h1, .footer p {
                        margin: 0;
                    }
                    .content {
                        margin: 20px 0;
                    }
                    .content p {
                        line-height: 1.6;
                    }
                    .table-container {
                        margin-top: 20px;
                        width: 100%;
                        border-collapse: collapse;
                    }
                    .table-container th, .table-container td {
                        padding: 10px;
                        border: 1px solid #ddd;
                    }
                    .table-container th {
                        background-color: #f2f2f2;
                        text-align: left;
                    }
                    .table-container td {
                        background-color: #fafafa;
                    }
                    .table-container tr:nth-child(even) td {
                        background-color: #f9f9f9;
                    }
                    .table-container tr:hover td {
                        background-color: #f1f1f1;
                    }
                    .highlight {
                        background-color: #fffae6;
                        font-weight: bold;
                    }
                    .section-header {
                        font-size: 1.5em;
                        margin-top: 20px;
                        color: #333;
                    }
                    .footer {
                        margin-top: 40px;
                        font-size: 0.9em;
                        color: #777;
                    }
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""header"">
                        <h1>Medical Record</h1>
                    </div>
                    <div class=""content"">
                        <div class=""hospital-info"">
                            <p><strong>Hospital:</strong> Springfield Medical Center</p>
                            <p><strong>Address:</strong> 456 Health Street, Springfield, IL, 62701</p>
                            <p><strong>Phone:</strong> (123) 555-9876</p>
                            <p><strong>Doctor:</strong> Dr. Jane Smith, MD</p>
                        </div>

                        <div class=""patient-info"">
                            <p><strong>Patient Name:</strong> John Doe</p>
                            <p><strong>Patient ID:</strong> 12345</p>
                            <p><strong>Date of Birth:</strong> March 14, 1979</p>
                            <p><strong>Gender:</strong> Male</p>
                            <p><strong>Address:</strong> 123 Main Street, Springfield, IL</p>
                            <p><strong>Phone:</strong> (123) 456-7890</p>
                        </div>

                        <div class=""details"">
                            <table class=""table-container"">
                                <tr>
                                    <th>Primary Care Physician</th>
                                    <td>Dr. Jane Smith</td>
                                </tr>
                                <tr>
                                    <th>Emergency Contact</th>
                                    <td>Mary Doe (Sister) - (987) 654-3210</td>
                                </tr>
                                <tr>
                                    <th>Medical History</th>
                                    <td>Hypertension, Type 2 Diabetes</td>
                                </tr>
                                <tr>
                                    <th>Current Medications</th>
                                    <td>Metformin (500mg), Lisinopril (10mg)</td>
                                </tr>
                                <tr>
                                    <th>Allergies</th>
                                    <td>Penicillin, Sulfa drugs</td>
                                </tr>
                                <tr>
                                    <th>Last Checkup Date</th>
                                    <td>June 15, 2024</td>
                                </tr>
                            </table>
                        </div>

                        <div class=""test-results"">
                            <h3 class=""section-header"">Test Results:</h3>
                            <table class=""table-container"">
                                <tr>
                                    <td><strong>Blood Pressure:</strong></td>
                                    <td class=""highlight"">145/90 mmHg (Elevated)</td>
                                </tr>
                                <tr>
                                    <td><strong>Blood Sugar Level:</strong></td>
                                    <td class=""highlight"">135 mg/dL (Stable)</td>
                                </tr>
                                <tr>
                                    <td><strong>Cholesterol:</strong></td>
                                    <td class=""highlight"">220 mg/dL (Borderline High)</td>
                                </tr>
                                <tr>
                                    <td><strong>ECG Result:</strong></td>
                                    <td>Normal</td>
                                </tr>
                            </table>
                        </div>

                        <div class=""recent-visits"">
                            <h3 class=""section-header"">Recent Visits:</h3>
                            <ul>
                                <li><strong>July 1, 2024:</strong> Annual physical exam. Blood pressure slightly elevated.</li>
                                <li><strong>June 10, 2024:</strong> Routine diabetes check. Blood sugar levels stable.</li>
                            </ul>
                        </div>

                        <div class=""notes"">
                            <h3 class=""section-header"">Notes:</h3>
                            <p>Patient advised to follow up for hypertension management and continue current diabetes treatment plan. Regular monitoring of blood pressure and blood glucose levels recommended.</p>
                        </div>

                        <div class=""prescriptions"">
                            <h3 class=""section-header"">Prescriptions:</h3>
                            <ul>
                                <li><strong>Metformin:</strong> 500mg, twice a day</li>
                                <li><strong>Lisinopril:</strong> 10mg, once a day</li>
                            </ul>
                        </div>

                        <p><strong>Next Appointment:</strong> August 1, 2024</p>

                        <p>Sincerely,</p>
                        <p>Dr. Jane Smith<br>
                            Primary Care Physician<br>
                            Springfield Medical Center</p>
                    </div>
                    <div class=""footer"">
                        <p>&copy; 2024 Springfield Medical Center. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>
            ";

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] pdfBytes = _converter.Convert(pdf);
            return File(pdfBytes, "application/pdf", "ConvertedDocument.pdf");
        }
    }
}
