using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
        blockType = BlockType.Single;
        followers = new List<Block>();
        possibleMoves = new List<PossibleMove>();
        FindAllNeighbours();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (blockType == BlockType.Single || blockType == BlockType.Head)
            {
                MakeRandomMove();
                FindAllNeighbours();
                if (blockType == BlockType.Head)
                {
                    MoveBody();
                }
            }
        }
    }

    public void FindAllNeighbours()
    {
        CheckForSingleNeighbour(posX - 1, posY);
        CheckForSingleNeighbour(posX + 1, posY);
        CheckForSingleNeighbour(posX, posY + 1);
        CheckForSingleNeighbour(posX, posY - 1);
        if (followers.Count > 0) 
        { 
            blockType = BlockType.Head;
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void CheckForSingleNeighbour(int x, int y)
    {
        if (gameField.PositionOnBoardExists(x, y) && gameField.squares[x, y] is Block)
        {
            Block block = gameField.squares[x, y].GetComponent<Block>();
            if (block.blockType == BlockType.Single)
            {
                followers.Add(block);
                block.blockType = BlockType.Body;
                block.head = this;
            }
        }
    }

    public void MoveBody()
    {
        // первый боди встает на место головы
        gameField.SetPositionEmpty(followers[0].posX, followers[0].posY);
        followers[0].prevX = followers[0].posX;
        followers[0].prevY = followers[0].posY;
        followers[0].posX = prevX;
        followers[0].posY = prevY;
        followers[0].transform.position = new Vector2(followers[0].posX, followers[0].posY);
        gameField.squares[followers[0].posX, followers[0].posY] = followers[0];

        if (followers.Count < 1) return; 
        for (int b = 1; b < followers.Count; b++)
        {
            gameField.SetPositionEmpty(followers[b].posX, followers[b].posY);
            followers[b].prevX = followers[b].posX;
            followers[b].prevY = followers[b].posY;
            followers[b].posX = followers[b-1].prevX;
            followers[b].posY = followers[b-1].prevY;
            followers[b].transform.position = new Vector2(followers[b].posX, followers[b].posY);
            gameField.squares[followers[b].posX, followers[b].posY] = followers[b];
        }
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

    private bool CheckMove(int x, int y)
    {
        if (gameField.PositionOnBoardExists(x, y) && gameField.squares[x, y] == null)
        {
            return true;
        }
        return false;
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
