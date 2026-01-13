
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Communication
{
    public class ChatConfig : BaseConfig<ChatEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ChatEntity> builder)
        {
            builder.ToTable("Chats", "Communication");

            builder.Property(e => e.SenderUserId)
                .IsRequired();
            builder.Property(e => e.ReceiverUserId)
                .IsRequired();

            builder.HasOne(x => x.SenderUser)
                .WithMany()
                .HasForeignKey(x => x.SenderUserId);

            builder.HasOne(x => x.ReceiverUser)
                .WithMany()
                .HasForeignKey(x => x.ReceiverUserId);

            builder.HasMany(x => x.Histories)
                .WithOne(x => x.Chat)
                .HasForeignKey(x => x.ChatId);
        }
    }
}
