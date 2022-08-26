using UnityEngine;

public class GetMemorySize:MonoBehaviour
{
    private AndroidJavaObject currentActivity;
    private AndroidJavaClass apiClass;
    private AndroidJavaObject apiInstance;
    
    /// <summary>
    /// Получить текущую активность
    /// </summary>
    /// <returns></returns>
    AndroidJavaObject GetCurrentActivity()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (null == currentActivity)
            currentActivity =
                new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
#endif
        return currentActivity;
    }
    
    
    /// <summary>
    /// Получить экземпляр класса инструментов Android
    /// </summary>
    /// <returns></returns>
    private AndroidJavaObject GetApiInstance()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (null == apiInstance)
        {
            if (null == apiClass)
                apiClass = new AndroidJavaClass("com.zp.utility.api");
 
            apiInstance = apiClass.CallStatic<AndroidJavaObject>("instance");
        }
#endif
        return apiInstance;
    }


    private int GetMemory()
    {
        int remainMemory=0;
#if UNITY_ANDROID && !UNITY_EDITOR
        var apiInstance = GetApiInstance();
        var activity = GetCurrentActivity();
        if (apiInstance != null && activity != null)
        {
            remainMemory = apiInstance.Call<int>("GetAndroidRemianMemory", activity);
        }
#endif
        return remainMemory;
    }

    public int GetMemoryMb()
    {
        return GetMemory();
    }
}