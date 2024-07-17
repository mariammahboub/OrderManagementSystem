using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Repository.Data.Migrations
{
    public partial class EditPaymentModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PaymentResults_DeliveryMethodId",
                table: "PaymentResults",
                column: "DeliveryMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentResults_DeliveryMethods_DeliveryMethodId",
                table: "PaymentResults",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentResults_DeliveryMethods_DeliveryMethodId",
                table: "PaymentResults");

            migrationBuilder.DropIndex(
                name: "IX_PaymentResults_DeliveryMethodId",
                table: "PaymentResults");
        }
    }
}
