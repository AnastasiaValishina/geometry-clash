using UnityEngine;

public class Cell : MonoBehaviour
{
    public int posX;
    public int posY;
    public bool empty = true;

    public Cell(int x, int y)
    {
        posX = x;
        posY = y;
    }
}
