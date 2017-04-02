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
    private int moveDir;
    private int typeDir;

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
        if(typeDir == 0)
        {
            y += moveDir;
        }else if(typeDir == 1)
        {
            x += moveDir;
        }
        gameManager.canMove = true;

    }

    public void OnMotionDoneDummy()
    {
        inMotion = false;
        gameManager.canMove = true;
      
    }

    public void MoveX(float value, bool recalc = false, int dir = 0)
    {
        MoveX(value);
        if (recalc)
        {
            typeDir = 1;
            moveDir = dir;
            gameManager.RecalculateBoard(x, y, dir, 0);
        }
    }

    public void MoveX(float value, int dirx = 0)
    {
        MoveX(value);
        typeDir = 1;
        moveDir = dirx;
    }

    public void MoveX(float value)
    {
        transform.DOLocalMoveX(value, 0.3f).SetRelative().OnComplete(OnMotionDone);
    }

    public void MoveY(float value, bool recalc = false, int dir = 0)
    {
        MoveY(value);
        if (recalc)
        {

            typeDir = 0;
            moveDir = dir;
            gameManager.RecalculateBoard(x, y, 0, dir);
        }
    }

    public void MoveY(float value, int diry = 0)
    {
        MoveY(value);
        typeDir = 0;
        moveDir = diry;
    }

    public void MoveY(float value)
    {
        transform.DOLocalMoveY(value, 0.3f).SetRelative().OnComplete(OnMotionDone);
    }

    public void FeintMoveX(float value)
    {
        transform.DOLocalMoveX(value, 0.3f).SetRelative().SetLoops(2, LoopType.Yoyo).OnComplete(OnMotionDoneDummy);
    }

    public void FeintMoveY(float value)
    {
        transform.DOLocalMoveY(value, 0.3f).SetRelative().SetLoops(2, LoopType.Yoyo).OnComplete(OnMotionDoneDummy);
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
        if(gameManager.canMove == false)
        {
            return;
        }

        if (inMotion == false)
        {


            //TODO clear pieces on board move

            //TODO Pool pieces that get removed by clear

            //TODO animate new pieces to fall down in their place

            //TODO disable touch while pieces are falling


            if (Mathf.Abs(eventData.delta.y) < 5)
            {
                if (eventData.delta.x > 0)
                {
                    Debug.Log(x + " " + y);
                    if (gameManager.CanMove(x, y, 1, 0))
                    {
                        MoveX(gameManager.pieceWidth,true,1);
                        gameManager.Animate(x+1,y, -gameManager.pieceWidth, 0);
                    }
                    else
                    {
                        FeintMoveX(gameManager.pieceWidth);
                        gameManager.AnimateFeint(x + 1, y, -gameManager.pieceWidth, 0);
                    }
                    inMotion = true;
                    gameManager.canMove = false;
                }
                else if (eventData.delta.x < 0)
                {
                    if (gameManager.CanMove(x, y, -1, 0))
                    {
                        MoveX(-gameManager.pieceWidth, true , -1);
                        gameManager.Animate(x - 1, y, gameManager.pieceWidth, 0);
                    }
                    else
                    {
                        FeintMoveX(-gameManager.pieceWidth);
                        gameManager.AnimateFeint(x - 1, y, gameManager.pieceWidth, 0);

                    }
                    inMotion = true;
                    gameManager.canMove = false;

                }
            }
            else
            {
                if (eventData.delta.y > 0)
                {
                    if (gameManager.CanMove(x, y, 0, 1))
                    {
                        MoveY(gameManager.pieceHeight, true, 1);
                        gameManager.Animate(x, y + 1, 0, -gameManager.pieceHeight);
                    }
                    else
                    {
                        FeintMoveY(gameManager.pieceHeight);
                        gameManager.AnimateFeint(x, y + 1, 0, -gameManager.pieceHeight);

                    }
                    inMotion = true;
                    gameManager.canMove = false;

                }
                else if (eventData.delta.y < 0)
                {
                    if (gameManager.CanMove(x, y, 0, -1))
                    {
                        MoveY(-gameManager.pieceHeight,true,-1);
                        gameManager.Animate(x, y - 1, 0, gameManager.pieceHeight);
                    }
                    else
                    {
                        FeintMoveY(-gameManager.pieceHeight);
                        gameManager.AnimateFeint(x, y - 1, 0, gameManager.pieceHeight);
                    }
                    inMotion = true;
                    gameManager.canMove = false;

                }
            }
        }
    }
    #endregion
}
