
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
                .WithMany(x => x.SentChats)
                .HasForeignKey(x => x.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ReceiverUser)
                .WithMany(x => x.ReceiverChats)
                .HasForeignKey(x => x.ReceiverUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Histories)
                .WithOne(x => x.Chat)
                .HasForeignKey(x => x.ChatId);
        }
    }
}
