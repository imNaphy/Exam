using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain;

public sealed record DateRange
{
    public DateTime StartUtc { get; private init; }
    public DateTime EndUtc { get; private init; }

    private DateRange() { }

    private DateRange(DateTime startUtc, DateTime endUtc)
    {
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public TimeSpan Duration => EndUtc - StartUtc;

    public static Result<DateRange> Create(DateTime start, DateTime end)
    {
        var startUtc = NormalizeToUtc(start);
        var endUtc = NormalizeToUtc(end);

        if (endUtc <= startUtc)
            return Error.Validation("InvalidRange", "The end must be strictly after the start.");

        return new DateRange(startUtc, endUtc);
    }

    public bool Overlaps(DateRange other) =>
        StartUtc < other.EndUtc && other.StartUtc < EndUtc;

    private static DateTime NormalizeToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
        _ => value.ToUniversalTime()
    };
}
