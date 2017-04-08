using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoardPieceController : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler{

    //TODO add shfit down tweens

    public SpriteRenderer spriteRenderer;
    public int x;
    public int y;
    public GameManager gameManager;
    public BoardPieceMeta boardPieceMeta;
    private int moveDir;
    private int typeDir;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    #region CLEAR BOARD TWEENS
    public void ShrinkDown()
    {
        transform.DOScale(new Vector3(0,0,1),0.3f).OnComplete(OnShrinkDone);
    }

    protected void OnShrinkDone()
    {
        gameManager.RemovePiece(this);
    }
    #endregion

    #region SHIFT_DOWN
    public void PromptShiftDownTween()
    {
        transform.DOLocalMoveY(boardPieceMeta.newY - y, 0.2f).SetRelative().OnComplete(OnShiftDone);
    }

    private void OnShiftDone()
    {
        boardPieceMeta.shiftDown = false;
        x = boardPieceMeta.newX;
        y = boardPieceMeta.newY;
        gameManager.PieceTweenDone(this);
    }
    #endregion

    #region PLAYER INTERACTION TWEENS
    protected void OnMotionDone()
    {
        if(typeDir == 0)
        {
            y += moveDir;
        }else if(typeDir == 1)
        {
            x += moveDir;
        }
        gameManager.PieceTweenDone(this);
    }

    protected void OnMotionDoneDummy()
    {
        gameManager.canMove = true;
        gameManager.PieceTweenDone(this);
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
    #endregion

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

        if (Mathf.Abs(eventData.delta.y) < 5)
        {
            if (eventData.delta.x > 0)
            {
                Debug.Log(x + " " + y);
                if (gameManager.CanMove(x, y, 1, 0))
                {
                    gameManager.DoMove(x, y, 1, 0);
                }
                else
                {
                    gameManager.DoFakeMove(x, y, 1, 0);
                }
                gameManager.canMove = false;
            }
            else if (eventData.delta.x < 0)
            {
                if (gameManager.CanMove(x, y, -1, 0))
                {
                    gameManager.DoMove(x, y, -1, 0);
                }
                else
                {
                    gameManager.DoFakeMove(x, y, -1, 0);
                }
                gameManager.canMove = false;
            }
        }
        else
        {
            if (eventData.delta.y > 0)
            {
                if (gameManager.CanMove(x, y, 0, 1))
                {
                    gameManager.DoMove(x, y, 0, 1);
                }
                else
                {
                    gameManager.DoFakeMove(x, y, 0, 1);
                }
                gameManager.canMove = false;

            }
            else if (eventData.delta.y < 0)
            {
                if (gameManager.CanMove(x, y, 0, -1))
                {
                    gameManager.DoMove(x, y, 0, -1);
                }
                else
                {
                    gameManager.DoFakeMove(x, y, 0, -1);
                }
                gameManager.canMove = false;

            }
        }
    }
    #endregion
}

public struct BoardPieceMeta
{
    public int newX;
    public int newY;
    public bool shiftDown;
}