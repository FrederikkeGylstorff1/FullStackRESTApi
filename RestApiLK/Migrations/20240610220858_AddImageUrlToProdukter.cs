using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestApiLK.Migrations
{
    public partial class AddImageUrlToProdukter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Produkter",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Produkter");
        }
    }
}
