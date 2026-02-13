namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class LanguageConfig : BaseConfig<LanguageEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<LanguageEntity> builder)
        {
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.ToTable("Languages", "Classifier");

            builder.Property(x => x.IsDefault)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();
        }
    }
}
