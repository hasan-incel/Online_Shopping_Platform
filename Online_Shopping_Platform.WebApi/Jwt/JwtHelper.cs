using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Online_Shopping_Platform.WebApi.Jwt
{
    public static class JwtHelper
    {
        // Method to generate a JWT token
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            // Create a security key from the secret key
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));

            // Create signing credentials using the security key and HMAC SHA256 algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Define the claims (user data) to be embedded in the token
            var claims = new[]
            {
            new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()),  // User ID claim
            new Claim(JwtClaimNames.FirstName, jwtInfo.FirstName),  // User's first name
            new Claim(JwtClaimNames.LastName, jwtInfo.LastName),  // User's last name
            new Claim(JwtClaimNames.Email, jwtInfo.Email),  // User's email
            new Claim(JwtClaimNames.Role, jwtInfo.Role.ToString()),  // User's role

            new Claim(ClaimTypes.Role, jwtInfo.Role.ToString())  // User's role as a standard claim
        };

            // Set the token expiration time
            var expireTime = DateTime.Now.AddMinutes(jwtInfo.ExpireMinutes);

            // Create a JwtSecurityToken with issuer, audience, claims, and expiration
            var tokenDescriptor = new JwtSecurityToken(jwtInfo.Issuer, jwtInfo.Audience, claims, null, expireTime, credentials);

            // Generate the JWT token as a string
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            // Return the generated JWT token
            return token;
        }
    }

}
