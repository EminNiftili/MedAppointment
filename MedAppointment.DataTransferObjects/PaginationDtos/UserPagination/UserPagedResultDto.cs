using MedAppointment.DataTransferObjects.UserDtos;

namespace MedAppointment.DataTransferObjects.PaginationDtos.UserPagination
{
    /// <summary>
    /// Paged result for user list.
    /// </summary>
    public record UserPagedResultDto : UserPaginationQueryDto
    {
        public IReadOnlyCollection<UserListItemDto> Items { get; set; } = new List<UserListItemDto>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
