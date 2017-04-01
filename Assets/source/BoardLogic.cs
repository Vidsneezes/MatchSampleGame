using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {

    //TODO add function that fills board with random value where a -1 is present

    //TODO write unit test for -1 fill

    private int boardWidth;
    private int boardHeight;

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
        boardWidth = _w;
        boardHeight = _h;
        boardMatrix = new int[boardWidth * boardHeight];
        InitBoard();
    }

    public void InitBoard()
    {
        for (int i = 0; i < boardWidth * boardHeight; i++)
        {
            boardMatrix[i] = -1;
        }
    }

    public void FillBoardWithCells()
    {
        for (int j = 0; j < boardHeight; j++)
        {
            for (int i = 0; i < boardHeight; i++)
            {
                int pieceIndex = i + j * boardWidth;
                boardMatrix[pieceIndex] = Mathf.FloorToInt(Random.Range(0, 1f) * 5);
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
            if (x + 1 < boardWidth)
            {
                ClearConnectedType(type, x + 1, y);
            }
            if (y - 1 >= 0)
            {
                ClearConnectedType(type, x, y - 1);
            }
            if (y + 1 < boardHeight)
            {
                ClearConnectedType(type, x, y + 1);
            }
        }
    }

    public bool CanMove(int orX, int orY, int dirX, int dirY)
    {
        int newPosX = orX + dirX;
        int newPosY = orY + dirY;
        int original = boardMatrix[orX + orY * boardWidth];
        int possibleConnections = 0;
        int val_1 = 0, val_2 = 0, val_3 = 0;

        if (dirY == -1)
        {
            val_1 = MakeConnection(original, newPosX , newPosY , -1, 0);
            val_2 = MakeConnection(original, newPosX , newPosY , 1, 0);
            val_3 = MakeConnection(original, newPosX, newPosY ,0, -1);
        }else if(dirY == 1)
        {
            val_1 = MakeConnection(original, newPosX , newPosY,-1,0);
            val_2 = MakeConnection(original, newPosX , newPosY, 1,0 );
            val_3 = MakeConnection(original, newPosX, newPosY ,0,1);
        }else if(dirX == -1)
        {
            val_1 = MakeConnection(original, newPosX , newPosY ,0,-1);
            val_2 = MakeConnection(original, newPosX , newPosY ,0,1);
            val_3 = MakeConnection(original, newPosX , newPosY,-1,0);
        }else if(dirX == 1)
        {
            val_1 = MakeConnection(original, newPosX , newPosY, 0,-1);
            val_2 = MakeConnection(original, newPosX , newPosY, 0 ,1);
            val_3 = MakeConnection(original, newPosX , newPosY,1,0);
        }
        possibleConnections = val_1 + val_2 + val_3;

        if (possibleConnections >= 2)
        {
            return true;
        }
        return false;
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

        if (newX >= 0 && newX < boardWidth && newY >= 0 && newY < boardHeight && boardMatrix[newX + newY * boardWidth] == original)
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
        boardWidth = _w;
        boardHeight = _h;
        boardMatrix = newBoard;
    }

    public int GetValue(int x, int y)
    {
        return boardMatrix[x + boardWidth * y];
    }

    public void SetValue(int value, int x, int y)
    {
        boardMatrix[x + boardWidth * y] = value;
    }
}
