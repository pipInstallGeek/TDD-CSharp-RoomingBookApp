using RoomBookingApp.Core.Enums;

namespace RoomBookingApp.Core.Model
{
    public class RoomBookingResult : RoomBookingBase
    {
        public BookingResultFlag Flag { get; set; }
        public int? RoomBookingId { get; internal set; }

        
    }
}