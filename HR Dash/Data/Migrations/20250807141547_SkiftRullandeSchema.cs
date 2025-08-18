using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class SkiftRullandeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RullandeScheman",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Startdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SkiftTimmar = table.Column<int>(type: "int", nullable: false),
                    SkiftMinuter = table.Column<int>(type: "int", nullable: false),
                    MonsterString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepeteraAntalGånger = table.Column<int>(type: "int", nullable: false),
                    AnstalldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RullandeScheman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RullandeScheman_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RullandeScheman_AnstalldId",
                table: "RullandeScheman",
                column: "AnstalldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RullandeScheman");
        }
    }
}
