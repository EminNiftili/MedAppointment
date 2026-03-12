namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class CenterBetweenOfPeriodPaddingStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.CenterBetweenOfPeriodPaddingStrategy";

    private readonly ITimeSlotPaddingStrategy _sut;

    public CenterBetweenOfPeriodPaddingStrategyTests()
    {
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StrategyTypeName);
    }

    [Fact]
    public void Position_ReturnsCenterBetweenOfPeriod()
    {
        Assert.Equal(PlanPaddingPosition.CenterBetweenOfPeriod, _sut.Position);
    }

    [Fact]
    public void Compute_SplitsGapBetweenTwoPeriods()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 4;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(11), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(688), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(692), nextStart);
    }

    [Fact]
    public void Compute_HalfPaddingFromEndHalfFromStart()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 6;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var slotDuration = slotEnd - slotStart;
        var gap = nextStart - slotEnd;
        
        Assert.Equal(TimeSpan.FromMinutes(27), slotDuration);
        Assert.Equal(TimeSpan.FromMinutes(6), gap);
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
    public void Compute_TotalGapEqualsPadding()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var totalGap = nextStart - slotEnd;
        Assert.Equal(TimeSpan.FromMinutes(10), totalGap);
    }

    [Fact]
    public void Compute_WithOddPadding_HandlesHalfMinutesCorrectly()
    {
        var currentStart = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var halfPadding = 5 / 2.0;
        var expectedSlotEnd = currentStart + periodDuration - TimeSpan.FromMinutes(halfPadding);
        var expectedNextStart = expectedSlotEnd + TimeSpan.FromMinutes(5);

        Assert.Equal(expectedSlotEnd, slotEnd);
        Assert.Equal(expectedNextStart, nextStart);
    }

    [Fact]
    public void Compute_WithLargePadding_ReturnsCorrectTimes()
    {
        var currentStart = TimeSpan.FromHours(8);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(8), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(505), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(515), nextStart);
    }

    [Fact]
    public void Compute_Example_FirstSlot11To1128_SecondSlot1132To12()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 4;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(new TimeSpan(11, 0, 0), slotStart);
        Assert.Equal(new TimeSpan(11, 28, 0), slotEnd);
        Assert.Equal(new TimeSpan(11, 32, 0), nextStart);
    }

    [Fact]
    public void Compute_SlotStartRemainsUnchanged()
    {
        var currentStart = TimeSpan.FromHours(13);
        var periodDuration = TimeSpan.FromMinutes(20);
        byte paddingTime = 4;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(currentStart, slotStart);
    }
}
