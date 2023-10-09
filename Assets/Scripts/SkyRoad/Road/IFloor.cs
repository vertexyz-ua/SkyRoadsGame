using SkyRoad.Obstacles;

namespace SkyRoad.Road
{
    public interface IFloor
    {
        int ObstaclesCount { get; }
        void ResetAllObstacles();
        void AddObstacle(IObstacle newObstacle);
    }
}