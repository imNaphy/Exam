using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext dbContext) : IBookingRepository
{
    public void Add(Booking booking) => dbContext.Bookings.Add(booking);

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListActiveByRoomIdAsync(
        Guid roomId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Bookings
            .AsNoTracking()
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public Task<int> CountActiveForUserOnUtcDayAsync(
        string bookedBy,
        DateTime utcDate,
        CancellationToken cancellationToken = default)
    {
        var dayStart = DateTime.SpecifyKind(utcDate.Date, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        return dbContext.Bookings.CountAsync(
            b => b.Status == BookingStatus.Active
                 && b.BookedBy == bookedBy
                 && b.Range.StartUtc >= dayStart
                 && b.Range.StartUtc < dayEnd,
            cancellationToken);
    }
}
