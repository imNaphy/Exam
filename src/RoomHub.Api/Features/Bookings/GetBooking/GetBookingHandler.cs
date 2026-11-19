using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookingRepository)
    : ICommandHandler<GetBookingCommand, Result<GetBookingResponse>>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(
        GetBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null)
            return Error.NotFound("BookingNotFound", "The booking does not exist.");

        return new GetBookingResponse(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.Range.StartUtc,
            booking.Range.EndUtc,
            booking.Attendees,
            booking.Status);
    }
}
