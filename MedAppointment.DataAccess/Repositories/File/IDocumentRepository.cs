namespace MedAppointment.DataAccess.Repositories.File
{
    public interface IDocumentRepository : IGenericRepository<DocumentEntity>
    {
        Task<DocumentEntity?> GetByDocumentIdAsync(Guid documentId);
    }
}
