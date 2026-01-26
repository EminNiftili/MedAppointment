namespace MedAppointment.Validations.DtoValidations.PaginationValidations
{
    public class PaginationQueryValidation : BaseValidator<PaginationQueryDto>
    {
        public PaginationQueryValidation()
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
        }
    }
}
