using System.Collections.Generic;
using SkyRoad.Obstacles;
using SkyRoad.Utils.Extensions;
using Zenject;

namespace SkyRoad.Road
{
    public class Floor : ExtendedMonoBehaviour, IPoolable<IMemoryPool>, IFloor
    {
        private readonly List<IObstacle> _obstacles = new List<IObstacle>();

        public int ObstaclesCount => _obstacles.Count;

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool pool)
        {
        }

        public void ResetAllObstacles()
        {
            _obstacles.ForEach(o => o.Reset());
            _obstacles.Clear();
        }

        public void AddObstacle(IObstacle newObstacle)
        {
            _obstacles.Add(newObstacle);
        }

        public class Factory : PlaceholderFactory<Floor>
        {
        }
    }
}