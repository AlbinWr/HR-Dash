using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class TaBortAnstalldIDRullandeSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RullandeScheman_Anstallda_AnstalldId",
                table: "RullandeScheman");

            migrationBuilder.DropIndex(
                name: "IX_RullandeScheman_AnstalldId",
                table: "RullandeScheman");

            migrationBuilder.DropColumn(
                name: "AnstalldId",
                table: "RullandeScheman");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnstalldId",
                table: "RullandeScheman",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RullandeScheman_AnstalldId",
                table: "RullandeScheman",
                column: "AnstalldId");

            migrationBuilder.AddForeignKey(
                name: "FK_RullandeScheman_Anstallda_AnstalldId",
                table: "RullandeScheman",
                column: "AnstalldId",
                principalTable: "Anstallda",
                principalColumn: "AnstalldId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
