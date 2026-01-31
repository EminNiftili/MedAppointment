namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    /// <summary>
    /// Input for creating DayPlans and PeriodPlans from a WeeklySchema (template).
    /// Template comes from DTO (not loaded from DB). Period and Padding are read from DB by IDs in the template.
    /// StartDate is the week start (e.g. Monday of the target week); only verified doctors can perform this.
    /// </summary>
    public record CreateDayPlansFromSchemaDto
    {
        public long DoctorId { get; set; }
        /// <summary>
        /// Template (WeeklySchema with DaySchemas). Provided by caller; not loaded from DB.
        /// </summary>
        public WeeklySchemaDto WeeklySchema { get; set; } = null!;
        /// <summary>
        /// Start date of the week (day-month-year). Used to compute BelongDate for each weekday.
        /// </summary>
        public DateTime StartDate { get; set; }
        public long CurrencyId { get; set; }
        public decimal PricePerPeriod { get; set; }
    }
}
