using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveInOut : MonoBehaviour {

    public float enterDuration;
    public float exitDuration;
    public Vector3 startPosition;
    public Vector3 midPosition;
    public Vector3 exitPosition;

	// Use this for initialization
	void Start () {
        BeginStartPanelAnimation();
    }
	
    private void BeginStartPanelAnimation()
    {
        transform.localPosition = startPosition;
        Sequence startPanelSequence = DOTween.Sequence();
        startPanelSequence.Append(transform.DOLocalMove(midPosition, enterDuration).SetEase(Ease.InCubic));
        startPanelSequence.Append(transform.DOLocalMove(exitPosition, exitDuration).SetEase(Ease.InCubic));
    }
}
