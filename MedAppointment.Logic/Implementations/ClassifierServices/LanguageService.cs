using System.Linq.Expressions;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class LanguageService : ILanguageService
    {
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<LanguageService> Logger;
        protected readonly IValidator<LanguageCreateDto> LanguageCreateValidator;
        protected readonly IValidator<LanguageUpdateDto> LanguageUpdateValidator;
        protected readonly IValidator<LanguagePaginationQueryDto> LanguagePaginationQueryValidator;

        public LanguageService(
            IUnitOfClassifier unitOfClassifier,
            ILogger<LanguageService> logger,
            IValidator<LanguageCreateDto> languageCreateValidator,
            IValidator<LanguageUpdateDto> languageUpdateValidator,
            IValidator<LanguagePaginationQueryDto> languagePaginationQueryValidator)
        {
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            LanguageCreateValidator = languageCreateValidator;
            LanguageUpdateValidator = languageUpdateValidator;
            LanguagePaginationQueryValidator = languagePaginationQueryValidator;
        }

        public async Task<Result<LanguagePagedResultDto>> GetLanguagesAsync(LanguagePaginationQueryDto query)
        {
            Logger.LogTrace("Getting language list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}", query.PageNumber, query.PageSize, query.NameFilter);
            var result = Result<LanguagePagedResultDto>.Create();

            if (!await ValidateModelAsync(LanguagePaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Pagination query validation failed for GetLanguagesAsync.");
                return result;
            }

            Expression<Func<LanguageEntity, bool>> predicate = BuildPredicate(query);
            var entities = (await UnitOfClassifier.Language.FindAsync(predicate)).ToList();
            var totalCount = entities.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            var items = entities
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(MapLanguage)
                .ToList();

            result.Success(new LanguagePagedResultDto
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NameFilter = query.NameFilter,
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
            Logger.LogInformation("Languages retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
            return result;
        }

        public async Task<Result<LanguageDto>> GetLanguageByIdAsync(long id)
        {
            Logger.LogTrace("Getting language by id {LanguageId}", id);
            var result = Result<LanguageDto>.Create();
            var entity = await UnitOfClassifier.Language.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Language not found for id {LanguageId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapLanguage(entity));
            Logger.LogInformation("Language retrieved for id {LanguageId}", id);
            return result;
        }

        public async Task<Result> CreateLanguageAsync(LanguageCreateDto language)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating language classifier. Name: {Name}, IsDefault: {IsDefault}", language.Name, language.IsDefault);
            if (!await ValidateModelAsync(LanguageCreateValidator, language, result))
            {
                return result;
            }

            if (await UnitOfClassifier.Language.AnyAsync(x => x.Name == language.Name))
            {
                Logger.LogInformation("Language name already exists: {Name}", language.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            if(language.IsDefault && await UnitOfClassifier.Language.AnyAsync(x => x.IsDefault))
            {
                result.AddMessage("ERR00146", "Default Language already setted.", HttpStatusCode.Conflict);
                return result;
            }

            var entity = new LanguageEntity
            {
                Name = language.Name,
                IsDefault = language.IsDefault
            };

            try
            {
                await UnitOfClassifier.Language.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Language created: {Name}", language.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create language classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateLanguageAsync(long id, LanguageUpdateDto language)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating language classifier {LanguageId}. Name: {Name}, IsDefault: {IsDefault}", id, language.Name, language.IsDefault);
            if (!await ValidateModelAsync(LanguageUpdateValidator, language, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.Language.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Language not found for id {LanguageId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.Language.AnyAsync(x => x.Id != id && x.Name == language.Name))
            {
                Logger.LogInformation("Language name already exists: {Name}", language.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = language.Name;
            entity.IsDefault = language.IsDefault;

            try
            {
                UnitOfClassifier.Language.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Language updated: {LanguageId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update language classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> DeleteLanguageAsync(long id)
        {
            var result = Result.Create();
            Logger.LogTrace("Deleting language classifier {LanguageId}", id);

            var entity = await UnitOfClassifier.Language.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Language not found for id {LanguageId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (entity.IsDefault)
            {
                Logger.LogInformation("Attempt to delete default language {LanguageId} rejected.", id);
                result.AddMessage("ERR00144", "Default language cannot be deleted.", HttpStatusCode.Conflict);
                return result;
            }

            try
            {
                await UnitOfClassifier.Language.RemoveAsync(id);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Language deleted: {LanguageId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Unhandled error while deleting language {LanguageId}.", id);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private static Expression<Func<LanguageEntity, bool>> BuildPredicate(LanguagePaginationQueryDto query)
        {
            if (string.IsNullOrWhiteSpace(query.NameFilter))
                return _ => true;

            var nameFilter = query.NameFilter.Trim().ToLowerInvariant();
            return x => x.Name.ToLower().Contains(nameFilter);
        }

        private static LanguageDto MapLanguage(LanguageEntity entity)
        {
            return new LanguageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsDefault = entity.IsDefault
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
