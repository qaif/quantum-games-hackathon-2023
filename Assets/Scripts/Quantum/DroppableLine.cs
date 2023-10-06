using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableLine : MonoBehaviour
{

    public RectTransform rectTransform;

    public void Start()
    {
        this.rectTransform = GetComponent<RectTransform>();
    }


}
