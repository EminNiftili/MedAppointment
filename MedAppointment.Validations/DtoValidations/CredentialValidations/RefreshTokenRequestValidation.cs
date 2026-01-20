namespace MedAppointment.Validations.DtoValidations.CredentialValidations
{
    public class RefreshTokenRequestValidation : BaseValidator<RefreshTokenRequestDto>
    {
        private const int RefreshTokenMaxLength = 512;

        public RefreshTokenRequestValidation()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                    .WithMessage("Refresh token is required.")
                    .WithErrorCode("ERR00038")
                .MaximumLength(RefreshTokenMaxLength)
                    .WithMessage("Refresh token must not exceed 512 characters.")
                    .WithErrorCode("ERR00039")
                .Must(NoWhitespace)
                    .WithMessage("Refresh token must not contain whitespace.")
                    .WithErrorCode("ERR00052")
                .Must(BeValidBase64)
                    .WithMessage("Refresh token format is invalid.")
                    .WithErrorCode("ERR00053");
        }
    }
}
