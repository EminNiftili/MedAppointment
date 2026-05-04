namespace MedAppointment.Logics.Services.FileServices
{
    public interface IDocumentService
    {
        Task<Result<DocumentUploadResultDto>> UploadAsync(Stream content, string filename, string mimeType, DocumentUploadMetaDto meta);
        Task<Result<List<DocumentInfoDto>>> GetDocumentsByDoctorIdAsync(long doctorId);
        Task<Result<DocumentInfoDto>> GetDocumentInfoAsync(Guid documentId);
        Task<Result<DocumentDownloadDto>> GetDownloadInfoAsync(Guid documentId);
    }
}
