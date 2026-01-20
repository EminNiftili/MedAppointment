namespace MedAppointment.Logics.Implementations.ClassifierServices
{
    internal class ClassifierService : IClassifierService
    {
        private readonly IUnitOfClassifier _unitOfClassifier;
        private readonly ILogger<ClassifierService> _logger;
        private readonly IValidator<CurrencyCreateDto> _currencyCreateValidator;
        private readonly IValidator<CurrencyUpdateDto> _currencyUpdateValidator;
        private readonly IValidator<PaymentTypeCreateDto> _paymentTypeCreateValidator;
        private readonly IValidator<PaymentTypeUpdateDto> _paymentTypeUpdateValidator;
        private readonly IValidator<PeriodCreateDto> _periodCreateValidator;
        private readonly IValidator<PeriodUpdateDto> _periodUpdateValidator;
        private readonly IValidator<SpecialtyCreateDto> _specialtyCreateValidator;
        private readonly IValidator<SpecialtyUpdateDto> _specialtyUpdateValidator;

        public ClassifierService(
            IUnitOfClassifier unitOfClassifier,
            ILogger<ClassifierService> logger,
            IValidator<CurrencyCreateDto> currencyCreateValidator,
            IValidator<CurrencyUpdateDto> currencyUpdateValidator,
            IValidator<PaymentTypeCreateDto> paymentTypeCreateValidator,
            IValidator<PaymentTypeUpdateDto> paymentTypeUpdateValidator,
            IValidator<PeriodCreateDto> periodCreateValidator,
            IValidator<PeriodUpdateDto> periodUpdateValidator,
            IValidator<SpecialtyCreateDto> specialtyCreateValidator,
            IValidator<SpecialtyUpdateDto> specialtyUpdateValidator)
        {
            _unitOfClassifier = unitOfClassifier;
            _logger = logger;
            _currencyCreateValidator = currencyCreateValidator;
            _currencyUpdateValidator = currencyUpdateValidator;
            _paymentTypeCreateValidator = paymentTypeCreateValidator;
            _paymentTypeUpdateValidator = paymentTypeUpdateValidator;
            _periodCreateValidator = periodCreateValidator;
            _periodUpdateValidator = periodUpdateValidator;
            _specialtyCreateValidator = specialtyCreateValidator;
            _specialtyUpdateValidator = specialtyUpdateValidator;
        }

        public async Task<Result<IEnumerable<CurrencyDto>>> GetCurrenciesAsync()
        {
            var result = Result<IEnumerable<CurrencyDto>>.Create();
            var entities = await _unitOfClassifier.Currency.GetAllAsync();
            var dtoList = entities.Select(MapCurrency).ToList();
            result.Success(dtoList);
            return result;
        }

        public async Task<Result<CurrencyDto>> GetCurrencyByIdAsync(long id)
        {
            var result = Result<CurrencyDto>.Create();
            var entity = await _unitOfClassifier.Currency.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapCurrency(entity));
            return result;
        }

        public async Task<Result> CreateCurrencyAsync(CurrencyCreateDto currency)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_currencyCreateValidator, currency, result))
            {
                return result;
            }

            if (await _unitOfClassifier.Currency.AnyAsync(x => x.Name == currency.Name))
            {
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
                await _unitOfClassifier.Currency.AddAsync(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create currency classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_currencyUpdateValidator, currency, result))
            {
                return result;
            }

            var entity = await _unitOfClassifier.Currency.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await _unitOfClassifier.Currency.AnyAsync(x => x.Id != id && x.Name == currency.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = currency.Name;
            entity.Description = currency.Description;
            entity.Coefficent = currency.Coefficent;

            try
            {
                _unitOfClassifier.Currency.Update(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update currency classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result<IEnumerable<PaymentTypeDto>>> GetPaymentTypesAsync()
        {
            var result = Result<IEnumerable<PaymentTypeDto>>.Create();
            var entities = await _unitOfClassifier.PaymentType.GetAllAsync();
            var dtoList = entities.Select(MapPaymentType).ToList();
            result.Success(dtoList);
            return result;
        }

        public async Task<Result<PaymentTypeDto>> GetPaymentTypeByIdAsync(long id)
        {
            var result = Result<PaymentTypeDto>.Create();
            var entity = await _unitOfClassifier.PaymentType.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapPaymentType(entity));
            return result;
        }

        public async Task<Result> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_paymentTypeCreateValidator, paymentType, result))
            {
                return result;
            }

            if (await _unitOfClassifier.PaymentType.AnyAsync(x => x.Name == paymentType.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var entity = new PaymentTypeEntity
            {
                Name = paymentType.Name,
                Description = paymentType.Description
            };

            try
            {
                await _unitOfClassifier.PaymentType.AddAsync(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create payment type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_paymentTypeUpdateValidator, paymentType, result))
            {
                return result;
            }

            var entity = await _unitOfClassifier.PaymentType.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await _unitOfClassifier.PaymentType.AnyAsync(x => x.Id != id && x.Name == paymentType.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = paymentType.Name;
            entity.Description = paymentType.Description;

            try
            {
                _unitOfClassifier.PaymentType.Update(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update payment type classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result<IEnumerable<PeriodDto>>> GetPeriodsAsync()
        {
            var result = Result<IEnumerable<PeriodDto>>.Create();
            var entities = await _unitOfClassifier.Period.GetAllAsync();
            var dtoList = entities.Select(MapPeriod).ToList();
            result.Success(dtoList);
            return result;
        }

        public async Task<Result<PeriodDto>> GetPeriodByIdAsync(long id)
        {
            var result = Result<PeriodDto>.Create();
            var entity = await _unitOfClassifier.Period.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapPeriod(entity));
            return result;
        }

        public async Task<Result> CreatePeriodAsync(PeriodCreateDto period)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_periodCreateValidator, period, result))
            {
                return result;
            }

            if (await _unitOfClassifier.Period.AnyAsync(x => x.Name == period.Name))
            {
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
                await _unitOfClassifier.Period.AddAsync(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create period classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdatePeriodAsync(long id, PeriodUpdateDto period)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_periodUpdateValidator, period, result))
            {
                return result;
            }

            var entity = await _unitOfClassifier.Period.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await _unitOfClassifier.Period.AnyAsync(x => x.Id != id && x.Name == period.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = period.Name;
            entity.Description = period.Description;
            entity.PeriodTime = period.PeriodTime;

            try
            {
                _unitOfClassifier.Period.Update(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update period classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result<IEnumerable<SpecialtyDto>>> GetSpecialtiesAsync()
        {
            var result = Result<IEnumerable<SpecialtyDto>>.Create();
            var entities = await _unitOfClassifier.Specialty.GetAllAsync();
            var dtoList = entities.Select(MapSpecialty).ToList();
            result.Success(dtoList);
            return result;
        }

        public async Task<Result<SpecialtyDto>> GetSpecialtyByIdAsync(long id)
        {
            var result = Result<SpecialtyDto>.Create();
            var entity = await _unitOfClassifier.Specialty.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapSpecialty(entity));
            return result;
        }

        public async Task<Result> CreateSpecialtyAsync(SpecialtyCreateDto specialty)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_specialtyCreateValidator, specialty, result))
            {
                return result;
            }

            if (await _unitOfClassifier.Specialty.AnyAsync(x => x.Name == specialty.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            var entity = new SpecialtyEntity
            {
                Name = specialty.Name,
                Description = specialty.Description
            };

            try
            {
                await _unitOfClassifier.Specialty.AddAsync(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create specialty classifier.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }

            return result;
        }

        public async Task<Result> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty)
        {
            var result = Result.Create();
            if (!await ValidateModelAsync(_specialtyUpdateValidator, specialty, result))
            {
                return result;
            }

            var entity = await _unitOfClassifier.Specialty.GetByIdAsync(id);
            if (entity == null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (await _unitOfClassifier.Specialty.AnyAsync(x => x.Id != id && x.Name == specialty.Name))
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            entity.Name = specialty.Name;
            entity.Description = specialty.Description;

            try
            {
                _unitOfClassifier.Specialty.Update(entity);
                await _unitOfClassifier.SaveChangesAsync();
                result.SetStatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update specialty classifier.");
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

        private PaymentTypeDto MapPaymentType(PaymentTypeEntity entity)
        {
            return new PaymentTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
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

        private SpecialtyDto MapSpecialty(SpecialtyEntity entity)
        {
            return new SpecialtyDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        private async Task<bool> ValidateModelAsync<TDto>(IValidator<TDto> validator, TDto model, Result result)
        {
            _logger.LogInformation("Model validation started for {Validator}.", typeof(TDto).Name);
            var validationResult = await validator.ValidateAsync(model);
            _logger.LogInformation("Model validation finished for {Validator}.", typeof(TDto).Name);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for {Validator}.", typeof(TDto).Name);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return false;
            }

            if (!validationResult.IsValid)
            {
                _logger.LogDebug("Validation failed for {Validator} with errors: {Errors}", typeof(TDto).Name, validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return false;
            }

            return true;
        }
    }
}
