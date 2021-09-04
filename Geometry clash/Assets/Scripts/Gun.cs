using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    public bool canShoot;
    Game game;

    private void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public void Shoot()
    {
        if (game.gameState == GameState.Player && canShoot)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            canShoot = false;
            game.SetGameState(GameState.Game);
        }
    }
}
