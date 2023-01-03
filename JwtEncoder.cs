using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtSigningLab
{
    public class JwtEncoder
    {
        public int Run()
        {
            Console.Error.WriteLine("encoding:");
            try
            {
                using (var rsa = System.Security.Cryptography.RSA.Create())
                {
                    string certContents = File.ReadAllText("private-key.pem");
                    rsa.ImportFromPem(certContents);
                    SecurityKey key = new Microsoft.IdentityModel.Tokens.RsaSecurityKey(rsa);
                    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(),
                        Audience = "expenseReports",
                        Issuer = "expenseReports",
                        Expires = DateTime.UtcNow.AddSeconds(30),
                        SigningCredentials = signingCredentials
                    };
                    tokenDescriptor.Subject.AddClaim(new Claim("memberNumber", "1064407"));
                    tokenDescriptor.Subject.AddClaim(new Claim("personalIdNumber", "191212-1212"));
                    tokenDescriptor.Subject.AddClaim(new Claim("lastName", "Tolvansson"));
                    tokenDescriptor.Subject.AddClaim(new Claim("firstName", "Tolvan"));
                    tokenDescriptor.Subject.AddClaim(new Claim("electedUnionRepresentative", "true"));
                    tokenDescriptor.Subject.AddClaim(new Claim("mobilePhone", "0700124578"));
                    tokenDescriptor.Subject.AddClaim(new Claim("emailAddress", "test@test.se"));
                    tokenDescriptor.Subject.AddClaim(new Claim("orgUnitId", "07 Gävleborg"));
                    tokenDescriptor.Subject.AddClaim(new Claim("streetAddress", "Testgatan 1"));
                    tokenDescriptor.Subject.AddClaim(new Claim("coAddress", "c/o Bengtsson"));
                    tokenDescriptor.Subject.AddClaim(new Claim("postalCode", "12345"));
                    tokenDescriptor.Subject.AddClaim(new Claim("city", "Testköping"));

                    tokenDescriptor.Subject.AddClaim(new Claim("id", "123456"));

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwt = tokenHandler.WriteToken(token);
                    Console.Error.WriteLine("This is our generated JWT:");
                    Console.WriteLine(jwt);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception caught, exiting");
                Console.Error.WriteLine(ex.Message);
                return -1;
            }

        }
    }
}
