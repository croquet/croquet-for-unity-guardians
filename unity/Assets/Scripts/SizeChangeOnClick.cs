using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SizeChangeOnClick : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rt;
    private bool enlarged = false;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("The cursor clicked the selectable UI element.");
        enlarged = !enlarged;
        rt.localScale = enlarged ? Vector3.one * 6.0f : Vector3.one * 2.0f;
    }

}
