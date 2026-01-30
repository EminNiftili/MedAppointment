namespace MedAppointment.Entities.Classifier
{
    public class PlanPaddingTypeEntity : BaseClassfierEntity
    {
        /// <summary>
        /// Padding position define where is Padding added. (in minutes)
        /// <list type="bullet">
        /// <item>1 -> Start of Period</item>
        /// <item>2 -> End of Period</item>
        /// <item>3 -> Linear Between of Period</item>
        /// <item>4 -> Center Between of Period</item>
        /// </list>
        /// </summary>
        public byte PaddingPosition { get; set; }
        /// <summary>
        /// Padding of between all Periods. (in minutes)
        /// </summary>
        public byte PaddingTime { get; set; }
    }
}
