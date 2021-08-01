using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    BlockSpawner spawner;
    public BlockType blockType;
    public bool isDebug = false;
    public List<Block> followers;
    public Block head;
    public int prevX, prevY;
    public List<PossibleMove> possibleMoves;
    bool extraAdded = false;

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
        spawner = FindObjectOfType<BlockSpawner>();
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
                    FindAllNeighbours();
                    if (!extraAdded)
                    {
                        AddExtraBlock(followers[followers.Count - 1].posX, followers[followers.Count - 1].posY);
                        extraAdded = true;
                    }
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
        if (followers.Count > 0 && blockType == BlockType.Single) 
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

    //check for single neighbours 
    //single - single done
    //head - single done
    //head - head done
    //head - body
    //body - body
            if (block.blockType == BlockType.Single)
            {
                followers.Add(block);
                block.blockType = BlockType.Body;
                block.head = this;
                block.GetComponent<SpriteRenderer>().color = Color.blue;
            }

    // check for other snakes
            else if (block.blockType == BlockType.Head)
            {
                // проверить длинну
                if (followers.Count > block.followers.Count)
                {
                    // голову добавляем в список
                    followers.Add(block);
                    // объединяем списки
                    followers.AddRange(block.followers);
                    // назначаем новую голову                    
                    block.blockType = BlockType.Body;
                    block.head = this;    
                    foreach (Block b in block.followers) { b.head = this; }
                    block.followers.Clear();
                }
                else
                {
                    // голову добавляем в список
                    block.followers.Add(this);
                    // объединяем списки
                    block.followers.AddRange(followers);
                    // назначаем новую голову                    
                    blockType = BlockType.Body;
                    head = block;
                    foreach (Block b in followers) { b.head = block; }
                    followers.Clear();
                }
            }
        }
    }

    void AddExtraBlock(int x, int y)
    {
        if (gameField.PositionOnBoardExists(x + 1, y) && gameField.squares[x + 1, y] == null)
        {
            CreateBlock(x + 1, y);
        }
        else if (gameField.PositionOnBoardExists(x - 1, y) && gameField.squares[x - 1, y] == null)
        {
            CreateBlock(x - 1, y);
        }
        else if (gameField.PositionOnBoardExists(x, y + 1) && gameField.squares[x, y + 1] == null)
        {
            CreateBlock(x, y + 1);
        }
        else if (gameField.PositionOnBoardExists(x, y - 1) && gameField.squares[x, y - 1] == null)
        {
            CreateBlock(x, y - 1);
        }
        else
        {
            Debug.Log("Extra block not added");
        }
    }

    private void CreateBlock(int x, int y)
    {
        Block block = spawner.SpawnBlockAt(x, y);
        followers.Add(block);
        block.blockType = BlockType.Body;
        block.head = this;
        block.GetComponent<SpriteRenderer>().color = Color.yellow;
        block.name = "Extra";
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
