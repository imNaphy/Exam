using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;

namespace RoomHub.Api.Features.Bookings;

public static class BookingsEndpoints
{
    public static void MapBookingsEndpoints(this IEndpointRouteBuilder app)
    {
        var roomBookings = app.MapGroup("/api/rooms/{roomId:guid}/bookings").WithTags("Bookings");
        roomBookings.MapPost("", CreateBooking).WithName("CreateBooking");

        var bookings = app.MapGroup("/api/bookings").WithTags("Bookings");
        bookings.MapGet("/{id:guid}", GetBookingEndpoint.HandleAsync).WithName("GetBooking");
        bookings.MapPost("/{id:guid}/cancel", CancelBooking).WithName("CancelBooking");
    }

    private static async Task<Results<Created<CreateBookingResponse>, ProblemHttpResult>> CreateBooking(
        Guid roomId,
        CreateBookingRequest request,
        ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new CreateBookingCommand(roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/api/bookings/{result.Value.Id}", result.Value)
            : result.Error.ToProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> CancelBooking(
        Guid id,
        ICommandHandler<CancelBookingCommand, Result<Unit>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CancelBookingCommand(id), cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblem();
    }
}
