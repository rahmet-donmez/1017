using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountManagment.Repository.Migrations
{
    public partial class transferAddAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "AccountTransactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AccountTransactions");
        }
    }
}
