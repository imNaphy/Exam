using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;

namespace RoomHub.Api.Features.Rooms;

public static class RoomsEndpoints
{
    public static void MapRoomsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");

        group.MapPost("", CreateRoom).WithName("CreateRoom");
        group.MapGet("/{id:guid}", GetRoomEndpoint.HandleAsync).WithName("GetRoom");
    }

    private static async Task<Results<Created<CreateRoomResponse>, ProblemHttpResult>> CreateRoom(
        CreateRoomRequest request,
        ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new CreateRoomCommand(request.Name, request.Capacity),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/api/rooms/{result.Value.Id}", result.Value)
            : result.Error.ToProblem();
    }
}
