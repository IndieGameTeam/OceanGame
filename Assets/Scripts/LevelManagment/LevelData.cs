public enum LevelState
{
    Locked,
    Unlocked,
    Completed
}

[System.Serializable]
public class LevelData
{
    public string name = string.Empty;
    public LevelState levelState = LevelState.Locked;
}