using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCountText : MonoBehaviour
{
    public AnimationCurve _scaleCurve;

    private CanvasGroup _cg;

    private Text _topText, _bottomText;

    void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        _topText = transform.Find("TopText").GetComponent<Text>();
        _bottomText = transform.Find("BottomText").GetComponent<Text>();
        transform.localScale = Vector3.zero;
    }

    public void SetTopText(string text)
    {
        _topText.text = text;
    }

    public void SetBottomText(string text)
    {
        _bottomText.text = text;
    }

    public void Flash()
    {
        _cg.alpha = 1;
        transform.localScale = Vector3.zero;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i <= 60; i++)
        {
            transform.localScale = Vector3.one * _scaleCurve.Evaluate((float)i / 50);
            if (i >= 40)
            {
                _cg.alpha = (float)(60 - i) / 20;
            }
            yield return null;
        }

        yield break;
    }
}