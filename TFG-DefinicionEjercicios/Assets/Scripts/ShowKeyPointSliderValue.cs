using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

public class ShowKeyPointSliderValue : MonoBehaviour {
    [SerializeField]
    private TextMeshPro textMesh = null;

    public void OnSliderUpdate (SliderEventData eventData) {
        if (textMesh == null) {
            textMesh = GetComponent<TextMeshPro> ();
        }

        if (textMesh != null) {
            float num = eventData.NewValue * 10 + 1;
            textMesh.text = $"{num:F0}";
        }
    }
}