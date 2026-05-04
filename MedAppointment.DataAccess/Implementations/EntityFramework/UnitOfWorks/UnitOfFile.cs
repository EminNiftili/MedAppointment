namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfFile : EfUnitOfWork, IUnitOfFile
    {
        public UnitOfFile(MedicalAppointmentContext medicalAppointmentContext,
            IImageRepository image,
            IDocumentRepository document) : base(medicalAppointmentContext)
        {
            Image = image;
            Document = document;
        }

        public IImageRepository Image { get; private set; }
        public IDocumentRepository Document { get; private set; }
    }
}
