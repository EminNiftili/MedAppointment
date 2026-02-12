namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    public record ClassifierPagedResultDto<TDto> : ClassifierPaginationQueryDto
    {
        public IReadOnlyCollection<TDto> Items { get; set; } = new List<TDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
