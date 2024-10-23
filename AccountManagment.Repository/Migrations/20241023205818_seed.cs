using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccountManagment.Repository.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    TcNo = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Adress = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Iban = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TargetAccountId = table.Column<int>(type: "integer", nullable: false),
                    SourceAccountId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_SourceAccountId",
                        column: x => x.SourceAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_TargetAccountId",
                        column: x => x.TargetAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Direction = table.Column<bool>(type: "boolean", nullable: false),
                    TransferId = table.Column<int>(type: "integer", nullable: true),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTransactions_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Adress", "City", "CreatedDate", "DeletedDate", "Email", "IsAdmin", "IsDeleted", "Name", "Password", "Phone", "State", "Surname", "TcNo", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "İstanbul Cad. No:1", "İstanbul", new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(2915), null, "rahmetdonmez@gmail.com", false, false, "Rahmet", "Password1", "05001234567", "İstanbul", "Dönmez", "12345678901", null },
                    { 2, "Ankara Cad. No:2", "Ankara", new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(2937), null, "ayse.kara@example.com", true, false, "Ayşe", "Password2", "05007654321", "Ankara", "Kara", "10987654321", null },
                    { 3, "İzmir Cad. No:3", "İzmir", new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(2938), null, "mehmet.demir@example.com", false, false, "Mehmet", "Password3", "05009876543", "İzmir", "Demir", "12345678902", null },
                    { 4, "Bursa Cad. No:4", "Bursa", new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(2940), null, "fatma.celik@example.com", false, false, "Fatma", "Password4", "05001234568", "Bursa", "Çelik", "12345678903", null }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "CreatedDate", "DeletedDate", "Iban", "IsActive", "IsDeleted", "Number", "Type", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3051), null, "TR123456789012345678901", true, false, "TR123456789012345678901", "Vadesiz", null, 1 },
                    { 2, 1500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3055), null, "TR234567890123456789012", true, false, "TR234567890123456789012", "Vadesiz", null, 1 },
                    { 3, 2000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3057), null, "TR345678901234567890123", false, false, "TR345678901234567890123", "Vadeli", null, 2 },
                    { 4, 2500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3057), null, "TR456789012345678901234", true, false, "TR456789012345678901234", "Vadesiz", null, 2 },
                    { 5, 500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3058), null, "TR567890123456789012345", true, false, "TR567890123456789012345", "Vadesiz", null, 3 },
                    { 6, 7500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3059), null, "TR678901234567890123456", true, false, "TR678901234567890123456", "Vadeli", null, 4 }
                });

            migrationBuilder.InsertData(
                table: "AccountTransactions",
                columns: new[] { "Id", "AccountId", "Amount", "CreatedDate", "DeletedDate", "Description", "Direction", "IsDeleted", "TransferId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, 500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3072), null, "Para Yatırma", true, false, null, null },
                    { 2, 1, 200m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3074), null, "Para Çekme", false, false, null, null },
                    { 3, 2, 1000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3075), null, "Fatura Ödeme", false, false, null, null },
                    { 4, 3, 2500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3076), null, "Havale", true, false, null, null },
                    { 5, 4, 1500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3077), null, "Transfer", false, false, null, null },
                    { 6, 5, 3000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3077), null, "Para Yatırma", true, false, null, null },
                    { 7, 6, 500m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3078), null, "Para Çekme", false, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "Transfers",
                columns: new[] { "Id", "Amount", "CreatedDate", "DeletedDate", "Description", "IsDeleted", "SourceAccountId", "TargetAccountId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3095), null, "Kredi Kartı Transferi", false, 1, 2, null },
                    { 2, 2000m, new DateTime(2024, 10, 23, 23, 58, 18, 686, DateTimeKind.Local).AddTicks(3096), null, "Havale", false, 3, 4, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_AccountId",
                table: "AccountTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTransactions_TransferId",
                table: "AccountTransactions",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SourceAccountId",
                table: "Transfers",
                column: "SourceAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TargetAccountId",
                table: "Transfers",
                column: "TargetAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTransactions");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
