namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class ProfessionConfig : BaseClassfierConfig<ProfessionEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ProfessionEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Professions", "Classifier");
        }
    }
}
