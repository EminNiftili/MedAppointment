using MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations
{
    internal class LocalizerService : ILocalizerService
    {
        protected readonly ILogger<LocalizerService> Logger;
        protected readonly IUnitOfLocalization UnitOfLocalization;
        protected readonly IUnitOfClassifier UnitOfClassifier;

        public LocalizerService(ILogger<LocalizerService> logger, 
            IUnitOfLocalization unitOfLocalization,
            IUnitOfClassifier unitOfClassifier)
        {
            Logger = logger;
            UnitOfLocalization = unitOfLocalization;
            UnitOfClassifier = unitOfClassifier;
        }

        //TODO ADD LOGGER and ADD error messages
        public async Task<Result<long>> AddResourceAsync(IEnumerable<LocalizationDto> localizations)
        {
            var result = Result<long>.Create();

            var groupLocalization = localizations.GroupBy(x => x.Key).ToList();
            if(groupLocalization.Count() != 1)
            {
                // multiple keys detected
                return result;
            }
            string key = groupLocalization.First().Key;
            
            var hasDuplicateLanguageId = HasDuplicateLanguageId(groupLocalization.First());
            if (hasDuplicateLanguageId)
            {
                // duplicate languageId detected
                return result;
            }
            var localizationDetails = groupLocalization.First().ToDictionary(x => x.LanguageId, x => x.Text) ?? new Dictionary<long, string>();
            var localizationResult = await AddLocalizationAsync(key, localizationDetails);
            result.MergeResult(localizationResult);
            return result;
        }

        private async Task<Result<long>> AddLocalizationAsync(string key, IDictionary<long, string> languageTexts)
        {
            var result = Result<long>.Create();
            var resource = await UnitOfLocalization.Resource.GetByKeyAsync(key);
            if (resource == null)
            {
                resource = new ResourceEntity
                {
                    Key = key,
                };
            }
            foreach (var localization in languageTexts)
            {
                var isExistLanguage = await UnitOfClassifier.Language.AnyAsync(x => x.Id == localization.Key);
                if (!isExistLanguage)
                {
                    //Language not found
                    return result;
                }
                var existTranslation = resource.Translations.FirstOrDefault(x => x.LanguageId == localization.Key);
                if (existTranslation == null)
                {
                    resource.Translations.Add(new TranslationEntity
                    {
                        LanguageId = localization.Key,
                        Text = localization.Value,
                    });
                }
                else
                {
                    existTranslation.Text = localization.Value;
                }
            }

            await UnitOfLocalization.Resource.AddAsync(resource);
            await UnitOfLocalization.SaveChangesAsync();
            result.Model = resource.Id;
            return result;
        }

        private bool HasDuplicateLanguageId(IEnumerable<LocalizationDto> localizations)
        {
            var set = new HashSet<long>();

            foreach (var item in localizations)
                if (!set.Add(item.LanguageId))
                    return true;

            return false;
        }
    }
}
