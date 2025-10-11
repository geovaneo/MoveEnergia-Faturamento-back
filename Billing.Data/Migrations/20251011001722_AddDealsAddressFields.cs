using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDealsAddressFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndRua",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndNumero",
                table: "Deals",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndCidade",
                table: "Deals",
                type: "nvarchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndUF",
                table: "Deals",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deal_custom_field_EndCEP",
                table: "Deals",
                type: "nvarchar(10)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
