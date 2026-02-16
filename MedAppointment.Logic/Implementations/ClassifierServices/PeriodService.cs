using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;

using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class PeriodService : IPeriodService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<PeriodService> Logger;
        protected readonly IValidator<PeriodCreateDto> PeriodCreateValidator;
        protected readonly IValidator<PeriodUpdateDto> PeriodUpdateValidator;
        protected readonly IValidator<PeriodPaginationQueryDto> PeriodPaginationQueryValidator;
        protected readonly IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto> FilterExpressionStrategy;
        protected readonly ITranslationLookupService TranslationLookup;

        public PeriodService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<PeriodService> logger,
            IValidator<PeriodCreateDto> periodCreateValidator,
            IValidator<PeriodUpdateDto> periodUpdateValidator,
            IValidator<PeriodPaginationQueryDto> periodPaginationQueryValidator,
            IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto> filterExpressionStrategy,
            ITranslationLookupService translationLookup)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            PeriodCreateValidator = periodCreateValidator;
            PeriodUpdateValidator = periodUpdateValidator;
            PeriodPaginationQueryValidator = periodPaginationQueryValidator;
            FilterExpressionStrategy = filterExpressionStrategy;
            TranslationLookup = translationLookup;
        }

        public async Task<Result<PeriodPagedResultDto>> GetPeriodsAsync(PeriodPaginationQueryDto query)
        {
            Logger.LogTrace("Getting period list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}, PeriodTime: {PeriodTime}", query.PageNumber, query.PageSize, query.NameFilter, query.PeriodTime);
            var result = Result<PeriodPagedResultDto>.Create();

            if (!await ValidateModelAsync(PeriodPaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Pagination query validation failed for GetPeriodsAsync.");
                return result;
            }

            Expression<Func<PeriodEntity, bool>> predicate;
            if (!string.IsNullOrWhiteSpace(query.NameFilter) || !string.IsNullOrWhiteSpace(query.DescriptionFilter))
            {
                var (nameIds, descIds) = await TranslationLookup.GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter);
                predicate = FilterExpressionStrategy.Build(query, nameIds, descIds);
            }
            else
            {
                predicate = FilterExpressionStrategy.Build(query);
            }
            var entities = (await UnitOfClassifier.Period.FindAsync(predicate)).ToList();
            var totalCount = entities.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            var items = entities
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(MapPeriod)
                .ToList();

            result.Success(new PeriodPagedResultDto
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NameFilter = query.NameFilter,
                DescriptionFilter = query.DescriptionFilter,
                PeriodTime = query.PeriodTime,
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
            Logger.LogInformation("Periods retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
            return result;
        }

        public async Task<Result<PeriodDto>> GetPeriodByIdAsync(long id)
        {
            Logger.LogTrace("Getting period by id {PeriodId}", id);
            var result = Result<PeriodDto>.Create();
            var entity = await UnitOfClassifier.Period.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Period not found for id {PeriodId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapPeriod(entity));
            Logger.LogInformation("Period retrieved for id {PeriodId}", id);
            return result;
        }

        public async Task<Result> CreatePeriodAsync(PeriodCreateDto period)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating period classifier");
            if (!await ValidateModelAsync(PeriodCreateValidator, period, result))
            {
                return result;
            }

            if (await UnitOfClassifier.Period.AnyAsync(x => x.Key == period.Key))
            {
                Logger.LogInformation("Period name already exists: {Name}", period.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync("period_name", period.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync("period_desc", period.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            var entity = new PeriodEntity
            {
                Key = period.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model,
                PeriodTime = period.PeriodTime
            };

            try
            {
                await UnitOfClassifier.Period.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Period created: {Name}", period.Key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create period classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdatePeriodAsync(long id, PeriodUpdateDto period)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating period classifier {PeriodId}", id);
            if (!await ValidateModelAsync(PeriodUpdateValidator, period, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.Period.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Period not found for id {PeriodId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.Period.AnyAsync(x => x.Id != id && x.Key == period.Key))
            {
                Logger.LogInformation("Period name already exists: {Name}", period.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync("period_name", period.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync("period_desc", period.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            entity.NameTextId = nameResult.Model;
            entity.DescriptionTextId = descriptionResult.Model;
            entity.PeriodTime = period.PeriodTime;

            try
            {
                UnitOfClassifier.Period.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Period updated: {PeriodId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update period classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private PeriodDto MapPeriod(PeriodEntity entity)
        {
            return new PeriodDto
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
                PeriodTime = entity.PeriodTime
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
