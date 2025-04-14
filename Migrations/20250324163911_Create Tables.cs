using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SESOPtracker.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salas",
                columns: table => new
                {
                    salaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    local = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salas", x => x.salaId);
                });

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    situacaoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    cor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situacoes", x => x.situacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    patrimonio = table.Column<string>(type: "TEXT", nullable: false),
                    item = table.Column<string>(type: "TEXT", nullable: true),
                    nome = table.Column<string>(type: "TEXT", nullable: true),
                    subCategoria = table.Column<string>(type: "TEXT", nullable: false),
                    categoria = table.Column<string>(type: "TEXT", nullable: false),
                    setor = table.Column<string>(type: "TEXT", nullable: true),
                    dataCriacao = table.Column<string>(type: "TEXT", nullable: false),
                    situacao = table.Column<int>(type: "INTEGER", nullable: false),
                    sala = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.patrimonio);
                    table.ForeignKey(
                        name: "FK_Equipamentos_Salas_sala",
                        column: x => x.sala,
                        principalTable: "Salas",
                        principalColumn: "salaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipamentos_Situacoes_situacao",
                        column: x => x.situacao,
                        principalTable: "Situacoes",
                        principalColumn: "situacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    historicoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    patrimonio = table.Column<string>(type: "TEXT", nullable: false),
                    dataAlteracao = table.Column<string>(type: "TEXT", nullable: false),
                    situacaoAtual = table.Column<int>(type: "INTEGER", nullable: false),
                    usuario = table.Column<string>(type: "TEXT", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    observacao = table.Column<string>(type: "TEXT", nullable: true),
                    importante = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.historicoId);
                    table.ForeignKey(
                        name: "FK_Historicos_Equipamentos_patrimonio",
                        column: x => x.patrimonio,
                        principalTable: "Equipamentos",
                        principalColumn: "patrimonio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Historicos_Situacoes_situacaoAtual",
                        column: x => x.situacaoAtual,
                        principalTable: "Situacoes",
                        principalColumn: "situacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_sala",
                table: "Equipamentos",
                column: "sala");

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_situacao",
                table: "Equipamentos",
                column: "situacao");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_patrimonio",
                table: "Historicos",
                column: "patrimonio");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_situacaoAtual",
                table: "Historicos",
                column: "situacaoAtual");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Salas");

            migrationBuilder.DropTable(
                name: "Situacoes");
        }
    }
}
