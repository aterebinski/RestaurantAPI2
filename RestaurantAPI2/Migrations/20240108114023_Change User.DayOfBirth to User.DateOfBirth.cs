using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI2.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserDayOfBirthtoUserDateOfBirth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DayOfBirth",
                table: "Users",
                newName: "DateOfBirth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Users",
                newName: "DayOfBirth");
        }
    }
}
