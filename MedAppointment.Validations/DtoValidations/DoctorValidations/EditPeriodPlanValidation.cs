using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.Validations.DtoValidations.DoctorValidations
{
    public class EditPeriodPlanValidation : BaseValidator<EditPeriodPlanDto>
    {
        public EditPeriodPlanValidation()
        {
            RuleFor(x => x.PeriodPlanId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00159")
                .WithMessage("Period plan id must be greater than 0.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00150")
                .WithMessage("Doctor id must be greater than 0.");

            RuleFor(x => x.PeriodStop)
                .Must((dto, periodStop) => periodStop > dto.PeriodStart)
                .WithErrorCode("ERR00162")
                .WithMessage("Period stop must be after period start.");

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00123")
                .WithMessage("Currency id must be greater than 0.");

            RuleFor(x => x.PricePerPeriod)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode("ERR00124")
                .WithMessage("Price per period must be greater than or equal to 0.");
        }
    }
}
