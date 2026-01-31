namespace MedAppointment.DataTransferObjects.Enums
{
    /// <summary>
    /// Matches PlanPaddingTypeEntity.PaddingPosition (0 = no padding, 1-4).
    /// Example: period 11:00-11:30 (30 min), padding 5 min.
    /// </summary>
    public enum PlanPaddingPosition : byte
    {
        /// <summary>0 - No padding: full period, next start = current end.</summary>
        NoPadding = 0,
        /// <summary>1 - Cut from start of period. 11:00-11:30 -> 11:05-11:30</summary>
        StartOfPeriod = 1,
        /// <summary>2 - Cut from end of period. 11:00-11:30 -> 11:00-11:25</summary>
        EndOfPeriod = 2,
        /// <summary>3 - Full period, add padding between. 1st 11:00-11:30, 2nd 11:35-12:05</summary>
        LinearBetweenOfPeriod = 3,
        /// <summary>4 - Split gap: take half from end of first, half from start of second. 1st 11:00-11:28, 2nd 11:32-12:00</summary>
        CenterBetweenOfPeriod = 4,
    }
}
