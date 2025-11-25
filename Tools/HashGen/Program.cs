using System;
using System.Security.Cryptography;

string password = args.Length > 0 ? args[0] : "Aa12345678";
int iterations = 100000;
using var rng = RandomNumberGenerator.Create();
var salt = new byte[16];
rng.GetBytes(salt);
using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
var hash = pbkdf2.GetBytes(32);
Console.WriteLine($"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}");
