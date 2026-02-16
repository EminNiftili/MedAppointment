using System.Reflection;

namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public abstract class BaseClassifierValidation<TModel> : BaseValidator<TModel> where TModel : ClassifierDto
    {
        public BaseClassifierValidation()
        {
            var hasOwnName = HasHiddenProperty(nameof(ClassifierDto.Name));
            var hasOwnDescription = HasHiddenProperty(nameof(ClassifierDto.Description));



            RuleFor(x => x.Key)
                .NotEmpty()
                    .WithErrorCode("ERR00140")
                    .WithMessage("Key is required.")
                .MaximumLength(150)
                    .WithErrorCode("ERR00141")
                    .WithMessage("Key must not exceed 150 characters.");

            if (!hasOwnName)
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
                    localization.RuleFor(x => x.Key)
                        .NotEmpty()
                            .WithErrorCode("ERR00134")
                            .WithMessage("Localization key is required.")
                        .MaximumLength(150)
                            .WithErrorCode("ERR00135")
                            .WithMessage("Localization key must not exceed 150 characters.");
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
            }

            if (!hasOwnDescription)
            {
                RuleFor(x => x.Description)
                .NotNull()
                    .WithErrorCode("ERR00043")
                    .WithMessage("Description is required.")
                .NotEmpty()
                    .WithErrorCode("ERR00043")
                    .WithMessage("Description is required.");

                RuleForEach(x => x.Description).ChildRules(localization =>
                {
                    localization.RuleFor(x => x.Key)
                        .NotEmpty()
                            .WithErrorCode("ERR00134")
                            .WithMessage("Localization key is required.")
                        .MaximumLength(150)
                            .WithErrorCode("ERR00135")
                            .WithMessage("Localization key must not exceed 150 characters.");
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

        private static bool HasHiddenProperty(string propertyName)
        {
            var t = typeof(TModel);

            var declared = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Any(p => p.Name == propertyName);

            if (declared)
                return true;

            return false;
        }
    }
}
