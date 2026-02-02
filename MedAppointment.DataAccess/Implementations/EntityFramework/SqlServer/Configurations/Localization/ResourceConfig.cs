namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Localization
{
    public class ResourceConfig : BaseConfig<ResourceEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ResourceEntity> builder)
        {
            builder.ToTable("Resources", "Localization");

            builder.HasIndex(x => x.Key)
                .IsUnique(true);

            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Resource)
                .HasForeignKey(x => x.ResourceId);
        }
    }
}
