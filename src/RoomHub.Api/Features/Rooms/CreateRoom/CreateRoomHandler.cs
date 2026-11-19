using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository roomRepository)
    : ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public async Task<Result<CreateRoomResponse>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (roomResult.IsFailure)
            return roomResult.Error;

        var room = roomResult.Value;
        await roomRepository.AddAsync(room, cancellationToken);

        return new CreateRoomResponse(room.Id, room.Name, room.Capacity);
    }
}
