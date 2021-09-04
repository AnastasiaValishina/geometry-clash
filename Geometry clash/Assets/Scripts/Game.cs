using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Game,
    Player,
}
public class Game : MonoBehaviour
{
    public GameState gameState;
    Gun gun;
    BlockManager blockManager;
    BlockSpawner blockSpawner;

    private void Start()
    {
        gun = FindObjectOfType<Gun>();
        blockManager = FindObjectOfType<BlockManager>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
        gun = FindObjectOfType<Gun>();
    }
    private void Update()
    {
        if (gameState == GameState.Game)
        {
            StartCoroutine(StartLoop());
            gameState = GameState.Player;
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    IEnumerator StartLoop()
    {
        yield return new WaitForSeconds(1);
        blockSpawner.TurnMarkersToBlocks();
        yield return new WaitForSeconds(0.1f);
        blockSpawner.SpawnMarkers();
        yield return new WaitForSeconds(0.5f);
        blockManager.MoveAllBlocks();
        yield return new WaitForSeconds(0.5f);
        gun.canShoot = true;
    }
}
