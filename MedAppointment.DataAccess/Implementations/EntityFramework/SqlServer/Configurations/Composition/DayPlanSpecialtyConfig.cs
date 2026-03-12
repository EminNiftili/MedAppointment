namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    public class DayPlanSpecialtyConfig : BaseConfig<DayPlanSpecialtyEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DayPlanSpecialtyEntity> builder)
        {
            builder.ToTable("DayPlanSpecialties", "Compositions");

            builder.Property(e => e.DayPlanId)
                .IsRequired();

            builder.Property(e => e.SpecialtyId)
                .IsRequired();

            builder.HasOne(x => x.DayPlan)
                .WithMany(x => x.DayPlanSpecialties)
                .HasForeignKey(x => x.DayPlanId);

            builder.HasOne(x => x.Specialty)
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId);

            builder.HasIndex(x => new { x.DayPlanId, x.SpecialtyId })
                .IsUnique()
                .HasDatabaseName("IX_DayPlanSpecialties_DayPlanId_SpecialtyId");
        }
    }
}
