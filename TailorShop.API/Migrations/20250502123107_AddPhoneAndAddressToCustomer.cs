using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TailorShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneAndAddressToCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Customers",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Customers",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
