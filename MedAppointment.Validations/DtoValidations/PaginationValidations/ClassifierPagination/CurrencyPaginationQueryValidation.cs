using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Validations.DtoValidations.PaginationValidations.ClassifierPagination
{
    public class CurrencyPaginationQueryValidation : BaseValidator<CurrencyPaginationQueryDto>
    {
        public CurrencyPaginationQueryValidation()
        {
            Include(new ClassifierPaginationQueryValidation());
            RuleFor(x => x.CoefficentMin)
                .GreaterThan(0.0001m)
                .When(x => x.CoefficentMin.HasValue)
                .WithErrorCode("ERR00046")
                .WithMessage("Coefficient must be greater than 0.");
            RuleFor(x => x.CoefficentMin)
                .LessThanOrEqualTo(999999.9999m)
                .When(x => x.CoefficentMin.HasValue)
                .WithErrorCode("ERR00047")
                .WithMessage("Coefficient must not exceed 999999.99.");
            RuleFor(x => x.CoefficentMax)
                .GreaterThan(0.0001m)
                .When(x => x.CoefficentMax.HasValue)
                .WithErrorCode("ERR00046")
                .WithMessage("Coefficient must be greater than 0.");
            RuleFor(x => x.CoefficentMax)
                .LessThanOrEqualTo(999999.9999m)
                .When(x => x.CoefficentMax.HasValue)
                .WithErrorCode("ERR00047")
                .WithMessage("Coefficient must not exceed 999999.99.");
            RuleFor(x => x)
                .Must(x => !x.CoefficentMin.HasValue || !x.CoefficentMax.HasValue || x.CoefficentMin <= x.CoefficentMax)
                .WithErrorCode("ERR00142")
                .WithMessage("CoefficentMin must be less than or equal to CoefficentMax.");
        }
    }
}
