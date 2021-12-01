using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MultipleVolumeLoader : MonoBehaviour
{
    [SerializeField] string _volumeOnePath;
    [SerializeField] string _volumeTwoPath;
    [SerializeField] string _volumeThreePath;
    [SerializeField] Button _loadOne;
    [SerializeField] Button _loadTwo;
    [SerializeField] Button _loadThree;
    [SerializeField] SliceVolumeAnimator _animator;

    private void Start()
    {
        _loadOne.onClick.AddListener(()=>OpenVolume(1));
        _loadTwo.onClick.AddListener(()=>OpenVolume(2));
        _loadThree.onClick.AddListener(()=>OpenVolume(3));
    }

    private void OpenVolume(int number)
    {
        _animator.Clear();
        string url = "";
        switch (number)
        {
            case 1:
                url = _volumeOnePath;
                break;
            case 2:
                url = _volumeTwoPath;
                break;
            case 3:
                url = _volumeThreePath;
                break;
            default:
                break;
        }
        var files = Directory.GetFiles(url);
        List<string> imageFiles = new List<string>();
        foreach (var file in files)
            if (file.EndsWith(".png"))
                imageFiles.Add(file);
        StartCoroutine(OpenImages(imageFiles.ToArray()));
    }

    private IEnumerator OpenImages(string[] urls)
    {
        VolumeImagesLocalOpener imagesOpener = new VolumeImagesLocalOpener(urls);
        yield return imagesOpener.Callback;
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
