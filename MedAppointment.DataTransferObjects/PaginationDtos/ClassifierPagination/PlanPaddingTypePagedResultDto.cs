namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Paged result for plan padding types. Derives from <see cref="PlanPaddingTypePaginationQueryDto"/> and adds result data.
    /// </summary>
    public record PlanPaddingTypePagedResultDto : PlanPaddingTypePaginationQueryDto
    {
        public IReadOnlyCollection<PlanPaddingTypeDto> Items { get; set; } = new List<PlanPaddingTypeDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
