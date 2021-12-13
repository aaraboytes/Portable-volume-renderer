using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIGradientWindow : MonoBehaviour
{
    public float GradientWidth
    {
        get
        {
            return _gradient.rectTransform.rect.width;
        }
    }
    public bool IndicatorAlreadySelected => isIndicatorSelected;

    [SerializeField] RawImage _gradient;
    [SerializeField] GameObject _indicator;
    private bool isIndicatorSelected = false;
    private Gradient gradient;
    private List<GameObject> indicators = new List<GameObject>();
    private UIGradientIndicator selectedIndicator;
    public void SetGradient(Gradient gradient)
    {
        this.gradient = gradient;
        int width = 256;
        Texture2D gradientTexture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for (int i = 0; i < width; i++)
        {
            colors[i] = gradient.Evaluate((float)i / width);
        }
        gradientTexture.SetPixels(colors);
        gradientTexture.Apply();
        _gradient.texture = gradientTexture;
    }

    public void SetIndicators()
    {
        foreach (var indicator in indicators)
            indicator.SetActive(false);

        if (indicators.Count < gradient.colorKeys.Length)
        {
            int missing = gradient.colorKeys.Length - indicators.Count;
            for (int i = 0; i < missing; i++)
            {
                GameObject indicator = Instantiate(_indicator, _gradient.transform);
                indicators.Add(indicator);
                UIGradientIndicator indicatorController = indicator.GetComponent<UIGradientIndicator>();
                indicatorController.Window = this;
            }
        }
        for (int i = 0; i < gradient.colorKeys.Length; i++)
        {
            Image indicator = indicators[i].GetComponent<Image>();
            UIGradientIndicator indicatorController = indicator.GetComponent<UIGradientIndicator>();
            indicatorController.Index = i;
            indicator.color = gradient.colorKeys[i].color;
            float width = _gradient.rectTransform.rect.width;
            indicator.transform.localPosition = new Vector2(gradient.colorKeys[i].time * width - width / 2, indicator.transform.localPosition.y);
            indicator.gameObject.SetActive(true);
        }
    }

    public void SelectIndicator(UIGradientIndicator indicator)
    {
        isIndicatorSelected = true;
        selectedIndicator = indicator;
    }

    public void UpdateGradient(UIGradientIndicator indicator)
    {
        var gradientKeys = gradient.colorKeys;
        var alphaKeys = gradient.alphaKeys;
        float x = indicator.transform.localPosition.x + GradientWidth/2f;
        float time = x / GradientWidth;
        gradientKeys[indicator.Index].time = time;
        gradient.SetKeys(gradientKeys,alphaKeys);
        SetGradient(gradient);
    }

    public void UnselectIndicator()
    {
        isIndicatorSelected = false;
        selectedIndicator = null;
    }
}
