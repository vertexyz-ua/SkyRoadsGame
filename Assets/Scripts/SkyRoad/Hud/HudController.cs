using TMPro;
using UnityEngine;

namespace SkyRoad.Hud
{
    public class HudController : MonoBehaviour, IHudController
    {
        [SerializeField]
        private GameObject waitingToStartPanel;

        [SerializeField]
        private GameObject playingPanel;
        
        [SerializeField]
        private GameObject gameOverPanel;

        
        [SerializeField]
        private TextMeshProUGUI hiScoreText;

        [SerializeField]
        private TextMeshProUGUI obstacleCountText;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        [SerializeField]
        private TextMeshProUGUI timeText;

        
        [SerializeField]
        private GameObject congratulationLabel;
        
        public void Init()
        {
            HideAllGui();
        }

        private void HideAllGui()
        {
            waitingToStartPanel.SetActive(false);
            playingPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        public void ShowGameOverGui()
        {
            HideAllGui();
            playingPanel.SetActive(true);
            gameOverPanel.SetActive(true);
        }

        public void ShowPlayingGui()
        {
            HideAllGui();
            playingPanel.SetActive(true);
        }

        public void ShowStartGui()
        {
            HideAllGui();
            waitingToStartPanel.SetActive(true);
        }

        public void UpdatePlayerScore(int gameScore)
        {
            scoreText.text = $"{gameScore}";
        }

        public void UpdatePlayerHiScore(int hiScore, bool newHiScore)
        {
            hiScoreText.text = $"{hiScore}";
            congratulationLabel.SetActive(newHiScore);
        }

        public void UpdatePlayerTime(float time)
        {
            timeText.text = time.ToString("N2");
        }

        public void UpdatePlayerObstacleCount(int obstacleCount)
        {
            obstacleCountText.text = $"{obstacleCount}";
        }
    }
}