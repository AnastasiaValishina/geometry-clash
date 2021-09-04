using System.Collections;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] public int width = 6;
    [SerializeField] public int height = 10;

    public SquareBase[,] squares;
    public PossibleMove possibleMovePrefab;

    void Start()
    {
        squares = new SquareBase[width, height];
    }

    public bool PositionOnBoardExists(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height; 
    }

    public void SetPositionEmpty(int x, int y) {
        squares[x, y] = null;
    }

    private bool CheckMove(int x, int y)
    {
        return PositionOnBoardExists(x, y) && squares[x, y] == null;
    }
}
