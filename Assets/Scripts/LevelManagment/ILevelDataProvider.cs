using System.Collections.Generic;

public interface ILevelDataProvider
{
    LevelData GetLevelData(string levelName);
    IEnumerable<LevelData> GetLevels();
}