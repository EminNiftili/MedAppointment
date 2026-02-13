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

            builder.Property(x => x.TitleTextId)
                .IsRequired();

            builder.Property(x => x.DescriptionTextId)
                .IsRequired();

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Doctor)
                .HasForeignKey<DoctorEntity>(x => x.UserId);

            builder.HasOne(x => x.Title)
                .WithOne()
                .HasForeignKey<DoctorEntity>(x => x.TitleTextId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Description)
                .WithOne()
                .HasForeignKey<DoctorEntity>(x => x.DescriptionTextId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
