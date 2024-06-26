﻿using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Model;
using RoomBookingApp.Core.Processors;
using Shouldly;

namespace RoomBookingApp.Core;

public class RoomBookingRequestProcessorTest
{
    private RoomBookingRequestProcessor _processor;
    private RoomBookingRequest _bookingRequest;
    private Mock<IRoomBookingService> _roomBookingServiceMock;
    private List<Room> _availableRooms;
    public RoomBookingRequestProcessorTest()
    {
        _bookingRequest = new RoomBookingRequest
        {
            FullName = "Test Name",
            Email = " test@gmail.com",
            Date = new DateTime(2024, 6, 20)
        };
        _availableRooms = new List<Room>() { new Room
        {
            Id = 1
        }};

        _roomBookingServiceMock = new Mock<IRoomBookingService>();
        _roomBookingServiceMock.Setup(r => r.GetAvailableRooms(_bookingRequest.Date))
            .Returns(_availableRooms);
        _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);


    }
    [Fact]
    public void Should_Return_Room_Booking_Response_With_Request_Values()
    {
        // Arrange



        // Act

        RoomBookingResult result = _processor.BookRoom(_bookingRequest);

        // Assert 
        Assert.NotNull(result);
        Assert.Equal(_bookingRequest.FullName, result.FullName);
        Assert.Equal(_bookingRequest.Email, result.Email);
        Assert.Equal(_bookingRequest.Date, result.Date);

        result.ShouldNotBeNull();
        result.FullName.ShouldBe(_bookingRequest.FullName);
        result.Email.ShouldBe(_bookingRequest.Email);
        result.Date.ShouldBe(_bookingRequest.Date);

    }

    [Fact]
    public void Should_Throw_Exception_For_Null_Request()
    {


        var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
        exception.ParamName.ShouldBe("bookingRequest");
    }

    [Fact]
    public void Should_Save_Room_Booking_Request()
    {
        RoomBooking savedBooking = null;
        _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>(booking =>
            {
                savedBooking = booking;
            });
        _processor.BookRoom(_bookingRequest);
        _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Once);
        savedBooking.ShouldNotBeNull();
        savedBooking.FullName.ShouldBe(_bookingRequest.FullName);
        savedBooking.Email.ShouldBe(_bookingRequest.Email);
        savedBooking.Date.ShouldBe(_bookingRequest.Date);
        savedBooking.RoomId.ShouldBe(_availableRooms.First().Id);
    }

    [Fact]
    public void Should_Not_Save_Room_Booking_Request_If_None_Available()
    {
        _availableRooms.Clear();
        _processor.BookRoom(_bookingRequest);
        _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
    }

    [Theory]
    [InlineData(BookingResultFlag.Failure, false)]
    [InlineData(BookingResultFlag.Success, true)]
    public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }
        var result = _processor.BookRoom(_bookingRequest);
        bookingSuccessFlag.ShouldBe(result.Flag);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(null, false)]
    public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }
        else
        {
            _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
           .Callback<RoomBooking>(booking =>
           {
               booking.Id = roomBookingId.Value;
           });
        }
        var result = _processor.BookRoom(_bookingRequest);
        result.RoomBookingId.ShouldBe(roomBookingId);
    }

}
