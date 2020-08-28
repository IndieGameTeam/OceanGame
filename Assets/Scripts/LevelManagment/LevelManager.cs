using UnityEngine;

public class LevelManager
{
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelManager();
            }

            return instance;
        }
    }
    public LevelData currentLevel 
    { 
        get => levels[currentLevelIndex]; 
    }
    public LevelData[] levels { get; }

    private static LevelManager instance = null;
    private int currentLevelIndex = 0;


    public LevelManager()
    {
        var options = Resources.LoadAll<LevelOptions>("Levels");
        var levels = new LevelData[options.Length];

        for (int i = 0; i < options.Length; i++)
        {
            LevelData levelData = new LevelData();

            levelData.name = options[i].name;
            levelData.levelState = LevelState.Unlocked;
            levelData.options = options[i];

            levels[i] = levelData;
        }

        this.levels = levels;
    }

}