using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player { get; private set; }
    public Bola bola { get; private set; }
    public Brick[] bricks { get; private set; }

    private const int MaxLevels = 2;

    private int currentLevel = 1;
    private int playerScore = 0;
    private int playerLives = 3;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        playerScore = 0;
        playerLives = 3;
        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        currentLevel = level;

        if (currentLevel > MaxLevels)
        {
            // Start over at level 1 when all levels are beaten
            // You can also load a "Win" scene instead
            LoadLevel(1);
            return;
        }

        SceneManager.LoadScene("Level" + currentLevel);
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        player = FindObjectOfType<Player>();
        bola = FindObjectOfType<Bola>();
        bricks = FindObjectsOfType<Brick>();
    }

    public void Miss()
    {
        playerLives--;

        if (playerLives > 0)
        {
            ResetCurrentLevel();
        }
        else
        {
            GameOver();
        }
    }

    private void ResetCurrentLevel()
    {
        player.ResetPlayer();
        bola.ResetBola();

        // Resetting the bricks is optional
        // for (int i = 0; i < bricks.Length; i++) {
        //     bricks[i].ResetBrick();
        // }
    }

    private void GameOver()
    {
        // Start a new game immediately
        // You can also load a "GameOver" scene instead
        StartNewGame();
    }

    public void Hit(Brick brick)
    {
        playerScore += brick.points;

        if (CheckIfCleared())
        {
            LoadLevel(currentLevel + 1);
        }
    }

    private bool CheckIfCleared()
    {
        for (int i = 0; i < bricks.Length; i++)
        {
            if (bricks[i].gameObject.activeInHierarchy && !bricks[i].unbreakable)
            {
                return false;
            }
        }

        return true;
    }
}
