using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Casino.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FIXED : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageSrc",
                value: "/img/carousel/1_carousel.png");

            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageSrc",
                value: "/img/carousel/2_carousel.png");

            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageSrc",
                value: "/img/carousel/3_carousel.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageSrc",
                value: "/img/carousel/carousel1.jpg");

            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageSrc",
                value: "/img/carousel/carousel2.jpg");

            migrationBuilder.UpdateData(
                table: "Carousels",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageSrc",
                value: "/img/carousel/carousel3.jpg");
        }
    }
}
