using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDownload : MonoBehaviour
{
    public GameObject checkMD5Window;
    public GameObject downloadQuestionWindoe;
    private string path;
    private bool isMd5Ok;

    public void onClick()
    {
        path = $"{NamespaceSource.FilePath}video.mp4";        
        CheckMD5();
    }

    private async void CheckMD5()
    {
        if (File.Exists(path))
        {
            checkMD5Window.SetActive(true);
            await Task.Run(() => { isMd5Ok = path.MD5result(NamespaceSource.MD5fileCheck); });
            checkMD5Window.SetActive(false);
            if (!isMd5Ok)
            {
                File.Delete(path);
                downloadQuestionWindoe.SetActive(true);
            }
            else
                SceneManager.LoadScene("game");
        }
        else
        {
            if (!File.Exists(NamespaceSource.FilePath))
                Directory.CreateDirectory(NamespaceSource.FilePath);
            downloadQuestionWindoe.SetActive(true);
        }
    }
}