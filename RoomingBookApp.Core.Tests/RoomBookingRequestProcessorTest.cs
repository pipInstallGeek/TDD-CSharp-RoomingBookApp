using RoomBookingApp.Core.Model;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core;

public class RoomBookingRequestProcessorTest
{
    [Fact] 
    public void ShouldReturnRoomBookingResponseWithRequestValues()
    {
        // Arrange
        var bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = " test@gmail.com",
            Date = new DateTime(2024, 6, 20)
        };
        var processor = new RoomBookingRequestProcessor();

        // Act
        RoomBookingResult result = processor.BookRoom(bookingRequest);

        // Assert 

        Assert.NotNull(result);
        Assert.Equal(bookingRequest.FullName, result.FullName);
        Assert.Equal(bookingRequest.Email, result.Email);
        Assert.Equal(bookingRequest.Date, result.Date);

        result.ShouldNotBeNull();
        result.FullName.ShouldBe(bookingRequest.FullName);
        result.Email.ShouldBe(bookingRequest.Email);
        result.Date.ShouldBe(bookingRequest.Date);

    }


}
