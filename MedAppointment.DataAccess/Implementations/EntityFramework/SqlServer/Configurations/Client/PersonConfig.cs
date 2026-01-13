namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client
{
    public class PersonConfig : BaseConfig<PersonEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.ToTable("People", "Client");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.FatherName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.ImageId)
                .IsRequired(false);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(x => x.BirthDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(x => x.User)
                .WithOne(x => x.Person)
                .HasForeignKey<UserEntity>(x => x.PersonId);

            builder.HasOne(x => x.Image)
                .WithOne()
                .HasForeignKey<PersonEntity>(x => x.ImageId);
        }
    }
}
