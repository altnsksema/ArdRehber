using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdRehber.Migrations
{
    public partial class usertypepop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_UserTypes_UserTypeId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_UserTypeId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "Persons");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UserTypeId",
                table: "Persons",
                column: "UserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_UserTypes_UserTypeId",
                table: "Persons",
                column: "UserTypeId",
                principalTable: "UserTypes",
                principalColumn: "UserTypeId");
        }
    }
}
