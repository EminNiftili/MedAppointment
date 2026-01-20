namespace MedAppointment.Validations.DtoValidations.CredentialValidations
{
    public class DeviceValidation : BaseValidator<DeviceDto>
    {
        public DeviceValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Device name is required.")
                    .WithErrorCode("ERR00027")
                .MaximumLength(150)
                    .WithMessage("Device name must not exceed 150 characters.")
                    .WithErrorCode("ERR00028");

            RuleFor(x => x.DeviceType)
                .IsInEnum()
                    .WithMessage("Invalid device type.")
                    .WithErrorCode("ERR00029");

            RuleFor(x => x.AppType)
                .IsInEnum()
                    .WithMessage("Invalid application type.")
                    .WithErrorCode("ERR00030");

            RuleFor(x => x.OSName)
                .MaximumLength(150)
                    .WithMessage("OS name must not exceed 150 characters.")
                    .WithErrorCode("ERR00031")
                .When(x => !string.IsNullOrWhiteSpace(x.OSName));

            RuleFor(x => x.OSVersion)
                .MaximumLength(150)
                    .WithMessage("OS version must not exceed 150 characters.")
                    .WithErrorCode("ERR00032")
                .When(x => !string.IsNullOrWhiteSpace(x.OSVersion));

            RuleFor(x => x.UUID)
                .NotEmpty()
                    .WithMessage("UUID is required.")
                    .WithErrorCode("ERR00033")
                .MaximumLength(300)
                    .WithMessage("UUID must not exceed 300 characters.")
                    .WithErrorCode("ERR00034");
        }
    }
}
