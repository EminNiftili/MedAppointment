namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class LanguageCreateValidation : BaseValidator<LanguageCreateDto>
    {
        public LanguageCreateValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode("ERR00040")
                    .WithMessage("Name is required.")
                .MaximumLength(100)
                    .WithErrorCode("ERR00145")
                    .WithMessage("Language name must not exceed 100 characters.");
        }
    }
}
