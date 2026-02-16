using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.UserPagination;

namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class AdminUserService : IAdminUserService
    {
        private readonly ILogger<AdminUserService> _logger;
        private readonly IUnitOfClient _unitOfClient;
        private readonly IPrivateClientInfoService _privateClientInfoService;
        private readonly IValidator<UserPaginationQueryDto> _userPaginationQueryValidator;

        public AdminUserService(
            ILogger<AdminUserService> logger,
            IUnitOfClient unitOfClient,
            IPrivateClientInfoService privateClientInfoService,
            IValidator<UserPaginationQueryDto> userPaginationQueryValidator)
        {
            _logger = logger;
            _unitOfClient = unitOfClient;
            _privateClientInfoService = privateClientInfoService;
            _userPaginationQueryValidator = userPaginationQueryValidator;
        }

        public async Task<Result<UserPagedResultDto>> GetUsersAsync(UserPaginationQueryDto query)
        {
            _logger.LogTrace("Getting user list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}, SurnameFilter: {SurnameFilter}, EmailFilter: {EmailFilter}, PhoneFilter: {PhoneFilter}, UserTypeFilter: {UserTypeFilter}",
                query.PageNumber, query.PageSize, query.NameFilter, query.SurnameFilter, query.EmailFilter, query.PhoneFilter, query.UserTypeFilter);
            var result = Result<UserPagedResultDto>.Create();

            var validationResult = await _userPaginationQueryValidator.ValidateAsync(query);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for UserPaginationQueryDto.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return result;
            }
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("User pagination query validation failed. Errors: {Errors}", validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return result;
            }

            try
            {
                IReadOnlyList<long> orgAdminUserIds = Array.Empty<long>();
                if (query.UserTypeFilter == UserType.OrganizationAdmin || query.UserTypeFilter == UserType.User)
                {
                    var orgAdmins = (await _unitOfClient.OrganizationUser.FindAsync(x => x.IsAdmin)).ToList();
                    orgAdminUserIds = orgAdmins.Select(x => x.UserId).Distinct().ToList();
                    _logger.LogDebug("Organization admin user ids loaded. Count: {Count}", orgAdminUserIds.Count);
                }

                var predicate = BuildUserListPredicate(query, orgAdminUserIds);
                var allMatching = (await _unitOfClient.User.FindAsync(predicate)).ToList();
                var totalCount = allMatching.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
                var pageUsers = allMatching
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                var items = new List<UserListItemDto>();
                foreach (var user in pageUsers)
                {
                    var userTypes = await _privateClientInfoService.GetUserTypesAsync(user.Id);
                    items.Add(MapToUserListItem(user, userTypes));
                }

                result.Success(new UserPagedResultDto
                {
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    NameFilter = query.NameFilter,
                    SurnameFilter = query.SurnameFilter,
                    EmailFilter = query.EmailFilter,
                    PhoneFilter = query.PhoneFilter,
                    UserTypeFilter = query.UserTypeFilter,
                    Items = items,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                });
                _logger.LogInformation("Users retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while getting user list.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
                return result;
            }
        }

        private static Expression<Func<UserEntity, bool>> BuildUserListPredicate(UserPaginationQueryDto query, IReadOnlyList<long> orgAdminUserIds)
        {
            var nameFilter = string.IsNullOrWhiteSpace(query.NameFilter) ? null : query.NameFilter.Trim().ToLowerInvariant();
            var surnameFilter = string.IsNullOrWhiteSpace(query.SurnameFilter) ? null : query.SurnameFilter.Trim().ToLowerInvariant();
            var emailFilter = string.IsNullOrWhiteSpace(query.EmailFilter) ? null : query.EmailFilter.Trim().ToLowerInvariant();
            var phoneFilter = string.IsNullOrWhiteSpace(query.PhoneFilter) ? null : query.PhoneFilter.Trim();

            return u => u.Person != null
                && (nameFilter == null || (u.Person.Name != null && u.Person.Name.ToLower().Contains(nameFilter)))
                && (surnameFilter == null || (u.Person.Surname != null && u.Person.Surname.ToLower().Contains(surnameFilter)))
                && (emailFilter == null || (u.Person.Email != null && u.Person.Email.ToLower().Contains(emailFilter)))
                && (phoneFilter == null || (u.Person.PhoneNumber != null && u.Person.PhoneNumber.Contains(phoneFilter)))
                && (query.UserTypeFilter == null
                    || (query.UserTypeFilter == UserType.SystemAdmin && u.Admin != null)
                    || (query.UserTypeFilter == UserType.Doctor && u.Doctor != null)
                    || (query.UserTypeFilter == UserType.OrganizationAdmin && orgAdminUserIds.Contains(u.Id))
                    || (query.UserTypeFilter == UserType.User && u.Admin == null && u.Doctor == null && !orgAdminUserIds.Contains(u.Id)));
        }

        private static UserListItemDto MapToUserListItem(UserEntity user, UserType[] userTypes)
        {
            var person = user.Person!;
            return new UserListItemDto
            {
                Id = user.Id,
                Name = person.Name,
                Surname = person.Surname,
                FatherName = person.FatherName,
                PhoneNumber = person.PhoneNumber,
                Email = person.Email,
                UserTypes = userTypes
            };
        }

        public async Task<Result<AdminUserDetailsDto>> GetUserDetailsByIdAsync(long userId)
        {
            _logger.LogTrace("Admin user details request started. UserId:{0}", userId);
            var result = Result<AdminUserDetailsDto>.Create();

            try
            {
                var user = await _unitOfClient.User.GetByIdAsync(userId);
                _logger.LogDebug("User fetch completed. UserId:{0}, Exists:{1}", userId, user is not null);

                if (user is null || user.Person is null)
                {
                    _logger.LogInformation("User cannot be found for details. UserId:{0}", userId);
                    result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                    return result;
                }

                var userTypes = await _privateClientInfoService.GetUserTypesAsync(userId);
                _logger.LogDebug("User types fetched. UserId:{0}, TypesCount:{1}", userId, userTypes.Length);

                result.Success(new AdminUserDetailsDto
                {
                    Id = user.Id,
                    Provider = user.Provider,
                    Name = user.Person.Name,
                    Surname = user.Person.Surname,
                    FatherName = user.Person.FatherName,
                    Email = user.Person.Email,
                    PhoneNumber = user.Person.PhoneNumber,
                    BirthDate = user.Person.BirthDate,
                    ImagePath = user.Person.Image?.FilePath,
                    UserTypes = userTypes,
                });

                _logger.LogInformation("Admin user details prepared successfully. UserId:{0}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while getting admin user details. UserId:{0}", userId);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
                return result;
            }
        }

        public async Task<Result> RemoveUserAsync(long userId)
        {
            _logger.LogTrace("Admin user remove request started. UserId:{0}", userId);
            var result = Result.Create();

            try
            {
                var user = await _unitOfClient.User.GetByIdAsync(userId, false);
                _logger.LogDebug("User fetch for delete completed. UserId:{0}, Exists:{1}", userId, user is not null);

                if (user is null)
                {
                    _logger.LogInformation("User cannot be found for delete. UserId:{0}", userId);
                    result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                    return result;
                }

                await _unitOfClient.User.RemoveAsync(userId);
                await _unitOfClient.SaveChangesAsync();

                _logger.LogInformation("User soft deleted successfully. UserId:{0}", userId);
                result.Success(HttpStatusCode.NoContent);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while deleting user. UserId:{0}", userId);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
                return result;
            }
        }
    }
}
