
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Communication
{
    public class MeetConfig : BaseConfig<MeetEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<MeetEntity> builder)
        {
            builder.ToTable("Meets", "Communication");

            builder.Property(e => e.SenderUserId)
                .IsRequired();
            builder.Property(e => e.ReceiverUserId)
                .IsRequired();

            builder.HasOne(x => x.SenderUser)
                .WithMany()
                .HasForeignKey(x => x.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceiverUser)
                .WithMany()
                .HasForeignKey(x => x.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
