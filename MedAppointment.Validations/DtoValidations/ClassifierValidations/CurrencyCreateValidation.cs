namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class CurrencyCreateValidation : ClassifierBaseValidator<CurrencyCreateDto>
    {
        public CurrencyCreateValidation()
        {
            RuleFor(x => x.Coefficent)
                .GreaterThan(0)
                    .WithErrorCode("ERR00046")
                    .WithMessage("Coefficient must be greater than 0.")
                .LessThanOrEqualTo(999999.99m)
                    .WithErrorCode("ERR00047")
                    .WithMessage("Coefficient must not exceed 999999.99.");
        }
    }
}
