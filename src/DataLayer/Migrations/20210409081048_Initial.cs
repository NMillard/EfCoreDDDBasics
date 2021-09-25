using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EfCore");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "EfCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BadAuthors",
                schema: "EfCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookTypes",
                schema: "EfCore",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Unspecified")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "EfCore",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => new { x.AuthorId, x.Title });
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "EfCore",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BadBooks",
                schema: "EfCore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Released = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadBooks_BadAuthors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "EfCore",
                        principalTable: "BadAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "EfCore",
                table: "BookTypes",
                columns: new[] { "id", "Genre" },
                values: new object[] { new Guid("26f40fdf-8f92-4c4f-80c1-71090d86aef4"), "Horror" });

            migrationBuilder.InsertData(
                schema: "EfCore",
                table: "BookTypes",
                columns: new[] { "id", "Genre" },
                values: new object[] { new Guid("1b0f8308-feb0-4d55-93ec-0765971e0bb7"), "Fiction" });

            migrationBuilder.InsertData(
                schema: "EfCore",
                table: "BookTypes",
                columns: new[] { "id", "Genre" },
                values: new object[] { new Guid("5f7f47e3-610f-499c-9119-b73e1df23b62"), "Crime" });

            migrationBuilder.CreateIndex(
                name: "IX_BadBooks_AuthorId",
                schema: "EfCore",
                table: "BadBooks",
                column: "AuthorId");

            migrationBuilder.Sql(
@"
EXEC('CREATE VIEW [EfCore].[BooksView] AS SELECT Title, BookType AS Genre FROM [EfCore].[Books]')
"
);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadBooks",
                schema: "EfCore");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "EfCore");

            migrationBuilder.DropTable(
                name: "BookTypes",
                schema: "EfCore");

            migrationBuilder.DropTable(
                name: "BadAuthors",
                schema: "EfCore");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "EfCore");

            migrationBuilder.Sql("DROP VIEW [EfCore].[BooksView]");
        }
    }
}
