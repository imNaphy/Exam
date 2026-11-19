using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository roomRepository)
    : ICommandHandler<GetRoomCommand, Result<GetRoomResponse>>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(
        GetRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(command.Id, cancellationToken);
        if (room is null)
            return Error.NotFound("RoomNotFound", "The room does not exist.");

        return new GetRoomResponse(room.Id, room.Name, room.Capacity);
    }
}
