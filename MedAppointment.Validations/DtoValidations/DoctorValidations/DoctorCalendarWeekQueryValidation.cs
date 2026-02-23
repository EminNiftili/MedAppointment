using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.Validations.DtoValidations.DoctorValidations
{
    public class DoctorCalendarWeekQueryValidation : BaseValidator<DoctorCalendarWeekQueryDto>
    {
        public DoctorCalendarWeekQueryValidation()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00150")
                .WithMessage("Doctor id must be greater than 0.");

            RuleFor(x => x.WeekStartDate)
                .NotEmpty()
                .WithErrorCode("ERR00151")
                .WithMessage("Week start date is required.");

            RuleFor(x => x.WeekStartDate)
                .Must(d => d.Date == d && d.DayOfWeek == DayOfWeek.Monday)
                .WithErrorCode("ERR00152")
                .WithMessage("Week start date must be a Monday (start of week).")
                .When(x => x.WeekStartDate != default);
        }
    }
}
