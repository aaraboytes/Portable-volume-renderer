using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForwardBackwardController : MonoBehaviour
{
    public Vector2 Offset;
    [SerializeField] Slider _slider;
    [SerializeField] Transform _handler;
    private void Update()
    {
        transform.position = transform.up * (_slider.value * -1) + transform.right * Offset.x + transform.forward  * Offset.y;
    }
}
