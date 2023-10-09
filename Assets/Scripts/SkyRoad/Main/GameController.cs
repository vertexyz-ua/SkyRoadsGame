using System;
using ModestTree;
using SkyRoad.Hud;
using SkyRoad.Road;
using SkyRoad.Ship;
using SkyRoad.Signals;
using UnityEngine;
using Zenject;

namespace SkyRoad.Main
{
    public enum GameStates
    {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable, IGameController
    {
        private readonly HudController _hudController;
        private readonly RoadManager _roadManager;
        private readonly Settings _settings;
        private readonly Ship.Ship _ship;
        private readonly SignalBus _signalBus;

        private GameModel _gameModel;

        private GameStates _state;

        public GameController(
            Ship.Ship ship,
            SignalBus signalBus,
            RoadManager roadManager,
            HudController hudController,
            Settings settings)
        {
            _signalBus = signalBus;
            _ship = ship;
            _roadManager = roadManager;
            _hudController = hudController;
            _settings = settings;
        }

        private void ChangeState(GameStates state)
        {
            _state = state;

            switch (_state)
            {
                case GameStates.WaitingToStart:
                {
                    SetStartingState();
                    break;
                }
                case GameStates.Playing:
                {
                    SetPlayingState();
                    break;
                }
                case GameStates.GameOver:
                {
                    SetGameOverState();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        public void Initialize()
        {
            Physics.gravity = Vector3.zero;
            
            _roadManager.Start();
            _roadManager.OnObstaclesPassed = OnObstaclesPassed;

            _hudController.Init();
            
            _signalBus.Subscribe<ShipCrashedSignal>(OnShipCrashed);
            
            ChangeState(GameStates.WaitingToStart);
        }

        public void Tick()
        {
            switch (_state)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
                case GameStates.GameOver:
                {
                    UpdateGameOver();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        private void UpdateStarting()
        {
            Assert.That(_state == GameStates.WaitingToStart);

            if (Input.GetMouseButton(0))
            {
                StartGame();
            }
        }
        
        private void UpdatePlaying()
        {
            Assert.That(_state == GameStates.Playing);
            
            _gameModel.LastTime = _gameModel.Time;
            _gameModel.Time += Time.deltaTime;
            
            _gameModel.SpeedChangeTimer += Time.deltaTime;
            
            if (_gameModel.SpeedChangeTimer > _settings.speedIncrementTimeSeconds)
            {
                _gameModel.SpeedChangeTimer = 0;
                _gameModel.AdditionalSpeed += _settings.speedIncrementRate;

                if (_gameModel.AdditionalSpeed > _settings.maxSpeed)
                {
                    _gameModel.AdditionalSpeed = _settings.maxSpeed;
                }

                _roadManager.AdditionalSpeed = _gameModel.AdditionalSpeed;
            }

            AddScoreForTime();
            
            UpdateHUD();
        }

        private void UpdateGameOver()
        {
            Assert.That(_state == GameStates.GameOver);

            UpdateHUD();

            if (Input.GetMouseButton(0))
            {
                StartGame();
            }
        }
        
        private void UpdateHUD()
        {
            _hudController.UpdatePlayerTime(_gameModel.Time);
            _hudController.UpdatePlayerScore(_gameModel.Score);
            _hudController.UpdatePlayerHiScore(_gameModel.HiScore, _gameModel.NewHiScore);
            _hudController.UpdatePlayerObstacleCount(_gameModel.ObstacleCount);
        }
        
        private void OnShipCrashed()
        {
            Assert.That(_state == GameStates.Playing);
            
            _roadManager.Stop();
            _roadManager.StopObstacleGenerator();
            ChangeState(GameStates.GameOver);
        }
        
        private void OnObstaclesPassed(int count)
        {
            AddScoreForObstacle(count);
            _gameModel.ObstacleCount += count;
        }

        private void AddScoreForObstacle(int count)
        {
            var score = count * _settings.scorePerObstacle;
            score *= _ship.Boosted ? _settings.perObstacleMultiplier : 1;
            _gameModel.Score += score;
        }
        
        private void AddScoreForTime()
        {
            if (Mathf.RoundToInt(_gameModel.Time) <= Mathf.RoundToInt(_gameModel.LastTime))
            {
                return;
            }
            
            var score = _settings.scorePerSecond;
            score *= _ship.Boosted ? _settings.perSecondMultiplier : 1;
            _gameModel.Score += score;
        }

        private void StartGame()
        {
            Assert.That(_state == GameStates.WaitingToStart || _state == GameStates.GameOver);

            ChangeState(GameStates.Playing);
        }

        private void SetStartingState()
        {
            _hudController.ShowStartGui();
        }
        
        private void SetPlayingState()
        {
            _gameModel.Reset();

            _roadManager.AdditionalSpeed = _gameModel.AdditionalSpeed;
            
            _roadManager.Start();
            _roadManager.StartObstacleGenerator();
            
            _ship.ChangeState(ShipStates.Moving);
            
            _hudController.ShowPlayingGui();
        }
        
        private void SetGameOverState()
        {
            _hudController.ShowGameOverGui();
        }
        
        [Serializable]
        public class Settings
        {
            public int scorePerObstacle;
            public int perObstacleMultiplier;
            public int scorePerSecond;
            public int perSecondMultiplier;
            public float speedIncrementRate;
            public float speedIncrementTimeSeconds;
            public float maxSpeed;
        }
    }
}