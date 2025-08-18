using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class LaggTillReferensSkift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnstalldSchemaId",
                table: "Skift",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Skift_AnstalldSchemaId",
                table: "Skift",
                column: "AnstalldSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skift_AnstalldSchema_AnstalldSchemaId",
                table: "Skift",
                column: "AnstalldSchemaId",
                principalTable: "AnstalldSchema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skift_AnstalldSchema_AnstalldSchemaId",
                table: "Skift");

            migrationBuilder.DropIndex(
                name: "IX_Skift_AnstalldSchemaId",
                table: "Skift");

            migrationBuilder.DropColumn(
                name: "AnstalldSchemaId",
                table: "Skift");
        }
    }
}
