using UnityEngine;

// Singleton that:
//  - Reads Doofus's Diary (JSON) once at startup and exposes the values to everyone else
//  - Owns the score and the Playing / GameOver state machine
//  - Is the single place other scripts report "player fell" / "player scored" to
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public DoofusDiaryData Diary { get; private set; }
    public int Score { get; private set; }
    public bool IsPlaying { get; private set; }
    public bool IsGameOver { get; private set; }

    // Assets/Resources/doofus_diary.json -> loaded as "doofus_diary" (no extension, no folder)
    private const string DiaryResourcePath = "doofus_diary";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadDiary();
    }

    void LoadDiary()
    {
        TextAsset json = Resources.Load<TextAsset>(DiaryResourcePath);

        if (json == null)
        {
            Debug.LogError($"[GameManager] Could not find Resources/{DiaryResourcePath}.json. " +
                            "Using hardcoded fallback values so the game can still run.");
            Diary = FallbackDiary();
            return;
        }

        try
        {
            Diary = JsonUtility.FromJson<DoofusDiaryData>(json.text);

            if (Diary == null || Diary.player_data == null || Diary.pulpit_data == null)
                throw new System.Exception("Parsed JSON is missing expected fields.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[GameManager] Failed to parse doofus_diary.json ({e.Message}). Using fallback values.");
            Diary = FallbackDiary();
        }
    }

    DoofusDiaryData FallbackDiary()
    {
        return new DoofusDiaryData
        {
            player_data = new PlayerData { speed = 3f },
            pulpit_data = new PulpitData
            {
                min_pulpit_destroy_time = 4f,
                max_pulpit_destroy_time = 5f,
                pulpit_spawn_time = 2.5f
            }
        };
    }

    // --- Game flow -------------------------------------------------------

    public void StartGame()
    {
        Score = 0;
        IsGameOver = false;
        IsPlaying = true;
        UIManager.Instance?.OnGameStarted();
    }

    public void EndGame()
    {
        if (IsGameOver) return; // guard against double-trigger (e.g. two collisions same frame)
        IsGameOver = true;
        IsPlaying = false;
        Debug.Log($"[GameManager] Game Over. Final Score: {Score}");
        UIManager.Instance?.ShowGameOver(Score);
    }

    public void AddScore(int amount = 1)
    {
        if (!IsPlaying) return;
        Score += amount;
        UIManager.Instance?.UpdateScore(Score);
    }
}
