using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_DoctorSpecializations_SpecializationsId",
                table: "DoctorSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpecializations",
                table: "DoctorSpecializations");

            migrationBuilder.RenameTable(
                name: "DoctorSpecializations",
                newName: "Specializations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationsId",
                table: "DoctorSpecialization",
                column: "SpecializationsId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialization_Specializations_SpecializationsId",
                table: "DoctorSpecialization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specializations",
                table: "Specializations");

            migrationBuilder.RenameTable(
                name: "Specializations",
                newName: "DoctorSpecializations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpecializations",
                table: "DoctorSpecializations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialization_DoctorSpecializations_SpecializationsId",
                table: "DoctorSpecialization",
                column: "SpecializationsId",
                principalTable: "DoctorSpecializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
