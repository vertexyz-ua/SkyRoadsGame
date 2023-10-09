using System;
using System.Collections.Generic;
using System.Linq;
using SkyRoad.Obstacles;
using SkyRoad.Signals;
using UnityEngine;
using Zenject;

namespace SkyRoad.Road
{
    public class RoadManager : IInitializable, ITickable, IDisposable, IRoadManager
    {
        private readonly AsteroidManager _asteroidSpawner;
        private readonly List<Floor> _roadElements = new List<Floor>();
        private readonly Floor.Factory _roadFactory;
        private readonly Settings _settings;

        private readonly SignalBus _signalBus;
        private bool _generatorStarted;

        private Floor _lastElement;
        private bool _started;
        
        private float _additionalSpeed;
        private bool _boosted;

        public Action<int> OnObstaclesPassed;

        public RoadManager(
            SignalBus signalBus,
            Floor.Factory roadFactory,
            Settings settings,
            AsteroidManager asteroidSpawner)
        {
            _signalBus = signalBus;
            _roadFactory = roadFactory;
            _settings = settings;
            _asteroidSpawner = asteroidSpawner;
        }

        public float AdditionalSpeed
        {
            get => _additionalSpeed;
            set => _additionalSpeed = value;
        }

        private float CurrentSpeed
        {
            get
            {
                if (_boosted)
                {
                    return (_settings.roadNormalSpeed + _additionalSpeed) * _settings.roadBoostedSpeedMulti;
                }
                
                return _settings.roadNormalSpeed + _additionalSpeed;
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BoostedSpeedSignal>(OnShipBoosted);
            _signalBus.Unsubscribe<NormalSpeedSignal>(OnShipNormal);
        }

        public void Initialize()
        {
            SetNormalSpeed();

            _signalBus.Subscribe<BoostedSpeedSignal>(OnShipBoosted);
            _signalBus.Subscribe<NormalSpeedSignal>(OnShipNormal);

            CreateRoad();
        }

        public void Tick()
        {
            if (!_started)
            {
                return;
            }
            
            foreach (var floor in _roadElements)
            {
                floor.PosZ -= CurrentSpeed * Time.deltaTime;
                if (floor.PosZ < _settings.roadCycleBound)
                {
                    OnObstaclesPassed(floor.ObstaclesCount);
                    floor.ResetAllObstacles();

                    floor.PosZ = _lastElement.PosZ + _settings.floorSize - CurrentSpeed * Time.deltaTime;
                    _lastElement = floor;

                    GenerateObstacle(floor);
                }
            }
        }

        private void OnShipNormal()
        {
            SetNormalSpeed();
        }

        private void SetNormalSpeed()
        {
            _boosted = false;
        }

        private void OnShipBoosted()
        {
            SetBoostedSpeed();
        }

        private void SetBoostedSpeed()
        {
            _boosted = true;
        }

        public void Start()
        {
            _started = true;
            ResetAllObstacles();
        }

        public void StartObstacleGenerator()
        {
            _generatorStarted = true;
        }

        public void StopObstacleGenerator()
        {
            _generatorStarted = false;
        }

        private void ResetAllObstacles()
        {
            _roadElements.ForEach(f => f.ResetAllObstacles());
        }

        public void Stop()
        {
            _started = false;
        }

        private void CreateRoad()
        {
            for (var i = 0; i < _settings.floorCount; i++)
            {
                var floor = _roadFactory.Create();
                floor.Scale = Vector3.one * _settings.floorSize;
                floor.PosZ = _settings.floorSize * i + _settings.roadOffset.z;
                floor.PosX = _settings.roadOffset.x;
                floor.PosY = _settings.roadOffset.y;
                _roadElements.Add(floor);
            }

            _lastElement = _roadElements.Last();
        }

        private void GenerateObstacle(Floor floor)
        {
            if (!_generatorStarted) return;

            var newAsteroid = _asteroidSpawner.SpawnNext(_lastElement.transform);
            if (newAsteroid != null)
            {
                floor.AddObstacle(newAsteroid);
            }
        }

        [Serializable]
        public class Settings
        {
            public int floorCount;
            public float floorSize;
            public float roadBoostedSpeedMulti;
            public float roadCycleBound;
            public float roadNormalSpeed;
            public Vector3 roadOffset;
        }
    }
}