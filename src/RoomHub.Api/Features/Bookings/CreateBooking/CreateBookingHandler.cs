using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return Error.NotFound("RoomNotFound", "The room does not exist.");

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        var bookingResult = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            nowUtc);

        if (bookingResult.IsFailure)
            return bookingResult.Error;

        var booking = bookingResult.Value;

        var activeBookings = await bookingRepository.ListActiveByRoomIdAsync(room.Id, cancellationToken);
        if (activeBookings.Any(existing => existing.Overlaps(booking.Range)))
            return Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

        var activeCountToday = await bookingRepository.CountActiveForUserOnUtcDayAsync(
            booking.BookedBy,
            booking.Range.StartUtc,
            cancellationToken);

        if (activeCountToday >= Booking.MaxActiveBookingsPerDayPerUser)
            return Error.Conflict(
                "DailyBookingLimitExceeded",
                $"A participant cannot have more than {Booking.MaxActiveBookingsPerDayPerUser} active bookings on the same UTC day.");

        bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status);
    }
}
