using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatev12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "AppointmentHealthServices",
                newName: "Status");

            migrationBuilder.AlterColumn<float>(
                name: "FileSize",
                table: "MedicalFiles",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MedicalFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentHealthServiceId",
                table: "MedicalFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "AppointmentHealthServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "AppointmentHealthServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EntryBy = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
                    EntryBy = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalFiles_AppointmentHealthServiceId",
                table: "MedicalFiles",
                column: "AppointmentHealthServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_RoomId",
                table: "Doctors",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentHealthServices_RoomId",
                table: "AppointmentHealthServices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentHealthServices_Rooms_RoomId",
                table: "AppointmentHealthServices",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Rooms_RoomId",
                table: "Doctors",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalFiles_AppointmentHealthServices_AppointmentHealthServiceId",
                table: "MedicalFiles",
                column: "AppointmentHealthServiceId",
                principalTable: "AppointmentHealthServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentHealthServices_Rooms_RoomId",
                table: "AppointmentHealthServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Rooms_RoomId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalFiles_AppointmentHealthServices_AppointmentHealthServiceId",
                table: "MedicalFiles");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_MedicalFiles_AppointmentHealthServiceId",
                table: "MedicalFiles");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_RoomId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentHealthServices_RoomId",
                table: "AppointmentHealthServices");

            migrationBuilder.DropColumn(
                name: "AppointmentHealthServiceId",
                table: "MedicalFiles");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "AppointmentHealthServices");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "AppointmentHealthServices");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AppointmentHealthServices",
                newName: "Quantity");

            migrationBuilder.AlterColumn<float>(
                name: "FileSize",
                table: "MedicalFiles",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MedicalFiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
