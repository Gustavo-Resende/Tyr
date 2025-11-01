using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tyr.Migrations
{
    /// <inheritdoc />
    public partial class ServicesNtoN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Profissionais_ProfissionalId",
                table: "Servicos");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_ProfissionalId",
                table: "Servicos");

            migrationBuilder.RenameColumn(
                name: "ProfissionalId",
                table: "Servicos",
                newName: "DuracaoMinutos");

            migrationBuilder.CreateTable(
                name: "ProfissionalServico",
                columns: table => new
                {
                    ProfissionaisId = table.Column<int>(type: "integer", nullable: false),
                    ServicosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalServico", x => new { x.ProfissionaisId, x.ServicosId });
                    table.ForeignKey(
                        name: "FK_ProfissionalServico_Profissionais_ProfissionaisId",
                        column: x => x.ProfissionaisId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfissionalServico_Servicos_ServicosId",
                        column: x => x.ServicosId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalServico_ServicosId",
                table: "ProfissionalServico",
                column: "ServicosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfissionalServico");

            migrationBuilder.RenameColumn(
                name: "DuracaoMinutos",
                table: "Servicos",
                newName: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_ProfissionalId",
                table: "Servicos",
                column: "ProfissionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Profissionais_ProfissionalId",
                table: "Servicos",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
