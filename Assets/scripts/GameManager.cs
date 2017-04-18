using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //TODO add main menu and results screen

    //TODO add boot loader

        //TODO add progress logic

        //TODO add Progress UI


    public BoardPieceController piecePrefab;
    public List<BoardPieceController> activePieces;
    public List<BoardPieceController> inactivePieces;

    public int width;
    public int height;
    public float pieceWidth;
    public float pieceHeight;

    public float waitDelay;

    public float time;
    public Text pointsDisplay;
    public Text timeDisplay;

    public float InvertedHeight
    {
        get
        {
            return -pieceHeight;
        }
    }
    public float horizontalOffset;
    public float verticalOffset;
    public Transform pieceHolder;
    public Transform inactiveHolder;
    public bool canMove;
    public string boardState;
    public int totdalPoints;
    public BoardClearSolution boardClearSolution;
    private BoardLogic boardLogic;
    private List<Sprite> pieceSprite;
    private List<BoardPieceController> tweeningPiece;

    private float delayer;

    private void Awake () {
        boardLogic = new BoardLogic(width, height);
        boardLogic.FillBoardWithCells();
        activePieces = new List<BoardPieceController>();
        inactivePieces = new List<BoardPieceController>();
        for (int i = 0; i < (width*height)*2; i++)
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
        delayer = 1;
        time = 25;
        timeDisplay.text = time.ToString("0:00");
    }

    private void Update()
    {
        StateReducer();
    }

    private void StateReducer()
    {
        switch (boardState)
        {
            case "INITIAL": canMove = true; break;
            case "FAKE_MOVE":
                if (tweeningPiece.Count == 0)
                {
                    boardState = "INITIAL";
                }
                break;
            case "FIRST_PIECE_TWEEN":
                if (tweeningPiece.Count == 0)
                {
                    boardState = "CLEAR_BOARD";
                }
                break;
            case "CLEAR_BOARD":
                boardLogic.MovePiece(boardClearSolution.orgX, boardClearSolution.orgY, boardClearSolution.dirX, boardClearSolution.dirY);
                if (boardLogic.CanMove(boardClearSolution.orgX, boardClearSolution.orgY, 0, 0).canMove)
                {
                    boardLogic.ClearConnectedType(boardClearSolution.oldType, boardClearSolution.orgX, boardClearSolution.orgY);
                }
                boardLogic.ClearConnectedType(boardClearSolution.orginType, boardClearSolution.destiX, boardClearSolution.destiY);
                boardState = "CLEAR_BOARD_TWEEN";
                delayer = Time.time + waitDelay;
                int[] board = boardLogic.GetBoard();
                int pointsToAdd = 0;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == -1)
                    {
                        int y = Mathf.FloorToInt(i / width);
                        int x = i - (y * width);
                        ShrinkPiece(x, y);
                        pointsToAdd += i * 23;
                    }
                }
                totdalPoints += pointsToAdd;
                break;
            case "CLEAR_BOARD_TWEEN":
                if (tweeningPiece.Count == 0)
                {
                    if(delayer - Time.time < 0)
                    {
                        boardState = "SHIFT_DOWN";
                        pointsDisplay.text = totdalPoints.ToString();
                    }
                }
                break;
            case "SHIFT_DOWN":
                ShiftPiecesDown();
                boardLogic.ShiftPiecesDown();
                boardState = "SHIFT_DOWN_TWEEN";
                delayer = Time.time + waitDelay;
                break;
            case "SHIFT_DOWN_TWEEN":
                if (tweeningPiece.Count == 0)
                {
                    if (delayer - Time.time < 0)
                    {
                        boardState = "FILL_BOARD";
                    }
                }
                break;
            case "FILL_BOARD":
                RefillBoard();
                boardState = "STAY_DROPDOWN_ANIMATION";
                delayer = Time.time + waitDelay;
                break;
            case "STAY_DROPDOWN_ANIMATION":
                if (tweeningPiece.Count == 0)
                {
                    if (delayer - Time.time < 0)
                    {
                        boardState = "INITIAL";
                    }
                }
                break;

        }
    }

    private void CreateFromBoard()
    {
        int[] board = boardLogic.GetBoard();
        boardLogic.PrintBoard();
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
            newPiece.transform.localPosition = new Vector3(x*pieceWidth, y*InvertedHeight,0);
            activePieces.Add(newPiece);
        }
    }

    public MovePieceMeta CanMove(int x, int y, int dirX, int dirY)
    {
        return boardLogic.CanMove(x, y, dirX, dirY);
    }

    #region SHRINK PIECE DOWN
    public void ShrinkPiece(int x, int y)
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
        activePieces[indexN].ShrinkDown();
        tweeningPiece.Add(activePieces[indexN]);
    }
    #endregion

    #region PIECE MOVE LOGIC
    public void DoMove(int orgX, int orgY, int dirX, int dirY)
    {
        //Fill up the solution to check at end of board state
        boardClearSolution.orgX = orgX;
        boardClearSolution.orgY = orgY;
        boardClearSolution.destiX = orgX + dirX;
        boardClearSolution.destiY = orgY + dirY;
        boardClearSolution.dirX = dirX;
        boardClearSolution.dirY = dirY;
        boardClearSolution.orginType = boardLogic.GetBoard()[orgX + orgY * width];
        boardClearSolution.oldType = boardLogic.GetBoard()[boardClearSolution.destiX + boardClearSolution.destiY * width];

        //Animate pieces
        AnimateMove(orgX, orgY, dirX, dirY);
        AnimateMove(boardClearSolution.destiX, boardClearSolution.destiY, -dirX, -dirY);

        //Move to a tween friendly state
        boardState = "FIRST_PIECE_TWEEN";
    }

    public void DoFakeMove(int orgX, int orgY, int dirX, int dirY)
    {
        int destX = orgX + dirX;
        int destY = orgY + dirY;
        AnimateFeintMove(orgX, orgY, dirX, dirY);
        AnimateFeintMove(destX, destY, -dirX, -dirY);
        boardState = "FAKE_MOVE";

    }

    public void AnimateMove(int x, int y, float positionX, float positionY)
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
            activePieces[indexN].MoveY(positionY*InvertedHeight,(int)Mathf.Sign(positionY));
        }
        tweeningPiece.Add(activePieces[indexN]);
    }

    public void AnimateFeintMove(int x, int y, float positionX, float positionY)
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
            activePieces[indexN].FeintMoveY(positionY*InvertedHeight);
        }
        tweeningPiece.Add(activePieces[indexN]);
    }
    #endregion

    #region CLEAN_UP_LOGIC
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
        if (boardLogic.CanMove(x, y,0,0).canMove)
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
            int y = Mathf.FloorToInt(i / width);
            int x = i - (y * width);
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

    public void RemovePiece(BoardPieceController bpc)
    {
        activePieces.Remove(bpc);
        bpc.gameObject.SetActive(false);
        bpc.transform.SetParent(inactiveHolder);
        inactivePieces.Add(bpc);
        tweeningPiece.Remove(bpc);
    }
    #endregion

    #region SHIFT LOGIC
    public void ShiftPiecesDown()
    {
        int[] boardMatrix = boardLogic.GetBoard();
        for (int i = boardMatrix.Length - 1; i >= 0; i--)
        {
            int y = Mathf.FloorToInt(i / width);
            int x = i - (y * width);
            BoardPieceMeta bpm = new BoardPieceMeta()
            {
                newX = 0,
                newY = 0,
                shiftDown = false
            };
            ShiftPieceDown(boardMatrix, x, y, ref bpm);
            if(bpm.shiftDown == true)
            {
                int indexPiece = GetActivePieceIndex(x, y);
                activePieces[indexPiece].boardPieceMeta = bpm;
                tweeningPiece.Add(activePieces[indexPiece]);
                activePieces[indexPiece].PromptShiftDownTween(InvertedHeight);
            }
        }
    }

    private void ShiftPieceDown(int[] boardMatrix, int x, int y, ref BoardPieceMeta bpm)
    {

        if (y + 1 < height && boardMatrix[x + y * width] >= 0 && boardMatrix[x + (y + 1) * width] == -1)
        {
            bpm.shiftDown = true;
            int val = boardMatrix[x + (y) * width];
            boardMatrix[x + y * width] = -1;
            boardMatrix[x + (y + 1) * width] = val;
            ShiftPieceDown(boardMatrix, x, y + 1, ref bpm);
        }
        else
        {
            if (bpm.shiftDown == true)
            {
                bpm.newX = x;
                bpm.newY = y;
            }
        }
    }
    #endregion

    public void RefillBoard()
    {
        List<int> positionsToRefil = new List<int>();

        int[] board = boardLogic.GetBoard();
        for (int i = 0; i < board.Length; i++)
        {
            if(board[i] == -1)
            {
                positionsToRefil.Add(i);
            }
        }

        boardLogic.RefillBoard();
        board = boardLogic.GetBoard();

        for (int v = 0; v < positionsToRefil.Count; v++)
        {
            int i = positionsToRefil[v];

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
            newPiece.transform.localPosition = new Vector3(x * pieceWidth, 20, 0);
            newPiece.transform.localScale = new Vector3(1, 1, 1);
            activePieces.Add(newPiece);
            tweeningPiece.Add(newPiece);
            newPiece.ShiftDownOnFill(y * InvertedHeight);
        }
      
    }

    public int GetActivePieceIndex(int x,int y)
    {
        for (int i = 0; i < activePieces.Count; i++)
        {
            if (activePieces[i].x == x && activePieces[i].y == y)
            {
                return i;
            }
        }
        return -1;
    }
}

public struct BoardClearSolution
{
    public int orgX;
    public int orgY;
    public int dirX;
    public int dirY;
    public int destiX;
    public int destiY;
    public int oldType;
    public int orginType;
}

