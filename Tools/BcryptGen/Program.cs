using System;

var pwd = args.Length > 0 ? args[0] : "Aa12345678";
var work = args.Length > 1 && int.TryParse(args[1], out var w) ? w : 10;
var hash = BCrypt.Net.BCrypt.HashPassword(pwd, work);
Console.WriteLine(hash);
