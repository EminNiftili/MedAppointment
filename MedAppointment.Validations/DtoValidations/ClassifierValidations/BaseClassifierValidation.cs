namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public abstract class BaseClassifierValidation<TModel> : BaseValidator<TModel> where TModel : ClassifierDto
    {
        public BaseClassifierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithErrorCode("ERR00040")
                    .WithMessage("Name is required.")
                .MaximumLength(150)
                    .WithErrorCode("ERR00041")
                    .WithMessage("Name must not exceed 150 characters.");
            //.Must(BeValidClassifierName)
            //    .WithErrorCode("ERR00042")
            //    .WithMessage("Name contains invalid characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                    .WithErrorCode("ERR00043")
                    .WithMessage("Description is required.")
                .MaximumLength(500)
                    .WithErrorCode("ERR00044")
                    .WithMessage("Description must not exceed 500 characters.");
                //.Must(BeValidClassifierDescription)
                //    .WithErrorCode("ERR00045")
                //    .WithMessage("Description contains invalid characters.");

        }
    }
}
