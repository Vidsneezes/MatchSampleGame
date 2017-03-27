using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class BoardLogicTests {

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
        int width = 2;
        int height = 2;
        int[] boardConnection = new int[]
        {
            0,1,1,1
        };

        BoardLogic boardLogic = new BoardLogic(width, height);
        boardLogic.SetBoard(boardConnection, 2, 2);
        boardLogic.ClearConnectedType(1, 1, 1);
        Assert.AreEqual(new int[] { 0, -1, -1, -1 }, boardLogic.GetBoard());
    }

    [Test]
    public void CanMoveVerticalIfClearExists()
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
        boardLogic.SetBoard(boardConnection, 3, 3);
        bool canMove = boardLogic.CanMove(0, 1, 0, 0);

        Assert.AreEqual(true, canMove);
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
        boardLogic.SetBoard(boardConnection, 3, 3);
        bool canMove = boardLogic.CanMove(0, 1, 0, 0);

        Assert.AreEqual(false, canMove);
    }
}
