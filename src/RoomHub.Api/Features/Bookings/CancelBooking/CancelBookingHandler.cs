using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(IBookingRepository bookingRepository)
    : ICommandHandler<CancelBookingCommand, Result<Unit>>
{
    public async Task<Result<Unit>> HandleAsync(
        CancelBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null)
            return Error.NotFound("BookingNotFound", "The booking does not exist.");

        return booking.Cancel();
    }
}
