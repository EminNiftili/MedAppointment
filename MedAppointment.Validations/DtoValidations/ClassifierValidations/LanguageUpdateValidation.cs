namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class LanguageUpdateValidation : BaseValidator<LanguageUpdateDto>
    {
        public LanguageUpdateValidation()
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
