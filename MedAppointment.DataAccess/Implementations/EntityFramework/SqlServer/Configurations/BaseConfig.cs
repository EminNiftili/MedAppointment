namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations
{
    public abstract class BaseConfig<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity, new()
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureEntity(builder);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .IsRequired()
                .UseIdentityColumn(1, 1)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValueSql("0");
        }
        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
