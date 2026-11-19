using System.Collections.Concurrent;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure;

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _bookings[booking.Id] = booking;
        return Task.CompletedTask;
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _bookings.TryGetValue(id, out var booking);
        return Task.FromResult(booking);
    }

    public Task<IReadOnlyList<Booking>> ListByRoomIdAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Booking> result = _bookings.Values
            .Where(b => b.RoomId == roomId)
            .ToList();

        return Task.FromResult(result);
    }
}
