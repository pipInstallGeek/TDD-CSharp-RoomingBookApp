namespace RoomBookingApp.Core.DataServices;

public interface IRoomBookingService
{
    void Save(RoomBooking bookingRoom);

    IEnumerable<Room> GetAvailableRooms(DateTime dateTime);
}
