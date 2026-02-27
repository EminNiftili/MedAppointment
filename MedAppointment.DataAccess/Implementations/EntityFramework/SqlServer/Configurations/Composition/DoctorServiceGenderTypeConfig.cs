namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    internal class DoctorServiceGenderTypeConfig : BaseConfig<DoctorServiceGenderTypeEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DoctorServiceGenderTypeEntity> builder)
        {
            builder.ToTable("DoctorServiceGenderTypes", "Compositions");

            builder.Property(e => e.DoctorId)
                .IsRequired();
            builder.Property(e => e.GenderId)
                .IsRequired();

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.ServiceGenderTypes)
                .HasForeignKey(x => x.DoctorId);

            builder.HasOne(x => x.Gender)
                .WithMany()
                .HasForeignKey(x => x.GenderId);

        }
    }
}
