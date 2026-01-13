namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client
{
    public class AdminConfig : BaseConfig<AdminEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<AdminEntity> builder)
        {
            builder.ToTable("Admins", "Client");

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Admin)
                .HasForeignKey<AdminEntity>(x => x.UserId);
        }
    }
}
