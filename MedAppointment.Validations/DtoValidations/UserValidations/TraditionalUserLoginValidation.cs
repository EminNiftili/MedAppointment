using MedAppointment.Validations.DtoValidations.CredentialValidations;

namespace MedAppointment.Validations.DtoValidations.UserValidations
{
    public class TraditionalUserLoginValidation : AbstractValidator<TraditionalUserLoginDto>
    {
        public TraditionalUserLoginValidation(IValidator<DeviceDto> deviceValidator)
        {
            RuleFor(x => x.Username)
            .NotEmpty()
                .WithMessage("Username is required.")
                .WithErrorCode("ERR00035")
            .MaximumLength(300)
                .WithMessage("Username must not exceed 300 characters.")
                .WithErrorCode("ERR00036");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required.")
                    .WithErrorCode("ERR00016")
                .MinimumLength(6)
                    .WithMessage("Password must be at least 8 characters long.")
                    .WithErrorCode("ERR00017");

            RuleFor(x => x.DeviceInfo)
                .NotNull()
                    .WithMessage("Device information is required.")
                    .WithErrorCode("ERR00037")
                .SetValidator(deviceValidator!);
        }
    }
}
