using MonitorIMP.Application.Abstractions;

namespace MonitorIMP.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}