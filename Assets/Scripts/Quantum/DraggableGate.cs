using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GateType
{
    H, X, Y, Z, RY
}

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DraggableGate : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Events events;
    public RectTransform initialGatesPanel;

    Canvas canvas;
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Vector2 initialPosition;

    DroppableLine[] lines;

    public GateType type = GateType.H;
    public bool interactable = true;

    public void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        Assert.IsNotNull(canvas);
        this.rectTransform = GetComponent<RectTransform>();
        this.canvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!interactable) return;
        this.initialPosition = this.rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable) return;
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

    public int FindOverlapingLine()
    {
        lines = FindObjectsOfType<DroppableLine>().OrderBy(line => -line.transform.position.y).ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            Rect draggedItemRect = rectTransformToRect(this.rectTransform);
            Rect lineRect = rectTransformToRect(lines[i].rectTransform);

            if (draggedItemRect.Overlaps(lineRect))
            {
                return i;
            }
        }

        return -1;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!interactable) return;

        int overlappingLine = FindOverlapingLine();
        if (overlappingLine != -1)
        {
            this.transform.position = new Vector3(this.transform.position.x, lines[overlappingLine].transform.position.y, this.transform.position.z);
            events.TransformViewUpdated();
            this.GetComponent<LayoutElement>().ignoreLayout = true;
            return;
        }

        var panelRect = rectTransformToRect(initialGatesPanel);
        if (panelRect.Overlaps(rectTransformToRect(this.rectTransform)))
        {
            events.TransformViewUpdated();
            this.GetComponent<LayoutElement>().ignoreLayout = false;
            initialGatesPanel.GetComponent<HorizontalLayoutGroup>().enabled = false;
            initialGatesPanel.GetComponent<HorizontalLayoutGroup>().enabled = true;
            return;
        }

        // No overlaps
        this.rectTransform.anchoredPosition = initialPosition;
    }
}
