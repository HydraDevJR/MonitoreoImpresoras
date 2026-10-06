using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorIMP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Nit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Franquicias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizacionId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franquicias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Franquicias_Organizaciones_OrganizacionId",
                        column: x => x.OrganizacionId,
                        principalTable: "Organizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Restaurantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FranquiciaId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurantes_Franquicias_FranquiciaId",
                        column: x => x.FranquiciaId,
                        principalTable: "Franquicias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NombreEquipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UltimoHeartBeat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UltimaIp = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agentes_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosAccesos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    OrganizacionId = table.Column<int>(type: "int", nullable: true),
                    FranquiciaId = table.Column<int>(type: "int", nullable: true),
                    RestauranteId = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAccesos", x => x.Id);
                    table.CheckConstraint("CK_UsuariosAccesos_Nivel_Referencia", "(\r\n    (Nivel = 1 AND OrganizacionId IS NOT NULL AND FranquiciaId IS NULL AND RestauranteId IS NULL)\r\n    OR\r\n    (Nivel = 2 AND OrganizacionId IS NULL AND FranquiciaId IS NOT NULL AND RestauranteId IS NULL)\r\n    OR\r\n    (Nivel = 3 AND OrganizacionId IS NULL AND FranquiciaId IS NULL AND RestauranteId IS NOT NULL)\r\n)");
                    table.ForeignKey(
                        name: "FK_UsuariosAccesos_Franquicias_FranquiciaId",
                        column: x => x.FranquiciaId,
                        principalTable: "Franquicias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosAccesos_Organizaciones_OrganizacionId",
                        column: x => x.OrganizacionId,
                        principalTable: "Organizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosAccesos_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosAccesos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgenteCredenciales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HashSecreto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRevocacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UltimoUso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteCredenciales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgenteCredenciales_Agentes_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "Agentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origen = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    AgenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizacionId = table.Column<int>(type: "int", nullable: true),
                    FranquiciaId = table.Column<int>(type: "int", nullable: true),
                    RestauranteId = table.Column<int>(type: "int", nullable: true),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Entidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntidadId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatosAnteriores = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatosNuevos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IpOrigen = table.Column<string>(type: "varchar(45)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                    table.CheckConstraint("CK_Auditorias_OrigenActor", "(\r\n    (Origen = 1 AND UsuarioId IS NOT NULL AND AgenteId IS NULL)\r\n    OR\r\n    (Origen = 2 AND AgenteId IS NOT NULL AND UsuarioId IS NULL)\r\n    OR\r\n    (Origen IN (3, 4, 5) AND UsuarioId IS NULL AND AgenteId IS NULL)\r\n)");
                    table.ForeignKey(
                        name: "FK_Auditorias_Agentes_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "Agentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditorias_Franquicias_FranquiciaId",
                        column: x => x.FranquiciaId,
                        principalTable: "Franquicias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditorias_Organizaciones_OrganizacionId",
                        column: x => x.OrganizacionId,
                        principalTable: "Organizaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditorias_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Impresoras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgenteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestauranteId = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mac = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    IpActual = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impresoras_Agentes_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "Agentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresoras_Restaurantes_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImpresoraEventos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImpresoraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoEvento = table.Column<int>(type: "int", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "int", nullable: true),
                    EstadoNuevo = table.Column<int>(type: "int", nullable: false),
                    FechaEvento = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EventoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpresoraEventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImpresoraEventos_Impresoras_ImpresoraId",
                        column: x => x.ImpresoraId,
                        principalTable: "Impresoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgenteCredenciales_AgenteId_Activo",
                table: "AgenteCredenciales",
                columns: new[] { "AgenteId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_Agentes_Estado",
                table: "Agentes",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Agentes_Estado_UltimoHeartBeat",
                table: "Agentes",
                columns: new[] { "Estado", "UltimoHeartBeat" });

            migrationBuilder.CreateIndex(
                name: "IX_Agentes_RestauranteId_Codigo",
                table: "Agentes",
                columns: new[] { "RestauranteId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agentes_UltimoHeartBeat",
                table: "Agentes",
                column: "UltimoHeartBeat");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_AgenteId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "AgenteId", "FechaEvento" },
                descending: new[] { false, true },
                filter: "[AgenteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_Entidad_EntidadId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "Entidad", "EntidadId", "FechaEvento" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_FechaEvento",
                table: "Auditorias",
                column: "FechaEvento",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_FranquiciaId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "FranquiciaId", "FechaEvento" },
                descending: new[] { false, true },
                filter: "[FranquiciaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_OrganizacionId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "OrganizacionId", "FechaEvento" },
                descending: new[] { false, true },
                filter: "[OrganizacionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_Origen_FechaEvento",
                table: "Auditorias",
                columns: new[] { "Origen", "FechaEvento" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_RestauranteId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "RestauranteId", "FechaEvento" },
                descending: new[] { false, true },
                filter: "[RestauranteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_UsuarioId_FechaEvento",
                table: "Auditorias",
                columns: new[] { "UsuarioId", "FechaEvento" },
                descending: new[] { false, true },
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Franquicias_Nombre",
                table: "Franquicias",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Franquicias_OrganizacionId_Codigo",
                table: "Franquicias",
                columns: new[] { "OrganizacionId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImpresoraEventos_FechaEvento",
                table: "ImpresoraEventos",
                column: "FechaEvento",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ImpresoraEventos_ImpresoraId_EventoId",
                table: "ImpresoraEventos",
                columns: new[] { "ImpresoraId", "EventoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImpresoraEventos_ImpresoraId_FechaEvento",
                table: "ImpresoraEventos",
                columns: new[] { "ImpresoraId", "FechaEvento" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_ImpresoraEventos_TipoEvento_FechaEvento",
                table: "ImpresoraEventos",
                columns: new[] { "TipoEvento", "FechaEvento" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_AgenteId",
                table: "Impresoras",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_Estado",
                table: "Impresoras",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_Mac",
                table: "Impresoras",
                column: "Mac",
                unique: true,
                filter: "[Mac] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_RestauranteId",
                table: "Impresoras",
                column: "RestauranteId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_RestauranteId_Codigo",
                table: "Impresoras",
                columns: new[] { "RestauranteId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_Serial",
                table: "Impresoras",
                column: "Serial",
                unique: true,
                filter: "[Serial] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organizaciones_Codigo",
                table: "Organizaciones",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizaciones_Nit",
                table: "Organizaciones",
                column: "Nit",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizaciones_Nombre",
                table: "Organizaciones",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_Ciudad",
                table: "Restaurantes",
                column: "Ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_FranquiciaId_Codigo",
                table: "Restaurantes",
                columns: new[] { "FranquiciaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_Nombre",
                table: "Restaurantes",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Apellido",
                table: "Usuarios",
                column: "Apellido");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ExternalId",
                table: "Usuarios",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Nombre",
                table: "Usuarios",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Rol",
                table: "Usuarios",
                column: "Rol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_FranquiciaId",
                table: "UsuariosAccesos",
                column: "FranquiciaId",
                filter: "[FranquiciaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_OrganizacionId",
                table: "UsuariosAccesos",
                column: "OrganizacionId",
                filter: "[OrganizacionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_RestauranteId",
                table: "UsuariosAccesos",
                column: "RestauranteId",
                filter: "[RestauranteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_UsuarioId_Activo",
                table: "UsuariosAccesos",
                columns: new[] { "UsuarioId", "Activo" });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_UsuarioId_FranquiciaId",
                table: "UsuariosAccesos",
                columns: new[] { "UsuarioId", "FranquiciaId" },
                unique: true,
                filter: "[FranquiciaId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_UsuarioId_OrganizacionId",
                table: "UsuariosAccesos",
                columns: new[] { "UsuarioId", "OrganizacionId" },
                unique: true,
                filter: "[OrganizacionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosAccesos_UsuarioId_RestauranteId",
                table: "UsuariosAccesos",
                columns: new[] { "UsuarioId", "RestauranteId" },
                unique: true,
                filter: "[RestauranteId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgenteCredenciales");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "ImpresoraEventos");

            migrationBuilder.DropTable(
                name: "UsuariosAccesos");

            migrationBuilder.DropTable(
                name: "Impresoras");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Agentes");

            migrationBuilder.DropTable(
                name: "Restaurantes");

            migrationBuilder.DropTable(
                name: "Franquicias");

            migrationBuilder.DropTable(
                name: "Organizaciones");
        }
    }
}
