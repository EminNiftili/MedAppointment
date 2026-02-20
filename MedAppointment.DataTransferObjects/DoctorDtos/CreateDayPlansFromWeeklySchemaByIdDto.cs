namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    /// <summary>
    /// Request to fill calendar from an existing WeeklySchema in the database.
    /// WeeklySchema is loaded by Id; other fields are provided by the client.
    /// </summary>
    public record CreateDayPlansFromWeeklySchemaByIdDto
    {
        /// <summary>
        /// Id of the WeeklySchema (template) in the database.
        /// </summary>
        public long WeeklySchemaId { get; init; }

        /// <summary>
        /// Start date of the week (Monday). Used to compute BelongDate for each weekday.
        /// </summary>
        public DateTime StartDate { get; init; }

        public long CurrencyId { get; init; }
        public decimal PricePerPeriod { get; init; }
    }
}
