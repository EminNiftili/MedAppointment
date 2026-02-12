namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Request DTO for paged plan padding type list. Base filters + PaddingPosition, PaddingTime.
    /// </summary>
    public record PlanPaddingTypePaginationQueryDto : ClassifierPaginationQueryDto
    {
        /// <summary>Optional. Filter by exact padding position (enum).</summary>
        public PlanPaddingPosition? PaddingPosition { get; set; }
        /// <summary>Optional. Filter by exact padding time (minutes).</summary>
        public byte? PaddingTime { get; set; }
    }
}
