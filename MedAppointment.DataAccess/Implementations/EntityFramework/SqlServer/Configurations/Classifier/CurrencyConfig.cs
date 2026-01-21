namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class CurrencyConfig : BaseClassfierConfig<CurrencyEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<CurrencyEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("Currencies", "Classifier");

            builder.Property(x => x.Coefficent)
                .IsRequired()
                .HasColumnType("decimal(8,4)");
        }
    }
}
