using System;
using UnityEngine;
using UnityEngine.UI;

using GameServices.UI;
using GameServices.Extensions;
using GameServices.CoroutineSequencing;

public class LevelCompleteUIWindow : UIElementBase
{
    public event Action OnRestart;
    public event Action OnNextLevel;

    public Button RestartButton;
    public Button NextLevelButton;
    public Button MenuButton;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        RestartButton.onClick.AddListener(() => { BeginHide(OnRestart); });
        NextLevelButton.onClick.AddListener(() => { BeginHide(OnNextLevel); });
        MenuButton.onClick.AddListener(() => { BeginHide(); });
    }

    protected override CoroutineSequence OnShowBegin()
    {
        return new CoroutineSequence(
            CoroutineBuilder.PlayAndWait(animator, "Show"),
            CoroutineBuilder.InvokeInCoroutine(ActivateButtons));
    }

    protected override CoroutineSequence OnHideBegin()
    {
        return new CoroutineSequence(
            CoroutineBuilder.InvokeInCoroutine(DisactivateButtons),
            CoroutineBuilder.PlayAndWait(animator, "Hide"));
    }

    private void ActivateButtons()
    {
        RestartButton.interactable = true;
        NextLevelButton.interactable = true;
        MenuButton.interactable = true;
    }

    private void DisactivateButtons()
    {
        RestartButton.interactable = false;
        NextLevelButton.interactable = false;
        MenuButton.interactable = false;
    }
}
