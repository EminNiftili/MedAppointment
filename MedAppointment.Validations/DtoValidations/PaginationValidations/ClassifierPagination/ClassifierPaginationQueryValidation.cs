using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Validations.DtoValidations.PaginationValidations.ClassifierPagination
{
    /// <summary>
    /// Validates base classifier pagination query (used for Specialty, PaymentType and as base for derived query DTOs).
    /// </summary>
    public class ClassifierPaginationQueryValidation : BaseValidator<ClassifierPaginationQueryDto>
    {
        public ClassifierPaginationQueryValidation()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithErrorCode("ERR00102")
                .WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithErrorCode("ERR00103")
                .WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithErrorCode("ERR00104")
                .WithMessage("Page size must not exceed 100.");
            RuleFor(x => x.NameFilter)
                .MaximumLength(150)
                .When(x => !string.IsNullOrEmpty(x.NameFilter))
                .WithErrorCode("ERR00041")
                .WithMessage("Name filter must not exceed 150 characters.");
            RuleFor(x => x.DescriptionFilter)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.DescriptionFilter))
                .WithErrorCode("ERR00044")
                .WithMessage("Description filter must not exceed 500 characters.");
        }
    }
}
