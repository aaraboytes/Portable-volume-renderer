using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatorController : MonoBehaviour
{
    [SerializeField] Slider _Xrotation;
    [SerializeField] Slider _Yrotation;
    [SerializeField] Slider _Zrotation;
    private Vector3 rotation;
    private void Start()
    {
        _Xrotation.onValueChanged.AddListener((val) => 
        {
            rotation.x = val * 360f;
            transform.localRotation = Quaternion.Euler(rotation);
        });
        _Yrotation.onValueChanged.AddListener((val) =>
        {
            rotation.y = val * 360f;
            transform.localRotation = Quaternion.Euler(rotation);
        });
        _Zrotation.onValueChanged.AddListener((val) => 
        {
            rotation.z = val * 360f;
            transform.localRotation = Quaternion.Euler(rotation);
        });
    }
}
