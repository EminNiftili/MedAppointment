namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security
{
    public class TraditionalUserConfig : BaseConfig<TraditionalUserEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TraditionalUserEntity> builder)
        {
            builder.ToTable("TraditionalUsers", "Security");


            builder.HasIndex(x => x.PasswordHash)
                .IsUnique(true);

            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(350);

            builder.HasOne(x => x.User)
                .WithOne(x => x.TraditionalUser)
                .HasForeignKey<TraditionalUserEntity>(x => x.UserId);

        }
    }
}
