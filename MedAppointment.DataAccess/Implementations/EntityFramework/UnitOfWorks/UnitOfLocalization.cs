namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    internal class UnitOfLocalization : EfUnitOfWork, IUnitOfLocalization
    {
        public UnitOfLocalization(MedicalAppointmentContext medicalAppointmentContext,
            IResourceRepository resource, 
            ITranslationRepository translation) : base(medicalAppointmentContext)
        {
            Resource = resource;
            Translation = translation;
        }

        public IResourceRepository Resource { get; private set; }

        public ITranslationRepository Translation { get; private set; }
    }
}
