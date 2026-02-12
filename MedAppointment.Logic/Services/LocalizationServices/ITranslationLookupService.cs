namespace MedAppointment.Logics.Services.LocalizationServices
{
    /// <summary>
    /// Resolves classifier entity IDs that match name/description filters using Localization.Translation.Text (same rule for both).
    /// </summary>
    public interface ITranslationLookupService
    {
        /// <summary>
        /// Gets entity IDs where Translation.Text contains the name and/or description filter for the given entity type and culture.
        /// </summary>
        /// <param name="entityType">Entity type (e.g. "Specialty", "Period", "Currency").</param>
        /// <param name="cultureCode">Culture code (e.g. "en", "az"). If null, default is used.</param>
        /// <param name="nameFilter">Optional name filter text.</param>
        /// <param name="descriptionFilter">Optional description filter text.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>(NameFilterEntityIds, DescriptionFilterEntityIds). Either can be null if the corresponding filter is not set.</returns>
        Task<(IReadOnlyList<long>? NameFilterEntityIds, IReadOnlyList<long>? DescriptionFilterEntityIds)> GetFilterIdsAsync(
            string? nameFilter,
            string? descriptionFilter);
    }
}
