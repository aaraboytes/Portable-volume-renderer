using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VolumeFilesDownloader
{
    private string key;
    private static string downloadUrl = "https://volumes-dev.fuve.app/";
    private List<Texture2D> volumeFiles = new List<Texture2D>();

    public VolumeFilesDownloader(string key)
    {
        this.key = key;
    }

    private void SaveFiles()
    {
        foreach(var texture in volumeFiles)
        {
            Debug.Log("Saving to local " + texture.name);
            byte[] bytes = texture.EncodeToPNG();
            var dirPath = Application.persistentDataPath+"/" + key;
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllBytes(dirPath + "/" + texture.name,bytes);
        }
    }

    public IEnumerator DownloadImages()
    {
        int volumesAmount = 6;
        for (int i = 0; i < volumesAmount; i++)
        {
            string fileName = key + "_" + string.Format("{0:0000}", i) + ".png";
            string imageUrl = downloadUrl + key + "/" + fileName;
            Debug.Log("Downloading " + imageUrl);
            TextureDownloader textureDownloader = new TextureDownloader(imageUrl);
            yield return textureDownloader.DownloadImage();
            Texture2D result = textureDownloader.Texture;
            result.name = fileName;
            volumeFiles.Add(result);
        }
        SaveFiles();
    }
}
