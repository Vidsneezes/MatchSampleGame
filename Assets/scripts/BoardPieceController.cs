using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPieceController : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
