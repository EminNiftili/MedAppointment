using System.Linq.Expressions;
using System.Reflection;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Single place for building classifier pagination filter expressions.
    /// Name and Description filters use Localization.Translation.Text (same rule for both).
    /// </summary>
    public static class ClassifierPaginationExpressionBuilder
    {
        private static readonly MethodInfo StringContainsMethod = typeof(string).GetMethod(
            nameof(string.Contains),
            new[] { typeof(string), typeof(StringComparison) })!;

        private static readonly MethodInfo HashSetLongContainsMethod = typeof(HashSet<long>).GetMethod(
            nameof(HashSet<long>.Contains),
            new[] { typeof(long) })!;

        /// <summary>
        /// Builds the combined Name and Description filter from precomputed entity ID sets
        /// (from Localization.Translation.Text). Use when filtering by translated text.
        /// Same rule for Name and Description: both come from Translation.Text.
        /// </summary>
        /// <param name="param">Parameter expression for the entity.</param>
        /// <param name="nameFilterEntityIds">Entity IDs that match the name filter in Translation.Text (optional).</param>
        /// <param name="descriptionFilterEntityIds">Entity IDs that match the description filter in Translation.Text (optional).</param>
        /// <returns>Combined expression body, or null if both ID sets are null/empty.</returns>
        public static Expression? BuildNameAndDescriptionFilterFromTranslationIds(
            ParameterExpression param,
            IReadOnlyList<long>? nameFilterEntityIds,
            IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            Expression? body = null;
            var idProp = Expression.Property(param, nameof(BaseEntity.Id));

            if (nameFilterEntityIds is { Count: > 0 })
            {
                var nameSetConst = Expression.Constant(nameFilterEntityIds);
                var nameContains = Expression.Call(nameSetConst, HashSetLongContainsMethod, idProp);
                body = body == null ? nameContains : Expression.AndAlso(body, nameContains);
            }

            if (descriptionFilterEntityIds is { Count: > 0 })
            {
                var descSetConst = Expression.Constant(descriptionFilterEntityIds);
                var descContains = Expression.Call(descSetConst, HashSetLongContainsMethod, idProp);
                body = body == null ? descContains : Expression.AndAlso(body, descContains);
            }

            return body;
        }

        /// <summary>
        /// Builds the combined Name and Description filter expression body from raw string filters
        /// using entity Name/Description columns. Prefer <see cref="BuildNameAndDescriptionFilterFromTranslationIds"/>
        /// when translations exist so that filtering uses Localization.Translation.Text.
        /// </summary>
        /// <param name="param">Parameter expression for the entity (e.g. Expression.Parameter(typeof(TEntity), "e")).</param>
        /// <param name="nameFilter">Optional name filter (contains, case-insensitive).</param>
        /// <param name="descriptionFilter">Optional description filter (contains, case-insensitive).</param>
        /// <returns>Combined expression body, or null if both filters are null/empty.</returns>
        public static Expression? BuildNameAndDescriptionFilter(
            ParameterExpression param,
            string? nameFilter,
            string? descriptionFilter)
        {
            Expression? body = null;

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                var nameProp = Expression.Property(param, nameof(BaseClassfierEntity.Name));
                var nameCall = Expression.Call(
                    nameProp,
                    StringContainsMethod,
                    Expression.Constant(nameFilter),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase));
                body = body == null ? nameCall : Expression.AndAlso(body, nameCall);
            }

            if (!string.IsNullOrWhiteSpace(descriptionFilter))
            {
                var descProp = Expression.Property(param, nameof(BaseClassfierEntity.Description));
                var descCall = Expression.Call(
                    descProp,
                    StringContainsMethod,
                    Expression.Constant(descriptionFilter),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase));
                body = body == null ? descCall : Expression.AndAlso(body, descCall);
            }

            return body;
        }
    }
}
