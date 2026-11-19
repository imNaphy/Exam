namespace RoomHub.Api.Domain;

public interface IRoomRepository
{
    void Add(Room room);
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
