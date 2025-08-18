using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoteFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moten",
                columns: table => new
                {
                    MoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlutTid = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Beskrivning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnstalldId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moten", x => x.MoteId);
                    table.ForeignKey(
                        name: "FK_Moten_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Moten_AspNetUsers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moten_AnstalldId",
                table: "Moten",
                column: "AnstalldId");

            migrationBuilder.CreateIndex(
                name: "IX_Moten_ManagerId",
                table: "Moten",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moten");
        }
    }
}
