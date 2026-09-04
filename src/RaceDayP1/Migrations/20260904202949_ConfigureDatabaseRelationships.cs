using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDayP1.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureDatabaseRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Events",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results",
                column: "EnrolmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeID",
                table: "Events",
                column: "EventTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganiserID",
                table: "Events",
                column: "OrganiserID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_CategoryID",
                table: "Enrolments",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_EventID",
                table: "Enrolments",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_UserID",
                table: "Enrolments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_EventID",
                table: "Categories",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Events_EventID",
                table: "Categories",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Categories_CategoryID",
                table: "Enrolments",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Events_EventID",
                table: "Enrolments",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "EventID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Users_UserID",
                table: "Enrolments",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_EventTypeID",
                table: "Events",
                column: "EventTypeID",
                principalTable: "EventTypes",
                principalColumn: "EventTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_OrganiserID",
                table: "Events",
                column: "OrganiserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Enrolments_EnrolmentID",
                table: "Results",
                column: "EnrolmentID",
                principalTable: "Enrolments",
                principalColumn: "EnrolmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Events_EventID",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Categories_CategoryID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Events_EventID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Users_UserID",
                table: "Enrolments");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_EventTypeID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_OrganiserID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Results_Enrolments_EnrolmentID",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_EnrolmentID",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventTypeID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganiserID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_CategoryID",
                table: "Enrolments");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_EventID",
                table: "Enrolments");

            migrationBuilder.DropIndex(
                name: "IX_Enrolments_UserID",
                table: "Enrolments");

            migrationBuilder.DropIndex(
                name: "IX_Categories_EventID",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
