using System.Collections;
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
    private BoardLogic boardLogic;
    private List<Sprite> pieceSprite;

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

    public void AnimateFeint(int x, int y, float positionX, float positionY)
    {
        int indexN = 0;
        for (int i = 0; i < activePieces.Count; i++)
        {
            if(activePieces[i].x == x && activePieces[i].y == y)
            {
                indexN = i;
                break;
            }
        }
        if (Mathf.Abs(positionX) > 0)
        {
            activePieces[indexN].FeintMoveX(positionX);
        }else if(Mathf.Abs(positionY) > 0)
        {
            activePieces[indexN].FeintMoveY(positionY);
        }
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
            activePieces[indexN].MoveX(positionX,(int)Mathf.Sign(positionX));
        }
        else if (Mathf.Abs(positionY) > 0)
        {
            activePieces[indexN].MoveY(positionY,(int)Mathf.Sign(positionY));
        }
    }

    public void RecalculateBoard(int x, int y, int dirx, int diry)
    {
        boardLogic.MovePiece(x, y, dirx, diry);
    }
}
