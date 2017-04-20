using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {


    public int boardWidth
    {
        get
        {
            return _boardWidth;
        }
    }

    public int boardHeight
    {
        get
        {
            return _boardHeight;
        }
    }

    private int _boardWidth;
    private int _boardHeight;

    private int[] boardMatrix;

    /*
    * 0 - green
    * 1 - blue
    * 2 - purple
    * 3 - red
    * 4 - yellow
    */


    public BoardLogic(int _w, int _h)
    {
        _boardWidth = _w;
        _boardHeight = _h;
        boardMatrix = new int[_boardWidth * _boardHeight];
        InitBoard();
    }

    public void InitBoard()
    {
        for (int i = 0; i < _boardWidth * _boardHeight; i++)
        {
            boardMatrix[i] = -1;
        }
    }

    public void FillBoardWithCells()
    {
        for (int j = 0; j < _boardHeight; j++)
        {
            for (int i = 0; i < _boardHeight; i++)
            {
                int pieceIndex = i + j * _boardWidth;
                boardMatrix[pieceIndex] = Mathf.FloorToInt(Random.Range(0, 1f) * 5);
            }
        }
    }

    public void RefillBoard()
    {
        for (int j = 0; j < _boardHeight; j++)
        {
            for (int i = 0; i < _boardHeight; i++)
            {
                int pieceIndex = i + j * _boardWidth;
                if (boardMatrix[pieceIndex] == -1)
                {
                    boardMatrix[pieceIndex] = Mathf.FloorToInt(Random.Range(0, 1f) * 5);
                }
            }
        }
    }

    public void MovePiece(int orX, int orY, int dirX, int dirY)
    {
        int startValue = GetValue(orX, orY);
        int nextValue = GetValue(orX + dirX, orY + dirY);
        SetValue(nextValue, orX, orY);
        SetValue(startValue, orX + dirX, orY + dirY);
    }

    public void ClearConnectedType(int type, int x, int y)
    {
        int pieceValue = GetValue(x, y);
        if (pieceValue == type)
        {
            SetValue(-1, x, y);
            if (x - 1 >= 0)
            {
                ClearConnectedType(type, x - 1, y);
            }
            if (x + 1 < _boardWidth)
            {
                ClearConnectedType(type, x + 1, y);
            }
            if (y - 1 >= 0)
            {
                ClearConnectedType(type, x, y - 1);
            }
            if (y + 1 < _boardHeight)
            {
                ClearConnectedType(type, x, y + 1);
            }
        }
    }

    public void ShiftPiecesDown()
    {
        for (int i = boardMatrix.Length-1; i >= 0; i--)
        {
            int y = Mathf.FloorToInt(i / boardWidth);
            int x = i - (y * boardWidth);
            ShiftPieceDown(x, y);
        }
    }

    protected void ShiftPieceDown(int x, int y)
    {
        if(y+1 < boardHeight && boardMatrix[x+y*boardWidth] >= 0 && boardMatrix[x+(y+1)*boardWidth] == -1)
        {
            int val = boardMatrix[x + (y) * boardWidth];
            boardMatrix[x + y * boardWidth] = -1;
            boardMatrix[x + (y+1) * boardWidth] = val;
            ShiftPieceDown(x, y + 1);
        }
    }

    public MovePieceMeta CanMove(int orX, int orY, int dirX, int dirY)
    {
        int newPosX = orX + dirX;
        int newPosY = orY + dirY;
        int original = boardMatrix[orX + orY * _boardWidth];
        int possibleConnections = 0;
        int val_1 = 0, val_2 = 0, val_3 = 0;
        MovePieceMeta movePieceMeta;
        movePieceMeta.canMove = false;
        movePieceMeta.same = false;
        movePieceMeta.dirX = dirX;
        movePieceMeta.dirY = dirY;

        if(orX + dirX >= _boardWidth || orX + dirX < 0 || orY + dirY >= _boardHeight || orY + dirY < 0)
        {
            movePieceMeta.same = true;  
            return movePieceMeta;
        }

        if (original == boardMatrix[newPosX + newPosY * boardWidth])
        {
            movePieceMeta.same = true;
            return movePieceMeta;
        }
        if (dirY == -1)
        {
            val_1 = MakeConnection(original, newPosX, newPosY, -1, 0);
            val_2 = MakeConnection(original, newPosX, newPosY, 1, 0);
            val_3 = MakeConnection(original, newPosX, newPosY, 0, -1);
        }else if(dirY == 1)
        {
            val_1 = MakeConnection(original, newPosX, newPosY, -1, 0);
            val_2 = MakeConnection(original, newPosX, newPosY, 1, 0);
            val_3 = MakeConnection(original, newPosX, newPosY, 0, 1);
        }else if(dirX == -1)
        {
            val_1 = MakeConnection(original, newPosX, newPosY, 0, -1);
            val_2 = MakeConnection(original, newPosX, newPosY, 0, 1);
            val_3 = MakeConnection(original, newPosX, newPosY, -1, 0);
        }else if(dirX == 1)
        {
            val_1 = MakeConnection(original, newPosX, newPosY, 0, -1);
            val_2 = MakeConnection(original, newPosX, newPosY, 0, 1);
            val_3 = MakeConnection(original, newPosX, newPosY, 1, 0);
        }
        possibleConnections = val_1 + val_2 + val_3;

        if (possibleConnections >= 2)
        {
            movePieceMeta.canMove = true;
            return movePieceMeta;
        }
        return movePieceMeta;
    }

    public int MakeConnection(int original, int newX, int newY, int dirX, int dirY)
    {
        int value = ConnectionCounter(original, newX + dirX, newY + dirY);
        value += value > 0 ? ConnectionCounter(original, newX + (dirX * 2), newY + (dirY*2)) : 0;
        return value;
    }

    public int ConnectionCounter(int original,int newX, int newY)
    {
        int possibleConnections = 0;

        if (newX >= 0 && newX < _boardWidth && newY >= 0 && newY < _boardHeight && boardMatrix[newX + newY * _boardWidth] == original)
        {
            possibleConnections += 1;
        }
        return possibleConnections;
    }

    public int[] GetBoard()
    {
        return boardMatrix;
    }

    public void SetBoard(int[] newBoard, int _w, int _h)
    {
        _boardWidth = _w;
        _boardHeight = _h;
        boardMatrix = newBoard;
    }

    public int GetValue(int x, int y)
    {
        return boardMatrix[x + _boardWidth * y];
    }

    public void SetValue(int value, int x, int y)
    {
        boardMatrix[x + _boardWidth * y] = value;
    }

    public void PrintBoard()
    {
        string boardDisplay = "";
        for (int i = 0; i < boardMatrix.Length; i++)
        {
            int y = Mathf.FloorToInt(i / boardWidth);
            int x = i - (y * boardWidth);
            int pieceType = boardMatrix[i];
            boardDisplay += pieceType;
            if(y >= boardWidth-1)
            {
                boardDisplay += "\n";
            }
        }

    }
}


public struct MovePieceMeta
{
    public bool canMove;
    public bool same;

    public int dirX;
    public int dirY;

}