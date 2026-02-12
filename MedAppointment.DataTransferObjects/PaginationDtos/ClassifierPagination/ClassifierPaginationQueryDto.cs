namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Base request DTO for classifier list (paged) endpoints.
    /// Pagination (PageNumber, PageSize) from <see cref="PaginationQueryDto"/>.
    /// Common filters: NameFilter, DescriptionFilter (optional, contains).
    /// Use this type when no entity-specific filters exist (e.g. Specialty, PaymentType).
    /// </summary>
    public record ClassifierPaginationQueryDto : PaginationQueryDto
    {
        /// <summary>Optional. Filter by name contains (case-insensitive).</summary>
        public string? NameFilter { get; set; }
        /// <summary>Optional. Filter by description contains (case-insensitive).</summary>
        public string? DescriptionFilter { get; set; }
    }
}
