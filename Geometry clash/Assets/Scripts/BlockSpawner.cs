using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] Block blockPrefab;
    [SerializeField] Marker markerPrefab;
    [SerializeField] int numberOfBlocks;

    Grid grid;
 
    private void Start()
    {
        grid = GetComponent<Grid>();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MoveAllBlocks();
            MakeBlocks();
            SpawnMarkers();
        }
    }

    private void SpawnMarkers()
    {
        int counter = 0;
        while (counter < numberOfBlocks)
        {
            int randomX = Random.Range(0, grid.width);
            int randomY = Random.Range(0, grid.height);
            if (grid.squares[randomX, randomY] == null)
            {
                Vector2 markerPos = new Vector2(randomX, randomY);
                Marker marker = Instantiate(markerPrefab, markerPos, Quaternion.identity);
                grid.squares[randomX, randomY] = marker;
                marker.posX = randomX;
                marker.posY = randomY;
                marker.transform.parent = transform;
                counter++;
            }
        }
    }

    private void MakeBlocks()
    {
        foreach (Square square in grid.squares)
        {
            if (square == null) continue;
            if (square is Marker)
            {
                Vector2 blockPos = new Vector2(square.posX, square.posY);
                Block block = Instantiate(blockPrefab, blockPos, Quaternion.identity);
                block.posX = square.posX;
                block.posY = square.posY;
                grid.squares[square.posX, square.posY] = block;
                Destroy(square.gameObject);
            }
        }
    }

    private void MoveAllBlocks()
    {
        foreach (Square square in grid.squares)
        {
            if (square is Block)
            {
                square.GetComponent<Block>().MakeRandomMove();
            }
        }
    }
}
