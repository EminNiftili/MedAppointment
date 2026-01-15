namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.File
{
    internal class ImageRepository : EfGenericRepository<ImageEntity>, IImageRepository
    {
        public ImageRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<ImageEntity>(), false)
        {
        }

        protected override IQueryable<ImageEntity> IncludeQuery(IQueryable<ImageEntity> query)
        {
            throw new NotImplementedException();
        }
    }
}
