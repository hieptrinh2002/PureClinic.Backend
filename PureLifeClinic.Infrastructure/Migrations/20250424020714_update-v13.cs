using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PureLifeClinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatev13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "AuditLogs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuditLogs", x => x.Id);
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
