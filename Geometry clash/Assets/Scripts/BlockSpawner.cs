using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] Block blockPrefab;
    [SerializeField] Marker markerPrefab;
    [SerializeField] int numberOfBlocks;

    GameField gameField;
 
    private void Start()
    {
        gameField = GetComponent<GameField>();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            MakeBlocks();
            SpawnMarkers();
        }
    }

    private void SpawnMarkers()
    {
        int counter = 0;
        while (counter < numberOfBlocks)
        {
            int randomX = Random.Range(0, gameField.width);
            int randomY = Random.Range(0, gameField.height);
            if (gameField.squares[randomX, randomY] == null)
            {
                Vector2 markerPos = new Vector2(randomX, randomY);
                Marker marker = Instantiate(markerPrefab, markerPos, Quaternion.identity);
                gameField.squares[randomX, randomY] = marker;
                marker.posX = randomX;
                marker.posY = randomY;
                marker.transform.parent = transform;
                counter++;
            }
        }
    }

    private void MakeBlocks()
    {
        foreach (Square square in gameField.squares)
        {
            if (square == null) continue;
            if (square is Marker)
            {
                Vector2 blockPos = new Vector2(square.posX, square.posY);
                Block block = Instantiate(blockPrefab, blockPos, Quaternion.identity);
                block.posX = square.posX;
                block.posY = square.posY;
                gameField.squares[square.posX, square.posY] = block;
                Destroy(square.gameObject);
            }
        }
    }
}
