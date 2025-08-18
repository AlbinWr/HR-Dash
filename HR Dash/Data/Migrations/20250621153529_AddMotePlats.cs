using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMotePlats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Plats",
                table: "Moten",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plats",
                table: "Moten");
        }
    }
}
