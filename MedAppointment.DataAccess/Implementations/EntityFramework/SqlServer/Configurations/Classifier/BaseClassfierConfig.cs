namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public abstract class BaseClassfierConfig<TEntity> : BaseConfig<TEntity> where TEntity : BaseClassfierEntity, new()
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(e => e.Key)
                .IsUnique(true);

            builder.Property(e => e.NameTextId)
                .IsRequired();

            builder.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(e => e.DescriptionTextId)
                .IsRequired();

            builder.HasOne(x => x.Name)
                .WithOne()
                .HasForeignKey<TEntity>(x => x.NameTextId);

            builder.HasOne(x => x.Description)
                .WithOne()
                .HasForeignKey<TEntity>(x => x.DescriptionTextId);
        }
    }
}
