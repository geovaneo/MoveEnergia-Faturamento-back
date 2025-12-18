using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixEnerCompensadaColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<decimal>(
                name: "energiacompensada",
                schema: "dbo",
                table: "LeituraFaturaPdf",
                type: "decimal(12,5)",
                precision: 12,
                scale: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "energiasaldo",
                schema: "dbo",
                table: "LeituraFaturaPdf",
                type: "decimal(12,5)",
                precision: 12,
                scale: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
