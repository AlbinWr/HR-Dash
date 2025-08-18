using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Dash.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixAnstalldSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepeteraAntalGånger",
                table: "RullandeScheman");

            migrationBuilder.DropColumn(
                name: "Startdatum",
                table: "RullandeScheman");

            migrationBuilder.RenameColumn(
                name: "SkiftId",
                table: "Skift",
                newName: "IdSkift");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RullandeScheman",
                newName: "IdSchema");

            migrationBuilder.AddColumn<string>(
                name: "NamnSkift",
                table: "RullandeScheman",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTid",
                table: "RullandeScheman",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "AnstalldSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnstalldId = table.Column<int>(type: "int", nullable: false),
                    RullandeSchemaId = table.Column<int>(type: "int", nullable: false),
                    Startdatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnstalldSchema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnstalldSchema_Anstallda_AnstalldId",
                        column: x => x.AnstalldId,
                        principalTable: "Anstallda",
                        principalColumn: "AnstalldId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnstalldSchema_RullandeScheman_RullandeSchemaId",
                        column: x => x.RullandeSchemaId,
                        principalTable: "RullandeScheman",
                        principalColumn: "IdSchema",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnstalldSchema_AnstalldId",
                table: "AnstalldSchema",
                column: "AnstalldId");

            migrationBuilder.CreateIndex(
                name: "IX_AnstalldSchema_RullandeSchemaId",
                table: "AnstalldSchema",
                column: "RullandeSchemaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnstalldSchema");

            migrationBuilder.DropColumn(
                name: "NamnSkift",
                table: "RullandeScheman");

            migrationBuilder.DropColumn(
                name: "StartTid",
                table: "RullandeScheman");

            migrationBuilder.RenameColumn(
                name: "IdSkift",
                table: "Skift",
                newName: "SkiftId");

            migrationBuilder.RenameColumn(
                name: "IdSchema",
                table: "RullandeScheman",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "RepeteraAntalGånger",
                table: "RullandeScheman",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Startdatum",
                table: "RullandeScheman",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
