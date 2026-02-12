using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Validations.DtoValidations.PaginationValidations.ClassifierPagination
{
    public class PeriodPaginationQueryValidation : BaseValidator<PeriodPaginationQueryDto>
    {
        public PeriodPaginationQueryValidation()
        {
            Include(new ClassifierPaginationQueryValidation());
            RuleFor(x => x.PeriodTime)
                .InclusiveBetween((byte)1, byte.MaxValue)
                .When(x => x.PeriodTime.HasValue)
                .WithErrorCode("ERR00048")
                .WithMessage("Period time must be between 1 and 255 minutes.");
        }
    }
}
