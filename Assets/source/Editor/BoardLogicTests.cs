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
    public void BecomesNegativeOnMovePieceAndClear()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            2,1,1,0,
            1,2,0,0,
            2,1,0,0,
            2,1,0,0
        };
        int[] moveExpectedBoard = new int[]
        {
            2,1,1,0,
            2,1,0,0,
            2,1,0,0,
            2,1,0,0
        };
        int[] clearExpectedBoard = new int[]
        {
            -1,-1,-1,0,
            -1,-1,0,0,
            -1,-1,0,0,
            -1,-1,0,0
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        int orginX = 0;
        int orginY = 1;
        int dirX = 1;
        int dirY = 0;
        int orginType = boardLogic.GetBoard()[orginX + orginY * width];
        bool canMove = boardLogic.CanMove(orginX, orginY, dirX, dirY);

        Assert.AreEqual(true, canMove);
        if (canMove)
        {
            int destinationX = orginX + dirX;
            int destinationY = orginY + dirY;
            int oldType = boardLogic.GetBoard()[destinationX + destinationY * width];
            boardLogic.MovePiece(orginX, orginY, dirX, dirY);
            Assert.AreEqual(moveExpectedBoard, boardLogic.GetBoard());
            boardLogic.ClearConnectedType(oldType, orginX, orginY);
            boardLogic.ClearConnectedType(orginType, destinationX, destinationY);
        }
        Assert.AreEqual(clearExpectedBoard, boardLogic.GetBoard());
    }

    [Test]
    public void TestFullPieceMoveToRefillCycle()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,0,0,0,
            0,0,0,0,
            2,1,1,1,
            1,2,2,2
        };
        int[] moveExpectedBoard = new int[]
        {
            0,0,0,0,
            0,0,0,0,
            1,1,1,1,
            2,2,2,2
        };
        int[] clearExpectedBoard = new int[]
        {
            0,0,0,0,
            0,0,0,0,
            -1,-1,-1,-1,
            -1,-1,-1,-1
        };

        int[] shiftBoardExpected = new int[]
        {
            -1,-1,-1,-1,
            -1,-1,-1,-1,
            0,0,0,0,
            0,0,0,0
        };

        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);
        int orginX = 0;
        int orginY = 2;
        int dirX = 0;
        int dirY = 1;
        int orginType = boardLogic.GetBoard()[orginX + orginY * width];
        bool canMove = boardLogic.CanMove(orginX, orginY, dirX, dirY);

        Assert.AreEqual(true, canMove);
        if (canMove)
        {
            int destinationX = orginX + dirX;
            int destinationY = orginY + dirY;
            int oldType = boardLogic.GetBoard()[destinationX + destinationY * width];
            boardLogic.MovePiece(orginX, orginY, dirX, dirY);
            Assert.AreEqual(moveExpectedBoard, boardLogic.GetBoard());
            boardLogic.ClearConnectedType(oldType, orginX, orginY);
            boardLogic.ClearConnectedType(orginType, destinationX, destinationY);
            Assert.AreEqual(clearExpectedBoard, boardLogic.GetBoard());
            boardLogic.ShiftPiecesDown();
            Assert.AreEqual(shiftBoardExpected, boardLogic.GetBoard());
            boardLogic.RefillBoard();
            int[] board = boardLogic.GetBoard();
            bool hasNegative = false;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] < 0)
                {
                    hasNegative = true;
                }
            }
            Assert.AreEqual(false, hasNegative);
        }
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
    public void ShiftPiecesBelowNegative()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,  1,  3,  0,
            1,  0,  -1, 0,
            -1, -1, -1, -1,
            1,   1,  1,  1
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);

        int[] expectedBoard = new int[]
        {
            -1,  -1,  -1,  -1,
            0,  1,  -1, 0,
            1,  0, 3, 0,
            1,   1,  1,  1
        };

        boardLogic.ShiftPiecesDown();

        Assert.AreEqual(expectedBoard, boardLogic.GetBoard());
    }


    [Test]
    public void ShiftsNegativePieceOnComplexBoard()
    {
        int width = 4;
        int height = 4;
        int[] boardConnection = new int[]
        {
            0,  1,  3,  0,
            -1,  0,  -1, 0,
            0, -1, -1, -1,
            1,   1,  1,  -1
        };
        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, width, height);

        int[] expectedBoard = new int[]
        {
            -1,  -1,  -1,  -1,
            0,  1,  -1, -1,
            0, 0, 3, 0,
            1,   1,  1,  0
        };

        boardLogic.ShiftPiecesDown();

        Assert.AreEqual(expectedBoard, boardLogic.GetBoard());
    }
}
