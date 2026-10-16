using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDayP1.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_UserID",
                table: "Enrolments");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results",
                column: "EnrolmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_UserID_EventID_CategoryID",
                table: "Enrolments",
                columns: new[] { "UserID", "EventID", "CategoryID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_UserID_EventID_CategoryID",
                table: "Enrolments");

            migrationBuilder.CreateIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results",
                column: "EnrolmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_UserID",
                table: "Enrolments",
                column: "UserID");
        }
    }
}
