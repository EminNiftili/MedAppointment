namespace MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination
{
    /// <summary>
    /// Request DTO for paged currency list. Base filters + coefficient range.
    /// </summary>
    public record CurrencyPaginationQueryDto : ClassifierPaginationQueryDto
    {
        /// <summary>Optional. Filter currencies with coefficient &gt;= this value.</summary>
        public decimal? CoefficentMin { get; set; }
        /// <summary>Optional. Filter currencies with coefficient &lt;= this value.</summary>
        public decimal? CoefficentMax { get; set; }
    }
}
