namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication
{
    internal class MeetRepository : EfGenericRepository<MeetEntity>, IMeetRepository
    {
        public MeetRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<MeetEntity>(), true)
        {
        }

        protected override IQueryable<MeetEntity> IncludeQuery(IQueryable<MeetEntity> query)
        {
            return query
                .Include(meet => meet.SenderUser)
                    .ThenInclude(user => user!.Admin)
                .Include(meet => meet.SenderUser)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(meet => meet.SenderUser)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(meet => meet.SenderUser)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(meet => meet.ReceiverUser)
                    .ThenInclude(user => user!.Admin)
                .Include(meet => meet.ReceiverUser)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(doctor => doctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(meet => meet.ReceiverUser)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(meet => meet.ReceiverUser)
                    .ThenInclude(user => user!.TraditionalUser);
        }
    }
}
