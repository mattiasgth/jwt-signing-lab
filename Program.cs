// See https://aka.ms/new-console-template for more information
using JwtSigningLab;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;

Console.Error.WriteLine("=============================");
Console.Error.WriteLine("       JWT signing lab       ");
Console.Error.WriteLine("=============================");

string cmd = args.Length > 0 ? args[0] : "encode";

switch (cmd)
{
    case "decode":
        var decoder = new JwtDecoder();
        decoder.Run();
        break;
    default:
    case "encode":
        var encoder = new JwtEncoder();
        encoder.Run();
        break;
}





