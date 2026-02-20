namespace MedAppointment.Logics.Implementations.ScheduleServices
{
    internal class TimeSlotService : ITimeSlotService
    {
        private readonly ILogger<TimeSlotService> _logger;
        private readonly ITimeSlotPaddingStrategyResolver _paddingStrategyResolver;

        public TimeSlotService(
            ILogger<TimeSlotService> logger,
            ITimeSlotPaddingStrategyResolver paddingStrategyResolver)
        {
            _logger = logger;
            _paddingStrategyResolver = paddingStrategyResolver;
        }

        public Result<List<(TimeSpan Start, TimeSpan End)>> GenerateDaySlots(
            TimeSpan openTime,
            byte periodTimeMinutes,
            byte? paddingTimeMinutes,
            PlanPaddingPosition? paddingPosition,
            int periodCount,
            IReadOnlyList<(TimeSpan Start, TimeSpan End)> breaks)
        {
            _logger.LogTrace("GenerateDaySlots started. OpenTime: {OpenTime}, PeriodMinutes: {PeriodMinutes}, PaddingMinutes: {PaddingMinutes}, PaddingPosition: {PaddingPosition}, PeriodCount: {PeriodCount}, BreaksCount: {BreaksCount}",
                openTime, periodTimeMinutes, paddingTimeMinutes, paddingPosition, periodCount, breaks?.Count ?? 0);

            var result = Result<List<(TimeSpan Start, TimeSpan End)>>.Create();
            var slots = new List<(TimeSpan Start, TimeSpan End)>();
            var breaksList = breaks?.ToList() ?? new List<(TimeSpan Start, TimeSpan End)>();

            for (var i = 0; i < breaksList.Count; i++)
            {
                for (var j = i + 1; j < breaksList.Count; j++)
                {
                    if (Overlap(breaksList[i].Start, breaksList[i].End, breaksList[j].Start, breaksList[j].End))
                    {
                        _logger.LogInformation("Break-overlap detected. Breaks must not overlap each other.");
                        result.AddMessage("ERR00128", "Period or break time overlap detected. Invalid schedule data.", HttpStatusCode.BadRequest);
                        return result;
                    }
                }
            }

            if (paddingPosition == null || (paddingPosition.HasValue && !Enum.IsDefined(typeof(PlanPaddingPosition), paddingPosition.Value)))
            {
                _logger.LogInformation("Invalid PlanPaddingPosition value. Value: {Value}", paddingPosition!.Value);
                result.AddMessage("ERR00129", "Invalid padding position. Must be a valid PlanPaddingPosition enum value (0-4).", HttpStatusCode.BadRequest);
                return result;
            }

            var paddingTime = paddingTimeMinutes ?? 0;
            var periodDuration = TimeSpan.FromMinutes(periodTimeMinutes);
            var currentStart = openTime;
            var strategy = _paddingStrategyResolver.GetStrategy(paddingPosition!.Value);

            while (slots.Count < periodCount)
            {
                var (slotStart, slotEnd, nextStart) = strategy.Compute(currentStart, periodDuration, paddingTime);

                if (slotEnd <= slotStart)
                    break;

                bool hasOverlap = false;
                foreach (var (breakStart, breakEnd) in breaksList)
                {
                    if (Overlap(slotStart, slotEnd, breakStart, breakEnd))
                    {
                        _logger.LogInformation("Slot-break overlap detected. OpenTime: {OpenTime}, Slot: {SlotStart}-{SlotEnd}, Breaks: {BreaksCount}",
                            openTime, slotStart, slotEnd, breaksList.Count);
                        //result.AddMessage("ERR00128", "Period or break time overlap detected. Invalid schedule data.", HttpStatusCode.BadRequest);
                        hasOverlap = true;
                        break;
                    }
                }
                if (hasOverlap)
                {
                    currentStart = nextStart;
                    while (breaksList.Any(b => currentStart >= b.Start && currentStart < b.End))
                    {
                        var containing = breaksList.First(b => currentStart >= b.Start && currentStart < b.End);
                        currentStart = containing.End;
                    }
                    continue;
                }

                slots.Add((slotStart, slotEnd));
                currentStart = nextStart;

                while (breaksList.Any(b => currentStart >= b.Start && currentStart < b.End))
                {
                    var containing = breaksList.First(b => currentStart >= b.Start && currentStart < b.End);
                    currentStart = containing.End;
                }
            }

            _logger.LogDebug("GenerateDaySlots completed. SlotsCount: {SlotsCount}", slots.Count);
            result.Success(slots);
            return result;
        }

        private static bool Overlap(TimeSpan aStart, TimeSpan aEnd, TimeSpan bStart, TimeSpan bEnd)
        {
            return aStart < bEnd && bStart < aEnd;
        }
    }
}
