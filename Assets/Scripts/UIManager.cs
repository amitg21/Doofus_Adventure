using UnityEngine;

// NOTE: This is a minimal placeholder for now.
// Level 3 of the assignment asks for a Start screen and Game Over screen -
// we'll expand this script with real UI (Canvas, buttons, panels) at that stage.
// Keeping it here now (rather than adding it later) so GameManager compiles
// and Levels 1-2 can be tested standalone.
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OnGameStarted()
    {
        Debug.Log("[UIManager] Game started.");
    }

    public void UpdateScore(int score)
    {
        Debug.Log($"[UIManager] Score updated: {score}");
    }

    public void ShowGameOver(int finalScore)
    {
        Debug.Log($"[UIManager] GAME OVER. Final score: {finalScore}");
    }
}
