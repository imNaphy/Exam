using System.Collections.Concurrent;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure;

public sealed class InMemoryRoomRepository : IRoomRepository
{
    private readonly ConcurrentDictionary<Guid, Room> _rooms = new();

    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        _rooms[room.Id] = room;
        return Task.CompletedTask;
    }

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _rooms.TryGetValue(id, out var room);
        return Task.FromResult(room);
    }
}
