using UnityEngine;

namespace SkyRoad.Obstacles
{
    public interface IAsteroidManager
    {
        IObstacle SpawnNext(Transform parent);
    }
}