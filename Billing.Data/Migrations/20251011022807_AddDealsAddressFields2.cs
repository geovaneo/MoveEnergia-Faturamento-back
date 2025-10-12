using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDealsAddressFields2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndBairro",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndComplemento",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deal_custom_field_EndBairro",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Deal_custom_field_EndComplemento",
                table: "Deals");

        }
    }
}
