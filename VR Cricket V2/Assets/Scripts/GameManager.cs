using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Optional: public state variables
    public int playerScore = 0;
    public bool isGameActive = false;

    private void Awake()
    {
        // Singleton check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    // Example usage
    public void StartGame()
    {
        isGameActive = true;
        playerScore = 0;
        Debug.Log("Game Started");
    }

    public void EndGame()
    {
        isGameActive = false;
        Debug.Log("Game Ended");
    }

    public void AddScore(int points)
    {
        playerScore += points;
        Debug.Log("Score: " + playerScore);
    }
}
