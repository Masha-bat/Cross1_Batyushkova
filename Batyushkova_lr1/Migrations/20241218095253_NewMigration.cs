using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Batyushkova_lr1.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
