using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class downloadManager : MonoBehaviour
{
    public GameObject windowQuestion;
    public GameObject loadingWindow;
    public Text loadingWindowTv;
    public GameObject downloadProgressWindow;
    public Text progressTV;
    public Slider progressBar;
    public UnpackManager unpack;
    private string pathArhive;
    private long howMuchDownloaded;
    private long howMuchDownloadedAllFiles;
    private long allMb;
    private WebClient wc;
    private Dictionary<string, string> filesInfo;

    public  void ClickYes()
    {
        windowQuestion.SetActive(false);
        loadingWindow.SetActive(true);
        StartCoroutine(NetworkManager.isInternetConnection((isConnection) =>
        {
            loadingWindowTv.text = "Проверка файлов";
            if (isConnection)//если есть интернет соединение, то переходим к загрузке
            {
                StartCoroutine(CheckMD5Archive(res =>
                {
                    loadingWindow.SetActive(false);
                    if(res.Count!=0)
                    {
                        downloadProgressWindow.SetActive(true);
                        DownloadFiles(res);
                    }
                    else
                        unpack.Init(pathArhive,filesInfo);
                }));
                //Dictionary<string, string> res=CheckMD5Arhive();
                
            }
        }));
    }

    private IEnumerator CheckMD5Archive(Action<Dictionary<string, string>> files)
    {
        pathArhive = $"{Application.dataPath}/Files/Archive";
        var json = new WebClient().DownloadString("https://vintstudio.ru/datafileTest/md5.json");
        filesInfo = json.JsonToDictionary();
        Dictionary<string, string> downloadList=new Dictionary<string, string>();
        foreach (var jsonFile in filesInfo)
        {
            if (File.Exists($"{pathArhive}/{jsonFile.Key}"))//проверяем, есть ли файл по такому пути
            {
                if ($"{pathArhive}/{jsonFile.Key}".MD5result(jsonFile.Value))
                    howMuchDownloadedAllFiles += new FileInfo($"{pathArhive}/{jsonFile.Key}").Length;
                else
                {
                    File.Delete($"{pathArhive}/{jsonFile.Key}");
                    downloadList.Add(jsonFile.Key, jsonFile.Value);
                }
            }
            else
                downloadList.Add(jsonFile.Key,jsonFile.Value);
        }
        files(downloadList);
        yield break;
    }
    private Dictionary<string, string> CheckMD5Arhive()
    {
        pathArhive = $"{Application.dataPath}/Files/Archive";
        var json = new WebClient().DownloadString("https://vintstudio.ru/datafileTest/md5.json");
        filesInfo = json.JsonToDictionary();
        Dictionary<string, string> downloadList=new Dictionary<string, string>();
        foreach (var jsonFile in filesInfo)
        {
            if (File.Exists($"{pathArhive}/{jsonFile.Key}"))//проверяем, есть ли файл по такому пути
            {
                if ($"{pathArhive}/{jsonFile.Key}".MD5result(jsonFile.Value))
                    howMuchDownloadedAllFiles += new FileInfo($"{pathArhive}/{jsonFile.Key}").Length;
                else
                {
                    File.Delete($"{pathArhive}/{jsonFile.Key}");
                    downloadList.Add(jsonFile.Key, jsonFile.Value);
                }
            }
            else
                downloadList.Add(jsonFile.Key,jsonFile.Value);
        }

        return downloadList;
    }
    
    private void Update()
    {
        progressTV.text = $"скачалось {howMuchDownloaded.LongToMbyte()} Mb из {allMb.LongToMbyte()} Mb";
        if (progressBar.maxValue != allMb)
            progressBar.maxValue = allMb;
        progressBar.value = howMuchDownloaded;
    }

    public void CancleDownload()
    {
        if(wc!=null)
            wc.CancelAsync();
    }
    
    private async void DownloadFiles(Dictionary<string, string> listFiles)
    {
        pathArhive = $"{Application.dataPath}/Files/Archive";
        
        //собираем узнаем, сколько все вместе весят
        allMb =
            NetworkManager.RemoteSizeFile(filesInfo, new Uri("https://vintstudio.ru/datafileTest"));

        await Task.Run(() =>
        {
            foreach (var file in listFiles)
            {
                bool downloadComplitedFile = false;
                var uri = new Uri($"https://vintstudio.ru/datafileTest/{file.Key}");

                using (wc = new WebClient())
                {
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        howMuchDownloaded =howMuchDownloadedAllFiles+ Convert.ToInt64(e.BytesReceived);
                    };
                    wc.DownloadFileCompleted += (s, e) =>
                    {
                        if ($"{pathArhive}/{file.Key}".MD5result(file.Value))
                        {
                            downloadComplitedFile = true;
                            howMuchDownloadedAllFiles += howMuchDownloaded;
                            
                        }
                    };
                    wc.DownloadFileAsync(uri, $"{pathArhive}/{file.Key}");
                }

                while (!downloadComplitedFile)
                {
                }
            }
        });
        loadingWindow.SetActive(true);
        StartCoroutine(CheckMD5Archive(res =>
        {
            loadingWindow.SetActive(false);
            if(res.Count!=0)
            {
                NetworkManager.isInternetConnection(isConnection =>
                {
                    DownloadFiles(res);
                });
            }
            else
                unpack.Init(pathArhive,filesInfo);
        }));
        
        
        
        /*Dictionary<string, string> checkFiles=CheckMD5Arhive();
        if(checkFiles.Count!=0)
        {
            NetworkManager.isInternetConnection(isConnection =>
            {
                DownloadFiles(checkFiles);
            });
        }
        else //Все файлы скачались успешно
        {
            unpack.Init(pathArhive,filesInfo);
        }*/
    }
    
    public void ClickNo()
    {
        windowQuestion.SetActive(false);
    }
    
}
