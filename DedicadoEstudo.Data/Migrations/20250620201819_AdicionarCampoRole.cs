using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DedicadoEstudo.Data.Migrations
{
    public partial class AdicionarCampoRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "tb_Usuario",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "tb_Usuario");
        }
    }
}
