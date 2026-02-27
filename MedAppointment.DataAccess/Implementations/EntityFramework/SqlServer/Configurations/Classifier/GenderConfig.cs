namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class GenderConfig : BaseClassfierConfig<GenderEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GenderEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Genders", "Classifier");
        }
    }
}
