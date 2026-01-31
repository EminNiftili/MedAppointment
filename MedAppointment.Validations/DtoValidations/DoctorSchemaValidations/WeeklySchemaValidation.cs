namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class WeeklySchemaValidation : BaseValidator<WeeklySchemaDto>
    {
        public WeeklySchemaValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode("ERR00105")
                    .WithMessage("Weekly schema name is required.")
                .MaximumLength(100)
                    .WithErrorCode("ERR00106")
                    .WithMessage("Weekly schema name must not exceed 100 characters.");

            RuleFor(x => x.ColorHex)
                .NotEmpty()
                    .WithErrorCode("ERR00107")
                    .WithMessage("Weekly schema ColorHex is required.")
                .Length(9)
                    .WithErrorCode("ERR00108")
                    .WithMessage("Weekly schema ColorHex must be 9 characters in format #RRGGBBAA.")
                .Must(hex => ColorHexRegex.IsMatch(hex))
                    .WithErrorCode("ERR00108")
                    .WithMessage("Weekly schema ColorHex must be 9 characters in format #RRGGBBAA.");

            RuleForEach(x => x.DaySchemas)
                .SetValidator(new DaySchemaValidation())
                .When(x => x.DaySchemas != null);
        }
    }
}
