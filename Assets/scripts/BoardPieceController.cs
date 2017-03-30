using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoardPieceController : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler{

    public SpriteRenderer spriteRenderer;
    public int x;
    public int y;
    public bool inMotion;
    public GameManager gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inMotion = false;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void OnMotionDone()
    {
        inMotion = false;
    }

    public void MoveX(float value)
    {
        transform.DOLocalMoveX(value, 0.3f).SetRelative().OnComplete(OnMotionDone);
    }

    public void FeintMoveX(float value)
    {
        transform.DOLocalMoveX(value, 0.3f).SetRelative().SetLoops(2, LoopType.Yoyo).OnComplete(OnMotionDone);
    }

    public void MoveY(float value)
    {
        transform.DOLocalMoveY(value, 0.3f).SetRelative().OnComplete(OnMotionDone);
    }

    public void FeintMoveY(float value)
    {
        transform.DOLocalMoveY(value, 0.3f).SetRelative().SetLoops(2, LoopType.Yoyo).OnComplete(OnMotionDone);
    }

    #region Inpute Logic
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (inMotion == false)
        {
            //TODO Add board logic comfirm before moving
            //TODO added movement to both piece after board has be moved
            //TODO disable touch while pieces move
            //TODO apply board re calculation on can move
            if (Mathf.Abs(eventData.delta.y) < 5)
            {
                if (eventData.delta.x > 0)
                {
                    Debug.Log(x + " " + y);
                    if (gameManager.CanMove(x, y, 1, 0))
                    {
                        MoveX(gameManager.pieceWidth);
                    }
                    else
                    {
                        FeintMoveX(gameManager.pieceWidth);
                        gameManager.Animate(x + 1, y, -gameManager.pieceWidth, 0);
                    }
                    inMotion = true;

                }
                else if (eventData.delta.x < 0)
                {
                    if (gameManager.CanMove(x, y, -1, 0))
                    {
                        MoveX(-gameManager.pieceWidth);
                    }
                    else
                    {
                        FeintMoveX(-gameManager.pieceWidth);
                        gameManager.Animate(x - 1, y, gameManager.pieceWidth, 0);

                    }
                    inMotion = true;

                }
            }
            else
            {
                if (eventData.delta.y > 0)
                {
                    if (gameManager.CanMove(x, y, 0, 1))
                    {
                        MoveY(gameManager.pieceHeight);
                    }
                    else
                    {
                        FeintMoveY(gameManager.pieceHeight);
                        gameManager.Animate(x, y + 1, 0, -gameManager.pieceHeight);

                    }
                    inMotion = true;

                }
                else if (eventData.delta.y < 0)
                {
                    if (gameManager.CanMove(x, y, 0, -1))
                    {
                        MoveY(-gameManager.pieceHeight);
                    }
                    else
                    {
                        FeintMoveY(-gameManager.pieceHeight);
                        gameManager.Animate(x, y - 1, 0, gameManager.pieceHeight);

                    }
                    inMotion = true;

                }
            }
        }
    }
    #endregion
}
