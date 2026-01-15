namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security
{
    public class SessionConfig : BaseConfig<SessionEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.ToTable("Sessions", "Security");

            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.DeviceId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Sessions)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Device)
                .WithOne()
                .HasForeignKey<SessionEntity>(x => x.DeviceId);

            builder.HasMany(x => x.Tokens)
                .WithOne(x => x.Session)
                .HasForeignKey(x => x.SessionId);

        }
    }
}
