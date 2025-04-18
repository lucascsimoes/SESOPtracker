using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SESOPtracker.Migrations
{
    /// <inheritdoc />
    public partial class Addedtagcolumninequipmenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tag",
                table: "Equipamentos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag",
                table: "Equipamentos");
        }
    }
}
