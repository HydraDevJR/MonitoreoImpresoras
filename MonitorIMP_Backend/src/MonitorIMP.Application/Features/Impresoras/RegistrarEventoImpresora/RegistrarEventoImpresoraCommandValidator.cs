using FluentValidation;

namespace MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

public sealed class RegistrarEventoImpresoraCommandValidator
    : AbstractValidator<RegistrarEventoImpresoraCommand>
{
    public RegistrarEventoImpresoraCommandValidator()
    {
        RuleFor(x => x.EventoId)
            .NotEmpty()
            .WithMessage("El identificador del evento es obligatorio.");

        RuleFor(x => x.ImpresoraId)
            .NotEmpty()
            .WithMessage("El identificador de la impresora es obligatorio.");

        RuleFor(x => x.TipoEvento)
            .IsInEnum()
            .WithMessage("El tipo de evento no es válido.");

        RuleFor(x => x.EstadoNuevo)
            .IsInEnum()
            .When(x => x.EstadoNuevo.HasValue)
            .WithMessage("El estado nuevo no es válido.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Descripcion))
            .WithMessage("La descripción no puede exceder 500 caracteres.");
    }
}