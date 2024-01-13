using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenWeather.DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_icon_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "Icon",
               table: "WeatherInfos",
               type: "nvarchar(max)",
               nullable: false,
               defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
