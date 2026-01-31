namespace MedAppointment.Logics.Services.ScheduleServices
{
    /// <summary>
    /// Resolves the padding strategy for a given PlanPaddingPosition.
    /// Caller should validate enum value before resolving; null is treated as NoPadding.
    /// </summary>
    public interface ITimeSlotPaddingStrategyResolver
    {
        ITimeSlotPaddingStrategy GetStrategy(PlanPaddingPosition position);
    }
}
