using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Builds filter expression from <see cref="PeriodPaginationQueryDto"/> (base from builder + PeriodTime).
    /// </summary>
    internal class PeriodFilterExpressionStrategy : IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto>
    {
        public Expression<Func<PeriodEntity, bool>> Build(PeriodPaginationQueryDto query)
        {
            return Build(query, null, null);
        }

        public Expression<Func<PeriodEntity, bool>> Build(PeriodPaginationQueryDto query, 
            IReadOnlyList<long>? nameFilterEntityIds, 
            IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            var param = Expression.Parameter(typeof(PeriodEntity), "e");
            Expression? body = (nameFilterEntityIds != null || descriptionFilterEntityIds != null)
                ? ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilterFromTranslationIds(param, nameFilterEntityIds, descriptionFilterEntityIds)
                : ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilter(param, query.NameFilter, query.DescriptionFilter);
            body = body ?? Expression.Constant(true);

            if (query.PeriodTime.HasValue)
            {
                var periodTimeProp = Expression.Property(param, nameof(PeriodEntity.PeriodTime));
                var periodTimeEq = Expression.Equal(periodTimeProp, Expression.Constant(query.PeriodTime.Value));
                body = Expression.AndAlso(body, periodTimeEq);
            }

            return Expression.Lambda<Func<PeriodEntity, bool>>(body, param);
        }
    }
}
