using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class SkiftRullandeSchemaNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RullandeSchemaId",
                table: "Skift",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skift_RullandeSchemaId",
                table: "Skift",
                column: "RullandeSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skift_RullandeScheman_RullandeSchemaId",
                table: "Skift",
                column: "RullandeSchemaId",
                principalTable: "RullandeScheman",
                principalColumn: "IdSchema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skift_RullandeScheman_RullandeSchemaId",
                table: "Skift");

            migrationBuilder.DropIndex(
                name: "IX_Skift_RullandeSchemaId",
                table: "Skift");

            migrationBuilder.DropColumn(
                name: "RullandeSchemaId",
                table: "Skift");
        }
    }
}
