using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MqttMotionSensorConfigs",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", unicode: false, nullable: false),
                    MotionDetectorTopic = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttMotionSensorConfigs", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MqttMotionSensorConfigs");
        }
    }
}
