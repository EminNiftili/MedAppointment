using System.Linq.Expressions;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.CustomExpressions.ClassifierExpressions
{
    /// <summary>
    /// Strategy for building filter expressions from classifier pagination query DTOs.
    /// </summary>
    /// <typeparam name="TEntity">Entity type (e.g. SpecialtyEntity).</typeparam>
    /// <typeparam name="TQuery">Query DTO type (ClassifierPaginationQueryDto or derived).</typeparam>
    public interface IClassifierFilterExpressionStrategy<TEntity, TQuery> where TQuery : PaginationQueryDto
    {
        /// <summary>
        /// Builds a filter expression from the query. Returns a predicate that can be compiled and used in Where().
        /// When no filters are applied, returns (e => true).
        /// </summary>
        Expression<Func<TEntity, bool>> Build(TQuery query);

        /// <summary>
        /// Builds a filter expression using precomputed entity ID sets from Localization.Translation.Text.
        /// Name and Description use the same rule (Translation.Text). When ID sets are null/empty, falls back to query string filters.
        /// </summary>
        Expression<Func<TEntity, bool>> Build(TQuery query, IReadOnlyList<long>? nameFilterEntityIds, IReadOnlyList<long>? descriptionFilterEntityIds);
    }
}
