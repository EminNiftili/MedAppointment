using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Builds filter expression from base <see cref="ClassifierPaginationQueryDto"/> (NameFilter, DescriptionFilter).
    /// Used for Specialty and PaymentType (no entity-specific filters). Uses <see cref="ClassifierPaginationExpressionBuilder"/>.
    /// </summary>
    internal class ClassifierFilterExpressionStrategy<TEntity> : IClassifierFilterExpressionStrategy<TEntity, ClassifierPaginationQueryDto>
        where TEntity : BaseClassfierEntity
    {
        public Expression<Func<TEntity, bool>> Build(ClassifierPaginationQueryDto query)
        {
            return Build(query, null, null);
        }

        public Expression<Func<TEntity, bool>> Build(ClassifierPaginationQueryDto query, IReadOnlyList<long>? nameFilterEntityIds, IReadOnlyList<long>? descriptionFilterEntityIds)
        {
            var param = Expression.Parameter(typeof(TEntity), "e");
            var body = (nameFilterEntityIds != null || descriptionFilterEntityIds != null)
                ? ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilterFromTranslationIds(param, nameFilterEntityIds, descriptionFilterEntityIds)
                : ClassifierPaginationExpressionBuilder.BuildNameAndDescriptionFilter(param, query.NameFilter, query.DescriptionFilter);
            body = body ?? Expression.Constant(true);
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }
    }
}
