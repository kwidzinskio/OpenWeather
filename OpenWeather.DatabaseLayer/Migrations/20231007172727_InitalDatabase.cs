using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenWeather.DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitalDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Base = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    Dt = table.Column<int>(type: "int", nullable: false),
                    IdApi = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherInfos");
        }
    }
}
