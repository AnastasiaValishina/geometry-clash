using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Player,
    Appear,
    Move,
}
public class GameField : MonoBehaviour
{
    [SerializeField] public int width = 6;
    [SerializeField] public int height = 10;

    public SquareBase[,] squares;
    public PossibleMove possibleMovePrefab;
    public GameState gameState;

    void Start()
    {
        squares = new SquareBase[width, height];
    }

    public bool PositionOnBoardExists(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height){
            return true;
        }
        else {
            return false; 
        }
    }

    public void SetPositionEmpty(int x, int y) {
        squares[x, y] = null;
    }
}
