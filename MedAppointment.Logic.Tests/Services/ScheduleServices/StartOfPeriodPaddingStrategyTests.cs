namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class StartOfPeriodPaddingStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.StartOfPeriodPaddingStrategy";

    private readonly ITimeSlotPaddingStrategy _sut;

    public StartOfPeriodPaddingStrategyTests()
    {
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StrategyTypeName);
    }

    [Fact]
    public void Position_ReturnsStartOfPeriod()
    {
        Assert.Equal(PlanPaddingPosition.StartOfPeriod, _sut.Position);
    }

    [Fact]
    public void Compute_CutsFromStartOfPeriod()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromMinutes(665), slotStart);
        Assert.Equal(TimeSpan.FromHours(11.5), slotEnd);
        Assert.Equal(TimeSpan.FromHours(11.5), nextStart);
    }

    [Fact]
    public void Compute_SlotStartIsDelayedByPadding()
    {
        var currentStart = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromMinutes(550), slotStart);
        Assert.Equal(TimeSpan.FromHours(9.5), slotEnd);
    }

    [Fact]
    public void Compute_WithZeroPadding_NoDelayInStart()
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
    public void Compute_NextStartEqualsSlotEnd()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(45);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(slotEnd, nextStart);
    }

    [Fact]
    public void Compute_EffectiveSlotDurationIsReduced()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        var effectiveDuration = slotEnd - slotStart;
        Assert.Equal(TimeSpan.FromMinutes(25), effectiveDuration);
    }

    [Fact]
    public void Compute_WithLargePadding_ReturnsCorrectTimes()
    {
        var currentStart = TimeSpan.FromHours(8);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 15;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromMinutes(495), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(510), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(510), nextStart);
    }
}
