using MedAppointment.Entities.Localization;

namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Helper to build ResourceEntity with Translations for classifier entities (used in GetById mapping).
/// </summary>
public static class MagicClassifierHelper
{
    public static ResourceEntity ResourceWithTranslation(string key, long languageId, string text)
    {
        var resource = new ResourceEntity { Key = key };
        var translation = new TranslationEntity { LanguageId = languageId, Text = text, Resource = resource };
        resource.Translations = new List<TranslationEntity> { translation };
        return resource;
    }
}
