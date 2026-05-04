namespace MedAppointment.Logics.Implementations.FileServices
{
    internal class DocumentService : IDocumentService
    {
        private readonly IUnitOfFile _unitOfFile;
        private readonly ILogger<DocumentService> _logger;
        private readonly IValidator<DocumentUploadMetaDto> _uploadMetaValidator;
        private readonly string _fileServerPath;

        public DocumentService(
            IUnitOfFile unitOfFile,
            ILogger<DocumentService> logger,
            IValidator<DocumentUploadMetaDto> uploadMetaValidator,
            IConfiguration configuration)
        {
            _unitOfFile = unitOfFile;
            _logger = logger;
            _uploadMetaValidator = uploadMetaValidator;
            _fileServerPath = configuration["Settings:FileServerPath"] ?? "C:\\MedAppointmentApp";
        }

        public async Task<Result<DocumentUploadResultDto>> UploadAsync(Stream content, string filename, string mimeType, DocumentUploadMetaDto meta)
        {
            _logger.LogTrace("Uploading document: {Filename}, MimeType: {MimeType}, DoctorId: {DoctorId}", filename, mimeType, meta.DoctorId);
            var result = Result<DocumentUploadResultDto>.Create();

            var validationResult = await _uploadMetaValidator.ValidateAsync(meta);
            if (!result.SetFluentValidationAndBadRequest(validationResult))
            {
                _logger.LogDebug("Validation failed for document upload meta. DoctorId: {DoctorId}", meta.DoctorId);
                return result;
            }

            try
            {
                Directory.CreateDirectory(_fileServerPath);

                var documentId = Guid.NewGuid();
                var extension = Path.GetExtension(filename);
                var storedFilename = $"{documentId}{extension}";
                var fullPath = Path.Combine(_fileServerPath, storedFilename);

                await using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await content.CopyToAsync(fileStream);
                }

                var entity = new DocumentEntity
                {
                    DocumentId = documentId,
                    DoctorId = meta.DoctorId,
                    SpecialtyId = meta.SpecialtyId,
                    IsProfessionBackground = meta.IsProfessionBackground,
                    IsExperience = meta.IsExperience,
                    Filename = filename,
                    MimeType = mimeType,
                    FilePath = fullPath,
                    Title = meta.Title,
                    Issuer = meta.Issuer,
                    PeriodOfYear = meta.PeriodOfYear,
                    Description = meta.Description,
                };

                await _unitOfFile.Document.AddAsync(entity);
                await _unitOfFile.SaveChangesAsync();

                result.Success(new DocumentUploadResultDto { DocumentId = entity.DocumentId }, HttpStatusCode.Created);
                _logger.LogInformation("Document uploaded. DocumentId: {DocumentId}, DoctorId: {DoctorId}", entity.DocumentId, entity.DoctorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload document: {Filename}", filename);
                result.AddMessage("ERR00200", "Failed to upload document. Contact admin.", HttpStatusCode.InternalServerError, ex);
            }

            return result;
        }

        public async Task<Result<List<DocumentInfoDto>>> GetDocumentsByDoctorIdAsync(long doctorId)
        {
            _logger.LogTrace("Fetching all documents for DoctorId: {DoctorId}", doctorId);
            var result = Result<List<DocumentInfoDto>>.Create();

            var entities = (await _unitOfFile.Document.FindAsync(x => x.DoctorId == doctorId)).ToList();

            result.Success(entities.Select(MapToInfo).ToList());

            _logger.LogInformation("Retrieved {Count} documents for DoctorId: {DoctorId}", entities.Count, doctorId);
            return result;
        }

        public async Task<Result<DocumentInfoDto>> GetDocumentInfoAsync(Guid documentId)
        {
            _logger.LogTrace("Fetching document info for DocumentId: {DocumentId}", documentId);
            var result = Result<DocumentInfoDto>.Create();

            var entity = await _unitOfFile.Document.GetByDocumentIdAsync(documentId);
            if (entity == null)
            {
                _logger.LogInformation("Document not found for DocumentId: {DocumentId}", documentId);
                result.AddMessage("ERR00201", "Document not found.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(MapToInfo(entity));
            _logger.LogInformation("Document info retrieved for DocumentId: {DocumentId}", documentId);
            return result;
        }

        public async Task<Result<DocumentDownloadDto>> GetDownloadInfoAsync(Guid documentId)
        {
            _logger.LogTrace("Fetching download info for DocumentId: {DocumentId}", documentId);
            var result = Result<DocumentDownloadDto>.Create();

            var entity = await _unitOfFile.Document.GetByDocumentIdAsync(documentId);
            if (entity == null)
            {
                _logger.LogInformation("Document not found for DocumentId: {DocumentId}", documentId);
                result.AddMessage("ERR00201", "Document not found.", HttpStatusCode.NotFound);
                return result;
            }

            if (!File.Exists(entity.FilePath))
            {
                _logger.LogWarning("Document record exists but physical file is missing. DocumentId: {DocumentId}, Path: {Path}", documentId, entity.FilePath);
                result.AddMessage("ERR00202", "Document not found on server.", HttpStatusCode.NotFound);
                return result;
            }

            result.Success(new DocumentDownloadDto
            {
                FilePath = entity.FilePath,
                MimeType = entity.MimeType,
                Filename = entity.Filename,
            });

            _logger.LogInformation("Download info retrieved for DocumentId: {DocumentId}", documentId);
            return result;
        }

        private static DocumentInfoDto MapToInfo(DocumentEntity e) => new()
        {
            DocumentId = e.DocumentId,
            DoctorId = e.DoctorId,
            SpecialtyId = e.SpecialtyId,
            IsProfessionBackground = e.IsProfessionBackground,
            IsExperience = e.IsExperience,
            Filename = e.Filename,
            MimeType = e.MimeType,
            Title = e.Title,
            Issuer = e.Issuer,
            PeriodOfYear = e.PeriodOfYear,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
        };
    }
}
