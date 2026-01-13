namespace MedAppointment.Entities.File
{
    public class ImageEntity : BaseEntity
    {
        public string Filename { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        /// <summary>
        /// This property show us where file in Server 
        /// </summary>
        public string FilePath { get; set; } = null!;
    }
}
