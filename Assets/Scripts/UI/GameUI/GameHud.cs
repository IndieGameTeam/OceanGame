using UnityEngine;
using GameServices.UI;
using UnityEngine.UI;

public class GameHud : UIElementView<GameModel>
{
    [Space]
    public Slider progressSlider;

    [Space]
    public Transform heartsContainer;
    public Color HealthlyColor = Color.red;
    public Color UnhealthyColor = Color.gray;

    private Image[] hearts;
    
    public override void UpdateView(GameModel model)
    {
        progressSlider.value = model.Progress;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = (i < model.HealthCount) ? HealthlyColor : UnhealthyColor;
        }
    }

    private void Awake()
    {
        hearts = heartsContainer.GetComponentsInChildren<Image>();
    }
}