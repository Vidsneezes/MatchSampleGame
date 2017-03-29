using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject piecePrefab;
    public List<GameObject> activePieces;
    public List<GameObject> inactivePieces;

    public int width;
    public int height;
    public float pieceWidth;
    public float pieceHeight;
    public float horizontalOffset;
    public float verticalOffset;
    public Transform pieceHolder;
    public Transform inactiveHolder;
    private BoardLogic boardLogic;

    // Use this for initialization
    private void Awake () {
        boardLogic = new BoardLogic(width, height);
        activePieces = new List<GameObject>();
        inactivePieces = new List<GameObject>();
        for (int i = 0; i < (width*height)+10; i++)
        {
            GameObject piece = GameObject.Instantiate(piecePrefab,inactiveHolder);
            piece.gameObject.SetActive(false);
            inactivePieces.Add(piece);
        }
        CreateFromBoard();
	}

    private void CreateFromBoard()
    {
        int[] board = boardLogic.GetBoard();
        for (int i = 0; i < board.Length; i++)
        {
            int y = Mathf.FloorToInt(i / width);
            int x = i - (y * width);
            GameObject newPiece = inactivePieces[0];
            inactivePieces.RemoveAt(0);
            newPiece.transform.SetParent(pieceHolder);
            newPiece.SetActive(true);
            newPiece.transform.localPosition = new Vector3(x*pieceWidth, y*pieceHeight,0);
        }
    }


}
