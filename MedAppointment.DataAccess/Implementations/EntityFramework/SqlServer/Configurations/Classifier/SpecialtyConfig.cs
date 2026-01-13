namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class SpecialtyConfig : BaseClassfierConfig<SpecialtyEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SpecialtyEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Specialties", "Classifier");
        }
    }
}
