namespace MedAppointment.Logic.Tests.Services.ScheduleServices;

public class TimeSlotPaddingStrategyResolverTests
{
    private const string ResolverTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.TimeSlotPaddingStrategyResolver";
    private const string NoPaddingStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.NoPaddingStrategy";
    private const string StartOfPeriodStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.StartOfPeriodPaddingStrategy";
    private const string EndOfPeriodStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.EndOfPeriodPaddingStrategy";
    private const string LinearBetweenStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.LinearBetweenOfPeriodPaddingStrategy";
    private const string CenterBetweenStrategyTypeName = "MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies.CenterBetweenOfPeriodPaddingStrategy";

    [Fact]
    public void Constructor_WithoutNoPaddingStrategy_ThrowsInvalidOperationException()
    {
        var startStrategy = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StartOfPeriodStrategyTypeName);
        var endStrategy = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(EndOfPeriodStrategyTypeName);
        IEnumerable<ITimeSlotPaddingStrategy> strategies = new[] { startStrategy, endStrategy };

        var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            new TimeSlotPaddingStrategyResolverWrapper(strategies));

        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Contains("NoPadding", exception.InnerException!.Message);
    }

    [Fact]
    public void Constructor_WithNullStrategies_ThrowsInvalidOperationException()
    {
        var exception = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
            new TimeSlotPaddingStrategyResolverWrapper(null!));

        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Contains("NoPadding", exception.InnerException!.Message);
    }

    private class TimeSlotPaddingStrategyResolverWrapper
    {
        public TimeSlotPaddingStrategyResolverWrapper(IEnumerable<ITimeSlotPaddingStrategy>? strategies)
        {
            var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);
        }
    }

    [Fact]
    public void Constructor_WithAllStrategies_CreatesSuccessfully()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();

        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        Assert.NotNull(resolver);
    }

    [Fact]
    public void GetStrategy_WithNoPadding_ReturnsNoPaddingStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.NoPadding);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.NoPadding, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithStartOfPeriod_ReturnsStartOfPeriodStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.StartOfPeriod, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithEndOfPeriod_ReturnsEndOfPeriodStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.EndOfPeriod);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.EndOfPeriod, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithLinearBetweenOfPeriod_ReturnsLinearBetweenStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.LinearBetweenOfPeriod);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.LinearBetweenOfPeriod, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithCenterBetweenOfPeriod_ReturnsCenterBetweenStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.CenterBetweenOfPeriod);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.CenterBetweenOfPeriod, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithUnknownPosition_ReturnsNoPaddingStrategy()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var unknownPosition = (PlanPaddingPosition)99;
        var strategy = resolver.GetStrategy(unknownPosition);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.NoPadding, strategy.Position);
    }

    [Fact]
    public void GetStrategy_WithMissingStrategy_FallsBackToNoPadding()
    {
        var noPaddingStrategy = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(NoPaddingStrategyTypeName);
        var startStrategy = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StartOfPeriodStrategyTypeName);
        IEnumerable<ITimeSlotPaddingStrategy> strategies = new[] { noPaddingStrategy, startStrategy };
        
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy = resolver.GetStrategy(PlanPaddingPosition.EndOfPeriod);

        Assert.NotNull(strategy);
        Assert.Equal(PlanPaddingPosition.NoPadding, strategy.Position);
    }

    [Fact]
    public void GetStrategy_CalledMultipleTimes_ReturnsSameStrategyInstance()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var strategy1 = resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod);
        var strategy2 = resolver.GetStrategy(PlanPaddingPosition.StartOfPeriod);

        Assert.Same(strategy1, strategy2);
    }

    [Fact]
    public void GetStrategy_AllPositions_ReturnsCorrectStrategies()
    {
        IEnumerable<ITimeSlotPaddingStrategy> strategies = CreateAllStrategies();
        var resolver = ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategyResolver>(ResolverTypeName, strategies);

        var allPositions = new[]
        {
            PlanPaddingPosition.NoPadding,
            PlanPaddingPosition.StartOfPeriod,
            PlanPaddingPosition.EndOfPeriod,
            PlanPaddingPosition.LinearBetweenOfPeriod,
            PlanPaddingPosition.CenterBetweenOfPeriod
        };

        foreach (var position in allPositions)
        {
            var strategy = resolver.GetStrategy(position);
            Assert.Equal(position, strategy.Position);
        }
    }

    private static IEnumerable<ITimeSlotPaddingStrategy> CreateAllStrategies()
    {
        return new[]
        {
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(NoPaddingStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(StartOfPeriodStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(EndOfPeriodStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(LinearBetweenStrategyTypeName),
            ServiceReflectionHelper.CreateService<ITimeSlotPaddingStrategy>(CenterBetweenStrategyTypeName)
        };
    }
}
