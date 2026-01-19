namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class PrivateClientInfoService : IPrivateClientInfoService
    {
        protected readonly ILogger<PrivateClientInfoService> Logger;
        protected readonly IUnitOfClient UnitOfClient;

        public PrivateClientInfoService(ILogger<PrivateClientInfoService> logger, 
            IUnitOfClient unitOfClient)
        {
            Logger = logger;
            UnitOfClient = unitOfClient;
        }

        public async Task<UserType[]> GetUserTypesAsync(long userId)
        {
            List<UserType> userTypes = new List<UserType>();
            var user = await UnitOfClient.User.GetByIdAsync(userId);
            var organizationUser = await UnitOfClient.OrganizationUser.FindFirstAsync(x => x.UserId == userId);
            if(user == null)
            {
                return PrepareReturnObject();
            }
            if (user.Admin != null)
            {
                userTypes.Add(UserType.SystemAdmin);
            }
            if (user.Doctor != null)
            {
                userTypes.Add(UserType.Doctor);
            }
            if(organizationUser != null && organizationUser.IsAdmin)
            {
                userTypes.Add(UserType.OrganizationAdmin);
            }
            
            if(!userTypes.Any())
            {
                userTypes.Add(UserType.User);
            }
            
            return PrepareReturnObject();

            UserType[] PrepareReturnObject()
            {
                return userTypes.ToArray();
            }
        }
    }
}
