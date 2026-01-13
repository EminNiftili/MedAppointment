namespace MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Payment
{
    public class PaymentConfig : BaseConfig<PaymentEntity>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("Payments", "Payment");

            builder.Property(e => e.PaymentTypeId)
                .IsRequired();
            builder.Property(e => e.Status)
                .IsRequired()
                .HasComment("0 -> Canceled\n1 -> Pending\n2- > Partailly Paid\n3 -> Paid\n4 -> Refund");

            builder.HasOne(x => x.PaymentType)
                .WithMany()
                .HasForeignKey(x => x.PaymentTypeId);

        }
    }
}
