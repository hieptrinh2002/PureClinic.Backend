using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_v15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuccessfulPatients",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuccessfulPatients",
                table: "Doctors");
        }
    }
}
