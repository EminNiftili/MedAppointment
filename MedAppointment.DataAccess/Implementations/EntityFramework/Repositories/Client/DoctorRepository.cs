namespace MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client
{
    internal class DoctorRepository : EfGenericRepository<DoctorEntity>, IDoctorRepository
    {
        public DoctorRepository(MedicalAppointmentContext medicalAppointmentContext)
            : base(medicalAppointmentContext, medicalAppointmentContext.Set<DoctorEntity>(), true)
        {
        }

        protected override IQueryable<DoctorEntity> IncludeQuery(IQueryable<DoctorEntity> query)
        {
            return query
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.Admin)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.Doctor)
                    .ThenInclude(userDoctor => userDoctor!.Specialties)
                    .ThenInclude(specialty => specialty.Specialty)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.Person)
                    .ThenInclude(person => person!.Image)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.TraditionalUser)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Device)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.Sessions)
                    .ThenInclude(session => session.Tokens)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.ReceiverUser)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.SentChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.SenderUser)
                .Include(doctor => doctor.User)
                    .ThenInclude(user => user!.ReceiverChats)
                    .ThenInclude(chat => chat.Histories)
                    .ThenInclude(history => history.User)
                .Include(doctor => doctor.Specialties)
                    .ThenInclude(specialty => specialty.Specialty);
        }
    }
}
