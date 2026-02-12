namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    public record CurrencyPagedResultDto : CurrencyPaginationQueryDto
    {
        public IReadOnlyCollection<CurrencyDto> Items { get; set; } = new List<CurrencyDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
