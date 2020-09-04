using UnityEngine;
using UnityEngine.Events;
using GameServices.Events;

[System.Serializable]
public class GameEventService : IGameEventService
{
    public UnityEvent OnLevelCompleted => _onLevelCompleted;

    public UnityEvent OnLevelLosing => _onLevelLosing;

    public UnityFloatEvent OnLevelProgressChanged => _onLevelProgressChanged;


    [SerializeField]
    private UnityEvent _onLevelCompleted = null;
    [SerializeField]
    private UnityEvent _onLevelLosing = null;
    [SerializeField]
    private UnityFloatEvent _onLevelProgressChanged = null;
}