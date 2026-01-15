namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfFile : EfUnitOfWork, IUnitOfFile
    {
        public UnitOfFile(MedicalAppointmentContext medicalAppointmentContext,
            IImageRepository image) : base(medicalAppointmentContext)
        {
            Image = image;
        }

        public IImageRepository Image { get; private set; }
    }
}
