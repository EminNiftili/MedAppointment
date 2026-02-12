using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Builds filter expression from <see cref="PlanPaddingTypePaginationQueryDto"/> (base from builder + PaddingPosition, PaddingTime).
    /// </summary>
    internal class PlanPaddingTypeFilterExpressionStrategy : IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto>
    {
        public Expression<Func<PlanPaddingTypeEntity, bool>> Build(PlanPaddingTypePaginationQueryDto query)
        {
            return Build(query, null, null);
        }

        public Expression<Func<PlanPaddingTypeEntity, bool>> Build(PlanPaddingTypePaginationQueryDto query, 
            IReadOnlyList<long>? nameFilterEntityIds, 
            IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            var param = Expression.Parameter(typeof(PlanPaddingTypeEntity), "e");
            Expression? body = (nameFilterEntityIds != null || descriptionFilterEntityIds != null)
                ? ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilterFromTranslationIds(param, nameFilterEntityIds, descriptionFilterEntityIds)
                : ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilter(param, query.NameFilter, query.DescriptionFilter);
            body = body ?? Expression.Constant(true);

            if (query.PaddingPosition.HasValue)
            {
                var posProp = Expression.Property(param, nameof(PlanPaddingTypeEntity.PaddingPosition));
                body = Expression.AndAlso(body, Expression.Equal(posProp, Expression.Constant((byte)query.PaddingPosition.Value)));
            }

            if (query.PaddingTime.HasValue)
            {
                var timeProp = Expression.Property(param, nameof(PlanPaddingTypeEntity.PaddingTime));
                body = Expression.AndAlso(body, Expression.Equal(timeProp, Expression.Constant(query.PaddingTime.Value)));
            }

            return Expression.Lambda<Func<PlanPaddingTypeEntity, bool>>(body, param);
        }
    }
}
