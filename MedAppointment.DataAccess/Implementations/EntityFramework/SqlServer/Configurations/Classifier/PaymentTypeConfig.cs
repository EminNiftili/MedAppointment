namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class PaymentTypeConfig : BaseClassfierConfig<PaymentTypeEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PaymentTypeEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("PaymentTypes", "Classifier");
        }
    }
}
