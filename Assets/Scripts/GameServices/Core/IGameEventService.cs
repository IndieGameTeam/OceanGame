using GameServices.Events;
using UnityEngine.Events;

public interface IGameEventService
{
    UnityEvent OnLevelCompleted { get; }
    UnityEvent OnLevelLosing { get; }
    UnityFloatEvent OnLevelProgressChanged { get; }
}