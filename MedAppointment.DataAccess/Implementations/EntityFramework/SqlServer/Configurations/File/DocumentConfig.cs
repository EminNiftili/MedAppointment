namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.File
{
    public class DocumentConfig : BaseConfig<DocumentEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<DocumentEntity> builder)
        {
            builder.ToTable("Documents", "File");

            builder.Property(e => e.DocumentId)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.HasIndex(e => e.DocumentId)
                .IsUnique();

            builder.Property(e => e.DoctorId)
                .IsRequired();

            builder.Property(e => e.SpecialtyId);

            builder.Property(e => e.IsProfessionBackground)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(e => e.IsExperience)
                .IsRequired()
                .HasDefaultValueSql("0");

            builder.Property(e => e.Filename)
                .IsRequired()
                .HasMaxLength(260);

            builder.Property(e => e.MimeType)
                .IsRequired()
                .HasMaxLength(127);

            builder.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.Title)
                .HasMaxLength(200);

            builder.Property(e => e.Issuer)
                .HasMaxLength(200);

            builder.Property(e => e.PeriodOfYear)
                .HasMaxLength(50);

            builder.Property(e => e.Description)
                .HasMaxLength(2000);

            builder.HasOne(e => e.Doctor)
                .WithMany()
                .HasForeignKey(e => e.DoctorId);

            builder.HasOne(e => e.Specialty)
                .WithMany()
                .HasForeignKey(e => e.SpecialtyId)
                .IsRequired(false);
        }
    }
}
