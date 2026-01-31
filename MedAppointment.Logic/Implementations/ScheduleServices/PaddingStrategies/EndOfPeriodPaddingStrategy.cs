namespace MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies
{
    /// <summary>
    /// 2 - Cut from end of period. 11:00-11:30 -> 11:00-11:25 (padding 5 min).
    /// </summary>
    internal class EndOfPeriodPaddingStrategy : ITimeSlotPaddingStrategy
    {
        public PlanPaddingPosition Position => PlanPaddingPosition.EndOfPeriod;

        public (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime)
        {
            var paddingTs = TimeSpan.FromMinutes(paddingTime);
            var slotStart = currentStart;
            var slotEnd = currentStart + periodDuration - paddingTs;
            var nextStart = currentStart + periodDuration;
            return (slotStart, slotEnd, nextStart);
        }
    }
}
