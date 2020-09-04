using GameServices.Events;
using GameServices.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IGameServiceProvider
{
    public int HealthCount
    {
        get => _gameModel.HealthCount;
        set
        {
            if (_gameModel.HealthCount != value)
            {
                _gameModel.HealthCount = value;
                _gameHUDView.UpdateView(_gameModel);
                OnHealthCountChanged();
            }
        }
    }

    public float Progress
    {
        get => _gameModel.Progress;
        set
        {
            if (Math.Abs(_gameModel.Progress - value) > minimalProgressDifference)
            {
                _gameModel.Progress = value;
                _gameHUDView.UpdateView(_gameModel);
                _gameEventService.OnLevelProgressChanged.Invoke(value);
                OnProgressChanged();
            }
        }
    }

    public IObjectPoolService ObjectPoolService { get => _poolService; }

    public IGameEventService GameEventService { get => _gameEventService; }

    [SerializeField]
    private GameEventService _gameEventService = null;

    private ShipUnit _shipUnit;
    private UIManager _UIManager;

    private int defaultHealthCount = 3;
    private Vector3 defaultShipPosition = Vector3.zero;
    private Vector3 defaultShipRotation = Vector3.zero;

    private GameHud _gameHUDView;
    private GameModel _gameModel;

    private IObjectPoolService _poolService = null;

    private const float minimalProgressDifference = 0.0025F;

    private void Start()
    {
        _gameModel = new GameModel(defaultHealthCount, 0);
        _poolService = new ObjectPool();

        _shipUnit = FindObjectOfType<ShipUnit>();
        _UIManager = FindObjectOfType<UIManager>();

        //TODO: Add checking of components.
        //...

        foreach (IGameServiceConsumer serviceConsumer in FindObjectOnScene<IGameServiceConsumer>())
        {
            serviceConsumer.Setup(this);
        }

        defaultShipPosition = _shipUnit.transform.position;
        defaultShipRotation = _shipUnit.transform.rotation.eulerAngles;

        _shipUnit.OnDamaged += () => HealthCount--;

        _UIManager.GetUIElement<LevelLosingUIWindow>().OnRestart += () => { Restart(); Play(); };
        _UIManager.GetUIElement<LevelCompleteUIWindow>().OnRestart += () => { Restart(); Play(); };

        _gameHUDView = _UIManager.GetUIElement<GameHud>();
        _gameHUDView.UpdateView(_gameModel);
    }

    private void FixedUpdate()
    {
        Progress = Vector3.Distance(defaultShipPosition, _shipUnit.transform.position) / LevelManager.Instance.currentLevel.options.levelLength;
    }

    private void Play()
    {
        _gameHUDView.BeginShow();
        StartCoroutine(CoroutineBuilder.LerpValue(Time.timeScale, 1F, 0.1F, value =>
        {
            Time.timeScale = value;
        }));
    }

    private void Pause()
    {
        _gameHUDView.BeginHide();
        StartCoroutine(CoroutineBuilder.LerpValue(Time.timeScale, 0, 0.1F, value =>
        {
            Time.timeScale = value;
        }));
    }

    private void Restart()
    {
        HealthCount = defaultHealthCount;
        _shipUnit.transform.position = defaultShipPosition;
        _shipUnit.transform.rotation = Quaternion.Euler(defaultShipRotation);
    }

    private void OnProgressChanged()
    {
        if (Progress >= 1.0F)
        {
            LevelCompleted();
        }
    }

    private void OnHealthCountChanged()
    {
        if (HealthCount == 0)
        {
            LevelLosing();
        }
    }

    private void LevelLosing()
    {
        Pause();
        _UIManager.GetUIElement<LevelLosingUIWindow>().BeginShow();
        _gameEventService.OnLevelLosing.Invoke();
    }

    private void LevelCompleted()
    {
        Pause();
        _UIManager.GetUIElement<LevelCompleteUIWindow>().BeginShow();
        _gameEventService.OnLevelCompleted.Invoke();
    }

    private IEnumerable<T> FindObjectOnScene<T>()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        
        foreach (GameObject rootGameObject in scene.GetRootGameObjects())
        {
            var tempObjects = rootGameObject.GetComponentInChildren<T>();

            if(tempObjects != null)
            {
                yield return tempObjects;
            }
        }
    }
}