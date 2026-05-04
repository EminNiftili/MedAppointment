namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.File
{
    internal class DocumentRepository : EfGenericRepository<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DocumentEntity>(), false)
        {
        }

        public async Task<DocumentEntity?> GetByDocumentIdAsync(Guid documentId)
        {
            return await Query.FirstOrDefaultAsync(x => x.DocumentId == documentId);
        }

        protected override IQueryable<DocumentEntity> IncludeQuery(IQueryable<DocumentEntity> query)
        {
            return query;
        }
    }
}
