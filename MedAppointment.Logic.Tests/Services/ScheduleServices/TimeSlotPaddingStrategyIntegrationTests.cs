namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

/// <summary>
/// Integration tests for TimeSlotPaddingStrategyResolver with all strategies working together.
/// </summary>
public class TimeSlotPaddingStrategyIntegrationTests
{
    private const string ResolverTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.TimeSlotPaddingStrategyResolver";
    private const string NoPaddingStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.NoPaddingStrategy";
    private const string StartOfPeriodStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.StartOfPeriodPaddingStrategy";
    private const string EndOfPeriodStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.EndOfPeriodPaddingStrategy";
    private const string LinearBetweenStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.LinearBetweenOfPeriodPaddingStrategy";
    private const string CenterBetweenStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.CenterBetweenOfPeriodPaddingStrategy";

    private readonly ITimeSlotPaddingStrategyResolver _resolver;

    public TimeSlotPaddingStrategyIntegrationTests()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = new[]
        {
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(NoPaddingStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StartOfPeriodStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(EndOfPeriodStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(LinearBetweenStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(CenterBetweenStrategyTypeName)
        };

        _resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);
    }

    [Fact]
    public void EndToEnd_GenerateTimeSlots_NoPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(17);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;
        var position = PlanPaddingPosition.NoPadding;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(16, slots.Count);
        Assert.Equal(new TimeSpan(9, 0, 0), slots[0].Start);
        Assert.Equal(new TimeSpan(9, 30, 0), slots[0].End);
        Assert.Equal(new TimeSpan(16, 30, 0), slots[^1].Start);
        Assert.Equal(new TimeSpan(17, 0, 0), slots[^1].End);
    }

    [Fact]
    public void EndToEnd_GenerateTimeSlots_StartOfPeriodPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(12);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;
        var position = PlanPaddingPosition.StartOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(6, slots.Count);
        Assert.Equal(new TimeSpan(9, 5, 0), slots[0].Start);
        Assert.Equal(new TimeSpan(9, 30, 0), slots[0].End);
        Assert.Equal(new TimeSpan(11, 35, 0), slots[^1].Start);
        Assert.Equal(new TimeSpan(12, 0, 0), slots[^1].End);
    }

    [Fact]
    public void EndToEnd_GenerateTimeSlots_EndOfPeriodPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(12);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;
        var position = PlanPaddingPosition.EndOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(6, slots.Count);
        Assert.Equal(new TimeSpan(9, 0, 0), slots[0].Start);
        Assert.Equal(new TimeSpan(9, 25, 0), slots[0].End);
        Assert.Equal(new TimeSpan(11, 30, 0), slots[^1].Start);
        Assert.Equal(new TimeSpan(11, 55, 0), slots[^1].End);
    }

    [Fact]
    public void EndToEnd_GenerateTimeSlots_LinearBetweenPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(12);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;
        var position = PlanPaddingPosition.LinearBetweenOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(5, slots.Count);
        Assert.Equal(new TimeSpan(9, 0, 0), slots[0].Start);
        Assert.Equal(new TimeSpan(9, 30, 0), slots[0].End);
        Assert.Equal(new TimeSpan(9, 35, 0), slots[1].Start);
        Assert.Equal(new TimeSpan(10, 5, 0), slots[1].End);
        Assert.Equal(new TimeSpan(11, 20, 0), slots[^1].Start);
        Assert.Equal(new TimeSpan(11, 50, 0), slots[^1].End);
    }

    [Fact]
    public void EndToEnd_GenerateTimeSlots_CenterBetweenPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(12);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 6;
        var position = PlanPaddingPosition.CenterBetweenOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(5, slots.Count);
        Assert.Equal(new TimeSpan(9, 0, 0), slots[0].Start);
        Assert.Equal(new TimeSpan(9, 27, 0), slots[0].End);
        Assert.Equal(new TimeSpan(9, 33, 0), slots[1].Start);
        Assert.Equal(new TimeSpan(10, 0, 0), slots[1].End);
    }

    [Fact]
    public void EndToEnd_CompareAllStrategies_SamePeriod()
    {
        var currentStart = TimeSpan.FromHours(11);
        var periodDuration = TimeSpan.FromMinutes(30);
        byte paddingTime = 5;

        var noPadding = _resolver.GetStrategy(PlanPaddingPosition.NoPadding).Compute(currentStart, periodDuration, paddingTime);
        var startOfPeriod = _resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod).Compute(currentStart, periodDuration, paddingTime);
        var endOfPeriod = _resolver.GetStrategy(PlanPaddingPosition.EndOfPeriod).Compute(currentStart, periodDuration, paddingTime);
        var linearBetween = _resolver.GetStrategy(PlanPaddingPosition.LinearBetweenOfPeriod).Compute(currentStart, periodDuration, paddingTime);
        var centerBetween = _resolver.GetStrategy(PlanPaddingPosition.CenterBetweenOfPeriod).Compute(currentStart, periodDuration, paddingTime);

        Assert.Equal(new TimeSpan(11, 0, 0), noPadding.SlotStart);
        Assert.Equal(new TimeSpan(11, 30, 0), noPadding.SlotEnd);
        
        Assert.Equal(new TimeSpan(11, 5, 0), startOfPeriod.SlotStart);
        Assert.Equal(new TimeSpan(11, 30, 0), startOfPeriod.SlotEnd);
        
        Assert.Equal(new TimeSpan(11, 0, 0), endOfPeriod.SlotStart);
        Assert.Equal(new TimeSpan(11, 25, 0), endOfPeriod.SlotEnd);
        
        Assert.Equal(new TimeSpan(11, 0, 0), linearBetween.SlotStart);
        Assert.Equal(new TimeSpan(11, 30, 0), linearBetween.SlotEnd);
        Assert.Equal(new TimeSpan(11, 35, 0), linearBetween.NextStart);
        
        Assert.Equal(new TimeSpan(11, 0, 0), centerBetween.SlotStart);
        Assert.Equal(new TimeSpan(11, 27, 30), centerBetween.SlotEnd);
        Assert.Equal(new TimeSpan(11, 32, 30), centerBetween.NextStart);
    }

    [Fact]
    public void EndToEnd_DifferentPaddingTimes_SameStrategy()
    {
        var currentStart = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(30);
        var strategy = _resolver.GetStrategy(PlanPaddingPosition.LinearBetweenOfPeriod);

        var padding0 = strategy.Compute(currentStart, periodDuration, 0);
        var padding5 = strategy.Compute(currentStart, periodDuration, 5);
        var padding10 = strategy.Compute(currentStart, periodDuration, 10);
        var padding15 = strategy.Compute(currentStart, periodDuration, 15);

        Assert.Equal(new TimeSpan(10, 30, 0), padding0.NextStart);
        Assert.Equal(new TimeSpan(10, 35, 0), padding5.NextStart);
        Assert.Equal(new TimeSpan(10, 40, 0), padding10.NextStart);
        Assert.Equal(new TimeSpan(10, 45, 0), padding15.NextStart);
    }

    [Fact]
    public void EndToEnd_FullDaySchedule_WithLinearPadding()
    {
        var workdayStart = TimeSpan.FromHours(8);
        var workdayEnd = TimeSpan.FromHours(18);
        var periodDuration = TimeSpan.FromMinutes(60);
        byte paddingTime = 15;
        var position = PlanPaddingPosition.LinearBetweenOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(8, slots.Count);
        
        for (int i = 0; i < slots.Count - 1; i++)
        {
            var gap = slots[i + 1].Start - slots[i].End;
            Assert.Equal(TimeSpan.FromMinutes(15), gap);
        }
    }

    [Fact]
    public void EndToEnd_ShortPeriods_WithCenterPadding()
    {
        var workdayStart = TimeSpan.FromHours(9);
        var workdayEnd = TimeSpan.FromHours(10);
        var periodDuration = TimeSpan.FromMinutes(15);
        byte paddingTime = 5;
        var position = PlanPaddingPosition.CenterBetweenOfPeriod;

        var slots = GenerateTimeSlots(workdayStart, workdayEnd, periodDuration, paddingTime, position);

        Assert.Equal(3, slots.Count);
        
        foreach (var slot in slots)
        {
            var duration = slot.End - slot.Start;
            Assert.Equal(TimeSpan.FromMinutes(12.5), duration);
        }
    }

    [Fact]
    public void EndToEnd_ResolverReturnsConsistentStrategy()
    {
        var strategy1 = _resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod);
        var strategy2 = _resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod);

        Assert.Same(strategy1, strategy2);
        Assert.Equal(PlanPaddingPosition.StartOfPeriod, strategy1.Position);
    }

    private List<TimeSlot> GenerateTimeSlots(
        TimeSpan workdayStart,
        TimeSpan workdayEnd,
        TimeSpan periodDuration,
        byte paddingTime,
        PlanPaddingPosition position)
    {
        var strategy = _resolver.GetStrategy(position);
        var slots = new List<TimeSlot>();
        var currentStart = workdayStart;

        while (currentStart < workdayEnd)
        {
            var (slotStart, slotEnd, nextStart) = strategy.Compute(currentStart, periodDuration, paddingTime);

            if (slotEnd <= workdayEnd)
            {
                slots.Add(new TimeSlot { Start = slotStart, End = slotEnd });
            }

            currentStart = nextStart;

            if (nextStart >= workdayEnd) break;
        }

        return slots;
    }

    private record TimeSlot
    {
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
    }
}
