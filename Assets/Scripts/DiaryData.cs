using System;

// These classes mirror the structure of doofus_diary.json exactly.
// Field names MUST match the JSON keys because JsonUtility maps by name.

[Serializable]
public class PlayerData
{
    public float speed;
}

[Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}

[Serializable]
public class DoofusDiaryData
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}
