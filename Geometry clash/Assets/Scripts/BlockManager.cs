using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public List<Block> allBlocks;
    GameField gameField;
    BlockSpawner spawner;

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
        spawner = FindObjectOfType<BlockSpawner>();
        allBlocks = new List<Block>();
        for (int i = 0; i < allBlocks.Count; i++)
        {
            FindAllNeighbours(allBlocks[i]);
        }
    }

    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    MoveAllBlocks();
        //}
    }

    public void MoveAllBlocks()
    {
        for (int i = 0; i < allBlocks.Count; i++)
        {
            //allBlocks[i].UpdateColors();
            if (allBlocks[i].blockType == BlockType.Single || allBlocks[i].blockType == BlockType.Head)
            {
                allBlocks[i].MakeRandomMove();
                FindAllNeighbours(allBlocks[i]);
                if (allBlocks[i].blockType == BlockType.Head)
                {
                    MoveBody(allBlocks[i]);
                    FindAllNeighbours(allBlocks[i]);
                    if (!allBlocks[i].extraAdded)
                    {
                        AddExtraBlock(allBlocks[i], allBlocks[i].followers[allBlocks[i].followers.Count - 1].posX, allBlocks[i].followers[allBlocks[i].followers.Count - 1].posY);
                        allBlocks[i].extraAdded = true;
                    }
                }
            }
            allBlocks[i].UpdateColors();
        }
    }

    void FindAllNeighbours(Block block)
    {
        CheckForSingleNeighbour(block, block.posX - 1, block.posY);
        CheckForSingleNeighbour(block, block.posX + 1, block.posY);
        CheckForSingleNeighbour(block, block.posX, block.posY + 1);
        CheckForSingleNeighbour(block, block.posX, block.posY - 1);
        if (block.followers.Count > 0 && block.blockType == BlockType.Single)
        {
            block.blockType = BlockType.Head;
            block.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)); ;
        }
    }

    void CheckForSingleNeighbour(Block block, int x, int y)
    {
        if (gameField.PositionOnBoardExists(x, y) && gameField.squares[x, y] is Block)
        {
            Block singleBlock = gameField.squares[x, y].GetComponent<Block>();

            //check for single neighbours 
            //single - single done
            //head - single   done
            //head - head     done
            //single - body   done
            //head - body
            //body - body
            if (singleBlock.blockType == BlockType.Single)
            {
                block.followers.Add(singleBlock);
                singleBlock.blockType = BlockType.Body;
                singleBlock.head = block;
            }
            else if (block.blockType == BlockType.Single && singleBlock.blockType == BlockType.Body)
            {
                singleBlock.head.followers.Add(block);
                block.blockType = BlockType.Body;
                block.head = singleBlock.head;
                OrderNewFollowers(singleBlock.head.followers, singleBlock.head.followers[singleBlock.head.followers.Count - 2]);
            }


            //check for other snakes
            else if (singleBlock.blockType == BlockType.Head && block.blockType == BlockType.Head)
            {
                // проверить длинну
                if (block.followers.Count > singleBlock.followers.Count)
                {
                    // голову добавляем в список
                    block.followers.Add(singleBlock);
                    Block lastBlock = block.followers[block.followers.Count - 1];
                    // объединяем списки
                    block.followers.AddRange(singleBlock.followers);
                    // назначаем новую голову                    
                    singleBlock.blockType = BlockType.Body;
                    singleBlock.head = block;
                    foreach (Block b in singleBlock.followers)
                    {
                        b.head = block;
                    }
                    singleBlock.followers.Clear();
                    OrderNewFollowers(block.followers, lastBlock);
                }
                else
                {
                    // голову добавляем в список
                    singleBlock.followers.Add(block);
                    // объединяем списки
                    singleBlock.followers.AddRange(block.followers);
                    // назначаем новую голову                    
                    block.blockType = BlockType.Body;
                    block.head = singleBlock;
                    foreach (Block b in block.followers)
                    {
                        b.head = singleBlock;
                    }
                    block.followers.Clear();
                    OrderNewFollowers(singleBlock.followers, singleBlock.followers[singleBlock.followers.Count - 1]);
                }
            }
        }
    }

    void OrderNewFollowers(List<Block> followers, Block lastBlock)
    {
        for (int b = followers.IndexOf(lastBlock); b < followers.Count - 1; b++)
        {
            if (gameField.PositionOnBoardExists(followers[b].posX, followers[b].posY + 1) &&
                gameField.squares[followers[b].posX, followers[b].posY + 1] == null)
            {
                MoveBlockTo(followers[b + 1], followers[b].posX, followers[b].posY + 1);
            }
            else if (gameField.PositionOnBoardExists(followers[b].posX, followers[b].posY - 1) &&
                        gameField.squares[followers[b].posX, followers[b].posY - 1] == null)
            {
                MoveBlockTo(followers[b + 1], followers[b].posX, followers[b].posY - 1);
            }
            else if (gameField.PositionOnBoardExists(followers[b].posX + 1, followers[b].posY) &&
                        gameField.squares[followers[b].posX + 1, followers[b].posY] == null)
            {
                MoveBlockTo(followers[b + 1], followers[b].posX + 1, followers[b].posY);
            }
            else if (gameField.PositionOnBoardExists(followers[b].posX - 1, followers[b].posY) &&
                        gameField.squares[followers[b].posX - 1, followers[b].posY] == null)
            {
                MoveBlockTo(followers[b + 1], followers[b].posX - 1, followers[b].posY);
            }
            else { Debug.Log("No place to move"); }
        }
    }

    void MoveBlockTo(Block block, int newX, int newY)
    {
        gameField.SetPositionEmpty(block.posX, block.posY);
        block.prevX = block.posX;
        block.prevY = block.posY;
        block.posX = newX;
        block.posY = newY;
        block.transform.position = new Vector2(block.posX, block.posY);
        gameField.squares[block.posX, block.posY] = block;
    }

    void MoveBody(Block headBlock)
    {
        try
        {
            // первый боди встает на место головы
            MoveBlockTo(headBlock.followers[0], headBlock.prevX, headBlock.prevY);
            if (headBlock.followers.Count <= 1) return;
            for (int b = 1; b < headBlock.followers.Count; b++)
            {
                MoveBlockTo(headBlock.followers[b], headBlock.followers[b - 1].prevX, headBlock.followers[b - 1].prevY);
            }
        }
        catch
        {
            Debug.Log("Tried to move body of " + headBlock.name);
        }        
    }

    void AddExtraBlock(Block lastBlock, int x, int y)
    {
        if (gameField.PositionOnBoardExists(x + 1, y) && gameField.squares[x + 1, y] == null)
        {
            CreateExtraBlock(lastBlock, x + 1, y);
        }
        else if (gameField.PositionOnBoardExists(x - 1, y) && gameField.squares[x - 1, y] == null)
        {
            CreateExtraBlock(lastBlock, x - 1, y);
        }
        else if (gameField.PositionOnBoardExists(x, y + 1) && gameField.squares[x, y + 1] == null)
        {
            CreateExtraBlock(lastBlock, x, y + 1);
        }
        else if (gameField.PositionOnBoardExists(x, y - 1) && gameField.squares[x, y - 1] == null)
        {
            CreateExtraBlock(lastBlock, x, y - 1);
        }
        else
        {
            Debug.Log("Extra block not added");
        }
    }

    void CreateExtraBlock(Block lastBlock, int x, int y)
    {
        Block block = spawner.SpawnBlockAt(x, y);
        lastBlock.followers.Add(block);
        block.blockType = BlockType.Body;
        block.head = lastBlock;
        block.name = "Extra";
    }
}
