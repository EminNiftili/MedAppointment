namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class CurrencyCreateValidation : BaseClassifierValidation<CurrencyCreateDto>
    {
        public CurrencyCreateValidation() : base()
        {
            RuleFor(x => x.Coefficent)
                .GreaterThan(0.0001m)
                    .WithErrorCode("ERR00046")
                    .WithMessage("Coefficient must be greater than 0.")
                .LessThanOrEqualTo(999999.9999m)
                    .WithErrorCode("ERR00047")
                    .WithMessage("Coefficient must not exceed 999999.99.");
        }
    }
}
