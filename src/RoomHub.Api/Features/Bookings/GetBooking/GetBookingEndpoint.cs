using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public static async Task<Results<Ok<GetBookingResponse>, ProblemHttpResult>> HandleAsync(
        Guid id,
        RoomHubDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new GetBookingResponse(
                b.Id,
                b.RoomId,
                b.BookedBy,
                b.Range.StartUtc,
                b.Range.EndUtc,
                b.Attendees,
                b.Status))
            .FirstOrDefaultAsync(cancellationToken);

        return booking is not null
            ? TypedResults.Ok(booking)
            : Error.NotFound("BookingNotFound", "The booking does not exist.").ToProblem();
    }
}
