using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Appointments_AppointmentId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalReports_Invoice_InvoiceId",
                table: "MedicalReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_AppointmentId",
                table: "Invoices",
                newName: "IX_Invoices_AppointmentId");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalReports_Invoices_InvoiceId",
                table: "MedicalReports",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalReports_Invoices_InvoiceId",
                table: "MedicalReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_AppointmentId",
                table: "Invoice",
                newName: "IX_Invoice_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Appointments_AppointmentId",
                table: "Invoice",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalReports_Invoice_InvoiceId",
                table: "MedicalReports",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id");
        }
    }
}
