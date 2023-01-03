# jwt-signing-lab
Signing a JWT using .NET 6.

This command line app encodes or decodes a JWT token using a private/public key pair.
I've used it to test different variants, and to play around with different algorithms and key lengths.
Use `JstSigningLab encode` to encode, and `JstSigningLab decode` to decode it.
The private/public keys can be generated in this way:
```
 openssl genrsa -out private-key.pem 2048
 openssl rsa -in private-key.pem -pubout -out public-key.crt
```
Included in the project is a sample key pair (`private-key-pem` and `public-key.crt`), don't use it in a production scenario.
When generating the JWT, you can redirect the output to a file, using
```
 JwtSigningLab encode > jwt.txt
```

