using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Model;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api;


[ApiController]
[Route("[controller]")]
public class RoomBookingController : ControllerBase
{
    private IRoomBookingRequestProcessor _roomBookingProcessor;

    public RoomBookingController(IRoomBookingRequestProcessor roomBookingProcessor)
    {
        this._roomBookingProcessor = roomBookingProcessor;  
    }

    [HttpPost]
    public async Task<IActionResult> BookRoom(RoomBookingRequest request)
    {
        if (ModelState.IsValid)
        {
            var result = _roomBookingProcessor.BookRoom(request);
            if (result.Flag == Core.Enums.BookingResultFlag.Success)
            {
                return Ok(result);
            }
            ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No Rooms Available");
        }
        return BadRequest(ModelState);
    }
}
