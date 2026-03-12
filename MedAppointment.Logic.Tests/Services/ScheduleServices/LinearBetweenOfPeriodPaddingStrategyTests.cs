namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class LinearBetweenOfPeriodPaddingStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.LinearBetweenOfPeriodPaddingStrategy";

    private readonly ITimeSlotPaddingStrategy _sut;

    public LinearBetweenOfPeriodPaddingStrategyTests()
    {
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StrategyTypeName);
    }

    [Fact]
    public void Position_ReturnsLinearBetweenOfPeriod()
    {
        Assert.Equal(PlanPaddingPosition.LinearBetweenOfPeriod, _sut.Position);
    }

    [Fact]
    public void Compute_AddsFullPaddingBetweenPeriods()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(11), slotStart);
        Assert.Equal(TimeSpan.FromHours(11.5), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(695), nextStart);
    }

    [Fact]
    public void Compute_NextStartIsDelayedByPadding()
    {
        var currentStart = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(9), slotStart);
        Assert.Equal(TimeSpan.FromHours(9.5), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(580), nextStart);
    }

    [Fact]
    public void Compute_WithZeroPadding_NoGapBetweenPeriods()
    {
        var currentStart = TimeSpan.FromHours(14);
        var periodDuration = TimeSpan.FromMinutes(60);
        byte paddingTime = 0;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(currentStart, slotStart);
        Assert.Equal(TimeSpan.FromHours(15), slotEnd);
        Assert.Equal(TimeSpan.FromHours(15), nextStart);
    }

    [Fact]
    public void Compute_SlotUsesFullPeriod()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(45);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var slotDuration = slotEnd - slotStart;
        Assert.Equal(periodDuration, slotDuration);
    }

    [Fact]
    public void Compute_GapBetweenSlotEndAndNextStart()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var gap = nextStart - slotEnd;
        Assert.Equal(TimeSpan.FromMinutes(5), gap);
    }

    [Fact]
    public void Compute_WithLargePadding_ReturnsCorrectTimes()
    {
        var currentStart = TimeSpan.FromHours(8);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 15;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(8), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(510), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(525), nextStart);
    }

    [Fact]
    public void Compute_Example_FirstSlot11To1130_SecondSlot1135To1205()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(new TimeSpan(11, 0, 0), slotStart);
        Assert.Equal(new TimeSpan(11, 30, 0), slotEnd);
        Assert.Equal(new TimeSpan(11, 35, 0), nextStart);
    }
}
