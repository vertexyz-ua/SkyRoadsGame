using System;
using DG.Tweening;
using SkyRoad.Signals;
using UnityEngine;
using Zenject;

namespace SkyRoad.Ship
{
    public class ShipCamera : MonoBehaviour
    {
        private Settings _settings;
        private Ship _ship;
        private SignalBus _signalBus;
        private float distance;

        private Tweener _distanceTween;
        private float _height;
        private float _heightDamping;
        private Tweener _heightTween;
        private float _rotationDamping;
        private Transform _target;

        [Inject]
        public void Construct(
            Settings settings,
            SignalBus signalBus,
            Ship ship
        )
        {
            _settings = settings;
            _signalBus = signalBus;
            _ship = ship;
        }

        private void Start()
        {
            _target = _ship.transform;
            _signalBus.Subscribe<BoostedSpeedSignal>(OnShipBoosted);
            _signalBus.Subscribe<NormalSpeedSignal>(OnShipNormal);
            _signalBus.Subscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<BoostedSpeedSignal>(OnShipBoosted);
            _signalBus.Unsubscribe<NormalSpeedSignal>(OnShipNormal);
            _signalBus.Unsubscribe<ShipCrashedSignal>(OnShipCrashed);
        }

        private void OnShipNormal()
        {
            ApplyFollowParameters(_settings.normalCameraSettings);
        }

        private void OnShipBoosted()
        {
            ApplyFollowParameters(_settings.boostedCameraSettings);
        }

        private void OnShipCrashed()
        {
            //TODO CameraShake
        }

        public void LateUpdate()
        {
            // Early out if we don't have a target
            if (!_target) return;

            // Calculate the current rotation angles
            var wantedRotationAngle = _target.eulerAngles.y;
            var wantedHeight = _target.position.y + _height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle =
                Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, _rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, _heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            var pos = transform.position;
            pos = _target.position - currentRotation * Vector3.forward * distance;
            pos.y = currentHeight;
            transform.position = pos;

            // Always look at the target
            transform.LookAt(_target);
        }

        private void ApplyFollowParameters(FollowSettings followSettings)
        {
            _distanceTween.Kill();
            _distanceTween = DOTween.To(() => distance, x => distance = x, followSettings.distance, _settings.cameraFollowSpeed);
            _heightTween.Kill();
            _heightTween = DOTween.To(() => _height, x => _height = x, followSettings.height, _settings.cameraFollowSpeed);
            
            _heightDamping = followSettings.damping;
            _rotationDamping = followSettings.damping;
        }

        [Serializable]
        public class Settings
        {
            public FollowSettings normalCameraSettings;
            public FollowSettings boostedCameraSettings;
            public float cameraFollowSpeed;
            public float cameraShakeTime;
            
        }

        [Serializable]
        public class FollowSettings
        {
            public float distance;
            public float height;
            public float damping;
        }
    }
}