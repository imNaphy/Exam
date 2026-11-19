using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public static class GetRoomEndpoint
{
    public static async Task<Results<Ok<GetRoomResponse>, ProblemHttpResult>> HandleAsync(
        Guid id,
        RoomHubDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new GetRoomResponse(r.Id, r.Name, r.Capacity))
            .FirstOrDefaultAsync(cancellationToken);

        return room is not null
            ? TypedResults.Ok(room)
            : Error.NotFound("RoomNotFound", "The room does not exist.").ToProblem();
    }
}
