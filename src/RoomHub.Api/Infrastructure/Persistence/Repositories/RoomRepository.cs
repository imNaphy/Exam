using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class RoomRepository(RoomHubDbContext dbContext) : IRoomRepository
{
    public void Add(Room room) => dbContext.Rooms.Add(room);

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
}
