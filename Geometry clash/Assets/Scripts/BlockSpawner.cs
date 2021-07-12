using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] Block blockPrefab;
    [SerializeField] GameObject markPrefab;
    [SerializeField] Transform blockContainer;
    [SerializeField] int numberOfBlocks;

    Grid grid;
    private void Start()
    {
        grid = GetComponent<Grid>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        int counter = 0;
        while (counter < numberOfBlocks)
        {
            int randomX = Random.Range(0, grid.width);
            int randomY = Random.Range(0, grid.height);
            if (grid.PositionOnBoardExists(randomX, randomY))
            {
                Vector2 blockPos = new Vector2(randomX, randomY);
                Block block = Instantiate(blockPrefab, blockPos, Quaternion.identity);
                grid.squares[randomX, randomY] = block;
                block.transform.parent = blockContainer;
                counter++;
            }
        }
    }
}
