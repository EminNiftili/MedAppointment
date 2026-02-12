using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Validations.DtoValidations.PaginationValidations.ClassifierPagination
{
    public class PlanPaddingTypePaginationQueryValidation : BaseValidator<PlanPaddingTypePaginationQueryDto>
    {
        public PlanPaddingTypePaginationQueryValidation()
        {
            Include(new ClassifierPaginationQueryValidation());
            RuleFor(x => x.PaddingPosition)
                .IsInEnum()
                .When(x => x.PaddingPosition.HasValue)
                .WithErrorCode("ERR00129")
                .WithMessage("Invalid padding position. Must be a valid PlanPaddingPosition enum value (0-4).");
            RuleFor(x => x.PaddingTime)
                .InclusiveBetween((byte)1, byte.MaxValue)
                .When(x => x.PaddingTime.HasValue)
                .WithErrorCode("ERR00142")
                .WithMessage("Padding time must be between 1 and 255 minutes.");
        }
    }
}
