using System.Linq.Expressions;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    internal class LanguageFilterExpressionStrategy : IClassifierFilterExpressionStrategy<LanguageEntity, LanguagePaginationQueryDto>
    {
        public Expression<Func<LanguageEntity, bool>> Build(LanguagePaginationQueryDto query)
        {
            return Build(query, null, null);
        }
        public Expression<Func<LanguageEntity, bool>> Build(LanguagePaginationQueryDto query, IReadOnlyList<long>? nameFilterEntityIds, IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            var param = Expression.Parameter(typeof(LanguageEntity), "e");
            Expression body = Expression.Constant(true);

            if (!string.IsNullOrWhiteSpace(query.NameFilter))
            {
                var nameProp = Expression.Property(param, nameof(LanguageEntity.Name));

                var nameContains = Expression.Call(
                    nameProp,
                    ClassifierPaginationExpressionBuilder.StringContainsMethod,
                    Expression.Constant(query.NameFilter),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase));

                body = Expression.AndAlso(body, nameContains);
            }

            if (query.IsDefaultFilter.HasValue)
            {
                var isDefaultProp = Expression.Property(param, nameof(LanguageEntity.IsDefault));
                var isDefaultEq = Expression.Equal(isDefaultProp, Expression.Constant(query.IsDefaultFilter.Value));

                body = Expression.AndAlso(body, isDefaultEq);
            }

            return Expression.Lambda<Func<LanguageEntity, bool>>(body, param);
        }


    }
}
