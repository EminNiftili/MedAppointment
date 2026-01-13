namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client
{
    public class UserConfig : BaseConfig<UserEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users", "Client");

            builder.Property(x => x.PersonId)
                .IsRequired();

            builder.Property(x => x.Provider)
                .IsRequired()
                .HasComment("0 -> Traditional\n1 -> Google\n2 -> Facebook\n3 -> Apple");

            builder.HasOne(x => x.Admin)
                .WithOne(x => x.User)
                .HasForeignKey<AdminEntity>(x => x.UserId);

            builder.HasOne(x => x.Doctor)
                .WithOne(x => x.User)
                .HasForeignKey<DoctorEntity>(x => x.UserId);

            builder.HasOne(x => x.Person)
                .WithOne(x => x.User)
                .HasForeignKey<UserEntity>(x => x.PersonId);

            builder.HasOne(x => x.TraditionalUser)
                .WithOne(x => x.User)
                .HasForeignKey<TraditionalUserEntity>(x => x.UserId);

            builder.HasMany(x => x.Sessions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.SentChats)
                .WithOne(x => x.SenderUser)
                .HasForeignKey(x => x.SenderUserId);

            builder.HasMany(x => x.ReceiverChats)
                .WithOne(x => x.ReceiverUser)
                .HasForeignKey(x => x.ReceiverUserId);
        }
    }
}
