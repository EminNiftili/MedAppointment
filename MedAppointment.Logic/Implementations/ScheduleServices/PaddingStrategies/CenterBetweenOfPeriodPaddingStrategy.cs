namespace MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies
{
    /// <summary>
    /// 4 - Split gap: take half from end of first, half from start of second. 1st 11:00-11:28, 2nd 11:32-12:00 (padding 5 min).
    /// </summary>
    internal class CenterBetweenOfPeriodPaddingStrategy : ITimeSlotPaddingStrategy
    {
        public PlanPaddingPosition Position => PlanPaddingPosition.CenterBetweenOfPeriod;

        public (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime)
        {
            var paddingHalf = paddingTime / 2.0;
            var paddingTs = TimeSpan.FromMinutes(paddingTime);
            var slotStart = currentStart;
            var slotEnd = currentStart + periodDuration - TimeSpan.FromMinutes(paddingHalf);
            var nextStart = slotEnd + paddingTs;
            return (slotStart, slotEnd, nextStart);
        }
    }
}
