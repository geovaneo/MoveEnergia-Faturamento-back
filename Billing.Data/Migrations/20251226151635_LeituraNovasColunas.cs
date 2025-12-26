using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class LeituraNovasColunas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                            name: "NomeCliente",
                            table: "LeituraFaturaPdf",
                            type: "nvarchar(150)",
                            nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CpfCnpj",
                table: "LeituraFaturaPdf",
                type: "nvarchar(14)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TarifaConsumo",
                table: "LeituraFaturaPdf",
                type: "decimal(12,8)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TarifaCompensada",
                table: "LeituraFaturaPdf",
                type: "decimal(12,8)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
