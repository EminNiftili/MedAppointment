namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Request DTO for paged language list. Pagination + optional name filter.
    /// </summary>
    public record LanguagePaginationQueryDto : PaginationQueryDto
    {
        public string? NameFilter { get; set; }
        public bool? IsDefaultFilter { get; set; }
    }
}
