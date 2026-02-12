namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Paged result for periods. Derives from <see cref="PeriodPaginationQueryDto"/> and adds result data.
    /// </summary>
    public record PeriodPagedResultDto : PeriodPaginationQueryDto
    {
        public IReadOnlyCollection<PeriodDto> Items { get; set; } = new List<PeriodDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
