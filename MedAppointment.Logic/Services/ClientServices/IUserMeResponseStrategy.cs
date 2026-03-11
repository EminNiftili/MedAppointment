namespace MedAppointment.Logics.Services.ClientServices
{
    /// <summary>
    /// Strategy for building the "users/me" response per user type (Admin, Doctor, plain User).
    /// </summary>
    public interface IUserMeResponseStrategy
    {
        /// <summary>
        /// Whether this strategy can handle the given user types (e.g. Doctor, SystemAdmin, User).
        /// </summary>
        bool CanHandle(UserType[] userTypes);

        /// <summary>
        /// Builds the me response for the given user. Returns Result with UserMeDto or DoctorUserMeDto.
        /// </summary>
        Task<Result<object>> BuildAsync(long userId);
    }
}
