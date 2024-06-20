
namespace RoomBookingApp.Persistence.Repositories;

public class RoomBookingService : IRoomBookingService
{
    private readonly RoomBookingAppDbContext _context;
    public RoomBookingService(RoomBookingAppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Room> GetAvailableRooms(DateTime dateTime)
    {
        var unavailableRooms = _context.RoomBookings.Where(q => q.Date == dateTime)
            .Select(q => q.RoomId)
            .ToList();
        var availableRooms = _context.Rooms.Where(q => !unavailableRooms.Contains(q.Id)).ToList();
        return availableRooms;
    }


    public void Save(RoomBooking bookingRoom)
    {
        _context.Add(bookingRoom);
        _context.SaveChanges();
    }
}
