namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class CreateDayPlansFromSchemaValidation : BaseValidator<CreateDayPlansFromSchemaDto>
    {
        public CreateDayPlansFromSchemaValidation()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00109")
                .WithMessage("DoctorId must be greater than 0.");

            RuleFor(x => x.WeeklySchema)
                .NotNull()
                .WithErrorCode("ERR00131")
                .WithMessage("Weekly schema (template) is required.");

            RuleFor(x => x.WeeklySchema)
                .SetValidator(new WeeklySchemaValidation())
                .When(x => x.WeeklySchema != null);

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithErrorCode("ERR00122")
                .WithMessage("Start date is required.");

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
