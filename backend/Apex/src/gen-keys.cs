#:property TargetFramework=net10.0
using System.Security.Cryptography;

//cd to folder and run the next command: dotnet run gen-keys.cs

using var rsa = RSA.Create(2048);
Console.WriteLine("PrivateKey (в Users.Api):");
Console.WriteLine(Convert.ToBase64String(rsa.ExportPkcs8PrivateKey()));
Console.WriteLine();
Console.WriteLine("PublicKey (в Portfolio.Api):");
Console.WriteLine(Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo()));