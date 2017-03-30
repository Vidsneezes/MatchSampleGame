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
    public BoardLogic boardLogic;
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
        }
    }


}
