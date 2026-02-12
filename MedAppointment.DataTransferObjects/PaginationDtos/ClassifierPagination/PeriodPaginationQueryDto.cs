namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Request DTO for paged period list. Base filters (Name, Description) + PeriodTime (exact).
    /// </summary>
    public record PeriodPaginationQueryDto : ClassifierPaginationQueryDto
    {
        /// <summary>Optional. Filter periods with this exact period time (minutes).</summary>
        public byte? PeriodTime { get; set; }
    }
}
