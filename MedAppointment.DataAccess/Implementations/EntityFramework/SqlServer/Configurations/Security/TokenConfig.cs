namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security
{
    public class TokenConfig : BaseConfig<TokenEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<TokenEntity> builder)
        {
            builder.ToTable("Tokens", "Security");

            builder.HasIndex(x => x.RefreshToken)
                .IsUnique(true);

            builder.HasIndex(x => x.AccessToken)
                .IsUnique(true);

            builder.Property(e => e.SessionId)
                .IsRequired();
            builder.Property(e => e.AccessToken)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.ExpiredDate)
                .IsRequired()
                .HasDefaultValueSql("getdate()")
                .HasColumnType("datetime");

            builder.Property(e => e.IsExpired)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.HasOne(x => x.Session)
                .WithMany(x => x.Tokens)
                .HasForeignKey(x => x.SessionId);

        }
    }
}
