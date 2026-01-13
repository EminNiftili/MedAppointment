
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Service
{
    public class DayPlanConfig : BaseConfig<DayPlanEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DayPlanEntity> builder)
        {
            builder.ToTable("DayPlans", "Service");

            builder.Property(e => e.DoctorId)
                .IsRequired();
            builder.Property(e => e.SpecialtyId)
                .IsRequired();
            builder.Property(e => e.PeriodId)
                .IsRequired();

            builder.Property(e => e.DayOfWeek)
                .IsRequired()
                .HasComment("1=Monday ... 7=Sunday");

            builder.Property(e => e.OpenTime)
                .IsRequired()
                .HasColumnType("time(7)");
            builder.Property(e => e.CloseTime)
                .IsRequired()
                .HasColumnType("time(7)");

            builder.Property(e => e.IsClosed)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId);

            builder.HasOne(x => x.Specialty)
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId);

            builder.HasOne(x => x.Period)
                .WithMany()
                .HasForeignKey(x => x.PeriodId);

        }
    }
}
