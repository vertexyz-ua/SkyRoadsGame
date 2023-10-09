namespace SkyRoad.Hud
{
    public interface IHudController
    {
        void Init();
        void ShowGameOverGui();
        void ShowPlayingGui();
        void ShowStartGui();
        void UpdatePlayerScore(int gameScore);
        void UpdatePlayerHiScore(int hiScore, bool newHiScore);
        void UpdatePlayerTime(float time);
        void UpdatePlayerObstacleCount(int obstacleCount);
    }
}