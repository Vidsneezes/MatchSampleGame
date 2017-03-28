using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {

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
    }

    public void ClearConnectedType(int type, int x, int y)
    {

    }

    public bool CanMove(int orX, int orY, int newX, int newY)
    {
        int newPosX = orX + newX;
        int newPosY = orY + newY;
        int original = boardMatrix[orX + orY * boardWidth];
        int possibleConnections = 0;

        if (newY == -1)
        {
            possibleConnections += ConnectionCounter(original, newPosX - 1, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX - 2, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX + 1, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX + 2, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX, newPosY - 1);
            possibleConnections += ConnectionCounter(original, newPosX, newPosY - 2);
        }else if(newY == 1)
        {
            possibleConnections += ConnectionCounter(original, newPosX - 1, newPosY);
            possibleConnections += ConnectionCounter(original, newPosX - 2, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX + 1, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX + 2, newPosY );
            possibleConnections += ConnectionCounter(original, newPosX, newPosY + 1);
            possibleConnections += ConnectionCounter(original, newPosX, newPosY + 2);
        }else if(newX == -1)
        {
            possibleConnections += ConnectionCounter(original, newPosX , newPosY - 1);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY - 2);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY + 1);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY + 2);
            possibleConnections += ConnectionCounter(original, newPosX - 1, newPosY);
            possibleConnections += ConnectionCounter(original, newPosX - 2, newPosY);
        }else if(newX == 1)
        {
            possibleConnections += ConnectionCounter(original, newPosX , newPosY - 1);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY - 2);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY + 1);
            possibleConnections += ConnectionCounter(original, newPosX , newPosY + 2);
            possibleConnections += ConnectionCounter(original, newPosX + 1, newPosY);
            possibleConnections += ConnectionCounter(original, newPosX + 2, newPosY);
        }

        if (possibleConnections >= 2)
        {
            return true;
        }
        return false;
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
