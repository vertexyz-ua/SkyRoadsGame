using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SkyRoad.Obstacles
{
    public class AsteroidManager : IAsteroidManager
    {
        private readonly Asteroid.Factory _asteroidFactory;
        private readonly Settings _settings;

        private int _spawnCounter;
        
        public AsteroidManager(
            Asteroid.Factory asteroidFactory,
            Settings settings)
        {
            _asteroidFactory = asteroidFactory;
            _settings = settings;
        }
        
        public IObstacle SpawnNext(Transform parent)
        {
            if (!_settings.enabled)
            {
                
                return null;
            }
            _spawnCounter++;

            if (_spawnCounter % _settings.skipEvery > 0) return null;

            if (Random.value >= _settings.probability) return null;

            var asteroid = _asteroidFactory.Create();
            asteroid.Position = parent.position + _settings.positionOffset;
            asteroid.Scale = Vector3.one * _settings.size;
            asteroid.PosX = Random.Range(-_settings.positionSpread, _settings.positionSpread);
            asteroid.transform.parent = parent;
            asteroid.RotationSpeed = _settings.rotationSpeed;
            return asteroid;
        }

        [Serializable]
        public class Settings
        {
            public bool enabled = true;
            
            public Vector3 positionOffset;
            public float positionSpread;

            [Range(0.0f, 1.0f)]
            public float probability;

            public float rotationSpeed;
            public float size;
            public float skipEvery;
        }
    }
}