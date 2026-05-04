namespace MedAppointment.Api.Controllers.FileControllers
{
    public class FilesController : BaseApiController
    {
        private readonly IDocumentService _documentService;

        public FilesController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadAsync([FromForm] DocumentUploadMetaDto meta, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            await using var stream = file.OpenReadStream();
            var result = await _documentService.UploadAsync(stream, file.FileName, file.ContentType, meta);
            return CustomResult(result);
        }

        [HttpGet("doctor/{doctorId:long}")]
        [Authorize]
        public async Task<IActionResult> GetDocumentsByDoctorIdAsync(long doctorId)
        {
            var result = await _documentService.GetDocumentsByDoctorIdAsync(doctorId);
            return CustomResult(result);
        }

        [HttpGet("{documentId:guid}/info")]
        [Authorize]
        public async Task<IActionResult> GetDocumentInfoAsync(Guid documentId)
        {
            var result = await _documentService.GetDocumentInfoAsync(documentId);
            return CustomResult(result);
        }

        [HttpGet("{documentId:guid}")]
        [Authorize]
        public async Task<IActionResult> DownloadAsync(Guid documentId)
        {
            var result = await _documentService.GetDownloadInfoAsync(documentId);
            if (!result.IsSuccess())
                return CustomResult(result);

            return PhysicalFile(result.Model!.FilePath, result.Model.MimeType, result.Model.Filename);
        }
    }
}
