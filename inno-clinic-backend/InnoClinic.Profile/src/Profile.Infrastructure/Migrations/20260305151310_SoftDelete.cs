using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "receptionists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "receptionists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "patients",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "patients",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "doctors",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "doctors",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "receptionists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "receptionists");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "doctors");
        }
    }
}