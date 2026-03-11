using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Composition;

namespace MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe
{
    internal class DoctorUserMeStrategy : IUserMeResponseStrategy
    {
        private readonly ILogger<DoctorUserMeStrategy> _logger;
        private readonly IUnitOfClient _unitOfClient;
        private readonly IUnitOfDoctor _unitOfDoctor;
        private readonly IPrivateClientInfoService _privateClientInfoService;

        public DoctorUserMeStrategy(
            ILogger<DoctorUserMeStrategy> logger,
            IUnitOfClient unitOfClient,
            IUnitOfDoctor unitOfDoctor,
            IPrivateClientInfoService privateClientInfoService)
        {
            _logger = logger;
            _unitOfClient = unitOfClient;
            _unitOfDoctor = unitOfDoctor;
            _privateClientInfoService = privateClientInfoService;
        }

        public bool CanHandle(UserType[] userTypes)
        {
            return userTypes.Contains(UserType.Doctor);
        }

        public async Task<Result<object>> BuildAsync(long userId)
        {
            _logger.LogTrace("Building doctor user me response. UserId: {UserId}", userId);
            var result = Result<object>.Create();

            var user = await _unitOfClient.User.GetByIdAsync(userId);
            _logger.LogDebug("User loaded for me. UserId: {UserId}, Found: {Found}", userId, user != null);

            if (user is null || user.Person is null)
            {
                _logger.LogInformation("User or person not found for me. UserId: {UserId}", userId);
                result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                return result;
            }

            if (user.Doctor is null)
            {
                _logger.LogInformation("User is not a doctor. UserId: {UserId}", userId);
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }

            var doctor = await _unitOfDoctor.Doctor.GetByIdAsync(user.Doctor.Id);
            _logger.LogDebug("Doctor loaded for me. DoctorId: {DoctorId}, Found: {Found}", user.Doctor.Id, doctor != null);

            if (doctor is null)
            {
                _logger.LogInformation("Doctor entity not found. UserId: {UserId}, DoctorId: {DoctorId}", userId, user.Doctor.Id);
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }

            var userTypes = await _privateClientInfoService.GetUserTypesAsync(userId);
            var titleText = GetFirstTranslationText(doctor.Title);
            var descriptionText = GetFirstTranslationText(doctor.Description);
            var specialties = MapSpecialties(doctor.Specialties);

            var dto = new DoctorUserMeDto
            {
                Id = user.Id,
                Provider = user.Provider,
                Name = user.Person.Name ?? string.Empty,
                Surname = user.Person.Surname ?? string.Empty,
                FatherName = user.Person.FatherName ?? string.Empty,
                Email = user.Person.Email ?? string.Empty,
                PhoneNumber = user.Person.PhoneNumber ?? string.Empty,
                BirthDate = user.Person.BirthDate,
                ImagePath = user.Person.Image?.FilePath,
                UserTypes = userTypes,
                DoctorId = doctor.Id,
                IsConfirm = doctor.IsConfirm,
                ProfessionId = doctor.ProfessionId,
                Title = titleText,
                Description = descriptionText,
                PresentationVideoUrl = doctor.PresentationVideoUrl,
                Specialties = specialties
            };

            result.Success(dto, HttpStatusCode.OK);
            _logger.LogInformation("Doctor user me response built. UserId: {UserId}, DoctorId: {DoctorId}", userId, doctor.Id);
            return result;
        }

        private static string GetFirstTranslationText(Entities.Localization.ResourceEntity? resource)
        {
            if (resource?.Translations == null || !resource.Translations.Any())
                return string.Empty;
            return resource.Translations.First().Text ?? string.Empty;
        }

        private static IReadOnlyCollection<DoctorSpecialtyDto> MapSpecialties(IEnumerable<DoctorSpecialtyEntity>? doctorSpecialties)
        {
            if (doctorSpecialties == null)
                return Array.Empty<DoctorSpecialtyDto>();

            return doctorSpecialties
                .Where(ds => ds.Specialty != null)
                .Select(ds =>
                {
                    var s = ds.Specialty!;
                    return new DoctorSpecialtyDto
                    {
                        Id = ds.SpecialtyId,
                        IsConfirm = ds.IsConfirm,
                        Key = s.Name?.Key ?? string.Empty,
                        Name = s.Name != null && s.Name.Translations != null
                            ? s.Name.Translations.Select(t => new LocalizationDto { Key = s.Name.Key, LanguageId = t.LanguageId, Text = t.Text }).ToList()
                            : new List<LocalizationDto>(),
                        Description = s.Description != null && s.Description.Translations != null
                            ? s.Description.Translations.Select(t => new LocalizationDto { Key = s.Description.Key, LanguageId = t.LanguageId, Text = t.Text }).ToList()
                            : new List<LocalizationDto>()
                    };
                })
                .ToList();
        }
    }
}
