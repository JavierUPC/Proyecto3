using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTag : MonoBehaviour
{
    void Start()
    {
        string parentTag = gameObject.tag;
        SetTagRecursively(transform, parentTag);
    }

    void SetTagRecursively(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            child.tag = tag;
            SetTagRecursively(child, tag);
        }
    }
}
