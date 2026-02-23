namespace MedAppointment.Logics.Services.CalendarServices
{
    public interface IDoctorCalendarService
    {
        Task<Result<DoctorCalendarWeekResponseDto>> GetWeeklyCalendarAsync(DoctorCalendarWeekQueryDto query);
    }
}
