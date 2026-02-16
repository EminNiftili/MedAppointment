using MedAppointment.DataTransferObjects.Enums;
using MedAppointment.DataTransferObjects.PaginationDtos;

namespace MedAppointment.DataTransferObjects.PaginationDtos.UserPagination
{
    /// <summary>
    /// Request DTO for paged user list. Pagination + optional filters (name, email, phone, user type).
    /// </summary>
    public record UserPaginationQueryDto : PaginationQueryDto
    {
        /// <summary>Optional. Filter by name (Person Name) contains (case-insensitive).</summary>
        public string? NameFilter { get; set; }
        /// <summary>Optional. Filter by surname contains (case-insensitive).</summary>
        public string? SurnameFilter { get; set; }
        /// <summary>Optional. Filter by email contains (case-insensitive).</summary>
        public string? EmailFilter { get; set; }
        /// <summary>Optional. Filter by phone number contains.</summary>
        public string? PhoneFilter { get; set; }
        /// <summary>Optional. Filter by user role (UserType).</summary>
        public UserType? UserTypeFilter { get; set; }
    }
}
