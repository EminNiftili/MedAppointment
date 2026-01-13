namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts.MedicalAppointment
{
    internal partial class MedicalAppointmentContext
    {
        internal DbSet<CurrencyEntity> Currencies { get; private set; }
        internal DbSet<PaymentTypeEntity> PaymentTypes { get; private set; }
        internal DbSet<PeriodEntity> Periods { get; private set; }
        internal DbSet<SpecialtyEntity> Specialties { get; private set; }



        internal DbSet<AdminEntity> Admins { get; private set; }
        internal DbSet<DoctorEntity> Doctors { get; private set; }
        internal DbSet<PersonEntity> People { get; private set; }
        internal DbSet<UserEntity> Users { get; private set; }
        internal DbSet<OrganizationEntity> Organizations { get; private set; }



        internal DbSet<ChatHistoryEntity> ChatHistories { get; private set; }
        internal DbSet<ChatEntity> Chats { get; private set; }
        internal DbSet<MeetEntity> Meets { get; private set; }



        internal DbSet<OrganizationUserEntity> OrganizationUsers { get; private set; }
        internal DbSet<DoctorSpecialtyEntity> DoctorSpecialties { get; private set; }



        internal DbSet<ImageEntity> Images { get; private set; }



        internal DbSet<PaymentEntity> Payments { get; private set; }



        internal DbSet<DeviceEntity> Devices { get; private set; }
        internal DbSet<SessionEntity> Sessions { get; private set; }
        internal DbSet<TokenEntity> Tokens { get; private set; }
        internal DbSet<TraditionalUserEntity> TraditionalUsers { get; private set; }



        internal DbSet<AppointmentEntity> Appointments { get; private set; }
        internal DbSet<DayPlanEntity> DayPlans { get; private set; }
        internal DbSet<PeriodPlanEntity> PeriodPlans { get; private set; }
    }
}
