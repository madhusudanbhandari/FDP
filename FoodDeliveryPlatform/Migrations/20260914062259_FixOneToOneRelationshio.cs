using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixOneToOneRelationshio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId1",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RestaurantId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId1",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Restaurants_RestaurantId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId1",
                table: "Menus",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Carts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RestaurantId1",
                table: "Menus",
                column: "RestaurantId1");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId1",
                table: "Carts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId1",
                table: "Carts",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Restaurants_RestaurantId1",
                table: "Menus",
                column: "RestaurantId1",
                principalTable: "Restaurants",
                principalColumn: "Id");
        }
    }
}
