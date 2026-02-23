namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class TimeSlotServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.TimeSlotService";

    private readonly ILogger _logger;
    private readonly ITimeSlotPaddingStrategyResolver _paddingStrategyResolver;
    private readonly ITimeSlotPaddingStrategy _paddingStrategy;
    private readonly ITimeSlotService _sut;

    public TimeSlotServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _paddingStrategyResolver = Substitute.For<ITimeSlotPaddingStrategyResolver>();
        _paddingStrategy = Substitute.For<ITimeSlotPaddingStrategy>();
        _paddingStrategy.Position.Returns(PlanPaddingPosition.NoPadding);
        _paddingStrategyResolver.GetStrategy(Arg.Any<PlanPaddingPosition>()).Returns(_paddingStrategy);
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotService>(ServiceTypeName, _logger, _paddingStrategyResolver);
    }

    [Fact]
    public void GenerateDaySlots_WhenBreaksOverlap_ReturnsBadRequest()
    {
        var breaks = new List<(TimeSpan Start, TimeSpan End)>
        {
            (TimeSpan.FromHours(10), TimeSpan.FromHours(11)),
            (TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(30)), TimeSpan.FromHours(12)) // overlaps first
        };

        var result = _sut.GenerateDaySlots(
            TimeSpan.FromHours(9),
            30,
            0,
            PlanPaddingPosition.NoPadding,
            2,
            breaks);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00128");
        _paddingStrategyResolver.DidNotReceive().GetStrategy(Arg.Any<PlanPaddingPosition>());
    }

    [Fact]
    public void GenerateDaySlots_WhenPaddingPositionInvalid_ReturnsBadRequest()
    {
        var invalidPosition = (PlanPaddingPosition)99;
        if (Enum.IsDefined(typeof(PlanPaddingPosition), invalidPosition))
            invalidPosition = (PlanPaddingPosition)255;

        var result = _sut.GenerateDaySlots(
            TimeSpan.FromHours(9),
            30,
            null,
            invalidPosition,
            2,
            new List<(TimeSpan Start, TimeSpan End)>());

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00129");
    }

    [Fact]
    public void GenerateDaySlots_WhenValidNoBreaks_ReturnsSlots()
    {
        var openTime = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(30);
        _paddingStrategy.Compute(Arg.Any<TimeSpan>(), Arg.Any<TimeSpan>(), Arg.Any<byte>())
            .Returns(callInfo =>
            {
                var current = (TimeSpan)callInfo[0];
                var duration = (TimeSpan)callInfo[1];
                var end = current + duration;
                return (current, end, end);
            });

        var result = _sut.GenerateDaySlots(
            openTime,
            30,
            null,
            PlanPaddingPosition.NoPadding,
            2,
            new List<(TimeSpan Start, TimeSpan End)>());

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(2, result.Model!.Count);
        Assert.Equal(openTime, result.Model[0].Start);
        Assert.Equal(openTime.Add(TimeSpan.FromMinutes(30)), result.Model[0].End);
        Assert.Equal(openTime.Add(TimeSpan.FromMinutes(30)), result.Model[1].Start);
        Assert.Equal(openTime.Add(TimeSpan.FromMinutes(60)), result.Model[1].End);
    }

    [Fact]
    public void GenerateDaySlots_WhenPeriodCountZero_ReturnsEmptyList()
    {
        _paddingStrategy.Compute(Arg.Any<TimeSpan>(), Arg.Any<TimeSpan>(), Arg.Any<byte>())
            .Returns(callInfo =>
            {
                var current = (TimeSpan)callInfo[0];
                var duration = (TimeSpan)callInfo[1];
                var end = current + duration;
                return (current, end, end);
            });

        var result = _sut.GenerateDaySlots(
            TimeSpan.FromHours(9),
            30,
            null,
            PlanPaddingPosition.NoPadding,
            0,
            new List<(TimeSpan Start, TimeSpan End)>());

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Empty(result.Model!);
    }

    [Fact]
    public void GenerateDaySlots_WhenBreaksNull_TreatsAsEmptyBreaks()
    {
        _paddingStrategy.Compute(Arg.Any<TimeSpan>(), Arg.Any<TimeSpan>(), Arg.Any<byte>())
            .Returns(callInfo =>
            {
                var current = (TimeSpan)callInfo[0];
                var duration = (TimeSpan)callInfo[1];
                var end = current + duration;
                return (current, end, end);
            });

        var result = _sut.GenerateDaySlots(
            TimeSpan.FromHours(9),
            30,
            null,
            PlanPaddingPosition.NoPadding,
            1,
            null!);

        Assert.True(result.IsSuccess());
        Assert.Single(result.Model!);
    }
}
