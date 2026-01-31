namespace MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies
{
    /// <summary>
    /// 3 - Full period, add padding between. 1st 11:00-11:30, 2nd 11:35-12:05 (padding 5 min).
    /// </summary>
    internal class LinearBetweenOfPeriodPaddingStrategy : ITimeSlotPaddingStrategy
    {
        public PlanPaddingPosition Position => PlanPaddingPosition.LinearBetweenOfPeriod;

        public (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime)
        {
            var paddingTs = TimeSpan.FromMinutes(paddingTime);
            var slotStart = currentStart;
            var slotEnd = currentStart + periodDuration;
            var nextStart = currentStart + periodDuration + paddingTs;
            return (slotStart, slotEnd, nextStart);
        }
    }
}
