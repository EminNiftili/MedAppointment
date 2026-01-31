namespace MedAppointment.Logics.Services.ScheduleServices
{
    /// <summary>
    /// Handles time-slot generation for a day: periods with optional padding (e.g. CenterBetweenOfPeriod)
    /// and validation that no slot overlaps any break. Used by plan/schedule orchestration.
    /// </summary>
    public interface ITimeSlotService
    {
        /// <summary>
        /// Generates time slots from open time, applying period duration and optional padding.
        /// Validates that breaks do not overlap each other and that no generated slot overlaps any break.
        /// </summary>
        /// <param name="openTime">Day open time (e.g. 09:00).</param>
        /// <param name="periodTimeMinutes">Duration of each period in minutes.</param>
        /// <param name="paddingTimeMinutes">Padding in minutes (e.g. 5). Meaning depends on paddingPosition.</param>
        /// <param name="paddingPosition">0=NoPadding, 1=StartOfPeriod, 2=EndOfPeriod, 3=LinearBetween, 4=CenterBetween.</param>
        /// <param name="periodCount">Number of periods (slots) to generate for this day (from DaySchema.PeriodCount).</param>
        /// <param name="breaks">Break intervals (Start, End). Must not overlap each other.</param>
        /// <returns>List of (Start, End) slots, or error (ERR00128) if overlap is detected.</returns>
        Result<List<(TimeSpan Start, TimeSpan End)>> GenerateDaySlots(
            TimeSpan openTime,
            byte periodTimeMinutes,
            byte? paddingTimeMinutes,
            PlanPaddingPosition? paddingPosition,
            int periodCount,
            IReadOnlyList<(TimeSpan Start, TimeSpan End)> breaks);
    }
}
