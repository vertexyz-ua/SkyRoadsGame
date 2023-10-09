using SkyRoad.Utils.Extensions;
using UnityEngine;
using Zenject;

namespace SkyRoad.Fx
{
    public class Explosion : ExtendedMonoBehaviour, IPoolable<IMemoryPool>
    {
        [SerializeField]
        private float lifeTime;

        [SerializeField]
        private ParticleSystem particles;

        private IMemoryPool pool;

        private float startTime;

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool memoryPool)
        {
            particles.Clear();
            particles.Play();

            startTime = Time.realtimeSinceStartup;
            pool = memoryPool;
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup - startTime > lifeTime) pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<Explosion>
        {
        }
    }
}