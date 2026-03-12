namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class EndOfPeriodPaddingStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.EndOfPeriodPaddingStrategy";

    private readonly ITimeSlotPaddingStrategy _sut;

    public EndOfPeriodPaddingStrategyTests()
    {
        _sut = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StrategyTypeName);
    }

    [Fact]
    public void Position_ReturnsEndOfPeriod()
    {
        Assert.Equal(PlanPaddingPosition.EndOfPeriod, _sut.Position);
    }

    [Fact]
    public void Compute_CutsFromEndOfPeriod()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(11), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(685), slotEnd);
        Assert.Equal(TimeSpan.FromHours(11.5), nextStart);
    }

    [Fact]
    public void Compute_SlotEndIsShortenedByPadding()
    {
        var currentStart = TimeSpan.FromHours(9);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 10;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(TimeSpan.FromHours(9), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(560), slotEnd);
        Assert.Equal(TimeSpan.FromHours(9.5), nextStart);
    }

    [Fact]
    public void Compute_WithZeroPadding_NoReductionInEnd()
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
    public void Compute_NextStartIsFullPeriod()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(45);
        byte paddingTime = 5;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(currentStart + periodDuration, nextStart);
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

        Assert.Equal(TimeSpan.FromHours(8), slotStart);
        Assert.Equal(TimeSpan.FromMinutes(495), slotEnd);
        Assert.Equal(TimeSpan.FromMinutes(510), nextStart);
    }

    [Fact]
    public void Compute_SlotStartRemainsUnchanged()
    {
        var currentStart = TimeSpan.FromHours(13);
        var periodDuration = TimeSpan.FromMinutes(20);
        byte paddingTime = 3;

        var (slotStart, slotEnd, nextStart) = _sut.Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(currentStart, slotStart);
    }
}
