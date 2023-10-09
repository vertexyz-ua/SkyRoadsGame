using System;
using UnityEngine;
using Zenject;

namespace SkyRoad.Ship.States
{
    public class ShipStateWaitingToStart : ShipState
    {
        private readonly Settings _settings;
        private readonly Ship _ship;

        private float _theta;

        public ShipStateWaitingToStart(
            Ship ship,
            Settings settings)
        {
            _settings = settings;
            _ship = ship;
        }

        public override void Start()
        {
            _ship.Position = _settings.startPosition;
            _ship.Rotation = Quaternion.identity;
        }

        public override void Update()
        {
            _ship.Position = _settings.startPosition + Vector3.up * _settings.startAmplitude * Mathf.Sin(_theta);
            _theta += Time.deltaTime * _settings.startFrequency;
        }

        [Serializable]
        public class Settings
        {
            public float startAmplitude;
            public float startFrequency;
            public Vector3 startPosition;
        }

        public class Factory : PlaceholderFactory<ShipStateWaitingToStart>
        {
        }
    }
}