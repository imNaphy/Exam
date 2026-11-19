using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain;

public sealed class Booking
{
    public const int LargeRoomCapacityThreshold = 10;
    public const int MaxActiveBookingsPerDayPerUser = 2;

    public static readonly TimeSpan StandardMaxDuration = TimeSpan.FromHours(4);
    public static readonly TimeSpan LargeRoomMaxDuration = TimeSpan.FromHours(2);
    public static readonly TimeSpan MinCancellationLeadTime = TimeSpan.FromMinutes(60);

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateRange Range { get; private set; } = null!;
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking() { }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Error.Validation("InvalidBookedBy", "BookedBy is required.");

        var rangeResult = DateRange.Create(startUtc, endUtc);
        if (rangeResult.IsFailure)
            return rangeResult.Error;

        var range = rangeResult.Value;

        if (range.StartUtc < nowUtc)
            return Error.Validation("BookingInPast", "A booking cannot start in the past.");

        var maxDuration = room.Capacity > LargeRoomCapacityThreshold
            ? LargeRoomMaxDuration
            : StandardMaxDuration;

        if (range.Duration > maxDuration)
            return Error.Validation(
                "DurationExceedsMaximum",
                $"A booking cannot exceed {(int)maxDuration.TotalHours} hours for this room.");

        if (attendees <= 0)
            return Error.Validation("InvalidAttendees", "Attendees must be positive.");

        if (attendees > room.Capacity)
            return Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            Range = range,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    public bool Overlaps(DateRange other) =>
        Status == BookingStatus.Active && Range.Overlaps(other);

    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
            return Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

        if (Range.StartUtc - nowUtc < MinCancellationLeadTime)
            return Error.Conflict(
                "CancellationTooLate",
                "A booking can only be cancelled at least 60 minutes before it starts.");

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }
}
