namespace MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks
{
    public interface IUnitOfLocalization : IUnitOfWork
    {
        IResourceRepository Resource { get; }
        ITranslationRepository Translation { get; }
    }
}
