namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Service
{
    public class AppointmentConfig : BaseConfig<AppointmentEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<AppointmentEntity> builder)
        {
            builder.ToTable("Appointments", "Service");

            builder.Property(e => e.PeriodPlanId)
                .IsRequired();
            builder.Property(e => e.PaymentId)
                .IsRequired();
            builder.Property(e => e.SelectedServiceType)
                .IsRequired()
                .HasComment("0 -> OnSite\n1 -> Online");

            builder.HasOne(x => x.Payment)
                .WithOne()
                .HasForeignKey<AppointmentEntity>(x => x.PaymentId);

            builder.HasOne(x => x.PeriodPlan)
                .WithMany()
                .HasForeignKey(x => x.PeriodPlanId);

        }
    }
}
