using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MultipleVolumeLoader : MonoBehaviour
{
    [SerializeField] string[] _keys;
    [SerializeField] Button[] _buttons;
    [SerializeField] SliceVolumeAnimator _animator;

    private void Start()
    {
        _buttons[0].onClick.AddListener(()=>LoadVolume(_keys[0]));
        _buttons[1].onClick.AddListener(()=>LoadVolume(_keys[1]));
        _buttons[2].onClick.AddListener(()=>LoadVolume(_keys[2]));
    }

    private string[] GetVolumeImagesPaths(string key)
    {
        string directory = Application.persistentDataPath+"/"+key;
        var files = Directory.GetFiles(directory);
        List<string> imageFiles = new List<string>();
        foreach (var file in files)
            if (file.EndsWith(".png"))
                imageFiles.Add(file);
        return imageFiles.ToArray();
    }

    private void LoadVolume(string key)
    {
        _animator.Pause();
        _animator.Clear();
        if (CheckVolumeImagesAreDownloaded(key))
        {
            var paths = GetVolumeImagesPaths(key);
            StartCoroutine(DecompressVolumes(paths));
        }
        else
        {
            StartCoroutine(DownloadThenOpen(key));
        }
    }

    private bool CheckVolumeImagesAreDownloaded(string key)
    {
        string directory = Application.persistentDataPath + "/" + key;
        return Directory.Exists(directory);
    }

    private IEnumerator DownloadThenOpen(string key)
    {
        Debug.Log("Downloading volume " + key);
        VolumeFilesDownloader volumeFilesDownloader = new VolumeFilesDownloader(key);
        yield return volumeFilesDownloader.DownloadImages();
        var paths = GetVolumeImagesPaths(key);
        StartCoroutine(DecompressVolumes(paths));
    }

    private IEnumerator DecompressVolumes(string[] urls)
    {
        Debug.Log("Decompressing volumes");
        VolumeImagesLocalOpener imagesOpener = new VolumeImagesLocalOpener(urls);
        yield return imagesOpener.OpenFrames();
        Texture2D[] frames = imagesOpener.Frames;
        _animator.Frames = new Texture3D[frames.Length];
        for (int i = 0; i < frames.Length; i++)
        {
            Texture3D volume = VolumeConstructor.ConstructVolume(frames[i]);
            _animator.Frames[i] = volume;
        }
        _animator.Play();
    }
}
