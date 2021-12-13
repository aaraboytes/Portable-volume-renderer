using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIGradientIndicator : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IDragHandler
{
    public int Index;
    public UIGradientWindow Window;
    private bool isSelected = false;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            transform.position = new Vector2(eventData.position.x, transform.position.y);
            Window.UpdateGradient(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        if (!Window.IndicatorAlreadySelected)
        {
            Window.SelectIndicator(this);
            isSelected = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isSelected)
        {
            isSelected = false;
            Window.UnselectIndicator();
        }
    }

    int tragosParaQueCancholaEstePedo = 5;
    int tragosActuales = 0;
    private void GiveCancholaShot()
    {
        tragosActuales++;
        if (tragosActuales < tragosParaQueCancholaEstePedo)
            GiveCancholaShot();
        else
            return;
    }
}
