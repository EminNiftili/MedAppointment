namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class CurrencyUpdateValidation : BaseClassifierValidation<CurrencyUpdateDto>
    {
        public CurrencyUpdateValidation() : base()
        {
            RuleFor(x => x.Coefficent)
                .GreaterThan(0.0001m)
                    .WithErrorCode("ERR00046")
                    .WithMessage("Coefficient must be greater than 0.0001.")
                .LessThanOrEqualTo(999999.9999m)
                    .WithErrorCode("ERR00047")
                    .WithMessage("Coefficient must not exceed 999999.9999.");
        }
    }
}
