namespace MedAppointment.DataAccess.Repositories.Classifier
{
    internal class PaymentTypeRepository : EfGenericRepository<PaymentTypeEntity>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(MedicalAppointmentContext medicalAppointmentContext) 
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<PaymentTypeEntity>(), true)
        {
        }

        protected override IQueryable<PaymentTypeEntity> IncludeQuery(IQueryable<PaymentTypeEntity> query)
        {
            return query.Include(x => x.Name)
                            .ThenInclude(r => r!.Translations)
                                .ThenInclude(t => t.Language)
                        .Include(x => x.Description)
                            .ThenInclude(r => r!.Translations)
                                .ThenInclude(t => t.Language);
        }
    }
}
