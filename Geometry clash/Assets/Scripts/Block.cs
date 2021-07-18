using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Leading,
    Folowing,
}

public class Block : Square
{
    Grid grid;
    public BlockType blockType;

    void Start()
    {
        blockType = BlockType.Leading;
        grid = FindObjectOfType<Grid>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
          //  if (blockType == BlockType.Leading)
        //    {
        //        MakeRandomMove();
            //}
        }
    }

    public void MakeRandomMove()
    {
        var moves = grid.FindPossibleMovesFor(posX, posY);
        if (moves == null) return;
        
            int randomIndex = Random.Range(0, moves.Count);
            grid.SetPositionEmpty(posX, posY);
            posX = moves[randomIndex].posX;
            posY = moves[randomIndex].posY;
            transform.position = new Vector2(posX, posY);
            grid.squares[posX, posY] = this;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Block>().blockType == BlockType.Leading)
        {
            other.GetComponent<Block>().blockType = BlockType.Folowing;
        }
    }
}
