
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Communication
{
    public class ChatHistoryConfig : BaseConfig<ChatHistoryEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ChatHistoryEntity> builder)
        {
            builder.ToTable("ChatHistories", "Communication");

            builder.Property(e => e.ChatId)
                .IsRequired();
            builder.Property(e => e.UserId)
                .IsRequired();

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode();

            builder.Property(x => x.IsSent)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(x => x.IsSeen)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Chat)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.ChatId);
        }
    }
}
