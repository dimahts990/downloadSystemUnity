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
    private string pathArhive;
    private bool isMd5Ok;

    public void onClick()
    {
        path = $"{Application.dataPath}/Files/Output/video.mp4";
        pathArhive = $"{Application.dataPath}/Files/Archive";
        
        CheckMD5();
    }

    private async void CheckMD5()
    {
        if (File.Exists(path))
        {
            checkMD5Window.SetActive(true);
            await Task.Run(() => { isMd5Ok = path.MD5result("C1984F6453642C47BD776DEB3081A5A5"); });
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
            downloadQuestionWindoe.SetActive(true);
    }
}