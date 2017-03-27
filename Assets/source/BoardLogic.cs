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
        for (int i = 0; i < boardWidth*boardHeight; i++)
        {
            boardMatrix[i] = -1;
        }
    }

    public int[] GetBoard()
    {
        return boardMatrix;
    }
}
