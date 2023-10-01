using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public enum GateType
{
    H
}

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DraggableGate : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Canvas canvas;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Vector2 initialPosition;

    DroppableLine[] lines;

    public GateType type = GateType.H;

    public void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        Assert.IsNotNull(canvas);
        this.rectTransform = GetComponent<RectTransform>();
        this.canvasGroup = this.GetComponent<CanvasGroup>();

        lines = FindObjectsOfType<DroppableLine>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("On drag begin");
        this.initialPosition = this.rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On drag");
        this.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    Rect rectTransformToRect(RectTransform rt)
    {
        return new Rect(
            rt.position.x - rt.rect.width / 2,
            rt.position.y - rt.rect.height / 2,
            rt.rect.width,
            rt.rect.height
        );
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On drag end");

        foreach (var line in lines)
        {
            Rect draggedItemRect = rectTransformToRect(this.rectTransform);
            Rect lineRect = rectTransformToRect(line.rectTransform);


            if (draggedItemRect.Overlaps(lineRect))
            {
                this.transform.position = new Vector3(this.transform.position.x, line.transform.position.y, this.transform.position.z);
                return;
            }
        }

        // No overlaps
        this.rectTransform.anchoredPosition = initialPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
