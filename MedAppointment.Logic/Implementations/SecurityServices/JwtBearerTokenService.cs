namespace MedAppointment.Logics.Implementations.SecurityServices
{
    internal class JwtBearerTokenService : ITokenService
    {
        protected readonly ILogger<JwtBearerTokenService> Logger;
        protected readonly IConfiguration Configuration;

        public JwtBearerTokenService(ILogger<JwtBearerTokenService> logger,
            IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            int bytesLen = 32;
            var bytesLenAsText = Configuration["Jwt:RefreshTokenBytes"];
            if (int.TryParse(bytesLenAsText, out var tokenLength))
            {
                bytesLen = tokenLength;
            }
            else
            {
                bytesLen = 32;
            }
            var bytes = new byte[bytesLen];
            RandomNumberGenerator.Fill(bytes);

            return Convert.ToBase64String(bytes);
        }

        public string GetToken(IDictionary<string, object>? claims = null)
        {
            Logger.LogTrace("Starting Token Generation method");
            var issuer = Configuration["Jwt:Issuer"];
            var audience = Configuration["Jwt:Audience"];
            var signingKey = Configuration["Jwt:SigningKey"];
            var minutesAsText = Configuration["Jwt:AccessTokenMinutes"];

            if (!int.TryParse(minutesAsText, out var minutes))
            {
                Logger.LogError("AppSettings does contain correct integer type for \"Jwt:AccessTokenMinutes\"");
                throw new ArgumentException("AppSettings does contain correct integer type for \"Jwt:AccessTokenMinutes\"");
            }
            Logger.LogInformation("Retrieve all settings for Token generation");

            if (string.IsNullOrWhiteSpace(issuer))
            {
                Logger.LogError("Missing config: Jwt:Issuer");
                throw new InvalidOperationException("Missing config: Jwt:Issuer");
            }
            if (string.IsNullOrWhiteSpace(audience))
            {
                Logger.LogError("Missing config: Jwt:Audience");
                throw new InvalidOperationException("Missing config: Jwt:Audience");
            }

            if (string.IsNullOrWhiteSpace(signingKey))
            {
                Logger.LogError("Missing config: Jwt:SigningKey");
                throw new InvalidOperationException("Missing config: Jwt:SigningKey");
            }

            // HS256 üçün 32+ simvol (256-bit) tövsiyə olunur
            if (signingKey.Length < 32)
            {
                Logger.LogWarning("Missing config: Jwt:SigningKey");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Logger.LogInformation("Created credentials for token generation");

            var nowUtc = DateTime.UtcNow;

            // Default claim-lər
            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(nowUtc).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // İstifadəçidən gələn claim-lər
            if (claims is not null)
            {
                foreach (var kvp in claims)
                {
                    if (kvp.Value is null) continue;

                    // Bir neçə dəyər göndərilirsə (role-lar kimi)
                    if (kvp.Value is IEnumerable<string> many && kvp.Value is not string)
                    {
                        foreach (var v in many)
                            claimList.Add(new Claim(kvp.Key, v));
                    }
                    else
                    {
                        claimList.Add(new Claim(kvp.Key, kvp.Value.ToString()!));
                    }
                }
            }

            Logger.LogDebug("All claims added");

            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claimList,
                notBefore: nowUtc,
                expires: nowUtc.AddMinutes(minutes),
                signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Logger.LogDebug("Created token for user"); 
            Logger.LogTrace("Finished token generation!");
            return token;
        }
    }
}
