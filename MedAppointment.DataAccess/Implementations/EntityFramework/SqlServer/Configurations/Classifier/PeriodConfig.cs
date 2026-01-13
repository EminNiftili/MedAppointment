namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class PeriodConfig : BaseClassfierConfig<PeriodEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PeriodEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Periods", "Classifier");

            builder.Property(e => e.PeriodTime)
                .IsRequired()
                .HasComment("Period time in minutes");
        }
    }
}
