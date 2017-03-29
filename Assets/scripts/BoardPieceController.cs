using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPieceController : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
