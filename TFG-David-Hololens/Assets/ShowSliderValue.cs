using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

public class ShowSliderValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh = null;

    public void OnSliderUpdated(SliderEventData eventData)
    {
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        if (textMesh != null)
        {
            float num = eventData.NewValue * 10;
            //Debug.Log(num);
            textMesh.text = $"{num:F1}";
        }
    }
}

