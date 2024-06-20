using RoomBookingApp.Core.Model;

namespace RoomBookingApp.Core.Processors
{
    public interface IRoomBookingRequestProcessor
    {
        RoomBookingResult BookRoom(RoomBookingRequest bookingRequest);
    }
}