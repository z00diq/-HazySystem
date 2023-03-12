using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBG : MonoBehaviour
{
    RawImage thisRawImage;
    [SerializeField] float alpha;

    private void Start()
    {
        thisRawImage = this.GetComponent<RawImage>();
        StartCoroutine(FillMyBG());
    }

    private IEnumerator FillMyBG()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            thisRawImage.color = new Color(1f, 1f, 1f, i);
            yield return Time.deltaTime;
        }
    }
}
