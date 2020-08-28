using System;

public enum LevelState
{
    Locked,
    Unlocked,
    Completed
}

[Serializable]
public class LevelData
{
    public string name = string.Empty;
    public LevelState levelState = LevelState.Locked;

    [NonSerialized]
    public LevelOptions options;
}