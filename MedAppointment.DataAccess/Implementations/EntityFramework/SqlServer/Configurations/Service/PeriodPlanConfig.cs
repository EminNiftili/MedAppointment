
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Service
{
    public class PeriodPlanConfig : BaseConfig<PeriodPlanEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PeriodPlanEntity> builder)
        {
            builder.ToTable("PeriodPlans", "Service");

            builder.Property(e => e.DayPlanId)
                .IsRequired();
            builder.Property(e => e.CurrencyId)
                .IsRequired();

            builder.Property(e => e.PeriodStart)
                .IsRequired()
                .HasColumnType("time(7)");
            builder.Property(e => e.PeriodStop)
                .IsRequired()
                .HasColumnType("time(7)");

            builder.Property(e => e.IsOnSiteService)
                .IsRequired()
                .HasDefaultValueSql("1");

            builder.Property(e => e.IsOnlineService)
                .IsRequired()
                .HasDefaultValueSql("1");

            builder.Property(e => e.IsBusy)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(e => e.PricePerPeriod)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            builder.HasOne(x => x.DayPlan)
                .WithMany()
                .HasForeignKey(x => x.DayPlanId);

            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);

        }
    }
}
