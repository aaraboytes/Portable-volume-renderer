using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeImagesLocalOpener
{
    public Texture2D[] Frames;
    private string[] urls;
    public VolumeImagesLocalOpener(string[] urls)
    {
        this.urls = urls;
    }
    public IEnumerator OpenFrames()
    {
        List<Texture2D> images = new List<Texture2D>();
        foreach(var url in urls)
        {
            Debug.Log("Openinig local image " + url);
            Texture2D image = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            image.filterMode = FilterMode.Point;
            byte[] bytes = System.IO.File.ReadAllBytes(url);
            image.LoadImage(bytes);
            images.Add(image);
            yield return null;
            /*var image = new WWW(url);
            yield return image;
            images.Add(image.texture);*/
        }
        Frames = images.ToArray();
    }
}

public class VolumeConstructor
{
    public static Texture3D ConstructVolume(Texture2D image)
    {
        Texture3D volume;
        Color32[] colors = image.GetPixels32();
        volume = new Texture3D(256, 256, 256, TextureFormat.RGB24, false);
        volume.name = image.name;
        volume.SetPixels32(colors);
        volume.Apply();
        return volume;
    }
}
