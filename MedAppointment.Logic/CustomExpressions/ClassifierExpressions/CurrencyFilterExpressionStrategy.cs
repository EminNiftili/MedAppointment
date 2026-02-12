using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Builds filter expression from <see cref="CurrencyPaginationQueryDto"/> (base from builder + CoefficentMin/Max).
    /// </summary>
    internal class CurrencyFilterExpressionStrategy : IClassifierFilterExpressionStrategy<CurrencyEntity, CurrencyPaginationQueryDto>
    {
        public Expression<Func<CurrencyEntity, bool>> Build(CurrencyPaginationQueryDto query)
        {
            return Build(query, null, null);
        }

        public Expression<Func<CurrencyEntity, bool>> Build(CurrencyPaginationQueryDto query,
            IReadOnlyList<long>? nameFilterEntityIds, 
            IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            var param = Expression.Parameter(typeof(CurrencyEntity), "e");
            Expression? body = (nameFilterEntityIds != null || descriptionFilterEntityIds != null)
                ? ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilterFromTranslationIds(param, nameFilterEntityIds, descriptionFilterEntityIds)
                : ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilter(param, query.NameFilter, query.DescriptionFilter);
            body = body ?? Expression.Constant(true);

            if (query.CoefficentMin.HasValue)
            {
                var coeffProp = Expression.Property(param, nameof(CurrencyEntity.Coefficent));
                body = Expression.AndAlso(body, Expression.GreaterThanOrEqual(coeffProp, Expression.Constant(query.CoefficentMin.Value)));
            }

            if (query.CoefficentMax.HasValue)
            {
                var coeffProp = Expression.Property(param, nameof(CurrencyEntity.Coefficent));
                body = Expression.AndAlso(body, Expression.LessThanOrEqual(coeffProp, Expression.Constant(query.CoefficentMax.Value)));
            }

            return Expression.Lambda<Func<CurrencyEntity, bool>>(body, param);
        }
    }
}
