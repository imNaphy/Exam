using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain;

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

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

        if (range.Duration > MaxDuration)
            return Error.Validation("DurationExceedsMaximum", "A booking cannot exceed 4 hours.");

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

    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }
}
