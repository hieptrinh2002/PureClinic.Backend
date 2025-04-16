using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_v11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Doctors_DoctorId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_HealthServices_HealthServiceId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Patients_PatientId",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DoctorId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Feedbacks");

            migrationBuilder.RenameTable(
                name: "Feedbacks",
                newName: "HealthServiceFeedbacks");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_PatientId",
                table: "HealthServiceFeedbacks",
                newName: "IX_HealthServiceFeedbacks_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_HealthServiceId",
                table: "HealthServiceFeedbacks",
                newName: "IX_HealthServiceFeedbacks_HealthServiceId");

            migrationBuilder.AlterColumn<int>(
                name: "HealthServiceId",
                table: "HealthServiceFeedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthServiceFeedbacks",
                table: "HealthServiceFeedbacks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClinicFeedBacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryBy = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReported = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicFeedBacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicFeedBacks_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    EntryBy = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReported = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorFeedbacks_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorFeedbacks_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicFeedBacks_PatientId",
                table: "ClinicFeedBacks",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeedbacks_DoctorId",
                table: "DoctorFeedbacks",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorFeedbacks_PatientId",
                table: "DoctorFeedbacks",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthServiceFeedbacks_HealthServices_HealthServiceId",
                table: "HealthServiceFeedbacks",
                column: "HealthServiceId",
                principalTable: "HealthServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthServiceFeedbacks_Patients_PatientId",
                table: "HealthServiceFeedbacks",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthServiceFeedbacks_HealthServices_HealthServiceId",
                table: "HealthServiceFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthServiceFeedbacks_Patients_PatientId",
                table: "HealthServiceFeedbacks");

            migrationBuilder.DropTable(
                name: "ClinicFeedBacks");

            migrationBuilder.DropTable(
                name: "DoctorFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthServiceFeedbacks",
                table: "HealthServiceFeedbacks");

            migrationBuilder.RenameTable(
                name: "HealthServiceFeedbacks",
                newName: "Feedbacks");

            migrationBuilder.RenameIndex(
                name: "IX_HealthServiceFeedbacks_PatientId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthServiceFeedbacks_HealthServiceId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_HealthServiceId");

            migrationBuilder.AlterColumn<int>(
                name: "HealthServiceId",
                table: "Feedbacks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DoctorId",
                table: "Feedbacks",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Doctors_DoctorId",
                table: "Feedbacks",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_HealthServices_HealthServiceId",
                table: "Feedbacks",
                column: "HealthServiceId",
                principalTable: "HealthServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Patients_PatientId",
                table: "Feedbacks",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
