namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class CurrencyService : ICurrencyService
    {
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<CurrencyService> Logger;
        protected readonly IValidator<CurrencyCreateDto> CurrencyCreateValidator;
        protected readonly IValidator<CurrencyUpdateDto> CurrencyUpdateValidator;

        public CurrencyService(
            IUnitOfClassifier unitOfClassifier,
            ILogger<CurrencyService> logger,
            IValidator<CurrencyCreateDto> currencyCreateValidator,
            IValidator<CurrencyUpdateDto> currencyUpdateValidator)
        {
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

            if (await UnitOfClassifier.Currency.AnyAsync(x => x.Name == currency.Name))
            {
                Logger.LogInformation("Currency name already exists: {Name}", currency.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var entity = new CurrencyEntity
            {
                Name = currency.Name,
                Description = currency.Description,
                Coefficent = currency.Coefficent
            };

            try
            {
                await UnitOfClassifier.Currency.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Currency created: {Name}", currency.Name);
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

            if (await UnitOfClassifier.Currency.AnyAsync(x => x.Id != id && x.Name == currency.Name))
            {
                Logger.LogInformation("Currency name already exists: {Name}", currency.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = currency.Name;
            entity.Description = currency.Description;
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
                Name = entity.Name,
                Description = entity.Description,
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
