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
