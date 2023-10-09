using System;
using ModestTree;
using SkyRoad.Obstacles;
using SkyRoad.Signals;
using UnityEngine;
using Zenject;

namespace SkyRoad.Ship.States
{
    public class ShipStateMoving : ShipState
    {
        private readonly Settings _settings;
        private readonly Ship _ship;
        private readonly SignalBus _signalBus;
        private bool _isBoosted;

        private float _lastTiltAngle;

        public ShipStateMoving(
            Settings settings, Ship ship,
            SignalBus signalBus)
        {
            _ship = ship;
            _settings = settings;
            _signalBus = signalBus;
        }

        public override void Update()
        {
            var inputAxis = Input.GetAxis("Horizontal");
            Move(inputAxis);
            Tilt(inputAxis);
            Boost();
        }

        private void Boost()
        {
            if (Input.GetButton("Boost"))
            {
                if (!_isBoosted)
                {
                    _isBoosted = true;
                    _ship.Boosted = true;
                    _signalBus.Fire<BoostedSpeedSignal>();
                }
            }
            else
            {
                if (_isBoosted)
                {
                    _isBoosted = false;
                    _ship.Boosted = false;
                    _signalBus.Fire<NormalSpeedSignal>();
                }
            }
        }

        private void Tilt(float inputAxis)
        {
            var tiltAngle = -inputAxis * _settings.tiltAngle;

            _lastTiltAngle = Mathf.Lerp(_lastTiltAngle, tiltAngle, _settings.tiltSpeed);

            _ship.Rotation = Quaternion.Euler(0.0f, 0.0f, _lastTiltAngle);
        }

        private void Move(float inputAxis)
        {
            var position = _ship.Position;
            position.x += inputAxis * _settings.moveSpeed;

            position.x = Mathf.Clamp(position.x, -_settings.shipBounds, _settings.shipBounds);

            _ship.Position = position;
        }

        public override void Start()
        {
            _lastTiltAngle = 0.0f;
            _isBoosted = false;
            _signalBus.Fire<NormalSpeedSignal>();
        }

        public override void Dispose()
        {
        }

        public override void OnTriggerEnter(Collider other)
        {
            Assert.That(other.GetComponent<Asteroid>() != null);
            _ship.ChangeState(ShipStates.Dead);
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float shipBounds;

            [Range(0, 90)]
            public float tiltAngle;

            [Range(0.0f, 1.0f)]
            public float tiltSpeed;
        }

        public class Factory : PlaceholderFactory<ShipStateMoving>
        {
        }
    }
}