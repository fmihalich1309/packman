using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform ghostsParent;
    [SerializeField] private SpriteRenderer[] heartSprites;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private float gameOverDuration = 2f;

    private static readonly Color HeartFull = Color.red;
    private static readonly Color HeartLost = Color.white;
    private const int StartingHearts = 3;

    private Rigidbody2D player;
    private Vector2 playerStartPosition;
    private GhostAI[] ghosts;
    private Vector2[] ghostStartPositions;
    private int hearts;

    private void Awake()
    {
        Instance = this;
        player = playerMovement.GetComponent<Rigidbody2D>();
        ghosts = ghostsParent.GetComponentsInChildren<GhostAI>();
    }

    private void Start()
    {
        playerStartPosition = player.position;

        ghostStartPositions = new Vector2[ghosts.Length];
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghostStartPositions[i] = ghosts[i].GetComponent<Rigidbody2D>().position;
        }

        hearts = StartingHearts;
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    public void TakeDamage()
    {
        if (hearts <= 0) return;

        hearts--;
        int lostIndex = StartingHearts - 1 - hearts;
        if (lostIndex >= 0 && lostIndex < heartSprites.Length)
        {
            heartSprites[lostIndex].color = HeartLost;
        }

        if (hearts <= 0)
        {
            StartCoroutine(GameOverRoutine());
        }
        else
        {
            ResetPositions();
        }
    }

    private IEnumerator GameOverRoutine()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(gameOverDuration);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        RestartGame();
    }

    private void RestartGame()
    {
        hearts = StartingHearts;
        foreach (SpriteRenderer heart in heartSprites)
        {
            heart.color = HeartFull;
        }

        ScoreManager.Instance.ResetDots();
        ResetPositions();
    }

    private void ResetPositions()
    {
        player.position = playerStartPosition;
        player.linearVelocity = Vector2.zero;
        playerMovement.ResetState();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetGhost(ghostStartPositions[i]);
        }
    }

    public void FreezeGame()
    {
        playerMovement.enabled = false;
        player.linearVelocity = Vector2.zero;

        foreach (GhostAI ghost in ghosts)
        {
            ghost.enabled = false;
            ghost.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
