using System;
using SkyRoad.Main;
using SkyRoad.Obstacles;
using SkyRoad.Road;
using SkyRoad.Ship;
using SkyRoad.Ship.States;
using UnityEngine;
using Zenject;

namespace SkyRoad.Installers
{
    [CreateAssetMenu(fileName = "SkyRoadSettings", menuName = "Installers/SkyRoadSettings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public AsteroidManager.Settings asteroid;
        public GameController.Settings gameController;
        public GameInstaller.Settings gameInstaller;
        public RoadManager.Settings road;
        public ShipSettings ship;
        public ShipCamera.Settings skyRoadCamera;

        public override void InstallBindings()
        {
            Container.BindInstance(ship.stateMoving);
            Container.BindInstance(ship.stateDead);
            Container.BindInstance(ship.stateStarting);
            Container.BindInstance(asteroid);
            Container.BindInstance(road);
            Container.BindInstance(skyRoadCamera);
            Container.BindInstance(gameInstaller);
            Container.BindInstance(gameController);
        }

        [Serializable]
        public class ShipSettings
        {
            public ShipStateDead.Settings stateDead;
            public ShipStateMoving.Settings stateMoving;
            public ShipStateWaitingToStart.Settings stateStarting;
        }
    }
}