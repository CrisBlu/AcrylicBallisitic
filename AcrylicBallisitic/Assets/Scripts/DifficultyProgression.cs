using System.Collections.Generic;
using UnityEngine;

public class DifficultyProgression : MonoBehaviour
{
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Difficult,
    }

    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;
    public float normalThreshold = 0.66f;
    public float difficultThreshold = 0.33f;

    static readonly Dictionary<DifficultyLevel, int> spawnCounts = new Dictionary<DifficultyLevel, int>()
    {
        { DifficultyLevel.Easy, 1 },
        { DifficultyLevel.Normal, 1 },
        { DifficultyLevel.Difficult, 2 },
    };

    static readonly Dictionary<DifficultyLevel, bool> ghostAllowance = new Dictionary<DifficultyLevel, bool>()
    {
        { DifficultyLevel.Easy, false },
        { DifficultyLevel.Normal, true },
        { DifficultyLevel.Difficult, true },
    };

    public void UpdateDifficulty(float netWorthRatio)
    {
        print("Updating difficulty with net worth ratio: " + netWorthRatio);
        DifficultyLevel newDifficulty;
        if (netWorthRatio < difficultThreshold) newDifficulty = DifficultyLevel.Difficult;
        else if (netWorthRatio < normalThreshold) newDifficulty = DifficultyLevel.Normal;
        else newDifficulty = DifficultyLevel.Easy;

        if (newDifficulty != currentDifficulty)
        {
            currentDifficulty = newDifficulty;
            EventManager.Broadcast(new DifficultyChangedEvent(currentDifficulty));
        }
    }

    public int GetSpawnCount() { return spawnCounts[currentDifficulty]; }
    public bool IsGhostAllowed() { return ghostAllowance[currentDifficulty]; }
}