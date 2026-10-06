using FluentAssertions;
using MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;
using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Application.Tests.Features.Impresoras.RegistrarEventoImpresora;

public sealed class RegistrarEventoImpresoraCommandValidatorTests
{
    private readonly RegistrarEventoImpresoraCommandValidator _validator = new();

    [Fact]
    public void Validator_ConComandoValido_NoDebeTenerErrores()
    {
        // Arrange
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: EstadoImpresora.Online,
            Descripcion: "Evento de prueba");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validator_ConEventoIdVacio_DebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.Empty,
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: EstadoImpresora.Online,
            Descripcion: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.EventoId));
    }

    [Fact]
    public void Validator_ConImpresoraIdVacio_DebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.Empty,
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: EstadoImpresora.Online,
            Descripcion: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.ImpresoraId));
    }

    [Fact]
    public void Validator_ConTipoEventoFueraDelEnum_DebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: (TipoImpresoraEvento)999,
            EstadoNuevo: EstadoImpresora.Online,
            Descripcion: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.TipoEvento));
    }

    [Fact]
    public void Validator_ConEstadoNuevoFueraDelEnum_DebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: (EstadoImpresora)999,
            Descripcion: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.EstadoNuevo));
    }

    [Fact]
    public void Validator_ConDescripcionMayorA500_DebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: EstadoImpresora.Online,
            Descripcion: new string('a', 501));

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(command.Descripcion));
    }

    [Fact]
    public void Validator_ConEstadoNuevoNull_NoDebeFallar()
    {
        var command = new RegistrarEventoImpresoraCommand(
            EventoId: Guid.NewGuid(),
            ImpresoraId: Guid.NewGuid(),
            TipoEvento: TipoImpresoraEvento.CambioEstado,
            EstadoNuevo: null,
            Descripcion: null);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}