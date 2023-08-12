using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure;

public class PasswordEncryptor
{
    public static string Encrpt(string Password)
    {
        using var md5 = MD5.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes(Password);
        byte[] hashBytes = md5.ComputeHash(inputBytes);


        return Convert.ToHexString(hashBytes);
    }
}
