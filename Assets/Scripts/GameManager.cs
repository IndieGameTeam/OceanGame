﻿using GameServices.Extensions;
using GameServices.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnLevelCompleted;
    public UnityEvent OnLevelLosing;

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
                OnProgressChanged();
            }
        }
    }

    public LevelOptions _options;
    private ShipUnit _shipUnit;
    private UIManager _UIManager;
    private ObjectsController _objectsController;

    private int defaultHealthCount = 3;
    private Vector3 defaultShipPosition = Vector3.zero;
    private Vector3 defaultShipRotation = Vector3.zero;

    private GameHud _gameHUDView;
    private GameModel _gameModel;

    private const float minimalProgressDifference = 0.0025F;

    private void Start()
    {
        _gameModel = new GameModel(defaultHealthCount, 0);
        _shipUnit = FindObjectOfType<ShipUnit>();
        _UIManager = FindObjectOfType<UIManager>();
        _objectsController = FindObjectOfType<ObjectsController>();

        //TODO:Add checking of components
        //...

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
        Progress = Vector3.Distance(defaultShipPosition, _shipUnit.transform.position) / _options.levelLength;
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
        if (Progress >= 0.1F && !_objectsController.IsSpawninig)
        {
            _objectsController.BeginAsteroidSpawning(_shipUnit.gameObject, _options);
        } 
        else if (Progress >= 0.9F && _objectsController.IsSpawninig)
        {
            _objectsController.EndAsteroidSpawning();
        }

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
        _objectsController.EndAsteroidSpawning();
        _UIManager.GetUIElement<LevelLosingUIWindow>().BeginShow();
        OnLevelLosing.Invoke();
    }

    private void LevelCompleted()
    {
        Pause();
        _objectsController.EndAsteroidSpawning();
        _UIManager.GetUIElement<LevelCompleteUIWindow>().BeginShow();
        OnLevelCompleted.Invoke();
    }
}