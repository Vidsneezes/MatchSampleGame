﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Sprite bluePiece;
    public Sprite greenPiece;
    public Sprite purplePiece;
    public Sprite redPiece;
    public Sprite yellowPiece;
    /*
   * 0 - green
   * 1 - blue
   * 2 - purple
   * 3 - red
   * 4 - yellow
   */

    //TODO clear pieces on board move clear

    //TODO animate new pieces to fall down in their place

    //TODO disable touch while pieces are falling

    //TODO build up pipeline for move piece, clear pieces, shift pieces and bring down pieces

    public BoardPieceController piecePrefab;
    public List<BoardPieceController> activePieces;
    public List<BoardPieceController> inactivePieces;

    public int width;
    public int height;
    public float pieceWidth;
    public float pieceHeight;
    public float horizontalOffset;
    public float verticalOffset;
    public Transform pieceHolder;
    public Transform inactiveHolder;
    public bool canMove;
    public string boardState;
    private BoardLogic boardLogic;
    private List<Sprite> pieceSprite;
    private List<BoardPieceController> tweeningPiece;

    private void Awake () {
        boardLogic = new BoardLogic(width, height);
        boardLogic.FillBoardWithCells();
        activePieces = new List<BoardPieceController>();
        inactivePieces = new List<BoardPieceController>();
        for (int i = 0; i < (width*height)+10; i++)
        {
            BoardPieceController piece = GameObject.Instantiate(piecePrefab,inactiveHolder);
            piece.gameObject.SetActive(false);
            inactivePieces.Add(piece);
        }
        pieceSprite = new List<Sprite>();
        pieceSprite.Add(greenPiece);
        pieceSprite.Add(bluePiece);
        pieceSprite.Add(purplePiece);
        pieceSprite.Add(redPiece);
        pieceSprite.Add(yellowPiece);
        CreateFromBoard();
        canMove = true;
        tweeningPiece = new List<BoardPieceController>();
        boardState = "INITIAL";
    }

    private void Update()
    {
        switch (boardState)
        {
            case "INITIAL":  canMove = true;break;
            case "FAKE_MOVE": if(tweeningPiece.Count == 0)
                {
                    boardState = "INITIAL";
                }break;
            case "FIRST_PIECE_TWEEN": if(tweeningPiece.Count == 0)
                {
                    boardState = "CLEAR_BOARD";
                }break;
            case "CLEAR_BOARD":
                break;
            case "CLEAR_BOARD_TWEEN":if(tweeningPiece.Count == 0)
                {
                    boardState = "SHIFT_DOWN";
                }break;
            case "SHIFT_DOWN":
                boardLogic.ShiftPiecesDown();
                boardState = "SHIFT_DOWN_TWEEN";
                break;
            case "SHIFT_DOWN_TWEEN":if(tweeningPiece.Count == 0)
                {
                    boardState = "FILL_BOARD";
                }break;
            case "FILL_BOARD":
                boardLogic.FillBoardWithCells();
                boardState = "ENTER_DROPDOWN_ANIMATION";
                break;
            case "ENTER_DROPDOWN_ANIMATION":
                break;
            case "STAY_DROPDOWN_ANIMATION": if(tweeningPiece.Count == 0)
                {
                    boardState = "INITIAL";
                }break;

        }
    }

    private void CreateFromBoard()
    {
        int[] board = boardLogic.GetBoard();
        for (int i = 0; i < board.Length; i++)
        {
            int y = Mathf.FloorToInt(i / width);
            int x = i - (y * width);
            int pieceType = board[i];
            BoardPieceController newPiece = inactivePieces[0];
            inactivePieces.RemoveAt(0);
            newPiece.transform.SetParent(pieceHolder);
            newPiece.x = x;
            newPiece.y = y;
            newPiece.gameManager = this;
            newPiece.SetSprite(pieceSprite[pieceType]);
            newPiece.gameObject.SetActive(true);
            newPiece.transform.localPosition = new Vector3(x*pieceWidth, y*pieceHeight,0);
            activePieces.Add(newPiece);
        }
    }

    public bool CanMove(int x, int y, int dirX, int dirY)
    {
        return boardLogic.CanMove(x, y, dirX, dirY);
    }

    public void DoMove(int orgX, int orgY, int dirX, int dirY)
    {
        int destX = orgX + dirX;
        int destY = orgY + dirY;
        Animate(orgX, orgY, dirX, dirY);
        Animate(destX, destY, -dirX, -dirY);
    }

    public void DoFakeMove(int orgX, int orgY, int dirX, int dirY)
    {
        int destX = orgX + dirX;
        int destY = orgY + dirY;
        AnimateFeint(orgX, orgY, dirX, dirY);
        AnimateFeint(destX, destY, -dirX, -dirY);
    }

    public void Animate(int x, int y, float positionX, float positionY)
    {
        int indexN = 0;
        for (int i = 0; i < activePieces.Count; i++)
        {
            if (activePieces[i].x == x && activePieces[i].y == y)
            {
                indexN = i;
                break;
            }
        }
        if (Mathf.Abs(positionX) > 0)
        {
            activePieces[indexN].MoveX(positionX*pieceWidth,(int)Mathf.Sign(positionX));
        }
        else if (Mathf.Abs(positionY) > 0)
        {
            activePieces[indexN].MoveY(positionY*pieceHeight,(int)Mathf.Sign(positionY));
        }
        tweeningPiece.Add(activePieces[indexN]);
    }

    public void AnimateFeint(int x, int y, float positionX, float positionY)
    {
        int indexN = 0;
        for (int i = 0; i < activePieces.Count; i++)
        {
            if (activePieces[i].x == x && activePieces[i].y == y)
            {
                indexN = i;
                break;
            }
        }
        if (Mathf.Abs(positionX) > 0)
        {
            activePieces[indexN].FeintMoveX(positionX*pieceWidth);
        }
        else if (Mathf.Abs(positionY) > 0)
        {
            activePieces[indexN].FeintMoveY(positionY*pieceHeight);
        }
        tweeningPiece.Add(activePieces[indexN]);
    }

    public void PieceTweenDone(BoardPieceController bpc)
    {
        tweeningPiece.Remove(bpc);
    }

    public void RecalculateBoard(int x, int y, int dirx, int diry)
    {
        boardLogic.MovePiece(x, y, dirx, diry);
    }

    public void ClearBoard(int x, int y)
    {
        int type = boardLogic.GetBoard()[x + y * boardLogic.boardWidth];
        if (boardLogic.CanMove(x, y,0,0))
        {
            boardLogic.ClearConnectedType(type, x, y);
            RemovePieces();
        }
    }

    public void RemovePieces()
    {
        int[] board = boardLogic.GetBoard();
        for (int i = 0; i < board.Length; i++)
        {
            int y = Mathf.FloorToInt(i / boardLogic.boardWidth);
            int x = i - (y * boardLogic.boardWidth);
            if (board[i] == -1)
            {
                int bpc = -1;
                for (int j = 0; j < activePieces.Count; j++)
                {
                    if (activePieces[j].x == x && activePieces[j].y == y)
                    {
                        bpc = j;
                        break;
                    }
                }
                if (bpc >= 0)
                {
                    RemovePiece(bpc);
                }
            }
        }
    }

    public void RemovePiece(int bpc)
    {
        BoardPieceController boardPieceController = activePieces[bpc];
        activePieces.RemoveAt(bpc);
        boardPieceController.gameObject.SetActive(false);
        boardPieceController.transform.SetParent(inactiveHolder);
        inactivePieces.Add(boardPieceController);
    }
}
