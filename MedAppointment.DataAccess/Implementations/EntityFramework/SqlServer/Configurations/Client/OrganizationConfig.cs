namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client
{
    public class OrganizationConfig : BaseConfig<OrganizationEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<OrganizationEntity> builder)
        {
            builder.ToTable("Organizations", "Client");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode();

            builder.Property(x => x.EmailCode)
                .IsRequired(false)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasMany(x => x.OrganizationUsers)
                .WithOne(x => x.Organization)
                .HasForeignKey(x => x.OrganizationId);
        }
    }
}
