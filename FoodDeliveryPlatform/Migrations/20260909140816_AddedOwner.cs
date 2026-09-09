using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddedOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantOwnerId",
                table: "Restaurants",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantOwnerId",
                table: "Restaurants",
                column: "RestaurantOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Users_RestaurantOwnerId",
                table: "Restaurants",
                column: "RestaurantOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Users_RestaurantOwnerId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_RestaurantOwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "RestaurantOwnerId",
                table: "Restaurants");
        }
    }
}
