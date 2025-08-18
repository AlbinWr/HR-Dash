using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anstallda",
                columns: table => new
                {
                    AnstalldId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnstalldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnstallningDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avdelning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilBild = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anstallda", x => x.AnstalldId);
                });

            migrationBuilder.CreateTable(
                name: "Ansokningar",
                columns: table => new
                {
                    AnsokanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlutDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnsokanTyp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnstalldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ansokningar", x => x.AnsokanId);
                    table.ForeignKey(
                        name: "FK_Ansokningar_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skift",
                columns: table => new
                {
                    SkiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlutTid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnstalldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skift", x => x.SkiftId);
                    table.ForeignKey(
                        name: "FK_Skift_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ansokningar_AnstalldId",
                table: "Ansokningar",
                column: "AnstalldId");

            migrationBuilder.CreateIndex(
                name: "IX_Skift_AnstalldId",
                table: "Skift",
                column: "AnstalldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ansokningar");

            migrationBuilder.DropTable(
                name: "Skift");

            migrationBuilder.DropTable(
                name: "Anstallda");
        }
    }
}
