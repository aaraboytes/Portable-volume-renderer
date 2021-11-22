using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

public class VolumeLoader : MonoBehaviour
{
    [SerializeField] Button _searchImagesButton;
    [SerializeField] TextMeshProUGUI _path;
    [SerializeField] TextMeshProUGUI _file;
    [SerializeField] SliceVolumeAnimator _animator;
    
    void Start()
    {
        _searchImagesButton.onClick.AddListener(SearchFiles);
    }
    private void SearchFiles()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Volume images", "", "png", true);
        if (paths.Length > 0)
        {
            string folderName = System.IO.Path.GetDirectoryName(paths[0]);
            _path.text = folderName;
            _file.text = paths[0];
            string[] urls = new string[paths.Length];
            for (int i = 0; i < urls.Length; i++)
            {
                urls[i] = new System.Uri(paths[i]).AbsoluteUri;
            }
            StartCoroutine(OpenImages(urls));
        }
    }
    private IEnumerator OpenImages(string[] urls)
    {
        List<Texture2D> images = new List<Texture2D>();
        foreach (var url in urls)
        {
            var image = new WWW(url);
            yield return image;
            images.Add(image.texture);
        }
        _animator.Frames = new Texture3D[images.Count];
        for (int i = 0; i < images.Count; i++)
        {
            Texture3D volume = ConstructVolume(images[i]);
            _animator.Frames[i] = volume;
        }
        _animator.Play();
    }
    private Texture3D ConstructVolume(Texture2D image)
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
