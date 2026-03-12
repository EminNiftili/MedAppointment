namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class DoctorSchemaCreateValidation : BaseValidator<DoctorSchemaCreateDto>
    {
        public DoctorSchemaCreateValidation()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00109")
                .WithMessage("DoctorId must be greater than 0.");

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

            RuleFor(x => x.SpecialtyIds)
                .NotEmpty()
                .WithErrorCode("ERR00111")
                .WithMessage("Weekly schema SpecialtyIds must contain at least one specialty.");

            RuleForEach(x => x.SpecialtyIds)
                .GreaterThan(0L)
                .WithErrorCode("ERR00167")
                .WithMessage("Each SpecialtyId in Weekly schema must be greater than 0.");

            RuleForEach(x => x.DaySchemas)
                .SetValidator(new DaySchemaCreateValidation())
                .When(x => x.DaySchemas != null);
        }
    }
}
