using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoPessoas.Migrations
{
    /// <inheritdoc />
    public partial class PortalDasFinancasDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PortalDasFinancasUserName",
                table: "Workers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PortalDasFinancasUserPassword",
                table: "Workers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PortalDasFinancasUserName",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "PortalDasFinancasUserPassword",
                table: "Workers");
        }
    }
}
