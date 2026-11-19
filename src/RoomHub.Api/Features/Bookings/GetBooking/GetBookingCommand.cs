using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingCommand(Guid Id) : ICommand<Result<GetBookingResponse>>;
