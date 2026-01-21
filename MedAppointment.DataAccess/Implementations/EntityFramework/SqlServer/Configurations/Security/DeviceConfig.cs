
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security
{
    public class DeviceConfig : BaseConfig<DeviceEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DeviceEntity> builder)
        {
            builder.ToTable("Devices", "Security");

            builder.HasIndex(x => x.UUID)
                .IsUnique(false);

            builder.HasIndex(x => x.Name)
                .IsUnique(false);

            builder.HasIndex(x => x.OSName)
                .IsUnique(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(e => e.DeviceType)
                .IsRequired()
                .HasComment("0 -> Android\n1 -> iOS\n2 -> Windows\n3 -> Mac\n4 -> Linux");
            builder.Property(e => e.AppType)
                .IsRequired()
                .HasComment("0 -> Web\n1 -> Mobile");

            builder.Property(e => e.OSName)
                .IsRequired(false)
                .HasMaxLength(150);
            builder.Property(e => e.OSVersion)
                .IsRequired(false)
                .HasMaxLength(150);
            builder.Property(e => e.UUID)
                .IsRequired()
                .HasMaxLength(300);
        }
    }
}
