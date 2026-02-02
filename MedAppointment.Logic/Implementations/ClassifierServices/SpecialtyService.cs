using MedAppointment.DataTransferObjects.DoctorDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class SpecialtyService : ISpecialtyService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<SpecialtyService> Logger;
        protected readonly IValidator<SpecialtyCreateDto> SpecialtyCreateValidator;
        protected readonly IValidator<SpecialtyUpdateDto> SpecialtyUpdateValidator;

        public SpecialtyService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<SpecialtyService> logger,
            IValidator<SpecialtyCreateDto> specialtyCreateValidator,
            IValidator<SpecialtyUpdateDto> specialtyUpdateValidator)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            SpecialtyCreateValidator = specialtyCreateValidator;
            SpecialtyUpdateValidator = specialtyUpdateValidator;
        }

        public async Task<Result<IEnumerable<SpecialtyDto>>> GetSpecialtiesAsync()
        {
            Logger.LogTrace("Getting specialty list");
            var result = Result<IEnumerable<SpecialtyDto>>.Create();
            var entities = await UnitOfClassifier.Specialty.GetAllAsync();
            var dtoList = entities.Select(MapSpecialty).ToList();
            result.Success(dtoList);
            Logger.LogInformation("Specialties retrieved: {Count}", dtoList.Count);
            return result;
        }

        public async Task<Result<SpecialtyDto>> GetSpecialtyByIdAsync(long id)
        {
            Logger.LogTrace("Getting specialty by id {SpecialtyId}", id);
            var result = Result<SpecialtyDto>.Create();
            var entity = await UnitOfClassifier.Specialty.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Specialty not found for id {SpecialtyId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapSpecialty(entity));
            Logger.LogInformation("Specialty retrieved for id {SpecialtyId}", id);
            return result;
        }

        public async Task<Result> CreateSpecialtyAsync(SpecialtyCreateDto specialty)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating specialty classifier");
            if (!await ValidateModelAsync(SpecialtyCreateValidator, specialty, result))
            {
                return result;
            }

            if (await UnitOfClassifier.Specialty.AnyAsync(x => x.Key == specialty.Key))
            {
                Logger.LogInformation("Specialty name already exists: {Name}", specialty.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(specialty.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(specialty.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            var entity = new SpecialtyEntity
            {
                Key = specialty.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model,
            };

            try
            {
                await UnitOfClassifier.Specialty.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Specialty created: {Name}", specialty.Key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create specialty classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating specialty classifier {SpecialtyId}", id);
            if (!await ValidateModelAsync(SpecialtyUpdateValidator, specialty, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.Specialty.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Specialty not found for id {SpecialtyId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.Specialty.AnyAsync(x => x.Id != id && x.Key == specialty.Key))
            {
                Logger.LogInformation("Specialty name already exists: {Name}", specialty.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(specialty.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(specialty.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            entity.NameTextId = nameResult.Model;
            entity.DescriptionTextId = descriptionResult.Model;

            try
            {
                UnitOfClassifier.Specialty.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Specialty updated: {SpecialtyId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update specialty classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private SpecialtyDto MapSpecialty(SpecialtyEntity entity)
        {
            return new SpecialtyDto
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name!.Translations.Select(x => new LocalizationDto
                {
                    Key = entity.Name.Key,
                    LanguageId = x.LanguageId,
                    Text = x.Text,
                }).ToList(),
                Description = entity.Description!.Translations.Select(x => new LocalizationDto
                {
                    Key = entity.Description.Key,
                    LanguageId = x.LanguageId,
                    Text = x.Text,
                }).ToList(),
            };
        }

        private async Task<bool> ValidateModelAsync<TDto>(IValidator<TDto> validator, TDto model, Result result)
        {
            Logger.LogInformation("Model validation started for {Validator}.", typeof(TDto).Name);
            var validationResult = await validator.ValidateAsync(model);
            Logger.LogInformation("Model validation finished for {Validator}.", typeof(TDto).Name);
            if (validationResult == null)
            {
                Logger.LogError("Validation result is null for {Validator}.", typeof(TDto).Name);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return false;
            }

            if (!validationResult.IsValid)
            {
                Logger.LogDebug("Validation failed for {Validator} with errors: {Errors}", typeof(TDto).Name, validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return false;
            }

            return true;
        }
    }
}
