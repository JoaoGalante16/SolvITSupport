using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolvITSupport.Migrations
{
    /// <inheritdoc />
    public partial class AddHelpfulCounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HelpfulCount",
                table: "BaseConhecimento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotHelpfulCount",
                table: "BaseConhecimento",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HelpfulCount",
                table: "BaseConhecimento");

            migrationBuilder.DropColumn(
                name: "NotHelpfulCount",
                table: "BaseConhecimento");
        }
    }
}
