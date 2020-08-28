using GameServices.UI;

public struct GameModel : IDataModel
{
    public int HealthCount;
    public float Progress;

    public GameModel(int healthCount, float progress)
    {
        HealthCount = healthCount;
        Progress = progress;
    }
}