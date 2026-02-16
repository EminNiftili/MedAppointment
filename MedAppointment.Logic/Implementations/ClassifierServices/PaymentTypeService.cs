using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;

using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class PaymentTypeService : IPaymentTypeService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<PaymentTypeService> Logger;
        protected readonly IValidator<PaymentTypeCreateDto> PaymentTypeCreateValidator;
        protected readonly IValidator<PaymentTypeUpdateDto> PaymentTypeUpdateValidator;
        protected readonly IValidator<ClassifierPaginationQueryDto> ClassifierPaginationQueryValidator;
        protected readonly IClassifierFilterExpressionStrategy<PaymentTypeEntity, ClassifierPaginationQueryDto> FilterExpressionStrategy;
        protected readonly ITranslationLookupService TranslationLookup;

        public PaymentTypeService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<PaymentTypeService> logger,
            IValidator<PaymentTypeCreateDto> paymentTypeCreateValidator,
            IValidator<PaymentTypeUpdateDto> paymentTypeUpdateValidator,
            IValidator<ClassifierPaginationQueryDto> classifierPaginationQueryValidator,
            IClassifierFilterExpressionStrategy<PaymentTypeEntity, ClassifierPaginationQueryDto> filterExpressionStrategy,
            ITranslationLookupService translationLookup)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            PaymentTypeCreateValidator = paymentTypeCreateValidator;
            PaymentTypeUpdateValidator = paymentTypeUpdateValidator;
            ClassifierPaginationQueryValidator = classifierPaginationQueryValidator;
            FilterExpressionStrategy = filterExpressionStrategy;
            TranslationLookup = translationLookup;
        }

        public async Task<Result<PaymentTypePagedResultDto>> GetPaymentTypesAsync(ClassifierPaginationQueryDto query)
        {
            Logger.LogTrace("Getting payment type list with pagination and filters. PageNumber: {PageNumber}, PageSize: {PageSize}, NameFilter: {NameFilter}", query.PageNumber, query.PageSize, query.NameFilter);
            var result = Result<PaymentTypePagedResultDto>.Create();

            if (!await ValidateModelAsync(ClassifierPaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Pagination query validation failed for GetPaymentTypesAsync.");
                return result;
            }

            Expression<Func<PaymentTypeEntity, bool>> predicate;
            if (!string.IsNullOrWhiteSpace(query.NameFilter) || !string.IsNullOrWhiteSpace(query.DescriptionFilter))
            {
                var (nameIds, descIds) = await TranslationLookup.GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter);
                predicate = FilterExpressionStrategy.Build(query, nameIds, descIds);
            }
            else
            {
                predicate = FilterExpressionStrategy.Build(query);
            }
            var entities = (await UnitOfClassifier.PaymentType.FindAsync(predicate)).ToList();
            var totalCount = entities.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            var items = entities
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(MapPaymentType)
                .ToList();

            result.Success(new PaymentTypePagedResultDto
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NameFilter = query.NameFilter,
                DescriptionFilter = query.DescriptionFilter,
                Items = items,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
            Logger.LogInformation("Payment types retrieved: {Count} items on page {PageNumber} of {TotalPages}", items.Count, query.PageNumber, totalPages);
            return result;
        }

        public async Task<Result<PaymentTypeDto>> GetPaymentTypeByIdAsync(long id)
        {
            Logger.LogTrace("Getting payment type by id {PaymentTypeId}", id);
            var result = Result<PaymentTypeDto>.Create();
            var entity = await UnitOfClassifier.PaymentType.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Payment type not found for id {PaymentTypeId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapPaymentType(entity));
            Logger.LogInformation("Payment type retrieved for id {PaymentTypeId}", id);
            return result;
        }

        public async Task<Result> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating payment type classifier");
            if (!await ValidateModelAsync(PaymentTypeCreateValidator, paymentType, result))
            {
                return result;
            }

            if (await UnitOfClassifier.PaymentType.AnyAsync(x => x.Key == paymentType.Key))
            {
                Logger.LogInformation("Payment type name already exists: {Name}", paymentType.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync("payment_type_name", paymentType.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync("payment_type_desc", paymentType.Description);

            var entity = new PaymentTypeEntity
            {
                Key = paymentType.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model
            };

            try
            {
                await UnitOfClassifier.PaymentType.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Payment type created: {Name}", paymentType.Key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create payment type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating payment type classifier {PaymentTypeId}", id);
            if (!await ValidateModelAsync(PaymentTypeUpdateValidator, paymentType, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.PaymentType.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Payment type not found for id {PaymentTypeId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.PaymentType.AnyAsync(x => x.Id != id && x.Key == paymentType.Key))
            {
                Logger.LogInformation("Payment type name already exists: {Name}", paymentType.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync("payment_type_name", paymentType.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync("payment_type_desc", paymentType.Description);

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
                UnitOfClassifier.PaymentType.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Payment type updated: {PaymentTypeId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update payment type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private PaymentTypeDto MapPaymentType(PaymentTypeEntity entity)
        {
            return new PaymentTypeDto
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
