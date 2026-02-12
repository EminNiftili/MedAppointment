using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class PlanPaddingTypeService : IPlanPaddingTypeService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<PlanPaddingTypeService> Logger;
        protected readonly IValidator<PlanPaddingTypeCreateDto> PlanPaddingTypeCreateValidator;
        protected readonly IValidator<PlanPaddingTypeUpdateDto> PlanPaddingTypeUpdateValidator;
        protected readonly IValidator<PlanPaddingTypePaginationQueryDto> PlanPaddingTypePaginationQueryValidator;
        protected readonly IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto> FilterExpressionStrategy;
        protected readonly ITranslationLookupService TranslationLookup;

        public PlanPaddingTypeService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<PlanPaddingTypeService> logger,
            IValidator<PlanPaddingTypeCreateDto> planPaddingTypeCreateValidator,
            IValidator<PlanPaddingTypeUpdateDto> planPaddingTypeUpdateValidator,
            IValidator<PlanPaddingTypePaginationQueryDto> planPaddingTypePaginationQueryValidator,
            IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto> filterExpressionStrategy,
            ITranslationLookupService translationLookup)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            PlanPaddingTypeCreateValidator = planPaddingTypeCreateValidator;
            PlanPaddingTypeUpdateValidator = planPaddingTypeUpdateValidator;
            PlanPaddingTypePaginationQueryValidator = planPaddingTypePaginationQueryValidator;
            FilterExpressionStrategy = filterExpressionStrategy;
            TranslationLookup = translationLookup;
        }

        public async Task<Result<PlanPaddingTypePagedResultDto>> GetPlanPaddingTypesAsync(PlanPaddingTypePaginationQueryDto query)
        {
            Logger.LogTrace("Getting plan padding type list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}, PaddingPosition: {PaddingPosition}, PaddingTime: {PaddingTime}", query.PageNumber, query.PageSize, query.NameFilter, query.PaddingPosition, query.PaddingTime);
            var result = Result<PlanPaddingTypePagedResultDto>.Create();

            if (!await ValidateModelAsync(PlanPaddingTypePaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Pagination query validation failed for GetPlanPaddingTypesAsync.");
                return result;
            }

            Expression<Func<PlanPaddingTypeEntity, bool>> predicate;
            if (!string.IsNullOrWhiteSpace(query.NameFilter) || !string.IsNullOrWhiteSpace(query.DescriptionFilter))
            {
                var (nameIds, descIds) = await TranslationLookup.GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter);
                predicate = FilterExpressionStrategy.Build(query, nameIds, descIds);
            }
            else
            {
                predicate = FilterExpressionStrategy.Build(query);
            }
            var entities = (await UnitOfClassifier.PlanPaddingType.FindAsync(predicate)).ToList();
            var totalCount = entities.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            var items = entities
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(MapPlanPaddingType)
                .ToList();

            result.Success(new PlanPaddingTypePagedResultDto
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NameFilter = query.NameFilter,
                DescriptionFilter = query.DescriptionFilter,
                PaddingPosition = query.PaddingPosition,
                PaddingTime = query.PaddingTime,
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
            Logger.LogInformation("Plan padding types retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
            return result;
        }

        public async Task<Result<PlanPaddingTypeDto>> GetPlanPaddingTypeByIdAsync(long id)
        {
            Logger.LogTrace("Getting plan padding type by id {PlanPaddingTypeId}", id);
            var result = Result<PlanPaddingTypeDto>.Create();
            var entity = await UnitOfClassifier.PlanPaddingType.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Plan padding type not found for id {PlanPaddingTypeId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapPlanPaddingType(entity));
            Logger.LogInformation("Plan padding type retrieved for id {PlanPaddingTypeId}", id);
            return result;
        }

        public async Task<Result> CreatePlanPaddingTypeAsync(PlanPaddingTypeCreateDto planPaddingType)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating plan padding type classifier");
            if (!await ValidateModelAsync(PlanPaddingTypeCreateValidator, planPaddingType, result))
            {
                return result;
            }

            if (await UnitOfClassifier.PlanPaddingType.AnyAsync(x => x.Key == planPaddingType.Key))
            {
                Logger.LogInformation("Plan padding type name already exists: {Name}", planPaddingType.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(planPaddingType.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(planPaddingType.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            var entity = new PlanPaddingTypeEntity
            {
                Key = planPaddingType.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model,
                PaddingPosition = (byte)planPaddingType.PaddingPosition,
                PaddingTime = planPaddingType.PaddingTime
            };

            try
            {
                await UnitOfClassifier.PlanPaddingType.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Plan padding type created: {Name}", planPaddingType.Name);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create plan padding type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdatePlanPaddingTypeAsync(long id, PlanPaddingTypeUpdateDto planPaddingType)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating plan padding type classifier {PlanPaddingTypeId}", id);
            if (!await ValidateModelAsync(PlanPaddingTypeUpdateValidator, planPaddingType, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.PlanPaddingType.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Plan padding type not found for id {PlanPaddingTypeId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.PlanPaddingType.AnyAsync(x => x.Id != id && x.Key == planPaddingType.Key))
            {
                Logger.LogInformation("Plan padding type name already exists: {0}", planPaddingType.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(planPaddingType.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(planPaddingType.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            entity.NameTextId = nameResult.Model;
            entity.DescriptionTextId = descriptionResult.Model;
            entity.PaddingPosition = (byte)planPaddingType.PaddingPosition;
            entity.PaddingTime = planPaddingType.PaddingTime;

            try
            {
                UnitOfClassifier.PlanPaddingType.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Plan padding type updated: {PlanPaddingTypeId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update plan padding type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private static PlanPaddingTypeDto MapPlanPaddingType(PlanPaddingTypeEntity entity)
        {
            return new PlanPaddingTypeDto
            {
                Id = entity.Id,
                Name = entity.Name!.Translations.Select(x => new LocalizationDto
                {
                    Key = x.Resource!.Key,
                    LanguageId = x.LanguageId,
                    Text = x.Text,
                }).ToList(),
                Description = entity.Description!.Translations.Select(x => new LocalizationDto
                {
                    Key = x.Resource!.Key,
                    LanguageId = x.LanguageId,
                    Text = x.Text,
                }).ToList(),
                PaddingPosition = (PlanPaddingPosition)entity.PaddingPosition,
                PaddingTime = entity.PaddingTime
            };
        }

        private async Task<bool> ValidateModelAsync<TDto>(IValidator<TDto> validator, TDto model, Result result)
        {
            Logger.LogDebug("Model validation started for {Validator}.", typeof(TDto).Name);
            var validationResult = await validator.ValidateAsync(model);
            Logger.LogDebug("Model validation finished for {Validator}.", typeof(TDto).Name);
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
