namespace MedAppointment.DataTransferObjects.PaginationDtos
{
    public record PaginationQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
