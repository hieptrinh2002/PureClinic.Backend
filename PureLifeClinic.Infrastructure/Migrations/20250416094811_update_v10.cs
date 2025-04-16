using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
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
                name: "HealthServiceId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_HealthServiceId",
                table: "Feedbacks",
                column: "HealthServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_HealthServices_HealthServiceId",
                table: "Feedbacks",
                column: "HealthServiceId",
                principalTable: "HealthServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_HealthServices_HealthServiceId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_HealthServiceId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "HealthServiceId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
