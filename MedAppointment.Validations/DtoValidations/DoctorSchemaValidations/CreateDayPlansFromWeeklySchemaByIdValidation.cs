namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class CreateDayPlansFromWeeklySchemaByIdValidation : BaseValidator<CreateDayPlansFromWeeklySchemaByIdDto>
    {
        public CreateDayPlansFromWeeklySchemaByIdValidation()
        {
            RuleFor(x => x.WeeklySchemaId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00121")
                .WithMessage("Weekly schema id must be greater than 0.");

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
