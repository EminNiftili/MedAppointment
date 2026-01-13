namespace MedAppointment.Entities.Payment
{
    public class PaymentEntity : BaseEntity
    {
        public long PaymentTypeId { get; set; }
        /// <summary>
        /// <list type="bullet">
        /// <item>0 -> Canceled</item>
        /// <item>1 -> Pending</item>
        /// <item>2- > Partailly Paid</item>
        /// <item>3 -> Paid</item>
        /// <item>4 -> Refund</item>
        /// </list>
        /// </summary>
        public byte Status{ get; set; }


        public PaymentTypeEntity? PaymentType { get; set; }
    }
}
