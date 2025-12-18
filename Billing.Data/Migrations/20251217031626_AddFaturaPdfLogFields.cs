using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveEnergia.Billing.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFaturaPdfLogFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                            name: "NomeDistr",
                            table: "LeituraFaturaPdfLog",
                            type: "nvarchar(50)",
                            nullable: true);

            migrationBuilder.AddColumn<string>(
                            name: "FileName",
                            table: "LeituraFaturaPdfLog",
                            type: "varchar(150)",
                            nullable: true);
            
            migrationBuilder.AddColumn<string>(
                            name: "Folder",
                            table: "LeituraFaturaPdfLog",
                            type: "varchar(250)",
                            nullable: true);
            
            migrationBuilder.AddColumn<string>(
                            name: "FileMD5",
                            table: "LeituraFaturaPdfLog",
                            type: "varchar(150)",
                            nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
