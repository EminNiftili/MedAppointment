namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class SpecialtyCreateValidation : BaseClassifierValidation<SpecialtyCreateDto>
    {
        public SpecialtyCreateValidation() : base()
        {
            RuleFor(x => x.Name)
            .NotNull()
                .WithErrorCode("ERR00040")
                .WithMessage("Name is required.")
            .NotEmpty()
                .WithErrorCode("ERR00040")
                .WithMessage("Name is required.");

            RuleForEach(x => x.Name).ChildRules(localization =>
            {
                localization.RuleFor(x => x.LanguageId)
                    .GreaterThan(0)
                        .WithErrorCode("ERR00136")
                        .WithMessage("Localization language id must be greater than 0.");
                localization.RuleFor(x => x.Text)
                    .NotEmpty()
                        .WithErrorCode("ERR00137")
                        .WithMessage("Localization text is required.")
                    .MaximumLength(150)
                        .WithErrorCode("ERR00138")
                        .WithMessage("Localization text must not exceed 150 characters.");
            });

            RuleFor(x => x.Description)
                .NotNull()
                    .WithErrorCode("ERR00043")
                    .WithMessage("Description is required.")
                .NotEmpty()
                    .WithErrorCode("ERR00043")
                    .WithMessage("Description is required.");

            RuleForEach(x => x.Description).ChildRules(localization =>
            {
                localization.RuleFor(x => x.LanguageId)
                    .GreaterThan(0)
                        .WithErrorCode("ERR00136")
                        .WithMessage("Localization language id must be greater than 0.");
                localization.RuleFor(x => x.Text)
                    .NotEmpty()
                        .WithErrorCode("ERR00137")
                        .WithMessage("Localization text is required.")
                    .MaximumLength(500)
                        .WithErrorCode("ERR00139")
                        .WithMessage("Localization text must not exceed 500 characters.");
            });
        }
    }
}
