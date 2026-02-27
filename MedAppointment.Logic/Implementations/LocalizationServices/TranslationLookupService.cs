using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations.LocalizationServices
{
    internal class TranslationLookupService : ITranslationLookupService
    {
        protected readonly IUnitOfLocalization localization;
        private readonly ILogger<TranslationLookupService> _logger;

        public TranslationLookupService(IUnitOfLocalization unitOfLocalization, ILogger<TranslationLookupService> logger)
        {
            localization = unitOfLocalization;
            _logger = logger;
        }

        public async Task<(IReadOnlyList<long>? NameFilterEntityIds, IReadOnlyList<long>? DescriptionFilterEntityIds)> GetFilterIdsAsync(
            string? nameFilter,
            string? descriptionFilter)
        {
            _logger.LogTrace("Resolving translation filter IDs for NameFilter: {NameFilter}, DescriptionFilter: {DescriptionFilter}",
                nameFilter, descriptionFilter);

            IEnumerable<long>? nameIds = null;
            IEnumerable<long>? descriptionIds = null;

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                nameIds = (await localization.Translation.FindAsync(x => x.Text.Contains(nameFilter, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x.ResourceId)
                    .Distinct();
                _logger.LogDebug("Name filter matched {Count} entity IDs.", nameIds.Count());
            }

            if (!string.IsNullOrWhiteSpace(descriptionFilter))
            {
                descriptionIds = (await localization.Translation.FindAsync(x => x.Text.Contains(descriptionFilter, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => x.ResourceId)
                    .Distinct();
                _logger.LogDebug("Description filter matched {Count} entity IDs.", descriptionIds.Count());
            }

            _logger.LogInformation("Translation lookup completed");
            return (nameIds?.ToList() ?? [], descriptionIds?.ToList() ?? []);
        }
    }
}
