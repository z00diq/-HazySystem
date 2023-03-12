using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityBounce : MonoBehaviour
{
    RawImage thisRawImage;
    [SerializeField] float alpha;

    private void Start()
    {
        thisRawImage = this.GetComponent<RawImage>();
    }

    void Update()
    {
        alpha = 0.75f + Mathf.Sin(Time.time)*0.25f;
        thisRawImage.color = new Color(thisRawImage.color.r, thisRawImage.color.g, thisRawImage.color.b, alpha);
    }
}
