using SkyRoad.Utils.Extensions;
using UnityEngine;
using Zenject;

namespace SkyRoad.Obstacles
{
    public class Asteroid : ExtendedMonoBehaviour, IPoolable<IMemoryPool>, IObstacle
    {
        private IMemoryPool _pool;

        private Vector3 _rotationVector;

        public float RotationSpeed
        {
            set => _rotationVector = Random.insideUnitSphere * value;
        }

        public void Reset()
        {
            DeSpawn();
        }

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool memoryPool)
        {
            _pool = memoryPool;
        }

        private void DeSpawn()
        {
            _pool?.Despawn(this);
        }

        private void Update()
        {
            transform.Rotate(_rotationVector * Time.deltaTime);
        }

        public class Factory : PlaceholderFactory<Asteroid>
        {
        }
    }
}