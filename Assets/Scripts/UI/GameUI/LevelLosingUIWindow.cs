using System;
using UnityEngine;
using UnityEngine.UI;

using GameServices.UI;
using GameServices.Extensions;
using GameServices.CoroutineSequencing;

public class LevelLosingUIWindow : UIElementBase
{
    public event Action OnRestart;

    public Button RestartButton;
    public Button MenuButton;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        RestartButton.onClick.AddListener(() => { BeginHide(OnRestart); });
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
        MenuButton.interactable = true;
    }

    private void DisactivateButtons()
    {
        RestartButton.interactable = false;
        MenuButton.interactable = false;
    }
}
