namespace RoomHub.Api.Domain;

public interface IBookingRepository
{
    void Add(Booking booking);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> ListActiveByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<int> CountActiveForUserOnUtcDayAsync(string bookedBy, DateTime utcDate, CancellationToken cancellationToken = default);
}
