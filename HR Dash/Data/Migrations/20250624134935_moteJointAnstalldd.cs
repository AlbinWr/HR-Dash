using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class moteJointAnstalldd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moten_Anstallda_AnstalldId",
                table: "Moten");

            migrationBuilder.DropIndex(
                name: "IX_Moten_AnstalldId",
                table: "Moten");

            migrationBuilder.DropColumn(
                name: "AnstalldId",
                table: "Moten");

            migrationBuilder.CreateTable(
                name: "MoteAnstallda",
                columns: table => new
                {
                    MoteId = table.Column<int>(type: "int", nullable: false),
                    AnstalldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoteAnstallda", x => new { x.MoteId, x.AnstalldId });
                    table.ForeignKey(
                        name: "FK_MoteAnstallda_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoteAnstallda_Moten_MoteId",
                        column: x => x.MoteId,
                        principalTable: "Moten",
                        principalColumn: "MoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoteAnstallda_AnstalldId",
                table: "MoteAnstallda",
                column: "AnstalldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoteAnstallda");

            migrationBuilder.AddColumn<int>(
                name: "AnstalldId",
                table: "Moten",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Moten_AnstalldId",
                table: "Moten",
                column: "AnstalldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moten_Anstallda_AnstalldId",
                table: "Moten",
                column: "AnstalldId",
                principalTable: "Anstallda",
                principalColumn: "AnstalldId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
