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
    public void PromptShiftDownTween(float value)
    {
        transform.DOLocalMoveY((boardPieceMeta.newY - y)*value, 0.2f).SetRelative().OnComplete(OnShiftDone);
    }

    private void OnShiftDone()
    {
        boardPieceMeta.shiftDown = false;
        x = boardPieceMeta.newX;
        y = boardPieceMeta.newY;
        gameManager.PieceTweenDone(this);
    }
    #endregion

    #region SHIFT_DOWN_ON_FILL
    public void ShiftDownOnFill(float pos)
    {
        transform.DOLocalMoveY(pos, 0.2f).OnComplete(OnShiftDownOnFill);
    }

    private void OnShiftDownOnFill()
    {
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

        MovePieceMeta movePieceMeta;
        movePieceMeta.canMove = false;
        movePieceMeta.same = false;
        bool moreHorizontal = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? true : false;
        movePieceMeta.dirY = moreHorizontal ? 0 : (int)Mathf.Sign(-eventData.delta.y);
        movePieceMeta.dirX = moreHorizontal ? (int)Mathf.Sign(eventData.delta.x) : 0;

        movePieceMeta = gameManager.CanMove(x, y, movePieceMeta.dirX, movePieceMeta.dirY);
        if (movePieceMeta.canMove)
        {
            gameManager.DoMove(x, y, movePieceMeta.dirX, movePieceMeta.dirY);
        }
        else
        {
            if (!movePieceMeta.same)
            {
                gameManager.DoFakeMove(x, y, movePieceMeta.dirX, movePieceMeta.dirY);
            }
        }
        gameManager.canMove = false;
    }
    #endregion
}

public struct BoardPieceMeta
{
    public int newX;
    public int newY;
    public bool shiftDown;
}