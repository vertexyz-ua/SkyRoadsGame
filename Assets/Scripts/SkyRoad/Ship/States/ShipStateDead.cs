using System;
using SkyRoad.Fx;
using SkyRoad.Signals;
using UnityEngine;
using Zenject;

namespace SkyRoad.Ship.States
{
    public class ShipStateDead : ShipState
    {
        private readonly Explosion.Factory _explosionFactory;
        private readonly Settings _settings;
        private readonly Ship _ship;
        private readonly SignalBus _signalBus;

        public ShipStateDead(
            Settings settings, Ship ship,
            Explosion.Factory explosionFactory,
            SignalBus signalBus)
        {
            _signalBus = signalBus;
            _explosionFactory = explosionFactory;
            _settings = settings;
            _ship = ship;
        }

        public override void Start()
        {
            _ship.EnableAllMeshes(false);

            var explosion = _explosionFactory.Create();
            explosion.Position = _ship.Position;
            explosion.Scale = Vector3.one * _settings.explosionScale;

            _signalBus.Fire<ShipCrashedSignal>();
        }

        public override void Dispose()
        {
            _ship.EnableAllMeshes(true);
        }

        public override void Update()
        {
        }

        [Serializable]
        public class Settings
        {
            public float explosionScale = 1.0f;
        }

        public class Factory : PlaceholderFactory<ShipStateDead>
        {
        }
    }
}