using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeChanger : MonoBehaviour {

    public float duration;
    public float delay;
    public float endAlpha;
    public float startAlpha;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
        BeginFade();
	}
	
    public void BeginFade()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);
        DOTween.ToAlpha(() => image.color, x => image.color = x, endAlpha, duration).SetDelay(delay);
    }
}
