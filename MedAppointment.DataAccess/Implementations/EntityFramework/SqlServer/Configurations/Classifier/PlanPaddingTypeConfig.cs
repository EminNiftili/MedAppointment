namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier
{
    public class PlanPaddingTypeConfig : BaseClassfierConfig<PlanPaddingTypeEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PlanPaddingTypeEntity> builder)
        {
            base.ConfigureEntity(builder);

            builder.ToTable("PlanPaddingTypes", "Classifier");

            builder.Property(x => x.PaddingPosition)
                .IsRequired()
                .HasComment("Padding position define where is Padding added. (in minutes)\n1 -> Start of Period\n2 -> End of Period\n3 -> Linear Between of Period\n4 -> Center Between of Period");

            builder.Property(x => x.PaddingTime)
                .IsRequired()
                .HasComment("Padding of between all Periods. (in minutes)");

        }
    }
}
