namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Localization
{
    public class TranslationConfig : BaseConfig<TranslationEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TranslationEntity> builder)
        {
            builder.ToTable("Translations", "Localization");

            builder.HasIndex(x => new { x.LanguageId, x.ResourceId })
                .IsUnique(true);

            builder.Property(x => x.LanguageId)
                .IsRequired();
            builder.Property(x => x.ResourceId)
                .IsRequired();

            builder.Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(x => x.Resource)
                .WithMany(x => x.Translations)
                .HasForeignKey(x => x.ResourceId);

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId);
        }
    }
}
