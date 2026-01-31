namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class WeeklySchemaValidation : BaseValidator<WeeklySchemaDto>
    {
        protected static readonly System.Text.RegularExpressions.Regex ColorHexRegex = new(@"^#[0-9A-Fa-f]{8}$", System.Text.RegularExpressions.RegexOptions.Compiled);

        protected WeeklySchemaValidation()
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
        }
    }
}
