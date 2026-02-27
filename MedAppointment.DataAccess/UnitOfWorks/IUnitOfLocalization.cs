namespace MedAppointment.DataAccess.UnitOfWorks
{
    public interface IUnitOfLocalization : IUnitOfWork
    {
        IResourceRepository Resource { get; }
        ITranslationRepository Translation { get; }
    }
}
