using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLogic {

    private int boardWidth;
    private int boardHeight;

    private int[] boardMatrix;

    public BoardLogic()
    {
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


}
