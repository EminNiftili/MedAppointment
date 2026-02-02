namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class LanguageConfig : BaseClassfierConfig<LanguageEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<LanguageEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Languages", "Classifier");

            builder.Property(x => x.IsDefault)
                .IsRequired()
                .HasDefaultValueSql("0");
        }
    }
}
