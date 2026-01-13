namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client
{
    public class DoctorConfig : BaseConfig<DoctorEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DoctorEntity> builder)
        {

            builder.ToTable("Doctors", "Client");

            builder.Property(e => e.IsConfirm)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode();

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Doctor)
                .HasForeignKey<DoctorEntity>(x => x.UserId);
        }
    }
}
