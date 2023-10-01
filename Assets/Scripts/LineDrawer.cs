using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject parent;
    public GameObject linePrefab;

    void Start()
    {
        foreach (var bit in GetComponentsInChildren(typeof(Bit)))
        {
            Debug.Log($"Instantiate for bit {bit.gameObject.name}, the positon of bit is: {bit.GetComponent<RectTransform>().position}");
            GameObject line = Instantiate(linePrefab, parent.transform);
            var rt = line.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, bit.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
}
