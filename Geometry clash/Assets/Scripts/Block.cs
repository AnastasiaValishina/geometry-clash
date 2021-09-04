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
    BlockManager blockManager;
    public Color color;
    public SpriteRenderer spriteRenderer;
    public BlockType blockType;
    public bool isDebug = false;
    public List<Block> followers;
    public Block head;
    public int prevX, prevY;
    public List<PossibleMove> possibleMoves;
    public bool extraAdded = false;
    public int health = 1;

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
        followers = new List<Block>();
        possibleMoves = new List<PossibleMove>();
        blockManager = FindObjectOfType<BlockManager>();
        color = GetComponent<SpriteRenderer>().color;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        health -= 1;
        if (health <= 0)
        {

        }
        if (blockType == BlockType.Single)
        {
            gameField.SetPositionEmpty(posX, posY);
            blockManager.allBlocks.Remove(this);
            Destroy(gameObject);
        }
        else if (blockType == BlockType.Head)
        {
            gameField.SetPositionEmpty(posX, posY);
            Block newHead = followers[0];
            followers.RemoveAt(0);
            if (followers == null)
            {
                newHead.blockType = BlockType.Single;
            }
            if (followers.Count == 0)
            {
                newHead.blockType = BlockType.Single;
            }
            else if (followers.Count >= 1)
            {
                newHead.blockType = BlockType.Head;
                newHead.followers = followers;
                foreach (Block b in newHead.followers)
                {
                    b.head = newHead;
                }
            }
            CheckAndMakeSingle(this);
            blockManager.allBlocks.Remove(this);
            Destroy(gameObject);
        }
        else if (blockType == BlockType.Body)
        {
            gameField.SetPositionEmpty(posX, posY);
            blockManager.allBlocks.Remove(this);
            var index = head.followers.IndexOf(this);
            Block myHead = head;
            if (head.followers.Count >= 1)
            {
                for (int i = index; i < myHead.followers.Count; i++)
                {
                    myHead.followers[i].blockType = BlockType.Single;
                    myHead.followers[i].head = null;
                }
                myHead.followers.RemoveRange(index, myHead.followers.Count - index);
            }               
            CheckAndMakeSingle(myHead);
            Destroy(gameObject);
        }
    }

    void CheckAndMakeSingle(Block b)
    {
        if (b.followers.Count == 0)
        {
            b.blockType = BlockType.Single;
        }
    }

    public void UpdateColors()
    {
        if (blockType == BlockType.Single)
        {
            color = Color.red;
            spriteRenderer.color = color;
        }
        if (blockType == BlockType.Head)
        {
            //color = Color.black;
            spriteRenderer.color = color;
            float alpha = 0.90f;
            for (int i = 0; i < followers.Count; i++)
            {
                followers[i].color = followers[i].head.color;
                followers[i].color.a = alpha;
                followers[i].spriteRenderer.color = followers[i].color;
                alpha -= 0.1f;
                if (alpha < 0.3f)
                {
                    alpha = 0.3f;
                }
            }
        }
    }
}
