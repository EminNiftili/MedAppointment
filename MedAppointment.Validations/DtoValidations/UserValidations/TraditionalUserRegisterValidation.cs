namespace MedAppointment.Validations.DtoValidations.UserValidations
{
    public class TraditionalUserRegisterValidation : BaseValidator<TraditionalUserRegisterDto>
    {
        public TraditionalUserRegisterValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode("ERR00001")
                    .WithMessage("Name is required.")
                .MaximumLength(50)
                    .WithErrorCode("ERR00002")
                    .WithMessage("Name must not exceed 50 characters.")
                .Must(BeOnlyLetter)
                    .WithErrorCode("ERR00003")
                    .WithMessage("Name can contain letters only.");

            RuleFor(x => x.Surname)
                .NotEmpty()
                    .WithErrorCode("ERR00004")
                    .WithMessage("Surname is required.")
                .MaximumLength(50)
                    .WithErrorCode("ERR00005")
                    .WithMessage("Surname must not exceed 50 characters.")
                .Must(BeOnlyLetter)
                    .WithErrorCode("ERR00006")
                    .WithMessage("Surname can contain letters only.");

            RuleFor(x => x.FatherName)
                .NotEmpty()
                    .WithErrorCode("ERR00007")
                    .WithMessage("Father name is required.")
                .MaximumLength(50)
                    .WithErrorCode("ERR00008")
                    .WithMessage("Father name must not exceed 50 characters.")
                .Must(BeOnlyLetter)
                    .WithErrorCode("ERR00009")
                    .WithMessage("Father name can contain letters only.");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                    .WithErrorCode("ERR00010")
                    .WithMessage("Birth date is required.")
                .LessThan(DateTime.Today)
                    .WithErrorCode("ERR00011")
                    .WithMessage("Birth date cannot be in the future.");

            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithErrorCode("ERR00012")
                    .WithMessage("Email is required.")
                .EmailAddress()
                    .WithErrorCode("ERR00013")
                    .WithMessage("Email format is invalid.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                    .WithErrorCode("ERR00014")
                    .WithMessage("Phone number is required.")
                .Must(BeValidPhone)
                    .WithErrorCode("ERR00015")
                    .WithMessage("Phone number must be in international E.164 format (e.g., +994501234567).");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithErrorCode("ERR00016")
                    .WithMessage("Password is required.")
                .MinimumLength(8)
                    .WithErrorCode("ERR00017")
                    .WithMessage("Password must be at least 8 characters long.")
                .Must(ContainsUpper)
                    .WithErrorCode("ERR00018")
                    .WithMessage("Password must contain at least one uppercase letter.")
                .Must(ContainsLower)
                    .WithErrorCode("ERR00019")
                    .WithMessage("Password must contain at least one lowercase letter.")
                .Must(ContainsDigit)
                    .WithErrorCode("ERR00020")
                    .WithMessage("Password must contain at least one digit.")
                .Must(ContainsSpecial)
                    .WithErrorCode("ERR00021")
                    .WithMessage("Password must contain at least one special character.");
        }
    }
}
