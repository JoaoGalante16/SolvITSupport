using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolvITSupport.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFinalStatusToStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinalStatus",
                table: "Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalStatus",
                table: "Statuses");
        }
    }
}
