using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Leading,
    Folowing,
}

public class Block : MonoBehaviour
{
    Grid grid;
    int posX, posY;
    public BlockType blockType;

    void Start()
    {
        blockType = BlockType.Leading;
        grid = FindObjectOfType<Grid>();
        posX = (int)transform.position.x;
        posY = (int)transform.position.y;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (blockType == BlockType.Leading)
            {
                MakeRandomMove();
            }
        }
    }

    void MakeRandomMove()
    {
        var moves = grid.FindPossibleMoves(posX, posY);
        int randomIndex = Random.Range(0, moves.Count);
        grid.squares[posX, posY] = null;
        posX = moves[randomIndex].posX;
        posY = moves[randomIndex].posY;
        transform.position = new Vector2(posX, posY);
        grid.squares[posX, posY] = this;
    //    moves[randomIndex].empty = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Block>().blockType == BlockType.Leading)
        {
            other.GetComponent<Block>().blockType = BlockType.Folowing;
        }
    }
}
