namespace MedAppointment.DataTransferObjects.CredentialDtos
{
    public record TokenDto
    {
        public TokenDto(string accessToken, 
            string refreshToken, 
            Dictionary<string, string>? datas = null)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Datas = datas ?? new Dictionary<string, string>();
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Dictionary<string, string> Datas { get; set; }
    }
}
