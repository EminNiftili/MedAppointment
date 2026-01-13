namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    public class OrganizationUserConfig : BaseConfig<OrganizationUserEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OrganizationUserEntity> builder)
        {
            builder.ToTable("OrganizationUsers", "Composition");

            builder.Property(e => e.IsAdmin)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.OrganizationId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<OrganizationUserEntity>(x => x.UserId);


            builder.HasOne(x => x.Organization)
                .WithMany(x => x.OrganizationUsers)
                .HasForeignKey(x => x.OrganizationId);
        }
    }
}
