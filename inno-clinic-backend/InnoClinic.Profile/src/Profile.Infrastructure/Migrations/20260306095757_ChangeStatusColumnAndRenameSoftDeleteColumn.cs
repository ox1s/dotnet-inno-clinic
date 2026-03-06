using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatusColumnAndRenameSoftDeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "receptionists",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "receptionists",
                newName: "deleted_on_utc");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "patients",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "patients",
                newName: "deleted_on_utc");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "profile",
                table: "doctors",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedOnUtc",
                schema: "profile",
                table: "doctors",
                newName: "deleted_on_utc");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                schema: "profile",
                table: "receptionists",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                schema: "profile",
                table: "patients",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                schema: "profile",
                table: "doctors",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_deleted",
                schema: "profile",
                table: "receptionists",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_on_utc",
                schema: "profile",
                table: "receptionists",
                newName: "DeletedOnUtc");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                schema: "profile",
                table: "patients",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_on_utc",
                schema: "profile",
                table: "patients",
                newName: "DeletedOnUtc");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                schema: "profile",
                table: "doctors",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_on_utc",
                schema: "profile",
                table: "doctors",
                newName: "DeletedOnUtc");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "receptionists",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "patients",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "profile",
                table: "doctors",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
