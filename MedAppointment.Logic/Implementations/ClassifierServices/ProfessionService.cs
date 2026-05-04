using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class ProfessionService : IProfessionService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<ProfessionService> Logger;
        protected readonly IValidator<ProfessionCreateDto> ProfessionCreateValidator;
        protected readonly IValidator<ProfessionUpdateDto> ProfessionUpdateValidator;
        protected readonly IValidator<ClassifierPaginationQueryDto> ClassifierPaginationQueryValidator;
        protected readonly IClassifierFilterExpressionStrategy<ProfessionEntity, ClassifierPaginationQueryDto> FilterExpressionStrategy;
        protected readonly ITranslationLookupService TranslationLookup;

        public ProfessionService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<ProfessionService> logger,
            IValidator<ProfessionCreateDto> professionCreateValidator,
            IValidator<ProfessionUpdateDto> professionUpdateValidator,
            IValidator<ClassifierPaginationQueryDto> classifierPaginationQueryValidator,
            IClassifierFilterExpressionStrategy<ProfessionEntity, ClassifierPaginationQueryDto> filterExpressionStrategy,
            ITranslationLookupService translationLookup)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            ProfessionCreateValidator = professionCreateValidator;
            ProfessionUpdateValidator = professionUpdateValidator;
            ClassifierPaginationQueryValidator = classifierPaginationQueryValidator;
            FilterExpressionStrategy = filterExpressionStrategy;
            TranslationLookup = translationLookup;
        }

        public async Task<Result<ProfessionPagedResultDto>> GetProfessionsAsync(ClassifierPaginationQueryDto query)
        {
            Logger.LogTrace("Getting profession list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}, DescriptionFilter: {DescriptionFilter}", query.PageNumber, query.PageSize, query.NameFilter, query.DescriptionFilter);
            var result = Result<ProfessionPagedResultDto>.Create();

            if (!await ValidateModelAsync(ClassifierPaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Pagination query validation failed for GetProfessionsAsync.");
                return result;
            }

            Expression<Func<ProfessionEntity, bool>> predicate;
            if (!string.IsNullOrWhiteSpace(query.NameFilter) || !string.IsNullOrWhiteSpace(query.DescriptionFilter))
            {
                var (nameIds, descIds) = await TranslationLookup.GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter);
                predicate = FilterExpressionStrategy.Build(query, nameIds, descIds);
            }
            else
            {
                predicate = FilterExpressionStrategy.Build(query);
            }

            var entities = (await UnitOfClassifier.Profession.FindAsync(predicate)).ToList();
            var totalCount = entities.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            var items = entities
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(MapProfession)
                .ToList();

            result.Success(new ProfessionPagedResultDto
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NameFilter = query.NameFilter,
                DescriptionFilter = query.DescriptionFilter,
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
            Logger.LogInformation("Professions retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
            return result;
        }

        public async Task<Result<ProfessionDto>> GetProfessionByIdAsync(long id)
        {
            Logger.LogTrace("Getting profession by id {ProfessionId}", id);
            var result = Result<ProfessionDto>.Create();
            var entity = await UnitOfClassifier.Profession.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Profession not found for id {ProfessionId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapProfession(entity));
            Logger.LogInformation("Profession retrieved for id {ProfessionId}", id);
            return result;
        }

        public async Task<Result> CreateProfessionAsync(ProfessionCreateDto profession)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating profession classifier");
            if (!await ValidateModelAsync(ProfessionCreateValidator, profession, result))
            {
                return result;
            }

            if (await UnitOfClassifier.Profession.AnyAsync(x => x.Key == profession.Key))
            {
                Logger.LogInformation("Profession key already exists: {Key}", profession.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync("profession_name", profession.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync("profession_desc", profession.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            var entity = new ProfessionEntity
            {
                Key = profession.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model,
            };

            try
            {
                await UnitOfClassifier.Profession.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Profession created: {Key}", profession.Key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create profession classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateProfessionAsync(long id, ProfessionUpdateDto profession)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating profession classifier {ProfessionId}", id);
            if (!await ValidateModelAsync(ProfessionUpdateValidator, profession, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.Profession.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Profession not found for id {ProfessionId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.Profession.AnyAsync(x => x.Id != id && x.Key == profession.Key))
            {
                Logger.LogInformation("Profession key already exists: {Key}", profession.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(entity.Name!.Key, profession.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(entity.Description!.Key, profession.Description);

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
                UnitOfClassifier.Profession.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Profession updated: {ProfessionId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update profession classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private ProfessionDto MapProfession(ProfessionEntity entity)
        {
            return new ProfessionDto
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
