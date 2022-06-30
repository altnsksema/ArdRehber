using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdRehber.Migrations
{
    public partial class titlee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "Persons",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TitleDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TitleDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TitleDepartments_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TitleId",
                table: "Persons",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleDepartments_DepartmentId",
                table: "TitleDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TitleDepartments_TitleId",
                table: "TitleDepartments",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Titles_TitleId",
                table: "Persons",
                column: "TitleId",
                principalTable: "Titles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Titles_TitleId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "TitleDepartments");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Persons_TitleId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "Persons");
        }
    }
}
