using System;
using SkyRoad.Fx;
using SkyRoad.Main;
using SkyRoad.Road;
using SkyRoad.Ship;
using SkyRoad.Ship.States;
using SkyRoad.Signals;
using SkyRoad.Obstacles;
using UnityEngine;
using Zenject;

namespace SkyRoad.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private Settings _settings;

        public override void InstallBindings()
        {
            InstallMain();
            InstallPlayer();
            InstallObstacles();
            InstallRoad();
            InstallFx();
            InstallSignals();
        }

        private void InstallMain()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        }

        private void InstallPlayer()
        {
            Container.Bind<ShipStateFactory>().AsSingle();

            Container.BindFactory<ShipStateWaitingToStart, ShipStateWaitingToStart.Factory>()
                .WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateDead, ShipStateDead.Factory>().WhenInjectedInto<ShipStateFactory>();
            Container.BindFactory<ShipStateMoving, ShipStateMoving.Factory>().WhenInjectedInto<ShipStateFactory>();
        }

        private void InstallObstacles()
        {
            Container.BindInterfacesAndSelfTo<AsteroidManager>().AsSingle();

            Container.BindFactory<Asteroid, Asteroid.Factory>()
                .FromPoolableMemoryPool<Asteroid, AsteroidPool>(poolBinder => poolBinder
                    .WithInitialSize(_settings.asteroidPoolCount)
                    .FromComponentInNewPrefab(_settings.asteroidPrefab)
                    .WithGameObjectName("Asteroid")
                    .UnderTransformGroup("AsteroidsPool"));
        }

        private void InstallRoad()
        {
            Container.BindInterfacesAndSelfTo<RoadManager>().AsSingle();

            Container.BindFactory<Floor, Floor.Factory>()
                .FromPoolableMemoryPool<Floor, FloorPool>(poolBinder => poolBinder
                    .WithInitialSize(_settings.floorPoolCount)
                    .FromComponentInNewPrefab(_settings.floorPrefab)
                    .WithGameObjectName("Floor")
                    .UnderTransformGroup("RoadPool"));
        }

        private void InstallFx()
        {
            Container.BindFactory<Explosion, Explosion.Factory>()
                .FromPoolableMemoryPool<Explosion, ExplosionPool>(poolBinder => poolBinder
                    .WithInitialSize(_settings.explosionPoolCount)
                    .FromComponentInNewPrefab(_settings.explosionPrefab)
                    .WithGameObjectName("Explosion")
                    .UnderTransformGroup("ExplosionsPool"));
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<ShipCrashedSignal>();
            Container.DeclareSignal<BoostedSpeedSignal>();
            Container.DeclareSignal<NormalSpeedSignal>();
        }

        [Serializable]
        public class Settings
        {
            public GameObject asteroidPrefab;
            public int asteroidPoolCount;
            public GameObject explosionPrefab;
            public int explosionPoolCount;
            public GameObject floorPrefab;
            public int floorPoolCount;
        }

        private class ExplosionPool : MonoPoolableMemoryPool<IMemoryPool, Explosion>
        {
        }

        private class FloorPool : MonoPoolableMemoryPool<IMemoryPool, Floor>
        {
        }

        private class AsteroidPool : MonoPoolableMemoryPool<IMemoryPool, Asteroid>
        {
        }
    }
}