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

    public void ClearConnectedType(int type, int x, int y)
    {

    }

    public bool CanMove(int orX, int orY, int newX, int newY)
    {
        int newIndex = newX + newY * boardWidth;
        int original = boardMatrix[orX + orY * boardWidth];
        int positionalIndex = newX + newY * boardWidth;
        int possibleConnections = 0;

        //Check Up of new
        if (newY - 1 >= 0 && boardMatrix[newX + (newY - 1) * boardWidth] == original)
        {
            possibleConnections += 1;
            possibleConnections += ConnectionCounter(original, newX - 1, newY - 1);
            possibleConnections += ConnectionCounter(original, newX + 1, newY - 1);
            possibleConnections += ConnectionCounter(original, newX , newY - 2);
        }
        //CHeck down of new
        if (newY + 1 < boardHeight && boardMatrix[newX + (newY + 1) * boardWidth] == original)
        {
            possibleConnections += 1;
            possibleConnections += ConnectionCounter(original, newX - 1, newY + 1);
            possibleConnections += ConnectionCounter(original, newX + 1, newY + 1);
            possibleConnections += ConnectionCounter(original, newX, newY + 2);
        }
        //check left of new
        if (newX - 1 >= 0 && boardMatrix[(newX - 1) + newY * boardWidth] == original)
        {
            possibleConnections += 1;
            possibleConnections += ConnectionCounter(original, newX - 1, newY - 1);
            possibleConnections += ConnectionCounter(original, newX - 1, newY + 1);
            possibleConnections += ConnectionCounter(original, newX - 2, newY);
        }
        //check right of new
        if (newX + 1 < boardWidth && boardMatrix[(newX + 1) + newY * boardWidth] == original)
        {
            possibleConnections += 1;
            possibleConnections += ConnectionCounter(original, newX + 1, newY - 1);
            possibleConnections += ConnectionCounter(original, newX + 1, newY + 1);
            possibleConnections += ConnectionCounter(original, newX + 2, newY);
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
}
