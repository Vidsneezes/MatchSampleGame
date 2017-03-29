using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Sprite bluePiece;
    public Sprite greenPiece;
    public Sprite purplePiece;
    public Sprite redPiece;
    public Sprite yellowPiece;

    public GameObject piecePrefab;
    public List<GameObject> activePieces;
    public List<GameObject> inactivePieces;

    public int width;
    public int height;
    private BoardLogic boardLogic;

    // Use this for initialization
    private void Awake () {
        boardLogic = new BoardLogic(width, height);
        activePieces = new List<GameObject>();
        inactivePieces = new List<GameObject>();
        for (int i = 0; i < (width*height)+10; i++)
        {
            GameObject piece = GameObject.Instantiate(piecePrefab);
            piece.gameObject.SetActive(false);
            inactivePieces.Add(piece);
        }
	}
}
