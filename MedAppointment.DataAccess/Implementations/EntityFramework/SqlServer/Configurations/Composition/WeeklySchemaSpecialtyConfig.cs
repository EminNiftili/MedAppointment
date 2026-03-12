namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition
{
    public class WeeklySchemaSpecialtyConfig : BaseConfig<WeeklySchemaSpecialtyEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WeeklySchemaSpecialtyEntity> builder)
        {
            builder.ToTable("WeeklySchemaSpecialties", "Compositions");

            builder.Property(e => e.WeeklySchemaId)
                .IsRequired();

            builder.Property(e => e.SpecialtyId)
                .IsRequired();

            builder.HasOne(x => x.WeeklySchema)
                .WithMany(x => x.WeeklySchemaSpecialties)
                .HasForeignKey(x => x.WeeklySchemaId);

            builder.HasOne(x => x.Specialty)
                .WithMany()
                .HasForeignKey(x => x.SpecialtyId);

            builder.HasIndex(x => new { x.WeeklySchemaId, x.SpecialtyId })
                .IsUnique()
                .HasDatabaseName("IX_WeeklySchemaSpecialties_WeeklySchemaId_SpecialtyId");
        }
    }
}
