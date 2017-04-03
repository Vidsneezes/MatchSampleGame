using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BoardLogicTests {
    //TODO add messages to asserts

	[Test]
	public void BoardFillEmpty() {
        int width = 2;
        int height = 2;
        BoardLogic boardLogic = new BoardLogic(width,height);

		Assert.AreEqual(new int[] { -1, -1, -1, -1 }, boardLogic.GetBoard());
	}

    [Test]
    public void FillBoardWithCells()
    {
        int width = 2;
        int height = 2;
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.FillBoardWithCells();
        Assert.AreNotEqual(new int[] { -1, -1, -1, -1 }, boardLogic.GetBoard());
    }

    [Test]
    public void ClearConnectedType()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,1,
            1,0,0,
            0,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(0, 1, 0, -1);
        Assert.AreEqual(true, canMove);
        if (canMove)
        {
            boardLogic.MovePiece(0, 1, 0, -1);
            boardLogic.ClearConnectedType(1, 0, 0);
        }
        Assert.AreEqual(new int[] { -1, -1, -1, 0, 0, 0, 0, 0, 0 }, boardLogic.GetBoard());
    }

    [Test]
    public void MovePiecesWithinBoard()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,1,
            1,0,0,
            0,0,0
        };

        int[] expectedBoard = new int[]
        {
            1,1,1,
            0,0,0,
            0,0,0
       };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(0, 1, 0, -1);
        Assert.AreEqual(true, canMove);
        if (canMove)
        {
            boardLogic.MovePiece(0, 1, 0, -1);
        }
        Assert.AreEqual(expectedBoard, boardLogic.GetBoard());

    }

    [Test]
    public void MovePiecesWithinBoardLarge()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,1,1,0,
            1,0,0,1,
            0,1,0,1,
            1,0,1,0
        };

        int[] expectedBoard = new int[]
        {
            0,1,1,0,
            0,1,0,1,
            0,1,0,1,
            1,0,1,0
       };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(0, 1, 1, 0);
        Assert.AreEqual(true, canMove);
        if (canMove)
        {
            boardLogic.MovePiece(0, 1, 1, 0);
        }
        Assert.AreEqual(expectedBoard, boardLogic.GetBoard());

    }

    [Test]
    public void CanMoveUponConnections()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,1,
            1,0,0,
            0,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(0, 1, 0, -1);
        Assert.AreEqual(true, canMove);

        width = 4;
        height = 4;
        boardConnection = new int[]
   {
            0,1,1,0,
            0,0,0,1,
            0,0,0,1,
            0,0,0,0
   };
        boardLogic.SetBoard(boardConnection, width, height);
        canMove = boardLogic.CanMove(3, 1, 0, -1);
        Assert.AreEqual(true, canMove);
    }

    [Test]
    public void CanMoveFalseOnNoConnections()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
       {
            0,1,1,0,
            0,0,0,1,
            0,0,0,1,
            0,1,0,0
       };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(1, 3, 0, -1);
        Assert.AreEqual(false, canMove,"no neighbors are connected cannot move");

        boardConnection = new int[]
{
            0,1,1,0,
            0,0,0,1,
            0,0,0,1,
            0,0,0,0
};
        boardLogic.SetBoard(boardConnection, width, height);
        canMove = boardLogic.CanMove(2, 0, 0, 1);
        Assert.AreEqual(false, canMove,"only 1 natural connection cannot move");
    }

    [Test]
    public void CannotMoveVerticalIfClearDoesntExists()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,3,
            1,0,0,
            0,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(0, 1, 0, -1);

        Assert.AreEqual(false, canMove);
    }

    [Test]
    public void CanMoveHorizontalOnConnection()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,0,
            1,0,0,
            1,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(1, 0, -1, 0);

        Assert.AreEqual(true, canMove);

        width = 4;
        height = 4;
        boardConnection = new int[]
{
            0,1,1,0,
            0,0,0,1,
            0,0,0,1,
            0,0,0,0
};
        boardLogic.SetBoard(boardConnection, width, height);
        canMove = boardLogic.CanMove(2, 0, 1, 0);
        Assert.AreEqual(true, canMove);
    }

    [Test]
    public void CannotMoveHorizontalIfClearDoesntExists()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,3,
            1,0,0,
            0,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(1, 0, -1, 0);

        Assert.AreEqual(false, canMove);
    }

    [Test]
    public void CannotMoveOutsieBoardBounds()
    {
        int width = 3;
        int height = 3;
        int[] boardConnection = new int[]
        {
            0,1,3,
            1,0,0,
            0,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        bool canMove = boardLogic.CanMove(1, 0, 0, -1);
        Assert.AreEqual(false, canMove);
        canMove = boardLogic.CanMove(0, 0, -1, 0);
        Assert.AreEqual(false, canMove);
        canMove = boardLogic.CanMove(2, 0, 1, 0);
        Assert.AreEqual(false, canMove);
        canMove = boardLogic.CanMove(0, 2, 0, 1);
        Assert.AreEqual(false, canMove);
    }

    [Test]
    public void RefillsBoardOnVoidedSpots()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,1,3,0,
            1,0,-1,0,
            -1,-1,-1,-1,
            1,1,1,1
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);

        boardLogic.RefillBoard();
        int[] board = boardLogic.GetBoard();
        bool hasNegative = false;
        for (int i = 0; i < board.Length; i++)
        {
            if(board[i] < 0)
            {
                hasNegative = true;
            }
        }

        Assert.AreEqual(false, hasNegative);
    }

    [Test]
    public void ShiftsNegativePiecesUp()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,1,3,0,
            1,0,-1,0,
            -1,-1,-1,-1,
            1,1,1,1
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);

        int[] expectedBoard = new int[]
        {
            -1,-1,-1,-1,
            0,1,-1,0,
            1,0,3,0,
            1,1,1,1
        };

        boardLogic.ShiftPiecesDown();

        Assert.AreEqual(expectedBoard, boardLogic.GetBoard());
    }
}
