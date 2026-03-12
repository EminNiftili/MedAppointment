namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class NoPaddingStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.NoPaddingStrategy";

    private readonly ITimeSlotPaddingStrategy _sut;

    public NoPaddingStrategyTests()
    {
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StrategyTypeName);
    }

    [Fact]
    public void Position_ReturnsNoPadding()
    {
        Assert.Equal(PlanPaddingPosition.NoPadding, _sut.Position);
    }

    [Fact]
    public void Compute_WithStandardPeriod_ReturnsFullPeriod()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(11), slotStart);
        Assert.Equal(TimeSpan.FromHours(11.5), slotEnd);
        Assert.Equal(TimeSpan.FromHours(11.5), nextStart);
    }

    [Fact]
    public void Compute_IgnoresPaddingTime()
    {
        var currentStart = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(15);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(9), slotStart);
        Assert.Equal(TimeSpan.FromHours(9.25), slotEnd);
        Assert.Equal(TimeSpan.FromHours(9.25), nextStart);
    }

    [Fact]
    public void Compute_WithZeroPadding_ReturnsFullPeriod()
    {
        var currentStart = TimeSpan.FromHours(14);
        var periodDuration = TimeSpan.FromMinutes(60);
        byte paddingTime = 0;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(14), slotStart);
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
    public void Compute_WithSmallPeriod_ReturnsCorrectTimes()
    {
        var currentStart = TimeSpan.FromHours(8);
        var periodDuration = TimeSpan.FromMinutes(5);
        byte paddingTime = 2;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(8), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(485), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(485), nextStart);
    }
}
