
namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    public class DoctorSpecialtyConfig : BaseConfig<DoctorSpecialtyEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DoctorSpecialtyEntity> builder)
        {
            builder.ToTable("DoctorSpecialties", "Compositions");

            builder.Property(e => e.IsConfirm)
                .IsRequired()
                .HasDefaultValueSql("0");
            builder.Property(e => e.DoctorId)
                .IsRequired();
            builder.Property(e => e.SpecialtyId)
                .IsRequired();

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.Specialties)
                .HasForeignKey(x => x.DoctorId);

            builder.HasOne(x => x.Specialty)
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId);

        }
    }
}
