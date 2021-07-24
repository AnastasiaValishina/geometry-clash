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
    GameField gameField;
    public BlockType blockType;
    public bool isDebug = false;

    void Start()
    {
        gameField = FindObjectOfType<GameField>();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MakeRandomMove();
        }
    }

    public void MakeRandomMove()
    {
        var possibleMoves = new List<PossibleMove>();
        if (CheckMove(posX - 1, posY))
        {
            possibleMoves.Add(CreatePosibleMoves(posX - 1, posY));
        }
        if (CheckMove(posX + 1, posY))
        {
            possibleMoves.Add(CreatePosibleMoves(posX + 1, posY));
        }
        if (CheckMove(posX, posY + 1))
        {
            possibleMoves.Add(CreatePosibleMoves(posX, posY + 1));
        }
        if (CheckMove(posX, posY - 1))
        {
            possibleMoves.Add(CreatePosibleMoves(posX, posY - 1));
        }

        if (possibleMoves != null)
        {
            int randomIndex = Random.Range(0, possibleMoves.Count);
            gameField.SetPositionEmpty(posX, posY);
            posX = possibleMoves[randomIndex].posX;
            posY = possibleMoves[randomIndex].posY;
            foreach (var move in possibleMoves)
            {
                Destroy(move.gameObject);
            }
            possibleMoves.Clear();
            transform.position = new Vector2(posX, posY);
            Debug.Log("to " + posX + ", " + posY);
            gameField.squares[posX, posY] = this;
        }
    }

    private bool CheckMove(int x, int y)
    {
        if (gameField.PositionOnBoardExists(x, y) && gameField.squares[x, y] == null)
        {
            Debug.Log(x + ", " + y + " should be true " + gameField.PositionOnBoardExists(x, y));
            return true;
        }
        return false;
    }
    private PossibleMove CreatePosibleMoves(int x, int y)
    {
        Vector2 position = new Vector2(x, y);
        PossibleMove possibleMove = Instantiate(gameField.possibleMovePrefab, position, Quaternion.identity);
        possibleMove.posX = x;
        possibleMove.posY = y;
        Debug.Log(possibleMove.posX + ", " + possibleMove.posY + " is written to Possible Moves");
        return possibleMove;
    }
}
