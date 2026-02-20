namespace MedAppointment.Logics.Services.PlanManagerServices
{
    public interface IDoctorPlanManagerService
    {
        /// <summary>
        /// Creates DayPlans and PeriodPlans from the given WeeklySchema (template) for the week starting at StartDate.
        /// Template comes from DTO; Period and Padding are read from DB. DoctorId is in the DTO.
        /// Only verified (confirmed) doctors can perform this operation.
        /// </summary>
        Task<Result> CreateDayPlansFromWeeklySchemaAsync(CreateDayPlansFromSchemaDto dto);

        /// <summary>
        /// Adds a new doctor weekly schema (template) with day schemas and day breaks.
        /// Only verified (confirmed) doctors can have schemas added.
        /// </summary>
        Task<Result<long>> AddDoctorSchemaAsync(DoctorSchemaCreateDto dto);
    }
}
