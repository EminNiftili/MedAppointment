namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class PeriodService : IPeriodService
    {
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly ILogger<PeriodService> Logger;
        protected readonly IValidator<PeriodCreateDto> PeriodCreateValidator;
        protected readonly IValidator<PeriodUpdateDto> PeriodUpdateValidator;

        public PeriodService(
            IUnitOfClassifier unitOfClassifier,
            ILogger<PeriodService> logger,
            IValidator<PeriodCreateDto> periodCreateValidator,
            IValidator<PeriodUpdateDto> periodUpdateValidator)
        {
            UnitOfClassifier = unitOfClassifier;
            Logger = logger;
            PeriodCreateValidator = periodCreateValidator;
            PeriodUpdateValidator = periodUpdateValidator;
        }

        public async Task<Result<IEnumerable<PeriodDto>>> GetPeriodsAsync()
        {
            Logger.LogTrace("Getting period list");
            var result = Result<IEnumerable<PeriodDto>>.Create();
            var entities = await UnitOfClassifier.Period.GetAllAsync();
            var dtoList = entities.Select(MapPeriod).ToList();
            result.Success(dtoList);
            Logger.LogInformation("Periods retrieved: {Count}", dtoList.Count);
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

            if (await UnitOfClassifier.Period.AnyAsync(x => x.Name == period.Name))
            {
                Logger.LogInformation("Period name already exists: {Name}", period.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var entity = new PeriodEntity
            {
                Name = period.Name,
                Description = period.Description,
                PeriodTime = period.PeriodTime
            };

            try
            {
                await UnitOfClassifier.Period.AddAsync(entity);
                await UnitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.LogInformation("Period created: {Name}", period.Name);
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

            if (await UnitOfClassifier.Period.AnyAsync(x => x.Id != id && x.Name == period.Name))
            {
                Logger.LogInformation("Period name already exists: {Name}", period.Name);
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = period.Name;
            entity.Description = period.Description;
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
                Name = entity.Name,
                Description = entity.Description,
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
