namespace MonitorIMP.Application.Common.Exceptions;

public sealed class UniqueConstraintViolationException : Exception
{
    public string? ConstraintName { get; }

    public UniqueConstraintViolationException(string? constraintName = null)
        : base("Violación de restricción única.")
    {
        ConstraintName = constraintName;
    }
}