namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Doctor
{
    public class WeeklySchemaConfig : BaseConfig<WeeklySchemaEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WeeklySchemaEntity> builder)
        {
            builder.ToTable("WeeklySchemas", "Doctor");

            builder.Property(e => e.DoctorId)
                .IsRequired();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ColorHex)
                .IsRequired()
                .HasColumnType("char(9)")
                .HasComment("Color format is RGBA. (#00000000)");

            builder.HasOne(x => x.Doctor)
                .WithMany()
                .HasForeignKey(x => x.DoctorId);

            builder.HasMany(x => x.DayPlans)
                .WithOne(x => x.WeeklySchema)
                .HasForeignKey(x => x.WeeklySchemaId);
        }
    }
}
