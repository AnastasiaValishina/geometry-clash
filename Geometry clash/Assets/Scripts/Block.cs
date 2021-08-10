using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Single,
    Head,
    Body,
}

public class Block : SquareBase
{
    GameField gameField;
    public BlockType blockType;
    public bool isDebug = false;
    public List<Block> followers;
    public Block head;
    public int prevX, prevY;
    public List<PossibleMove> possibleMoves;
    public bool extraAdded = false;

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
        followers = new List<Block>();
        possibleMoves = new List<PossibleMove>();        
    }
    void Update()
    {

    }

    public void MakeRandomMove() // для Single и Head
    {
        AddSquareIfEmpty(posX + 1, posY);
        AddSquareIfEmpty(posX - 1, posY);
        AddSquareIfEmpty(posX, posY + 1);
        AddSquareIfEmpty(posX, posY - 1);
       
        if (possibleMoves.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleMoves.Count);
            gameField.SetPositionEmpty(posX, posY);
            // for head
            prevX = posX;
            prevY = posY;

            posX = possibleMoves[randomIndex].posX;
            posY = possibleMoves[randomIndex].posY;
            ClearList();
            transform.position = new Vector2(posX, posY);
            gameField.squares[posX, posY] = this;
        }
    }

    private void AddSquareIfEmpty(int x, int y)
    {
        if (gameField.PositionOnBoardExists(x, y) && gameField.squares[x, y] == null)
        {
            possibleMoves.Add(CreatePosibleMoves(x, y));
        }
    }

    private void ClearList()
    {
        foreach (var move in possibleMoves)
        {
            Destroy(move.gameObject);
        }
        possibleMoves.Clear();
    }

    private PossibleMove CreatePosibleMoves(int x, int y)
    {
        Vector2 position = new Vector2(x, y);
        PossibleMove possibleMove = 
            Instantiate(gameField.possibleMovePrefab, position, Quaternion.identity);
        possibleMove.posX = x;
        possibleMove.posY = y;
        return possibleMove;
    }
}
