namespace RoomHub.Api.Domain;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> ListByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default);
}
