namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security
{
    public class TokenConfig : BaseConfig<TokenEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TokenEntity> builder)
        {
            builder.ToTable("Tokens", "Security");

            builder.Property(e => e.SessionId)
                .IsRequired();
            builder.Property(e => e.AccessToken)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.Session)
                .WithMany(x => x.Tokens)
                .HasForeignKey(x => x.SessionId);

        }
    }
}
