using MedAppointment.DataTransferObjects.ClassifierDtos;

namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    public record LanguagePagedResultDto : LanguagePaginationQueryDto
    {
        public IReadOnlyCollection<LanguageDto> Items { get; set; } = new List<LanguageDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
