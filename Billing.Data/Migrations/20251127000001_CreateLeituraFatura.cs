using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateLeituraFatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeituraFaturaPdf",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UC = table.Column<string>(type: "varchar(20)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    MesRef = table.Column<string>(type: "varchar(10)", nullable: true),
                    Vencimento = table.Column<DateTime>(type: "datetime", nullable: true),
                    Emissao = table.Column<DateTime>(type: "datetime", nullable: true),
                    LeiAnterior = table.Column<DateTime>(type: "datetime", nullable: true),
                    LeiAtual = table.Column<DateTime>(type: "datetime", nullable: true),
                    FlagPainelSolar = table.Column<int>(type: "int", nullable: true),
                    CodBarras = table.Column<string>(type: "varchar(50)", nullable: true),
                    EnergiaConsumida = table.Column<int>(type: "int", nullable: true),
                    EnergiaCompensada = table.Column<int>(type: "int", nullable: true),
                    EnergiaSaldo = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "varchar(150)", nullable: true),
                    Folder = table.Column<string>(type: "varchar(250)", nullable: true),
                    FileMD5 = table.Column<string>(type: "varchar(150)", nullable: true),
                    NomeDistr = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeituraFaturaPdf", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeituraFaturaLinha",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: true),
                    Unidade = table.Column<string>(type: "varchar(10)", nullable: true),
                    Qtd = table.Column<decimal>(type: "decimal(12,5)", nullable: true),
                    PrecoUnit = table.Column<decimal>(type: "decimal(12,8)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    CofinsPIS = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    ICMSBase = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    ICMSAliq = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    ICMS = table.Column<decimal>(type: "decimal(10,4)", nullable: true),
                    TarifaUnit = table.Column<decimal>(type: "decimal(12,8)", nullable: true),
                    FaturaPdfId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeituraFaturaLinha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeituraFaturaPdfProcesso",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inicio = table.Column<DateTime>(type: "datetime", nullable: true),
                    termino = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeituraFaturaPdfProcesso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeituraFaturaPdfLog",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    processo = table.Column<int>(type: "int", nullable: true),
                    datahora = table.Column<DateTime>(type: "datetime", nullable: true),
                    mensagem = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeituraFaturaPdfLog", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "LeituraFaturaPdf");
        }
    }
}
