namespace SkyRoad.Road
{
    public interface IRoadManager
    {
        float AdditionalSpeed { get; set; }
        void Start();
        void StartObstacleGenerator();
        void StopObstacleGenerator();
        void Stop();
    }
}