using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.Validations.DtoValidations.DoctorValidations
{
    public class EditDayPlanValidation : BaseValidator<EditDayPlanDto>
    {
        public EditDayPlanValidation()
        {
            RuleFor(x => x.DayPlanId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00155")
                .WithMessage("Day plan id must be greater than 0.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00150")
                .WithMessage("Doctor id must be greater than 0.");

            RuleFor(x => x.SpecialtyId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00157")
                .WithMessage("Specialty id must be greater than 0.");
        }
    }
}
