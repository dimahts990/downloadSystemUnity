using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public static class NetworkManager
{
    public static IEnumerator isInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("https://google.com/");
        yield return www;
        if (www.error != null)
            action(false);
        else
            action(true);
    }

    public static Dictionary<string, string> JsonToDictionary(this string jsonRequest) =>
        JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonRequest);

    public static long RemoteSizeFile(Dictionary<string, string> remoteFiles, Uri uri)
    {
        long size=0;
        foreach (var file in remoteFiles)
        {
            WebClient wc = new WebClient();
            wc.OpenRead($"{uri}{file.Key}");
            size += Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
            wc.CancelAsync();
        }

        return size;
    }
//.ToString("#.#")
    public static string LongToMbyte(this long longSize)
    {
        string request = "";
        string mb = (Convert.ToDouble(longSize) / 1048576).ToString("#.#");
        string[] parts = mb.Split(',');
        if(parts.Length>1)
            request = $"{(parts[0] == "" ? "0" : parts[0])},{(parts[1] == "" ? "0" : parts[1])}";
        else
            request = $"{(parts[0] == "" ? "0" : parts[0])},0";
        return request;
    }


}
    