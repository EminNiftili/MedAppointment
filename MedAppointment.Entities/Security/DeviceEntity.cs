namespace MedAppointment.Entities.Security
{
    public class DeviceEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        /// <summary>
        /// <list type="bullet">
        /// <item>0 -> Android </item>
        /// <item>1 -> iOS </item>
        /// <item>2 -> Windows </item>
        /// <item>3 -> Mac </item>
        /// <item>4 -> Linux </item>
        /// </list>
        /// </summary>
        public byte DeviceType { get; set; }

        /// <summary>
        /// <list type="bullet">
        /// <item>0 -> Web </item>
        /// <item>1 -> Mobile </item>
        /// </list>
        /// </summary>
        public byte AppType { get; set; }
        public string? OSName { get; set; }
        public string? OSVersion { get; set; }
        public string UUID { get; set; } = null!;
    }
}
