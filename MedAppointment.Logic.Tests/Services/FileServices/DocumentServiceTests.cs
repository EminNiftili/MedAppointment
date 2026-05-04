namespace MedAppointment.Logic.Tests.Services.FileServices;

public class DocumentServiceTests : IDisposable
{
    private const string DocumentServiceTypeName = "MedAppointment.Logics.Implementations.FileServices.DocumentService";

    private readonly string _tempPath;
    private readonly IUnitOfFile _unitOfFile;
    private readonly IDocumentRepository _documentRepo;
    private readonly IValidator<DocumentUploadMetaDto> _uploadMetaValidator;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IDocumentService _sut;

    public DocumentServiceTests()
    {
        _tempPath = Path.Combine(Path.GetTempPath(), $"DocTests_{Guid.NewGuid()}");

        _unitOfFile = Substitute.For<IUnitOfFile>();
        _documentRepo = Substitute.For<IDocumentRepository>();
        _uploadMetaValidator = Substitute.For<IValidator<DocumentUploadMetaDto>>();
        _configuration = Substitute.For<IConfiguration>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(DocumentServiceTypeName);

        _configuration["Settings:FileServerPath"].Returns(_tempPath);
        _unitOfFile.Document.Returns(_documentRepo);

        _sut = ServiceReflectionHelper.CreateService<IDocumentService>(
            DocumentServiceTypeName,
            _unitOfFile,
            _logger,
            _uploadMetaValidator,
            _configuration);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempPath))
            Directory.Delete(_tempPath, true);
    }

    // ─── UploadAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UploadAsync_WhenValidationFails_ReturnsBadRequest_AndDoesNotPersist()
    {
        var meta = MagicDocument.InvalidMetaNoTypeSet;
        _uploadMetaValidator.ValidateAsync(meta, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure("IsExperience", "Exactly one document type must be set.")
                {
                    ErrorCode = "ERR00212"
                }
            }));

        using var stream = new MemoryStream(new byte[] { 1, 2, 3 });
        var result = await _sut.UploadAsync(stream, "test.pdf", "application/pdf", meta);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00212");
        await _documentRepo.DidNotReceive().AddAsync(Arg.Any<DocumentEntity>());
        await _unitOfFile.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task UploadAsync_WhenValidWithSpecialtyId_CreatesFileAndPersistsEntity()
    {
        var meta = MagicDocument.ValidMetaWithSpecialty;
        _uploadMetaValidator.ValidateAsync(meta, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        using var stream = new MemoryStream(new byte[] { 10, 20, 30 });
        var result = await _sut.UploadAsync(stream, "cert.pdf", "application/pdf", meta);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);
        Assert.NotEqual(Guid.Empty, result.Model!.DocumentId);

        await _documentRepo.Received(1).AddAsync(Arg.Is<DocumentEntity>(e =>
            e.DoctorId == meta.DoctorId &&
            e.SpecialtyId == meta.SpecialtyId &&
            e.IsProfessionBackground == false &&
            e.IsExperience == false &&
            e.Filename == "cert.pdf" &&
            e.MimeType == "application/pdf" &&
            e.Title == meta.Title &&
            e.Issuer == meta.Issuer &&
            e.PeriodOfYear == meta.PeriodOfYear &&
            e.Description == meta.Description));
        await _unitOfFile.Received(1).SaveChangesAsync();

        var writtenFiles = Directory.GetFiles(_tempPath);
        Assert.Single(writtenFiles);
    }

    [Fact]
    public async Task UploadAsync_WhenValidWithIsProfessionBackground_PersistsCorrectFlags()
    {
        var meta = MagicDocument.ValidMetaWithProfessionBackground;
        _uploadMetaValidator.ValidateAsync(meta, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        using var stream = new MemoryStream(new byte[] { 5, 6, 7 });
        var result = await _sut.UploadAsync(stream, "diploma.jpg", "image/jpeg", meta);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);

        await _documentRepo.Received(1).AddAsync(Arg.Is<DocumentEntity>(e =>
            e.IsProfessionBackground == true &&
            e.IsExperience == false &&
            e.SpecialtyId == null));
    }

    [Fact]
    public async Task UploadAsync_WhenValidWithIsExperience_PersistsCorrectFlags()
    {
        var meta = MagicDocument.ValidMetaWithExperience;
        _uploadMetaValidator.ValidateAsync(meta, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        using var stream = new MemoryStream(new byte[] { 8, 9 });
        var result = await _sut.UploadAsync(stream, "experience.pdf", "application/pdf", meta);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);

        await _documentRepo.Received(1).AddAsync(Arg.Is<DocumentEntity>(e =>
            e.IsExperience == true &&
            e.IsProfessionBackground == false &&
            e.SpecialtyId == null));
    }

    [Fact]
    public async Task UploadAsync_WhenSaveChangesThrows_ReturnsInternalServerError()
    {
        var meta = MagicDocument.ValidMetaWithSpecialty;
        _uploadMetaValidator.ValidateAsync(meta, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        _unitOfFile.SaveChangesAsync()
            .Returns<Task>(_ => throw new Exception("DB connection lost"));

        using var stream = new MemoryStream(new byte[] { 1 });
        var result = await _sut.UploadAsync(stream, "fail.pdf", "application/pdf", meta);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00200");
    }

    // ─── GetDocumentsByDoctorIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task GetDocumentsByDoctorIdAsync_WhenNoDocuments_ReturnsOkWithEmptyList()
    {
        _documentRepo.FindAsync(Arg.Any<Expression<Func<DocumentEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<DocumentEntity>>(new List<DocumentEntity>()));

        var result = await _sut.GetDocumentsByDoctorIdAsync(MagicIds.DoctorIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.Model);
        Assert.Empty(result.Model!);
    }

    [Fact]
    public async Task GetDocumentsByDoctorIdAsync_WhenDocumentsExist_ReturnsMappedDtoList()
    {
        var entities = new List<DocumentEntity>
        {
            MagicDocument.EntityWithSpecialty,
            MagicDocument.EntityWithProfessionBackground,
        };
        _documentRepo.FindAsync(Arg.Any<Expression<Func<DocumentEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<DocumentEntity>>(entities));

        var result = await _sut.GetDocumentsByDoctorIdAsync(MagicIds.DoctorIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(2, result.Model!.Count);

        var first = result.Model.First(d => d.DocumentId == MagicDocument.EntityWithSpecialty.DocumentId);
        Assert.Equal(MagicDocument.EntityWithSpecialty.DoctorId, first.DoctorId);
        Assert.Equal(MagicDocument.EntityWithSpecialty.SpecialtyId, first.SpecialtyId);
        Assert.False(first.IsProfessionBackground);
        Assert.False(first.IsExperience);
        Assert.Equal(MagicDocument.EntityWithSpecialty.Title, first.Title);
        Assert.Equal(MagicDocument.EntityWithSpecialty.CreatedAt, first.CreatedAt);

        var second = result.Model.First(d => d.DocumentId == MagicDocument.EntityWithProfessionBackground.DocumentId);
        Assert.True(second.IsProfessionBackground);
        Assert.Null(second.SpecialtyId);
    }

    // ─── GetDocumentInfoAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetDocumentInfoAsync_WhenNotFound_ReturnsNotFound()
    {
        _documentRepo.GetByDocumentIdAsync(MagicDocument.DocumentGuidNonExistent)
            .Returns((DocumentEntity?)null);

        var result = await _sut.GetDocumentInfoAsync(MagicDocument.DocumentGuidNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00201");
    }

    [Fact]
    public async Task GetDocumentInfoAsync_WhenFound_ReturnsMappedDocumentInfoDto()
    {
        var entity = MagicDocument.EntityWithSpecialty;
        _documentRepo.GetByDocumentIdAsync(entity.DocumentId)
            .Returns(entity);

        var result = await _sut.GetDocumentInfoAsync(entity.DocumentId);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);

        var dto = result.Model!;
        Assert.Equal(entity.DocumentId, dto.DocumentId);
        Assert.Equal(entity.DoctorId, dto.DoctorId);
        Assert.Equal(entity.SpecialtyId, dto.SpecialtyId);
        Assert.Equal(entity.IsProfessionBackground, dto.IsProfessionBackground);
        Assert.Equal(entity.IsExperience, dto.IsExperience);
        Assert.Equal(entity.Filename, dto.Filename);
        Assert.Equal(entity.MimeType, dto.MimeType);
        Assert.Equal(entity.Title, dto.Title);
        Assert.Equal(entity.Issuer, dto.Issuer);
        Assert.Equal(entity.PeriodOfYear, dto.PeriodOfYear);
        Assert.Equal(entity.Description, dto.Description);
        Assert.Equal(entity.CreatedAt, dto.CreatedAt);
    }

    // ─── GetDownloadInfoAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetDownloadInfoAsync_WhenNotFound_ReturnsNotFound()
    {
        _documentRepo.GetByDocumentIdAsync(MagicDocument.DocumentGuidNonExistent)
            .Returns((DocumentEntity?)null);

        var result = await _sut.GetDownloadInfoAsync(MagicDocument.DocumentGuidNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00201");
    }

    [Fact]
    public async Task GetDownloadInfoAsync_WhenEntityExistsButFileIsMissing_ReturnsNotFound()
    {
        var entity = new DocumentEntity
        {
            Id = MagicDocument.EntityWithSpecialty.Id,
            DocumentId = MagicDocument.EntityWithSpecialty.DocumentId,
            DoctorId = MagicDocument.EntityWithSpecialty.DoctorId,
            SpecialtyId = MagicDocument.EntityWithSpecialty.SpecialtyId,
            IsProfessionBackground = MagicDocument.EntityWithSpecialty.IsProfessionBackground,
            IsExperience = MagicDocument.EntityWithSpecialty.IsExperience,
            Filename = MagicDocument.EntityWithSpecialty.Filename,
            MimeType = MagicDocument.EntityWithSpecialty.MimeType,
            FilePath = Path.Combine(_tempPath, "nonexistent_file.pdf"),
        };
        _documentRepo.GetByDocumentIdAsync(entity.DocumentId).Returns(entity);

        var result = await _sut.GetDownloadInfoAsync(entity.DocumentId);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00202");
    }

    [Fact]
    public async Task GetDownloadInfoAsync_WhenFileExists_ReturnsDocumentDownloadDto()
    {
        Directory.CreateDirectory(_tempPath);
        var physicalPath = Path.Combine(_tempPath, "real_file.pdf");
        await File.WriteAllBytesAsync(physicalPath, new byte[] { 1, 2, 3 });

        var entity = new DocumentEntity
        {
            Id = MagicDocument.EntityWithSpecialty.Id,
            DocumentId = MagicDocument.EntityWithSpecialty.DocumentId,
            DoctorId = MagicDocument.EntityWithSpecialty.DoctorId,
            SpecialtyId = MagicDocument.EntityWithSpecialty.SpecialtyId,
            IsProfessionBackground = MagicDocument.EntityWithSpecialty.IsProfessionBackground,
            IsExperience = MagicDocument.EntityWithSpecialty.IsExperience,
            Filename = MagicDocument.EntityWithSpecialty.Filename,
            MimeType = MagicDocument.EntityWithSpecialty.MimeType,
            FilePath = physicalPath,
        };
        _documentRepo.GetByDocumentIdAsync(entity.DocumentId).Returns(entity);

        var result = await _sut.GetDownloadInfoAsync(entity.DocumentId);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.Equal(physicalPath, result.Model!.FilePath);
        Assert.Equal(entity.MimeType, result.Model.MimeType);
        Assert.Equal(entity.Filename, result.Model.Filename);
    }
}
