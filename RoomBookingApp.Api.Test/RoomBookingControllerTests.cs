using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Model;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Api.Test
{
    public class RoomBookingControllerTests
    {
        private Mock<IRoomBookingRequestProcessor> _roomProcessor;
        private RoomBookingController _controller;
        private RoomBookingRequest _request;
        private RoomBookingResult _result;

        public RoomBookingControllerTests()
        {
            _roomProcessor = new Mock<IRoomBookingRequestProcessor>();
            _controller = new RoomBookingController(_roomProcessor.Object);
            _request = new RoomBookingRequest();
            _result = new RoomBookingResult();
            _roomProcessor.Setup(x => x.BookRoom(_request)).Returns(_result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
        public async Task Should_Call_Booking_Method_When_Valid(int expectedMethodCalls, bool isModelValid,
            Type expectedActionResultsType, BookingResultFlag bookingResultFlag)
        {
            // Arrange
            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Key", "ErrorMessage");
            }

            _result.Flag = bookingResultFlag;

            //Act
            var result = await _controller.BookRoom(_request);


            // Assert 

            result.ShouldBeOfType(expectedActionResultsType);
            _roomProcessor.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCalls));
        }
    }
}
