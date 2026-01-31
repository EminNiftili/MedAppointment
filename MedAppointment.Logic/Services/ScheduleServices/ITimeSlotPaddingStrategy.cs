namespace MedAppointment.Logics.Services.ScheduleServices
{
    /// <summary>
    /// Strategy for computing one time slot and the next period start based on padding position.
    /// Each enum value (including NoPadding) has a corresponding strategy.
    /// </summary>
    public interface ITimeSlotPaddingStrategy
    {
        /// <summary>
        /// The padding position this strategy handles.
        /// </summary>
        PlanPaddingPosition Position { get; }

        /// <summary>
        /// Computes (SlotStart, SlotEnd, NextStart) for the current period.
        /// </summary>
        (TimeSpan SlotStart, TimeSpan SlotEnd, TimeSpan NextStart) Compute(
            TimeSpan currentStart,
            TimeSpan periodDuration,
            byte paddingTime);
    }
}
