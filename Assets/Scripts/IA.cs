using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;
using SimpleJSON;
using TMPro;

public class IA : MonoBehaviour
{
    [SerializeField] Button _iaSwitch;
    [SerializeField] RawImage _reference;
    [SerializeField] RenderTexture _cameraTexture; 
    [SerializeField] Button _referenceChangeButton;
    [SerializeField] TextMeshProUGUI _percentage;
    [SerializeField] Image _percentageFill;
    [SerializeField] GameObject _iaPanel;
    [SerializeField] float _cooldown;

    private bool isIAOn = false;
    private string base64Reference;
    private void Start()
    {
        _percentageFill.fillAmount = 0;
        _percentage.text = "-";
        _iaPanel.SetActive(false);
        _iaSwitch.onClick.AddListener(SwitchIA);
        _referenceChangeButton.onClick.AddListener(ChangeReference);
    }

    private IEnumerator RunComparison()
    {
        while (isIAOn)
        {
            yield return IAComparison(_cameraTexture);
            yield return new WaitForSeconds(_cooldown);
        }
    }

    private IEnumerator IAComparison(RenderTexture camTexture)
    {
        string uri = "https://fuve-ia.cloudrad.ai";
        string camTextureBase64 = Texture2DToBase64( RenderTexture2Texture2D(camTexture));
        ImageCompareForm form = new ImageCompareForm()
        {
            image = new string[2] { base64Reference, camTextureBase64 }
        };
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(form));
        UnityWebRequest www = UnityWebRequest.Post(uri, "");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        yield return www.SendWebRequest();
        string[] pages = uri.Split('/');
        int page = pages.Length - 1;
        switch (www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(pages[page] + ": Error: " + www.error);
                Debug.LogError(www.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(pages[page] + ": HTTP Error: " + www.error);
                Debug.LogError(www.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                JSONObject jobject = (JSONObject)JSON.Parse(www.downloadHandler.text);
                Debug.Log(jobject["message"]);
                float percentaje = jobject["message"].AsFloat;
                _percentageFill.fillAmount = percentaje;
                _percentage.text = string.Format("{0:00}", percentaje * 100) + "%";
                break;
        }
    }

    private void ChangeReference()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Select IA reference", "","png", false);
        if (paths.Length > 0)
        {
            string url = new System.Uri(paths[0]).AbsoluteUri;
            StartCoroutine(OpenReference(url));
        }
    }

    private static string Texture2DToBase64(Texture2D texture)
    {
        byte[] bytes;
        bytes = texture.EncodeToPNG();
        return System.Convert.ToBase64String(bytes);
    }

    private static Texture2D RenderTexture2Texture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        RenderTexture.active = old_rt;
        return tex;
    }
    private void SwitchIA()
    {
        isIAOn = !isIAOn;
        _iaPanel.SetActive(isIAOn);
        if (isIAOn)
            StartCoroutine(RunComparison());
    }

    private IEnumerator OpenReference(string url)
    {
        var image = new WWW(url);
        yield return image;
        _reference.texture = image.texture;
        base64Reference = Texture2DToBase64(image.texture);
    }
}

[System.Serializable]
public class ImageCompareForm
{
    public string[] image;
}
