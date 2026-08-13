using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileLookupIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_receptionists_account_id",
                schema: "profile",
                table: "receptionists",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_receptionists_office_id",
                schema: "profile",
                table: "receptionists",
                column: "office_id");

            migrationBuilder.CreateIndex(
                name: "IX_patients_account_id",
                schema: "profile",
                table: "patients",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_account_id",
                schema: "profile",
                table: "doctors",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_office_id",
                schema: "profile",
                table: "doctors",
                column: "office_id");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_specialization_id",
                schema: "profile",
                table: "doctors",
                column: "specialization_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_receptionists_account_id",
                schema: "profile",
                table: "receptionists");

            migrationBuilder.DropIndex(
                name: "IX_receptionists_office_id",
                schema: "profile",
                table: "receptionists");

            migrationBuilder.DropIndex(
                name: "IX_patients_account_id",
                schema: "profile",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_doctors_account_id",
                schema: "profile",
                table: "doctors");

            migrationBuilder.DropIndex(
                name: "IX_doctors_office_id",
                schema: "profile",
                table: "doctors");

            migrationBuilder.DropIndex(
                name: "IX_doctors_specialization_id",
                schema: "profile",
                table: "doctors");
        }
    }
}
