using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeeDescriptionOnPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _descriptionTextMeshPro;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _descriptionTextMeshPro.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _descriptionTextMeshPro.enabled = false;
    }
}
