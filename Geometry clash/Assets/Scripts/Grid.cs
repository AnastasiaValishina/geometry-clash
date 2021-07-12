using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] public int width = 6;
    [SerializeField] public int height = 10;

    public Block[,] squares;

    void Start()
    {
        squares = new Block[width, height];
    }

    public bool PositionOnBoardExists(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height){
            return false;
        }
        else { return true; }
    }

    public List<Cell> FindPossibleMoves(int x, int y)
    {
        var moves = new List<Cell>();

        if (PositionOnBoardExists(x + 1, y)){
            Cell cell = new Cell(x + 1, y);
            moves.Add(cell);
        }
        if (PositionOnBoardExists(x - 1, y)){
            Cell cell = new Cell(x - 1, y);
            moves.Add(cell);
        }
        if (PositionOnBoardExists(x, y + 1)){
            Cell cell = new Cell(x, y + 1);
            moves.Add(cell);
        }
        if (PositionOnBoardExists(x, y - 1)){
            Cell cell = new Cell(x, y - 1);
            moves.Add(cell);
        }
        return moves;
    }

    public void SetPositionEmpty(int x, int y) 
    {
        squares[x, y] = null;
    }
}
