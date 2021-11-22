using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SliceVolumeAnimator : MonoBehaviour
{
    public bool Paused => _paused;
    public int FramesCount => Frames.Length;
    public int FrameIndex => animIndex;
    public UnityAction OnPause, OnResume;
    public Texture3D[] Frames;

    [Header("Components")]
    [SerializeField] MeshRenderer _sliceRenderer;
    

    [Header("Properties")]
    [SerializeField] bool _paused = false;
    [SerializeField] int _frameRate;
    [SerializeField] Gradient _gradient;
    [SerializeField] Button _applyNewGradientButton;

    private int animIndex = 0;
    private float timer;
    private Material sliceRenderingMaterial;
    private float speedMultiplier = 1;
    private float frameSpeed;

    private void OnDestroy()
    {
        if(Frames.Length > 0)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                Destroy(Frames[i]);
            }
        }
    }

    private void Awake()
    {
        sliceRenderingMaterial = _sliceRenderer.material;

        if(Frames.Length>0)
            sliceRenderingMaterial.SetTexture("_DataTex", Frames[0]);
       
    }

    private void Start()
    {
        frameSpeed = 1 / (float)_frameRate;
        sliceRenderingMaterial.SetTexture("_TFTex", GenerateGradientImage(_gradient));
        _applyNewGradientButton.onClick.AddListener(() =>
        {
            sliceRenderingMaterial.SetTexture("_TFTex", GenerateGradientImage(_gradient));
        });
    }

    private void Update()
    {
        if(!_paused && sliceRenderingMaterial)
        {
            timer += Time.deltaTime;
            if (timer >= frameSpeed * speedMultiplier)
            {
                animIndex++;
                if (animIndex >= Frames.Length)
                    animIndex = 0;
                sliceRenderingMaterial.SetTexture("_DataTex", Frames[animIndex]);
                timer = 0;
            }
        }
    }

    private Texture2D GenerateGradientImage(Gradient gradient)
    {
        Texture2D gradientTexture = new Texture2D(256, 1);
        Color[] colors = new Color[256];
        for(int i = 0; i < 256; i++)
        {
            Color pixelColor = gradient.Evaluate(i / 256f);
            colors[i] = pixelColor;
        }
        gradientTexture.SetPixels(colors);
        gradientTexture.Apply();
        return gradientTexture;
    }

    public void SetTextures(Texture3D[] textures)
    {
        animIndex = 0;
        if(Frames.Length>0)
            for (int i = 0; i < Frames.Length; i++)
            {
                DestroyImmediate(Frames[i],true);
            }
        Frames = new Texture3D[textures.Length];
        Frames = textures;
    }

    public void SetFrame(int frame)
    {
        if (frame < Frames.Length)
        {
            animIndex = frame;
            sliceRenderingMaterial.SetTexture("_DataTex", Frames[animIndex]);
        }
    }

    public void Pause()
    {
        _paused = true;
        OnPause?.Invoke();
    }

    public void Play()
    {
        if (Frames.Length > 0)
        {
            _paused = false;
            OnResume?.Invoke();
        }
    }

    public void Clear()
    {
        if (Frames.Length > 0)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                Destroy(Frames[i]);
            }
            Frames = new Texture3D[0];
        }
    }
}
