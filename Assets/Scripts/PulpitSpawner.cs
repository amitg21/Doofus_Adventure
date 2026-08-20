using System.Collections.Generic;
using UnityEngine;

// Spawning rules implemented here (from the assignment doc + doofus_diary.json):
//   - At most 2 pulpits exist at the same time.
//   - Each pulpit's lifetime is random between min_pulpit_destroy_time and
//     max_pulpit_destroy_time (from JSON).
//   - pulpit_spawn_time seconds after a pulpit spawns, the NEXT one should
//     appear - but if 2 already exist at that moment, it waits until a slot
//     frees up (a pulpit gets destroyed) rather than breaking the "max 2" rule.
//   - Each new pulpit appears adjacent (sharing an edge) to the most recently
//     spawned one, in a position that isn't already occupied.
public class PulpitSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject pulpitPrefab;
    public float pulpitSize = 9f;
    public Vector3 firstPulpitPosition = Vector3.zero;

    private readonly List<Pulpit> activePulpits = new List<Pulpit>();
    private Vector3 lastSpawnedPosition;
    private float spawnTimer;
    private bool spawnDue;
    private PulpitData config;

    void Start()
    {
        config = GameManager.Instance.Diary.pulpit_data;
        SpawnPulpit(firstPulpitPosition);
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= config.pulpit_spawn_time)
        {
            spawnDue = true;
        }

        if (spawnDue && activePulpits.Count < 2)
        {
            SpawnPulpit(GetAdjacentPosition(lastSpawnedPosition));
            spawnTimer = 0f;
            spawnDue = false;
        }
    }

    Vector3 GetAdjacentPosition(Vector3 from)
    {
        Vector3[] directions =
        {
            new Vector3(pulpitSize, 0f, 0f),
            new Vector3(-pulpitSize, 0f, 0f),
            new Vector3(0f, 0f, pulpitSize),
            new Vector3(0f, 0f, -pulpitSize),
        };

        List<Vector3> free = new List<Vector3>();
        foreach (var dir in directions)
        {
            Vector3 candidate = from + dir;
            if (!IsOccupied(candidate))
                free.Add(candidate);
        }

        // Fallback (shouldn't normally happen with only 2 pulpits max):
        // if every neighbor is somehow occupied, just pick a random direction.
        if (free.Count == 0)
            return from + directions[Random.Range(0, directions.Length)];

        return free[Random.Range(0, free.Count)];
    }

    bool IsOccupied(Vector3 position)
    {
        foreach (var p in activePulpits)
        {
            if (p != null && Vector3.Distance(p.transform.position, position) < 0.1f)
                return true;
        }
        return false;
    }

    void SpawnPulpit(Vector3 position)
    {
        GameObject obj = Instantiate(pulpitPrefab, position, Quaternion.identity);
        Pulpit pulpit = obj.GetComponent<Pulpit>();

        if (pulpit == null)
        {
            Debug.LogError("[PulpitSpawner] pulpitPrefab needs a Pulpit component.");
            Destroy(obj);
            return;
        }

        float lifeTime = Random.Range(config.min_pulpit_destroy_time, config.max_pulpit_destroy_time);
        pulpit.Initialize(lifeTime);
        pulpit.OnDestroyed += HandlePulpitDestroyed;

        activePulpits.Add(pulpit);
        lastSpawnedPosition = position;
    }

    void HandlePulpitDestroyed(Pulpit pulpit)
    {
        activePulpits.Remove(pulpit);
    }
}
