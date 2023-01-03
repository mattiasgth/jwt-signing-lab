using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace JwtSigningLab
{
    public class JwtDecoder
    {
        public int Run()
        {
            Console.Error.WriteLine("decoding:");
            try
            {
                string inputJwt = File.ReadAllText("jwt.txt");
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(inputJwt))
                {
                    Console.Error.WriteLine("Failed validation, inputJwt is not valid.");
                    return -1;
                }
                SecurityKey? key = null;
                var cert = File.ReadAllText("public-key.crt");
                var rs256key = cert
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "");
                var keyBytes = Convert.FromBase64String(rs256key);
                AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
                RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
                RSAParameters rsaParameters = new RSAParameters
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                };
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaParameters);
                key = new RsaSecurityKey(rsa);
                ClaimsPrincipal principal = tokenHandler.ValidateToken(inputJwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidIssuer = "???",
                    ValidAudience = "expenseReports",
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);

                foreach(var claim in principal.Claims)
                {
                    if (claim.Type.Equals("memberNumber"))
                    {
                        Console.WriteLine($"memberNumber claim is {claim.Value}");
                    }
                }
                Console.WriteLine("JWT validated ok");

                return 0;
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
