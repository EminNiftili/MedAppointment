namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Doctor
{
    public class DaySchemaConfig : BaseConfig<DaySchemaEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DaySchemaEntity> builder)
        {
            builder.ToTable("DaySchemas", "Doctor");

            builder.Property(e => e.WeeklySchemaId)
                .IsRequired();
            builder.Property(e => e.SpecialtyId)
                .IsRequired();
            builder.Property(e => e.PeriodId)
                .IsRequired();
            builder.Property(e => e.PlanPaddingTypeId)
                .IsRequired(false);

            builder.Property(e => e.DayOfWeek)
                .IsRequired()
                .HasComment("1=Monday ... 7=Sunday");

            builder.Property(e => e.OpenTime)
                .IsRequired()
                .HasColumnType("time(7)");

            builder.Property(e => e.PeriodCount)
                .IsRequired()
                .HasDefaultValueSql("0")
                .HasComment("Number of periods (slots) for this day; 0 when closed.");

            builder.Property(e => e.IsClosed)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(e => e.IsOnlineService)
                .IsRequired()
                .HasDefaultValueSql("1");

            builder.Property(e => e.IsOnSiteService)
                .IsRequired()
                .HasDefaultValueSql("1");

            builder.HasOne(x => x.WeeklySchema)
                .WithMany()
                .HasForeignKey(x => x.WeeklySchemaId);

            builder.HasOne(x => x.Specialty)
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId);

            builder.HasOne(x => x.Period)
                .WithMany()
                .HasForeignKey(x => x.PeriodId);

            builder.HasOne(x => x.PlanPaddingType)
                .WithOne()
                .HasForeignKey<DaySchemaEntity>(x => x.PlanPaddingTypeId);
        }
    }
}
