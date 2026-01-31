namespace MedAppointment.Logics.Implementations.ScheduleServices
{
    internal class TimeSlotPaddingStrategyResolver : ITimeSlotPaddingStrategyResolver
    {
        private readonly Dictionary<PlanPaddingPosition, ITimeSlotPaddingStrategy> _strategiesByPosition;

        public TimeSlotPaddingStrategyResolver(IEnumerable<ITimeSlotPaddingStrategy> strategies)
        {
            var list = strategies?.ToList() ?? new List<ITimeSlotPaddingStrategy>();
            _strategiesByPosition = list.ToDictionary(s => s.Position);
            if (!_strategiesByPosition.ContainsKey(PlanPaddingPosition.NoPadding))
                throw new InvalidOperationException("No ITimeSlotPaddingStrategy for PlanPaddingPosition.NoPadding registered.");
        }

        public ITimeSlotPaddingStrategy GetStrategy(PlanPaddingPosition position)
        {
            return _strategiesByPosition.TryGetValue(position, out var strategy)
                ? strategy
                : _strategiesByPosition[PlanPaddingPosition.NoPadding];
        }
    }
}
