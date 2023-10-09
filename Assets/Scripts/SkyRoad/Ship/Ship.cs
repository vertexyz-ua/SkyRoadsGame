using SkyRoad.Ship.States;
using SkyRoad.Utils.Extensions;
using UnityEngine;
using Zenject;

namespace SkyRoad.Ship
{
    public class Ship : ExtendedMonoBehaviour, IPlayer
    {
        private ShipState _state;

        private ShipStateFactory _stateFactory;

        [SerializeField]
        private MeshRenderer engineLeftMeshRenderer;

        [SerializeField]
        private MeshRenderer engineRightMeshRenderer;

        [SerializeField]
        private MeshRenderer shipMeshRenderer;
  
        public bool Boosted { get; set; }

        [Inject]
        public void Construct(ShipStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void EnableAllMeshes(bool enable)
        {
            shipMeshRenderer.enabled = enable;
            engineLeftMeshRenderer.enabled = enable;
            engineRightMeshRenderer.enabled = enable;
        }
        
        private void Start()
        {
            ChangeState(ShipStates.WaitingToStart);
        }

        private void Update()
        {
            _state.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            _state.OnTriggerEnter(other);
        }

        public void ChangeState(ShipStates state)
        {
            if (_state != null)
            {
                _state.Dispose();
                _state = null;
            }

            _state = _stateFactory.CreateState(state);
            _state.Start();
        }
    }
}