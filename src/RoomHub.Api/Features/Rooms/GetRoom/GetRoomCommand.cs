using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomCommand(Guid Id) : ICommand<Result<GetRoomResponse>>;
