namespace MedAppointment.DataTransferObjects.PaginationDtos
{
    public record PagedResultDto<TItem>
    {
        public IReadOnlyCollection<TItem> Items { get; set; } = Array.Empty<TItem>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
