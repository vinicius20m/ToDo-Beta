using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Migrations
{
    public partial class Permissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddMember = table.Column<bool>(type: "bit", nullable: false),
                    DeleteMember = table.Column<bool>(type: "bit", nullable: false),
                    EditQuadro = table.Column<bool>(type: "bit", nullable: false),
                    DeleteQuadro = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quadro_Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuadroId = table.Column<int>(type: "int", nullable: false),
                    PermissionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quadro_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quadro_Permissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Quadro_Permissions_Permission_Types_PermissionTypeId",
                        column: x => x.PermissionTypeId,
                        principalTable: "Permission_Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quadro_Permissions_Quadros_QuadroId",
                        column: x => x.QuadroId,
                        principalTable: "Quadros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quadro_Permissions_PermissionTypeId",
                table: "Quadro_Permissions",
                column: "PermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Quadro_Permissions_QuadroId",
                table: "Quadro_Permissions",
                column: "QuadroId");

            migrationBuilder.CreateIndex(
                name: "IX_Quadro_Permissions_UserId",
                table: "Quadro_Permissions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quadro_Permissions");

            migrationBuilder.DropTable(
                name: "Permission_Types");
        }
    }
}
