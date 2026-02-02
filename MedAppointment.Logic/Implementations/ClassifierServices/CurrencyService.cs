using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class CurrencyService : ICurrencyService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<CurrencyService> Logger;
        protected readonly IValidator<CurrencyCreateDto> CurrencyCreateValidator;
        protected readonly IValidator<CurrencyUpdateDto> CurrencyUpdateValidator;

        public CurrencyService(
            ILocalizerService localizerService,
            IUnitOfClassifier unitOfClassifier,
            ILogger<CurrencyService> logger,
            IValidator<CurrencyCreateDto> currencyCreateValidator,
            IValidator<CurrencyUpdateDto> currencyUpdateValidator)
        {
            LocalizerService = localizerService;
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            CurrencyCreateValidator = currencyCreateValidator;
            CurrencyUpdateValidator = currencyUpdateValidator;
        }

        public async Task<Result<IEnumerable<CurrencyDto>>> GetCurrenciesAsync()
        {
            Logger.LogTrace("Getting currency list");
            var result = Result<IEnumerable<CurrencyDto>>.Create();
            var entities = await UnitOfClassifier.Currency.GetAllAsync();
            var dtoList = entities.Select(MapCurrency).ToList();
            result.Success(dtoList);
            Logger.LogInformation("Currencies retrieved: {Count}", dtoList.Count);
            return result;
        }

        public async Task<Result<CurrencyDto>> GetCurrencyByIdAsync(long id)
        {
            Logger.LogTrace("Getting currency by id {CurrencyId}", id);
            var result = Result<CurrencyDto>.Create();
            var entity = await UnitOfClassifier.Currency.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Currency not found for id {CurrencyId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapCurrency(entity));
            Logger.LogInformation("Currency retrieved for id {CurrencyId}", id);
            return result;
        }

        public async Task<Result> CreateCurrencyAsync(CurrencyCreateDto currency)
        {
            var result = Result.Create();
            Logger.LogTrace("Creating currency classifier");
            if (!await ValidateModelAsync(CurrencyCreateValidator, currency, result))
            {
                return result;
            }

            if (await UnitOfClassifier.Currency.AnyAsync(x => x.Key == currency.Key))
            {
                Logger.LogInformation("Currency name already exists: {Name}", currency.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }
            var nameResult = await LocalizerService.AddResourceAsync(currency.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(currency.Description);

            if(!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            var entity = new CurrencyEntity
            {
                Key = currency.Key,
                NameTextId = nameResult.Model,
                DescriptionTextId = descriptionResult.Model,
                Coefficent = currency.Coefficent
            };

            try
            {
                await UnitOfClassifier.Currency.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Currency created: {Name}", currency.Key);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to create currency classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency)
        {
            var result = Result.Create();
            Logger.LogTrace("Updating currency classifier {CurrencyId}", id);
            if (!await ValidateModelAsync(CurrencyUpdateValidator, currency, result))
            {
                return result;
            }

            var entity = await UnitOfClassifier.Currency.GetByIdAsync(id);
            if (entity == null)
            {
                Logger.LogInformation("Currency not found for id {CurrencyId}", id);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await UnitOfClassifier.Currency.AnyAsync(x => x.Id != id && x.Key == currency.Key))
            {
                Logger.LogInformation("Currency name already exists: {Name}", currency.Key);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var nameResult = await LocalizerService.AddResourceAsync(currency.Name);
            var descriptionResult = await LocalizerService.AddResourceAsync(currency.Description);

            if (!nameResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
                result.MergeResult(nameResult);
                result.MergeResult(descriptionResult);
                return result;
            }

            entity.NameTextId = nameResult.Model;
            entity.DescriptionTextId = descriptionResult.Model;
            entity.Coefficent = currency.Coefficent;

            try
            {
                UnitOfClassifier.Currency.Update(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Currency updated: {CurrencyId}", id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to update currency classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        private CurrencyDto MapCurrency(CurrencyEntity entity)
        {
            return new CurrencyDto
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
                Coefficent = entity.Coefficent
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
