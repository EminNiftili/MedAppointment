namespace MedAppointment.Logics.Implementations.ScheduleServices.PaddingStrategies
{
    /// <summary>
    /// No padding: full period, next start = current end.
    /// </summary>
    internal class NoPaddingStrategy : ITimeSlotPaddingStrategy
    {
        public PlanPaddingPosition Position => PlanPaddingPosition.NoPadding;

        public (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime)
        {
            var end = currentStart + periodDuration;
            return (currentStart, end, end);
        }
    }
}
