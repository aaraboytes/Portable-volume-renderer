using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGradientButton : MonoBehaviour
{
    [SerializeField] UIGradientWindow _gradientWindow;
    [SerializeField] Gradient _gradient;
    private RawImage rawImage;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        rawImage = GetComponent<RawImage>();
    }

    private void Start()
    {
        UpdateGradient(_gradient);
        button.onClick.AddListener(OpenGradientEditor);
    }

    private void OpenGradientEditor()
    {
        _gradientWindow.gameObject.SetActive(true);
        _gradientWindow.SetGradient(_gradient);
        _gradientWindow.SetIndicators();
    }

    public void UpdateGradient(Gradient gradient)
    {
        int width = 256;
        Texture2D gradientTexture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for (int i = 0; i < width; i++)
        {
            float time = (float)i / width;
            Debug.Log(time);
            colors[i] = gradient.Evaluate(time);
        }
        gradientTexture.SetPixels(colors);
        gradientTexture.Apply();
        rawImage.texture = gradientTexture;
    }

    public void SetGradient(Gradient gradient)
    {
        _gradient = gradient;
    }
}
