
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Doctor
{
    public class DayBreakConfig : BaseConfig<DayBreakEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DayBreakEntity> builder)
        {
            builder.ToTable("DayBreaks", "Doctor");

            builder.Property(e => e.DaySchemaId)
                .IsRequired();

            builder.Property(e => e.StartTime)
                .IsRequired()
                .HasColumnType("time(7)");
            builder.Property(e => e.EndTime)
                .IsRequired()
                .HasColumnType("time(7)");

            builder.Property(e => e.Name)
                .IsRequired(false);

            builder.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(x => x.DaySchema)
                .WithMany(x => x.DayBreaks)
                .HasForeignKey(x => x.DaySchemaId);
        }
    }
}
