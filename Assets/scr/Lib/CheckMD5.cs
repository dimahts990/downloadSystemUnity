using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

public static class CheckMD5
{
    /// <summary>
    /// проверка MD5
    /// </summary>
    /// <param name="path">путь к файлу</param>
    public static bool MD5result(this string path, string fileMD5)
    {
        string result = "";
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(path))
            {
                byte[] checkSum = md5.ComputeHash(stream);
                result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                Debug.Log(result);
            }
        }
        return result == fileMD5;
    } 
}