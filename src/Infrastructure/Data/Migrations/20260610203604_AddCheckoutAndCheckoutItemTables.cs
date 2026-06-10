using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace E_commerce_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckoutAndCheckoutItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_attempts_orders_order_id",
                table: "payment_attempts");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "users",
                newName: "cart_id");

            migrationBuilder.RenameColumn(
                name: "order_id",
                table: "payment_attempts",
                newName: "checkout_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_attempts_order_id",
                table: "payment_attempts",
                newName: "IX_payment_attempts_checkout_id");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "neighborhood",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payment_method",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "recipient_name",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "shipping_cost",
                table: "orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "total_paid",
                table: "orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "zip_code",
                table: "orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "checkouts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    payment_method = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    state = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    zip_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    recipient_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    shipping_cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkouts", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkouts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkout_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkout_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkout_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkout_items_checkouts_checkout_id",
                        column: x => x.checkout_id,
                        principalTable: "checkouts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_checkout_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_checkout_items_checkout_id",
                table: "checkout_items",
                column: "checkout_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkout_items_product_id",
                table: "checkout_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkouts_user_id",
                table: "checkouts",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_attempts_checkouts_checkout_id",
                table: "payment_attempts",
                column: "checkout_id",
                principalTable: "checkouts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_attempts_checkouts_checkout_id",
                table: "payment_attempts");

            migrationBuilder.DropTable(
                name: "checkout_items");

            migrationBuilder.DropTable(
                name: "checkouts");

            migrationBuilder.DropColumn(
                name: "city",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "neighborhood",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "number",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_method",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "recipient_name",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipping_cost",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "state",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "street",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "total_paid",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "zip_code",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "cart_id",
                table: "users",
                newName: "CartId");

            migrationBuilder.RenameColumn(
                name: "checkout_id",
                table: "payment_attempts",
                newName: "order_id");

            migrationBuilder.RenameIndex(
                name: "IX_payment_attempts_checkout_id",
                table: "payment_attempts",
                newName: "IX_payment_attempts_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_attempts_orders_order_id",
                table: "payment_attempts",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
