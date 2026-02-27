namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    internal class DoctorServiceLanguageConfig : BaseConfig<DoctorServiceLanguageEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DoctorServiceLanguageEntity> builder)
        {
            builder.ToTable("DoctorServiceLanguages", "Compositions");

            builder.Property(e => e.DoctorId)
                .IsRequired();
            builder.Property(e => e.LanguageId)
                .IsRequired();

            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.ServiceLanguages)
                .HasForeignKey(x => x.DoctorId);

            builder.HasOne(x => x.Language)
                .WithMany()
                .HasForeignKey(x => x.LanguageId);

        }
    }
}
