using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winScreen;

    private Dot[] dots;
    private int score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dots = FindObjectsByType<Dot>(FindObjectsSortMode.None);
        UpdateScoreText();
        if (winScreen != null) winScreen.SetActive(false);
    }

    public void AddScore()
    {
        score++;
        UpdateScoreText();

        if (score >= dots.Length)
        {
            WinGame();
        }
    }

    public void ResetDots()
    {
        score = 0;
        UpdateScoreText();

        foreach (Dot dot in dots)
        {
            dot.ResetDot();
        }
    }

    private void WinGame()
    {
        if (winScreen != null) winScreen.SetActive(true);
        GameManager.Instance.FreezeGame();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null) scoreText.text = $"РАХУНОК: {score}";
    }
}
