using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using zipTestNuGert;

public class UnpackManager:MonoBehaviour
{
    private List<string> allPathFiles=new List<string>();
    private string extractPath;
    public Progress<ZipProgress> _progress;

    public UnpackManager()
    {
        _progress = new Progress<ZipProgress>();
        _progress.ProgressChanged += Report;
    }

    private void Report(object sender, ZipProgress zipProgress)
    {
        Debug.Log($"Processed: {zipProgress.Processed}; Total: {zipProgress.Total}; CurrentItem: {zipProgress.CurrentItem}");
    }

    
    public async void Init(string path, Dictionary<string,string> files)
    {
        extractPath = NamespaceSource.FilePath;
        foreach (var file in files)
        {
            allPathFiles.Add($"{path}{file.Key}");
        }

        await Task.Run(() =>
        {
            using (var zipFile =
                   new ZipArchive(
                       new CombinationStream(allPathFiles.Select(x => new FileStream(x, FileMode.Open) as Stream)
                           .ToList()), ZipArchiveMode.Read))
            {
                zipFile.ExtractToDirectory($"{extractPath}",_progress);
            }
        });
        SceneManager.LoadScene("game");
    }
}




/*
public async void extract()
{
  
    await Task.Run(() =>
    {
        using (var zipFile =
               new ZipArchive(
                   new CombinationStream(files.Select(x => new FileStream(x, FileMode.Open) as Stream).ToList()),
                   ZipArchiveMode.Read))
        {
            zipFile.ExtractToDirectory(extract);
        }
    });

}*/