using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenWeather.DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_new_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "Pressure",
               table: "WeatherInfos",
               type: "int",
               nullable: false,
               defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "WeatherInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descrpition",
                table: "WeatherInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Humidity",
                table: "WeatherInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Temp",
                table: "WeatherInfos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TempFeelsLike",
                table: "WeatherInfos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WindSpeed",
                table: "WeatherInfos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
