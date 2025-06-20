using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DedicadoEstudo.Data.Migrations
{
    public partial class AlterandoParaRoleInvesDePerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Perfil",
                table: "tb_Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Perfil",
                table: "tb_Usuario",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
