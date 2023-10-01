using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu]
public class Events : ScriptableObject
{
    public UnityAction transformViewUpdated;

    public void TransformViewUpdated()
    {
        transformViewUpdated?.Invoke();
    }
}
