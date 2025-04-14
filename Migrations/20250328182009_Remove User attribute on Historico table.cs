using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SESOPtracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserattributeonHistoricotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "usuario",
                table: "Historicos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuario",
                table: "Historicos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
