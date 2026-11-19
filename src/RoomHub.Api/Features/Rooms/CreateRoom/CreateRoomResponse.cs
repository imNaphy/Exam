namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);
