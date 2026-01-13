namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public abstract class BaseClassfierConfig<TEntity> : BaseConfig<TEntity> where TEntity : BaseClassfierEntity, new()
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(e => e.Name)
                .IsUnique();
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode();


            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode();
        }
    }
}
