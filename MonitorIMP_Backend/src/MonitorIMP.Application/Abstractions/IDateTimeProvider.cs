namespace MonitorIMP.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}