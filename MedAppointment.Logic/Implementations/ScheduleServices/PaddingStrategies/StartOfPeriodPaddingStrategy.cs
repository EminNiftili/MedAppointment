namespace MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies
{
    /// <summary>
    /// 1 - Cut from start of period. 11:00-11:30 -> 11:05-11:30 (padding 5 min).
    /// </summary>
    internal class StartOfPeriodPaddingStrategy : ITimeSlotPaddingStrategy
    {
        public PlanPaddingPosition Position => PlanPaddingPosition.StartOfPeriod;

        public (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime)
        {
            var paddingTs = TimeSpan.FromMinutes(paddingTime);
            var slotStart = currentStart + paddingTs;
            var slotEnd = currentStart + periodDuration;
            var nextStart = currentStart + periodDuration;
            return (slotStart, slotEnd, nextStart);
        }
    }
}
