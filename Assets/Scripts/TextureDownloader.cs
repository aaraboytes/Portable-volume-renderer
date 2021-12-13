using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureDownloader
{
    public Texture2D Texture;
    public string URL;
    public TextureDownloader(string url)
    {
        URL = url;
    }
    public IEnumerator DownloadImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
            Texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    }
}
