namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.File
{
    public class ImageConfig : BaseConfig<ImageEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ImageEntity> builder)
        {
            builder.ToTable("Images", "File");

            builder.Property(e => e.Filename)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.MimeType)
                .IsRequired()
                .HasMaxLength(75);
            builder.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
