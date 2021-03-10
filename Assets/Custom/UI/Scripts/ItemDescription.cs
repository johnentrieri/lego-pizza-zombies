using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour, ISelectHandler
{
    [SerializeField] string shortItemDescription;
    [SerializeField] string longItemDescription;
    [SerializeField] Text shortTextObject;
    [SerializeField] Text longTextObject;

    public void OnSelect(BaseEventData eventData) {
        if(eventData.selectedObject == gameObject)
         {
            shortTextObject.text = shortItemDescription;
            longTextObject.text = longItemDescription;
         }
    }
}
