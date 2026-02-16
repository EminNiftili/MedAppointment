using MedAppointment.DataTransferObjects.PaginationDtos.UserPagination;

namespace MedAppointment.Validations.DtoValidations.PaginationValidations.UserPagination
{
    /// <summary>
    /// Validates user pagination query (pagination + optional filters).
    /// </summary>
    public class UserPaginationQueryValidation : BaseValidator<UserPaginationQueryDto>
    {
        public UserPaginationQueryValidation()
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
            RuleFor(x => x.SurnameFilter)
                .MaximumLength(150)
                .When(x => !string.IsNullOrEmpty(x.SurnameFilter))
                .WithErrorCode("ERR00041")
                .WithMessage("Surname filter must not exceed 150 characters.");
            RuleFor(x => x.EmailFilter)
                .MaximumLength(256)
                .When(x => !string.IsNullOrEmpty(x.EmailFilter))
                .WithErrorCode("ERR00147")
                .WithMessage("Email filter must not exceed 256 characters.");
            RuleFor(x => x.PhoneFilter)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.PhoneFilter))
                .WithErrorCode("ERR00148")
                .WithMessage("Phone filter must not exceed 50 characters.");
        }
    }
}
