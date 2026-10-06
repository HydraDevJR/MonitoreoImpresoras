using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MonitorIMP.Application.Abstractions;
using MonitorIMP.Application.Common.Errors;
using MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;
using MonitorIMP.Application.Tests.Common;
using MonitorIMP.Domain.Enums;
using NSubstitute;

namespace MonitorIMP.Application.Tests.Features.Impresoras.RegistrarEventoImpresora;

public sealed class RegistrarEventoImpresoraCommandHandlerTests
{
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();
    private readonly IAuditoriaService _auditoria = Substitute.For<IAuditoriaService>();

    // ═══════════════════════════════════════════════════════════════════
    // 1. Autenticación
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_SinAgenteAutenticado_RetornaNoAutenticado()
    {
        // Arrange
        await using var context = InMemoryDbContextFactory.Create();
        _currentUser.AgenteId.Returns((Guid?)null);
        _currentUser.UsuarioId.Returns((int?)null);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CommonErrors.NoAutenticado);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 2. Impresora no encontrada / inactiva
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_ImpresoraNoExiste_RetornaImpresoraNotFound()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var agente = TestHelpers.AgenteActivo();
        context.Agentes.Add(agente);
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido();

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RegistrarEventoImpresoraErrors.ImpresoraNotFound);
    }

    [Fact]
    public async Task Handle_ImpresoraInactiva_RetornaImpresoraNotFound()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var agente = TestHelpers.AgenteActivo();
        var impresora = TestHelpers.ImpresoraInactiva();
        impresora.RestauranteId = agente.RestauranteId;
        impresora.AgenteId = agente.Id;

        context.Agentes.Add(agente);
        context.Impresoras.Add(impresora);
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RegistrarEventoImpresoraErrors.ImpresoraNotFound);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 3. Agente inactivo
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_AgenteInactivoEnBd_RetornaNoAutenticado()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var agente = TestHelpers.AgenteInactivo();
        var impresora = TestHelpers.ImpresoraActiva(agenteId: agente.Id);
        impresora.RestauranteId = agente.RestauranteId;

        context.Agentes.Add(agente);
        context.Impresoras.Add(impresora);
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CommonErrors.NoAutenticado);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 4. Agente no autorizado (distinto restaurante)
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_AgenteDeOtroRestaurante_RetornaAgenteNoAutorizado()
    {
        await using var context = InMemoryDbContextFactory.Create();
        var agente = TestHelpers.AgenteActivo(restauranteId: 1);
        var impresora = TestHelpers.ImpresoraActiva(restauranteId: 2);

        context.Agentes.Add(agente);
        context.Impresoras.Add(impresora);
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RegistrarEventoImpresoraErrors.AgenteNoAutorizado);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 5. Evento nuevo — casos exitosos
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_EventoNuevoSinCambioDeEstado_RegistraEventoYNoCambiaEstado()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);
        _currentUser.Ip.Returns("127.0.0.1");

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(
            impresoraId: impresora.Id,
            estadoNuevo: EstadoImpresora.Online); // mismo estado

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.EsDuplicado.Should().BeFalse();

        var eventoPersistido = await context.ImpresoraEventos.SingleAsync();
        eventoPersistido.EventoId.Should().Be(command.EventoId);
        eventoPersistido.EstadoAnterior.Should().Be(EstadoImpresora.Online);
        eventoPersistido.EstadoNuevo.Should().Be(EstadoImpresora.Online);

        var impresoraDb = await context.Impresoras.SingleAsync();
        impresoraDb.Estado.Should().Be(EstadoImpresora.Online);
    }

    [Fact]
    public async Task Handle_EventoNuevoConCambioDeEstado_ActualizaEstadoImpresora()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(
            impresoraId: impresora.Id,
            estadoNuevo: EstadoImpresora.Error);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var evento = await context.ImpresoraEventos.SingleAsync();
        evento.EstadoAnterior.Should().Be(EstadoImpresora.Online);
        evento.EstadoNuevo.Should().Be(EstadoImpresora.Error);

        var impresoraDb = await context.Impresoras.SingleAsync();
        impresoraDb.Estado.Should().Be(EstadoImpresora.Error);
    }

    [Fact]
    public async Task Handle_EventoConEstadoNuevoNull_UsaEstadoAnteriorComoNuevo()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(
            impresoraId: impresora.Id,
            estadoNuevo: null);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var evento = await context.ImpresoraEventos.SingleAsync();
        evento.EstadoAnterior.Should().Be(EstadoImpresora.Online);
        evento.EstadoNuevo.Should().Be(EstadoImpresora.Online);

        var impresoraDb = await context.Impresoras.SingleAsync();
        impresoraDb.Estado.Should().Be(EstadoImpresora.Online);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 6. Duplicado (idempotencia)
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_EventoDuplicado_RetornaSuccessConEsDuplicadoTrue()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();
        var eventoId = Guid.NewGuid();

        // Insertar el evento primero
        var eventoExistente = new Domain.Entities.ImpresoraEvento
        {
            Id = Guid.NewGuid(),
            ImpresoraId = impresora.Id,
            EventoId = eventoId,
            TipoEvento = TipoImpresoraEvento.CambioEstado,
            EstadoAnterior = EstadoImpresora.Online,
            EstadoNuevo = EstadoImpresora.Online,
            Activo = true
        };
        context.ImpresoraEventos.Add(eventoExistente);
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id, eventoId: eventoId);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.EsDuplicado.Should().BeTrue();
        result.Value.EventoId.Should().Be(eventoId);
        result.Value.Id.Should().Be(eventoExistente.Id);

        // No debe haberse insertado un segundo evento
        var total = await context.ImpresoraEventos.CountAsync();
        total.Should().Be(1);
    }

    // ═══════════════════════════════════════════════════════════════════
    // 7. Auditoría
    // ═══════════════════════════════════════════════════════════════════

    [Fact]
    public async Task Handle_EventoNuevo_RegistraAuditoriaConOrigenAgente()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id);

        await handler.Handle(command, CancellationToken.None);

        // Verificar que IAuditoriaService fue llamado
        await _auditoria.Received(1).RegistrarAsync(
            origen: OrigenAuditoria.Agente,
            accion: "RegistrarEventoImpresora",
            entidad: "ImpresoraEvento",
            entidadId: Arg.Any<string>(),
            datosAnteriores: Arg.Any<object?>(),
            datosNuevos: Arg.Any<object?>(),
            organizacionId: Arg.Any<int?>(),
            franquiciaId: Arg.Any<int?>(),
            restauranteId: impresora.RestauranteId,
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_EventoDuplicado_NoRegistraAuditoria()
    {
        await using var context = await SetupContextoValidoAsync(
            estadoImpresora: EstadoImpresora.Online);

        var agente = await context.Agentes.FirstAsync();
        var impresora = await context.Impresoras.FirstAsync();
        var eventoId = Guid.NewGuid();

        context.ImpresoraEventos.Add(new Domain.Entities.ImpresoraEvento
        {
            Id = Guid.NewGuid(),
            ImpresoraId = impresora.Id,
            EventoId = eventoId,
            TipoEvento = TipoImpresoraEvento.CambioEstado,
            EstadoAnterior = EstadoImpresora.Online,
            EstadoNuevo = EstadoImpresora.Online,
            Activo = true
        });
        await context.SaveChangesAsync();

        _currentUser.AgenteId.Returns(agente.Id);
        _currentUser.EsAgente.Returns(true);

        var handler = new RegistrarEventoImpresoraCommandHandler(context, _currentUser, _auditoria);
        var command = CrearCommandValido(impresoraId: impresora.Id, eventoId: eventoId);

        await handler.Handle(command, CancellationToken.None);

        // No debe registrarse auditoría para un duplicado
        await _auditoria.DidNotReceive().RegistrarAsync(
            Arg.Any<OrigenAuditoria>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<object?>(),
            Arg.Any<object?>(),
            Arg.Any<int?>(),
            Arg.Any<int?>(),
            Arg.Any<int?>(),
            Arg.Any<CancellationToken>());
    }

    // ═══════════════════════════════════════════════════════════════════
    // Helpers
    // ═══════════════════════════════════════════════════════════════════

    private static RegistrarEventoImpresoraCommand CrearCommandValido(
        Guid? impresoraId = null,
        Guid? eventoId = null,
        EstadoImpresora? estadoNuevo = EstadoImpresora.Online,
        TipoImpresoraEvento tipoEvento = TipoImpresoraEvento.CambioEstado,
        string? descripcion = null)
    {
        return new RegistrarEventoImpresoraCommand(
            EventoId: eventoId ?? Guid.NewGuid(),
            ImpresoraId: impresoraId ?? Guid.NewGuid(),
            TipoEvento: tipoEvento,
            EstadoNuevo: estadoNuevo,
            Descripcion: descripcion);
    }

    private static async Task<Infrastructure.Persistence.ApplicationDbContext>
        SetupContextoValidoAsync(EstadoImpresora estadoImpresora)
    {
        var context = InMemoryDbContextFactory.Create();
        var agente = TestHelpers.AgenteActivo();
        var impresora = TestHelpers.ImpresoraActiva(
            agenteId: agente.Id,
            restauranteId: agente.RestauranteId,
            estado: estadoImpresora);

        context.Agentes.Add(agente);
        context.Impresoras.Add(impresora);
        await context.SaveChangesAsync();

        return context;
    }
}